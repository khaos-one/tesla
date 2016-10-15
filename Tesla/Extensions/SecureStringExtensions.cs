using System;
using System.Runtime.InteropServices;
using System.Security;

namespace Tesla.Extensions {
    public static class SecureStringExtensions {
        /// <summary>
        /// Converts <see cref="SecureString"/> instance to regular string.
        /// </summary>
        /// <param name="str">Secure string to convert.</param>
        /// <returns>Regular string with the content of secure string.</returns>
        public static string ToUnsecureString(this SecureString str) {
            if (str == null) {
                throw new ArgumentNullException(nameof(str));
            }

            var unmanagedPtr = IntPtr.Zero;

            try {
                unmanagedPtr = Marshal.SecureStringToGlobalAllocUnicode(str);
                return Marshal.PtrToStringUni(unmanagedPtr);
            }
            finally {
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedPtr);
            }
        }
    }
}
