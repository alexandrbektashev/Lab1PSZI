using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace FileSecurityUtil
{
    public static class FS
    {

        

        public static string SetAtributes(string filename)
        {
            FileInfo fi = new FileInfo(filename);

            StringBuilder sb = new StringBuilder();

            sb.Append(string.Format("{0}, {1}, {2}",
            fi.Directory, fi.CreationTime, fi.FullName));

           // fi.SetAccessControl(System.Security.AccessControl.FileSecurity filesec)
           //очень полезная вещь - позволяет установить права доступа не используя всю эту шнягу с WinAPI

           //по сути нужно теперь только изучить как оставлять атрибуты достпуа и все 
            return sb.ToString();

        }

    }
}
