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
            
            string fn = Console.ReadLine();

            Console.WriteLine(FS.SetAtributes(fn));

            Console.Read();
            return 0;
        }

        
    }


   
}

