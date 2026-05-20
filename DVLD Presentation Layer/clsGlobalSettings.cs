using DVLDBusinessLayer;
using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace DVLD
{
    public static class clsGlobalSettings
    {
        public static clsUser CurrentUser;
        public static string Username;
        public static string Password;

        public static void LogError(Exception ex)
        {
            string SourceName = "DVLD";

            if (!EventLog.SourceExists(SourceName))
            {
                EventLog.CreateEventSource(SourceName, "Application");
            }
            EventLog.WriteEntry(SourceName, ex.Message, EventLogEntryType.Error);
        }

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
                LogError(ex);
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
                LogError(ex);

                MessageBox.Show(ex.Message);
                return false;
            }

            return false;
        }
    }
}
