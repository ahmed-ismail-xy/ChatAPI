using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ChatAPI.Application.FilesHandler
{
    public static class CredentialManager
    {

        [DllImport("Advapi32.dll", EntryPoint = "CredReadW", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern bool CredRead(string target, CRED_TYPE type, int reservedFlag, out IntPtr CredentialPtr);

        [DllImport("Advapi32.dll", EntryPoint = "CredWriteW", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern bool CredWrite([In] ref NativeCredential userCredential, [In] uint flags);

        [DllImport("Advapi32.dll", EntryPoint = "CredFree", SetLastError = true)]
        private static extern bool CredFree([In] IntPtr cred);

        public enum CRED_TYPE : uint
        {
            GENERIC = 1U,
            DOMAIN_PASSWORD = 2U,
            DOMAIN_CERTIFICATE = 3U,
            DOMAIN_VISIBLE_PASSWORD = 4U,
            GENERIC_CERTIFICATE = 5U,
            DOMAIN_EXTENDED = 6U,
            MAXIMUM = 7U,      // Maximum supported cred type
            //MAXIMUM_EX = MAXIMUM + 1000L  // Allow new applications to run on old OSes
        }
        public enum CRED_PERSIST : uint
        {
            SESSION = 1U,
            LOCAL_MACHINE = 2U,
            ENTERPRISE = 3U
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        private partial struct NativeCredential
        {
            public uint Flags;
            public CRED_TYPE Type;
            public IntPtr TargetName;
            public IntPtr Comment;
            public System.Runtime.InteropServices.ComTypes.FILETIME LastWritten;
            public uint CredentialBlobSize;
            public IntPtr CredentialBlob;
            public uint Persist;
            public uint AttributeCount;
            public IntPtr Attributes;
            public IntPtr TargetAlias;
            public IntPtr UserName;

            /// <summary>
            /// This method derives a NativeCredential instance from a given Credential instance.
            /// </summary>
            /// <paramname="cred">The managed Credential counterpart containing data to be stored.</param>
            /// <returns>A NativeCredential instance that is derived from the given Credential
            /// instance.</returns>
            internal static NativeCredential GetNativeCredential(Credential cred)
            {
                var ncred = new NativeCredential();
                ncred.AttributeCount = 0U;
                ncred.Attributes = IntPtr.Zero;
                ncred.Comment = IntPtr.Zero;
                ncred.TargetAlias = IntPtr.Zero;
                ncred.Type = cred.Type;
                ncred.Persist = (uint)cred.Persist;
                ncred.CredentialBlobSize = cred.CredentialBlobSize;
                ncred.TargetName = Marshal.StringToCoTaskMemUni(cred.TargetName);
                ncred.CredentialBlob = Marshal.StringToCoTaskMemUni(cred.CredentialBlob);
                ncred.UserName = Marshal.StringToCoTaskMemUni(cred.UserName);
                return ncred;
            }
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        private partial struct Credential
        {
            public uint Flags;
            public CRED_TYPE Type;
            public string TargetName;
            public string Comment;
            public System.Runtime.InteropServices.ComTypes.FILETIME LastWritten;
            public uint CredentialBlobSize;
            public string CredentialBlob;
            public CRED_PERSIST Persist;
            public uint AttributeCount;
            public IntPtr Attributes;
            public string TargetAlias;
            public string UserName;
        }

        #region Critical Handle Type definition
        internal sealed partial class CriticalCredentialHandle : CriticalHandleZeroOrMinusOneIsInvalid
        {
            // Set the handle.
            internal CriticalCredentialHandle(IntPtr preexistingHandle)
            {
                SetHandle(preexistingHandle);
            }


            // Perform any specific actions to release the handle in the ReleaseHandle method.
            // Often, you need to use Pinvoke to make a call into the Win32 API to release the 
            // handle. In this case, however, we can use the Marshal class to release the unmanaged memory.

            protected override bool ReleaseHandle()
            {
                // If the handle was set, free it. Return success.
                if (!IsInvalid)
                {
                    // NOTE: We should also ZERO out the memory allocated to the handle, before free'ing it
                    // so there are no traces of the sensitive data left in memory.
                    CredFree(handle);
                    // Mark the handle as invalid for future users.
                    SetHandleAsInvalid();
                    return true;
                }
                // Return false. 
                return false;
            }
        }
        #endregion

        public static int WriteCred()
        {
            // Validations.
            string password = "1234";
            string userName = "10";
            string ip = "172.16.8.45";

            var byteArray = Encoding.Unicode.GetBytes(password);
            if (byteArray.Length > 1024)
                throw new ArgumentOutOfRangeException("The secret message has exceeded 1024 bytes.");

            // Go ahead with what we have are stuff it into the CredMan structures.
            var cred = new Credential();
            cred.TargetName = ip;
            cred.UserName = userName;
            cred.CredentialBlob = password;
            cred.CredentialBlobSize = (uint)Encoding.Unicode.GetBytes(password).Length;
            cred.AttributeCount = 0U;
            cred.Attributes = IntPtr.Zero;
            cred.Comment = null;
            cred.TargetAlias = null;
            cred.Type = CRED_TYPE.DOMAIN_PASSWORD;
            cred.Persist = CRED_PERSIST.ENTERPRISE;
            var ncred = NativeCredential.GetNativeCredential(cred);
            // Write the info into the CredMan storage.
            bool written = CredWrite(ref ncred, 0U);
            int lastError = Marshal.GetLastWin32Error();
            if (written)
            {
                return 0;
            }
            else
            {
                string message = string.Format("CredWrite failed with the error code {0}.", lastError);
                throw new Exception(message);
            }
        }
    }
}
