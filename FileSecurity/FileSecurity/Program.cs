using System;
using System.Security.AccessControl;
using System.IO;

namespace FileSecurityChanger
{
    class Program
    {
        static void Main(string[] args)
        {
            const string fileconf = "rights.txt";
            //Программа изменяет список прав доступа (ACL) на файлы
            //Записывает список правил доступа в файл rights.txt

            //This program changes access rules (ACL) of files 
            //Writes list of rules into file rights.txt

            try
            {
                StreamReader sr;
                StreamWriter sw;
                string pswrd;
                if (!File.Exists(fileconf))
                {
                    Console.WriteLine("The password isn't set.");
                    Console.WriteLine("Enter the new password");
                    pswrd = Console.ReadLine();
                    sw = new StreamWriter(fileconf);
                    sw.WriteLine(pswrd.GetHashCode());
                    sw.Close();

                }
                else
                {
                    Console.WriteLine("Enter the password");
                    pswrd = Console.ReadLine();
                }
                sr = new StreamReader(fileconf);
                if (sr.ReadLine() != pswrd.GetHashCode().ToString()) throw new Exception("Wrong password.");

                Console.WriteLine("Enter filename:");
                string filename = Console.ReadLine();
                
                //Displays current list of rules
                ShowInfo(filename);

                //Chooses method
                int key = -1;
                while (key != 0)
                {
                    Console.WriteLine("\n" +
                        "1 - Add file security rule\n" +
                        "2 - Remove file security rule\n" +
                        "0 - exit");
                    key = Int32.Parse(Console.ReadLine());
                   

                    switch (key)
                    {
                        case 1: 
                            ChangeFileSecurityRule(filename, 1); 
                            //second parameter defines add or remove rule
                            break;
                        case 2:
                            ChangeFileSecurityRule(filename, 2);
                            break;
                    }


                    ShowInfo(filename);
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            Console.Read();
        }

        public static void ChangeFileSecurityRule(string FileName, int controlTypeCode)
        {
            //get the user's info
            string currun = Environment.UserName;
            string currudn = Environment.UserDomainName;

            //Choose account to use
            Console.WriteLine(
                "Enter user domain name and user access account\n" +
                "Example: @{0}\\{1}\n(for current account don't put anything)", currudn, currun);

            string account = Console.ReadLine();

            account = (account == "") ? string.Format(@"{0}\{1}", currudn, currun) : account;
            Console.WriteLine("Choose rule:\n" +
                "1 - Read\n2 - Write\n3 - Modify\n4 - FullControl\n5 - Synchronize\n6 - Delete");
            int key = Int32.Parse(Console.ReadLine());


            //Create rights object
            FileSystemRights fsr = new FileSystemRights();
            //and chooses the type of them
            switch (key)
            {
                case 1:
                    fsr = FileSystemRights.Read;
                    break;
                case 2:
                    fsr = FileSystemRights.Write;
                    break;
                case 3:
                    fsr = FileSystemRights.Modify;
                    break;
                case 4:
                    fsr = FileSystemRights.FullControl;
                    break;
                case 5:
                    fsr = FileSystemRights.Synchronize;
                    break;
                case 6:
                    fsr = FileSystemRights.Delete;
                    break;
                default:
                    throw new Exception("Wrong input");
            }

            
            Console.WriteLine("Allow or deny?(put a/d)");
            string str = Console.ReadLine();
            if ((str != "a") && (str != "d")) throw new Exception("Wrong input");

            // Choose control type allow or deny
            AccessControlType act = (str == "a") ? AccessControlType.Allow : AccessControlType.Deny;

            // Create fileinfo object
            FileInfo fInfo = new FileInfo(FileName);
            // Get a FileSecurity object that represents the  current security settings
            FileSecurity fSecurity = fInfo.GetAccessControl();

            // Add the FileSystemAccessRule to the security settings
            if (controlTypeCode == 1)
                fSecurity.AddAccessRule(new FileSystemAccessRule(account, fsr, act));
            if (controlTypeCode == 2)
                fSecurity.RemoveAccessRule(new FileSystemAccessRule(account, fsr, act));

            // Set the new access settings
            fInfo.SetAccessControl(fSecurity);
            
        }



        public static void ShowInfo(string filename)
        {
            //This method displays list of rules

            //create fileInfo object
            FileInfo fInfo = new FileInfo(filename);
            //Get security info
            FileSecurity fSecurity = fInfo.GetAccessControl();
            //Get the collection of rules
            fSecurity.GetAccessRules(true, true, typeof(System.Security.Principal.NTAccount));

            Console.WriteLine("\nCurrent ACL for the {0}:", filename);
            Console.WriteLine("SID\tRights\tControlType\tSource");
            //write each to the console
            foreach (FileSystemAccessRule fsar in fSecurity.GetAccessRules(true, true, typeof(System.Security.Principal.NTAccount)))
                Console.WriteLine("{0}\t{1}\t{2}\t{3}",
                    fsar.IdentityReference.Value,
                    fsar.FileSystemRights.ToString(),
                    fsar.AccessControlType.ToString(),
                    fsar.IsInherited ? "Inherited" : "Explicit");

        }
    }
}
