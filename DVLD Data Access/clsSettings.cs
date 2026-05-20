using System;
using System.Diagnostics;

namespace DVLDDataAccess
{
    public static class clsSettings
    {
        // ConnectionString moved to App.Config
        
        public static void LogError(Exception ex)
        {
            string SourceName = "DVLD";

            if (!EventLog.SourceExists(SourceName))
            {
                EventLog.CreateEventSource(SourceName, "Application");
            }
            EventLog.WriteEntry(SourceName, ex.Message, EventLogEntryType.Error);
        }
    }
}
