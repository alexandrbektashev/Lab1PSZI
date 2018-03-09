using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace FileSecurityUtil
{
    public static class FileSecurity
    {
        //всей своей душой понадеюсь, что эта функция там
        //на самом деле, она реально в той либе
        [DllImport("advapi32.dll", CharSet = CharSet.Unicode)]
        private static extern uint SetFileSecurity(String lpFileName, SECURITY_INFORMATION SecurityInfo, PSECURITY_DESCRIPTOR pSecurityDescriptor);
        //но хрен его знает, делается это так или нет
        //интересно, что на МСДН написано, что она бул, а возвращает целочисленные значения

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
        //игра "РЕАЛИЗУЙ СВОЮ СТРУКТУРУ ДЕСКРИПТОРА, ВЕДЬ ЕЕ НЕТ НА САЙТЕ PINVOKE:)"

        [StructLayoutAttribute(LayoutKind.Sequential)]
        struct PSECURITY_DESCRIPTOR
        {
            public byte revision;
            public byte size;
            public short control;
            public IntPtr owner;
            public IntPtr group;
            public IntPtr sacl;
            public IntPtr dacl;
        }
        //вообще не алё че с этим теперь делать



    }
}
