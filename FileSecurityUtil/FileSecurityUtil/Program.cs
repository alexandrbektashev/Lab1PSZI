using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices; //Подключаем библиотеку для импорта и работы с WinApi

namespace FileSecurityUtil
{
    class Program
    {
        static int Main(string[] args)
        {
            IntPtr ptr = WinAPI.FindWindow(null, "Документы");
            Console.WriteLine(ptr.ToString());
            IntPtr child;
            StringBuilder title = new StringBuilder();
            if (ptr.ToInt32() != 0)
            {
                child = WinAPI.GetWindow(ptr, WinAPI.GetWindow_Cmd.GW_CHILD);
                Console.WriteLine(child.ToString());
                WinAPI.SendMessage(child, Convert.ToInt32(WinAPI.GetWindow_Cmd.WM_GETTEXT), (IntPtr)20, title);
                Console.WriteLine(title.ToString());
            }

            string fn = Console.ReadLine();
            Security.SetFileOrFolderOwner(fn);

            Console.Read();
            return 0;
        }

        
    }


   
}

