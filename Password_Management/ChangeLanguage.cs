using Generate_Password.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Generate_Password
{
    public class ChangeLanguage
    {
        
        public static void ChangeLanguageFile(string fileName)
        {
            filePath = fileName;
        }

        static string filePath = Application.StartupPath + "\\Lang\\English.ini";

        public static string GetValueFromIniFile(string key,string section="Main")
        {
            string a = "";
            IniFile ini = new IniFile(filePath);
            a = ini.Read(section, key);
            return a;
        }
    }
}
