using Generate_Password.Enums;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace Generate_Password
{
    [Serializable]
    public class PasswordInfo
    {
        public string ItemName { get; set; }
        public long ItemIndex { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string URL { get; set; }
        public string Note { get; set; }
        public string Group { get; set; }
        public DateTime LastModified { get; set; }
        public long PasswordLength { get; set; }


    }

    [Serializable]
    public class User
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public List<PasswordInfo> Passwords { get; set; } = new List<PasswordInfo>();
    }

    public class PasswordManager
    {
        private Dictionary<string, User> users = new Dictionary<string, User>();
        private string fileName = Application.StartupPath + "\\DataBase\\UsersDatLog.dat";

        static PasswordManager _instance = null;
        public static PasswordManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new PasswordManager();
                }
                return _instance;
            }
        }

        public PasswordManager()
        {
            _instance = this;
            if (!Directory.Exists(Application.StartupPath + "\\DataBase"))
            {
                Directory.CreateDirectory(Application.StartupPath + "\\DataBase");
            }
            ReadEncryptedBinaryFile(fileName);
        }

        public bool AddUser(string userName, string password)
        {
            if (users.ContainsKey(userName)) return false;
            users[userName] = new User { UserName = userName, Password = password };
            //SaveUsers();
            WriteEncryptedBinaryFile(fileName);
            return true;
        }

        public void DeletePassword(string userName, long itemIndex)
        {
            if (users.ContainsKey(userName))
            {
                users[userName].Passwords.RemoveAll(x => x.ItemIndex == itemIndex);
                WriteEncryptedBinaryFile(fileName);
            }
        }

        public void ChangePassword(string userName, int itemIndex, PasswordInfo passwordInfo)
        {
            if (users.ContainsKey(userName))
            {
                users[userName].Passwords[itemIndex] = passwordInfo;
            }

        }

        public User AuthenticateUser(string userName, string password)
        {
            if (users.ContainsKey(userName) && users[userName].Password == password)
            {
                return users[userName];
            }
            return null;
        }

        public List<User> GetUsers()
        {
            return users.Values.ToList();
        }

        public void SaveData()
        {
            WriteEncryptedBinaryFile(fileName);
        }

        //private void SaveUsers()
        //{
        //    using (FileStream fs = new FileStream(filePath, FileMode.Create))
        //    {
        //        BinaryFormatter formatter = new BinaryFormatter();
        //        formatter.Serialize(fs, users);
        //    }
        //}

        //private void LoadUsers()
        //{
        //    if (File.Exists(filePath))
        //    {
        //        using (FileStream fs = new FileStream(filePath, FileMode.Open))
        //        {
        //            BinaryFormatter formatter = new BinaryFormatter();
        //            users = (Dictionary<string, User>)formatter.Deserialize(fs);
        //        }
        //    }
        //}

        public void ReadEncryptedBinaryFile(string fileName, string password = "password")
        {
            if (File.Exists(fileName))
            {
                byte[] key, iv;

                // Generate key and IV from password
                using (var aes = Aes.Create())
                {
                    var keyGen = new Rfc2898DeriveBytes(password, Encoding.UTF8.GetBytes("SaltValue"), 1000);
                    key = keyGen.GetBytes(aes.KeySize / 8);
                    iv = keyGen.GetBytes(aes.BlockSize / 8);
                }

                using (FileStream fs = new FileStream(fileName, FileMode.Open))
                using (Aes aes = Aes.Create())
                using (CryptoStream cs = new CryptoStream(fs, aes.CreateDecryptor(key, iv), CryptoStreamMode.Read))
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    users = (Dictionary<string, User>)bf.Deserialize(cs);
                }
            }
        }

        public void WriteEncryptedBinaryFile(string fileName, string password = "password")
        {
            byte[] key, iv;

            // Generate key and IV from password
            using (var aes = Aes.Create())
            {
                var keyGen = new Rfc2898DeriveBytes(password, Encoding.UTF8.GetBytes("SaltValue"), 1000);
                key = keyGen.GetBytes(aes.KeySize / 8);
                iv = keyGen.GetBytes(aes.BlockSize / 8);
            }

            using (FileStream fs = new FileStream(fileName, FileMode.Create))
            using (Aes aes = Aes.Create())
            using (CryptoStream cs = new CryptoStream(fs, aes.CreateEncryptor(key, iv), CryptoStreamMode.Write))
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(cs, users);
            }
        }
    }

    public class PaswordUtil
    {
        public static PasswordSecureLevel EvaluatePassword(string password)
        {
            int score = 0;

            // Length points
            if (password.Length >= 8) score++;
            if (password.Length >= 12) score++;

            // Character variety points
            if (System.Text.RegularExpressions.Regex.IsMatch(password, @"\d")) score++; // Contains digits
            if (System.Text.RegularExpressions.Regex.IsMatch(password, @"[a-z]")) score++; // Contains lowercase
            if (System.Text.RegularExpressions.Regex.IsMatch(password, @"[A-Z]")) score++; // Contains uppercase
            if (System.Text.RegularExpressions.Regex.IsMatch(password, @"[\W_]")) score++; // Contains special characters

            // Determine password secure level
            if (score >= 5)
                return PasswordSecureLevel.Safe;
            else if (score >= 3)
                return PasswordSecureLevel.Normal;
            else
                return PasswordSecureLevel.Danger;
        }

        public static string ConvertEnumsToSecureString(PasswordSecureLevel passwordSecureLevel)
        {
            switch (passwordSecureLevel)
            {
                case PasswordSecureLevel.Danger:
                    return "Danger";
                case PasswordSecureLevel.Normal:
                    return "Normal";
                case PasswordSecureLevel.Safe:
                    return "Safe";
                default:
                    return "Unknown";
            }
        }

        public static Color ConvertEnumsToColor(PasswordSecureLevel passwordSecureLevel)
        {
            switch (passwordSecureLevel)
            {
                case PasswordSecureLevel.Danger:
                    return Color.Red;
                case PasswordSecureLevel.Normal:
                    return Color.Orange;
                case PasswordSecureLevel.Safe:
                    return Color.FromArgb(0, 168, 148);
                default:
                    return Color.Red;
            }
        }
    }
}
