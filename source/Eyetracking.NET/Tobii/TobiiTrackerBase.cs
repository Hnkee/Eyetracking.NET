using System;
using System.Linq;
using System.Threading;
using Tobii.StreamEngine;

namespace Eyetracking.NET.Tobii
{
    internal abstract class TobiiTrackerBase : IDisposable
    {
        protected DeviceContext _device;

        private readonly Thread _thread;

        private bool _continue = true;
        private tobii_wearable_data_callback_t _wearableCallbackInstance;
        private tobii_gaze_point_callback_t _gazePointCallbackInstance;
        private tobii_head_pose_callback_t _headPoseCallbackInstance;

        internal TobiiTrackerBase(ApiContext api, bool isWearable, bool trackHead = false)
        {
            _device = api
                .GetDeviceUrls()
                .Select(u => new DeviceContext(api.Handle, u))
                .FirstOrDefault(d => d.IsWearable == isWearable);

            if (_device != null)
            {
                if (isWearable)
                {
                    var result = Interop.tobii_stream_supported(_device.Handle, tobii_stream_t.TOBII_STREAM_WEARABLE, out var wearableSupported);
                    if (result == tobii_error_t.TOBII_ERROR_NO_ERROR && wearableSupported)
                    {
                        _wearableCallbackInstance = WearableCallback;
                        result = Interop.tobii_wearable_data_subscribe(_device.Handle, _wearableCallbackInstance);
                    }
                }
                else
                {
                    var resultGaze = Interop.tobii_stream_supported(_device.Handle, tobii_stream_t.TOBII_STREAM_GAZE_POINT, out var gazePointSupported);
                    if (resultGaze == tobii_error_t.TOBII_ERROR_NO_ERROR && gazePointSupported)
                    {
                        _gazePointCallbackInstance = GazePointCallback;
                        resultGaze = Interop.tobii_gaze_point_subscribe(_device.Handle, _gazePointCallbackInstance);
                    }
                    if (trackHead)
                    {
                        var resultHead = Interop.tobii_stream_supported(_device.Handle, tobii_stream_t.TOBII_STREAM_HEAD_POSE, out var headPoseSupported);
                        if (resultHead == tobii_error_t.TOBII_ERROR_NO_ERROR && headPoseSupported)
                        {
                            _headPoseCallbackInstance = HeadPoseCallback;
                            resultHead = Interop.tobii_head_pose_subscribe(_device.Handle, _headPoseCallbackInstance);
                        }
                    }
                }

                _thread = new Thread(DataPump);
                _thread.IsBackground = true;
                _thread.Name = $"Data pump for {_device.SerialNumber}";
                _thread.Start();
            }
        }

        public void Dispose()
        {
            _continue = false;
            if (_gazePointCallbackInstance != null) Interop.tobii_gaze_point_unsubscribe(_device.Handle);
            if (_wearableCallbackInstance != null) Interop.tobii_wearable_data_unsubscribe(_device.Handle);
            if (_headPoseCallbackInstance != null) Interop.tobii_head_pose_unsubscribe(_device.Handle);
            _device.Dispose();
            _thread.Join(300);
            _gazePointCallbackInstance = null;
            _wearableCallbackInstance = null;
            _headPoseCallbackInstance = null;
        }

        protected virtual void GazePointCallback(ref tobii_gaze_point_t gaze_point) { }

        protected virtual void HeadPoseCallback(ref tobii_head_pose_t data) { }

        protected virtual void WearableCallback(ref tobii_wearable_data_t data) { }

        private void DataPump()
        {
            while (_continue)
            {
                _device.Pump();
            }
        }
    }

}
