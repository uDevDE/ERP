using System;
using System.Collections.Generic;

namespace ERP.Client.Core
{
    public class LocalUserSettings
    {
        public string LastSelectedWorkDir { get; set; }
        public string LastSelectedProjectDir { get; set; }
        public string LastSelectedYear { get; set; }

        public static Dictionary<int, LocalUserSettings> UserSettings = new Dictionary<int, LocalUserSettings>();

        public static bool Exists(int employeeId) => UserSettings.ContainsKey(employeeId);

        public static LocalUserSettings Load(int employeeId)
        {
            if (UserSettings.Count == 0)
            {
                var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
                var value = localSettings.Values["LocalUserSettings"];
                if (value != null)
                {
                    if (value is Dictionary<int, LocalUserSettings> userSettings)
                    {
                        UserSettings = userSettings;
                    }
                }
            }

            if (UserSettings.TryGetValue(employeeId, out LocalUserSettings settings))
            {
                return settings;
            }

            return null;
        }

        public static bool Save(int employeeId, LocalUserSettings settings)
        {
            try
            {
                if (UserSettings.ContainsKey(employeeId))
                {
                    UserSettings[employeeId] = settings;
                }
                else
                {
                    UserSettings.Add(employeeId, settings);
                }

                var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
                localSettings.Values["LocalUserSettings"] = settings;

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}
