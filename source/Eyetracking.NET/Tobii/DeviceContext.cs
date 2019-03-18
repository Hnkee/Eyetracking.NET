using System;
using System.Threading;
using Tobii.StreamEngine;

namespace Eyetracking.NET.Tobii
{
    internal sealed class DeviceContext : IDisposable
    {
        private IntPtr _deviceHandle;
        private IntPtr[] _deviceHandles;

        internal DeviceContext(IntPtr apiHandle, string url, ILogger logger = null)
        {
            var result = Interop.tobii_device_create(apiHandle, url, out _deviceHandle);
            if (result != tobii_error_t.TOBII_ERROR_NO_ERROR)
            {
                throw new Exception("Failed to create device context. " + result);
            }

            tobii_device_info_t info;
            result = Interop.tobii_get_device_info(_deviceHandle, out info);
            if (result != tobii_error_t.TOBII_ERROR_NO_ERROR)
            {
                logger?.Warning("Failed to get device info. " + result);
            }

            logger?.Debug("Firmware version: " + info.firmware_version);
            SerialNumber = info.serial_number;
            _deviceHandles = new[] { _deviceHandle };
        }

        public string SerialNumber { get; }

        public bool IsWearable => SerialNumber.StartsWith("VR");

        public IntPtr Handle => _deviceHandle;

        public void Dispose()
        {
            if (_deviceHandle != IntPtr.Zero) Interop.tobii_device_destroy(_deviceHandle);
            _deviceHandle = IntPtr.Zero;
        }

        public void Pump()
        {
            var result = Interop.tobii_wait_for_callbacks(IntPtr.Zero, _deviceHandles);
            result = Interop.tobii_device_process_callbacks(_deviceHandle);
            if (result != tobii_error_t.TOBII_ERROR_CONNECTION_FAILED) return;

            result = Interop.tobii_device_reconnect(_deviceHandle);
            while (result == tobii_error_t.TOBII_ERROR_CONNECTION_FAILED && _deviceHandle != IntPtr.Zero)
            {
                Thread.Sleep(500);
                if (_deviceHandle == IntPtr.Zero) return;
                result = Interop.tobii_device_reconnect(_deviceHandle);
            }

        }
    }
}
