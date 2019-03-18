using System;
using System.Collections.Generic;
using Tobii.StreamEngine;

namespace Eyetracking.NET.Tobii
{
    internal class ApiContext : IDisposable
    {
        private static readonly tobii_custom_log_t SeLogger = new tobii_custom_log_t { log_func = new Interop.tobii_log_func_t(SeLogMethod) };
        private static ILogger _logger;

        private IntPtr _apiHandle;

        internal ApiContext(ILogger logger = null)
        {
            _logger = logger;
            var result = Interop.tobii_get_api_version(out var version);
            if (result != tobii_error_t.TOBII_ERROR_NO_ERROR)
            {
                throw new Exception("Failed to interop with tobii native library. " + result);
            }

            _logger?.Debug($"Using tobii api version: {version.major}.{version.minor}.{version.revision}.{version.build}");

            result = Interop.tobii_api_create(out _apiHandle, SeLogger);
            if (result != tobii_error_t.TOBII_ERROR_NO_ERROR)
            {
                throw new Exception("Failed to create api context. " + result);
            }
        }

        public IntPtr Handle => _apiHandle;

        public void Dispose()
        {
            if (_apiHandle != IntPtr.Zero) Interop.tobii_api_destroy(_apiHandle);
            _apiHandle = IntPtr.Zero;
        }

        public IEnumerable<string> GetDeviceUrls()
        {
            List<string> urls;
            tobii_error_t result;
            result = Interop.tobii_enumerate_local_device_urls(_apiHandle, out urls);
            if (result != tobii_error_t.TOBII_ERROR_NO_ERROR)
            {
                _logger?.Error("Failed to get list of devices. " + result);
                return null;
            }

            if (urls.Count == 0)
            {
                _logger?.Warning("No eye trackers found. Result: " + result);
                return null;
            }
         
            return urls;
        }

        private static void SeLogMethod(IntPtr context, tobii_log_level_t level, string text)
        {
            _logger?.Debug($"SE-LOG: {text} ({level})");
        }
    }
}
