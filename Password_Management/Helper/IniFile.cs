using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Generate_Password.Helper
{
    public class IniFile
    {
        public string Path;

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        public IniFile(string path)
        {
            Path = path;
        }

        public string Read(string section, string key)
        {
            var retVal = new StringBuilder(255);
            GetPrivateProfileString(section, key, "", retVal, 255, Path);
            return retVal.ToString();
        }

        public void Write(string section, string key, string value)
        {
            WritePrivateProfileString(section, key, value, Path);
        }

        public void DeleteKey(string section, string key)
        {
            WritePrivateProfileString(section, key, null, Path);
        }

        public void DeleteSection(string section)
        {
            WritePrivateProfileString(section, null, null, Path);
        }
    }
}
