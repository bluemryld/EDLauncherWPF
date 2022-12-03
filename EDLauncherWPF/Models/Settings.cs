using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDLauncherWPF.Models
{
    public class Settings : ObservableObject
    {
        private class Setting
        {
            [JsonProperty] public static bool DarkMode { get; set; } = false;
            [JsonProperty] public static string DefaultProfileName { get; set; } = string.Empty;
            [JsonProperty] public static string AutoLaunchProfile { get; set; } = string.Empty;                        
        }

        private static Setting setting = new();

        // setup some variables
        public static readonly string SettingsFilePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Elite Add On Helper wpf\\";
        public static readonly string SettingsFile = SettingsFilePath + "settings.json";
        private static bool SettingsLoaded = false;
        public static string DefaultProfile
        {
            get
            {
                return Setting.DefaultProfileName;
            }
            set
            {
                Setting.DefaultProfileName = value;
            }
        }

        public Settings()
        {
            //if there are no addons loaded attempt to load
            if (!SettingsLoaded)
            {
                SettingsLoaded = LoadSettings();
            }
        }

        public bool LoadSettings()
        {
            // load all the settings file
            if (!File.Exists(SettingsFile))
            {
                //There is no settings file so attempt to save the default settings
                SaveSettings();
            }
            var Json = File.ReadAllText(SettingsFile);
            try
            {
                setting = JsonConvert.DeserializeObject<Setting>(Json, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Objects,
                    TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple
                });
            }
            catch
            {
                //oops something went wrong
                return false;
            }
            SettingsLoaded= true;
            return true;
        }

        internal static bool SaveSettings()
        {
            var Json = JsonConvert.SerializeObject(setting, Formatting.Indented);
            //    , new JsonSerializerSettings
            //{
            //    TypeNameHandling = TypeNameHandling.Objects,
            //    TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple
            //});
            
            //look for settings path, create if needed
            if (!Path.Exists(SettingsFilePath))
            {
                try
                {
                    System.IO.Directory.CreateDirectory(SettingsFilePath);

                }
                catch (System.IO.DirectoryNotFoundException)
                {
                    //Error code goes here
                    return false;
                }
            }
            try
            {
                File.WriteAllText(SettingsFile, Json);
            }
            catch
            {
                return false; 
            }
            SettingsLoaded= true;
            return true;
        }

    }
}
