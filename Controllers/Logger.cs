using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace SignalR.Controllers
{
    public class Logger
    {
        public static void Log(string message)
        {
           /* using (FileStream fs = new FileStream(@"C:\Users\Petya\Desktop\log.txt", FileMode.Append, FileAccess.Write))
            using (StreamWriter sw = new StreamWriter(fs))
            {
                sw.WriteLine(message);
            }*/
        }
    }
}