using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.Configuration;

namespace PM3UniversalGUI
{



    static class Program
    {
        //@"C:\Dist\pm3-bin-v3-official\win32\proxmark3.exe"; 
        //@"C:\Dist\pm3-bin-v3_20180711\win32\proxmark3.exe"
        public static PM3Client PM3 = new PM3Client(ConfigurationManager.AppSettings["PM3ClientFilename"]);
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
