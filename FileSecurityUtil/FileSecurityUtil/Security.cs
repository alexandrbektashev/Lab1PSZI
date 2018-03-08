using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;


namespace FileSecurityUtil
{
    public static class Security
    {
        //В этом классе находится все, что имеет отношение к изменению доступа файла
        [DllImport("advapi32.dll", CharSet = CharSet.Unicode)]
        private static extern uint SetNamedSecurityInfoW(String pObjectName, SE_OBJECT_TYPE ObjectType, SECURITY_INFORMATION SecurityInfo, IntPtr psidOwner, IntPtr psidGroup, IntPtr pDacl, IntPtr pSacl);

        [DllImport("Advapi32.dll", SetLastError = true)]
        private static extern bool ConvertStringSidToSid(String StringSid, ref IntPtr Sid);

        private enum SE_OBJECT_TYPE
        {
            SE_UNKNOWN_OBJECT_TYPE = 0,
            SE_FILE_OBJECT,
            SE_SERVICE,
            SE_PRINTER,
            SE_REGISTRY_KEY,
            SE_LMSHARE,
            SE_KERNEL_OBJECT,
            SE_WINDOW_OBJECT,
            SE_DS_OBJECT,
            SE_DS_OBJECT_ALL,
            SE_PROVIDER_DEFINED_OBJECT,
            SE_WMIGUID_OBJECT, SE_REGISTRY_WOW64_32KEY
        }

        [Flags]
        private enum SECURITY_INFORMATION : uint
        {
            Owner = 0x00000001,
            Group = 0x00000002,
            Dacl = 0x00000004,
            Sacl = 0x00000008,
            ProtectedDacl = 0x80000000,
            ProtectedSacl = 0x40000000,
            UnprotectedDacl = 0x20000000,
            UnprotectedSacl = 0x10000000
        }

        public static void SetFileOrFolderOwner(String objectName) //Note this is very basic and is silent on fail as I havent checked GetlastError and thrown an exception etc
        {
            IntPtr sidPtr = IntPtr.Zero;
            SECURITY_INFORMATION sFlags = SECURITY_INFORMATION.Owner;

            System.Security.Principal.NTAccount user = new System.Security.Principal.NTAccount("P1R4T3\\Harris");
            System.Security.Principal.SecurityIdentifier sid = (System.Security.Principal.SecurityIdentifier)user.Translate(typeof(System.Security.Principal.SecurityIdentifier));

            ConvertStringSidToSid(sid.ToString(), ref sidPtr);

            SetNamedSecurityInfoW(objectName, SE_OBJECT_TYPE.SE_FILE_OBJECT, sFlags, sidPtr, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);

            //Probably should release the IntPtr here to avoid memory leakage?????

            
        }
    }
}
