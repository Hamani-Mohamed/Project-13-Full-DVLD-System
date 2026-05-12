using DVLDBusinessLayer;
using Microsoft.Win32;
using System;
using System.Windows.Forms;

namespace DVLD
{
    internal class clsGlobalSettings
    {
        public static clsUser CurrentUser;
        public static string Username;
        public static string Password;

        public static void SaveCredentialsInRegistry(string Username, string Password)
        {
            string keyPath = @"HKEY_CURRENT_USER\SOFTWARE\DVLD";

            try
            {
                Registry.SetValue(keyPath, "Username", Username, RegistryValueKind.String);
                Registry.SetValue(keyPath, "Password", Password, RegistryValueKind.String);
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public static bool LoadCredentialsFromRegistry()
        {
            string keyPath = @"HKEY_CURRENT_USER\SOFTWARE\DVLD";

            try
            {
                string usernameValue = Registry.GetValue(keyPath, "Username", null) as string;
                string passwordValue = Registry.GetValue(keyPath, "Password", null) as string;

                if (usernameValue != null && passwordValue != null)
                {
                    Username = usernameValue;
                    Password = passwordValue;
                    return true;
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }

            return false;
        }
    }
}
