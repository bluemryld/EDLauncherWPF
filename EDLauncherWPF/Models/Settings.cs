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
    class Settings
    {
        class Setting
        {
            static public bool DarkMode { get; set; } = false;
            static private bool SettingsLoaded = false;

        }
        
        // setup some variables
        static readonly string SettingsFilePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Elite Add On Helper wpf\\";
        static readonly string SettingsFile = SettingsFilePath + "settings.json";

        public Settings()
        {
            //if there are no addons loaded attempt to load
            if (!SettingsLoaded)
            {
                LoadSettings();
            }
        }

        public bool LoadSettings()
        {
            //look for settings path, create if needed
            if (!Path.Exists(settingsFilePath))
            {
                try
                {
                    System.IO.Directory.CreateDirectory(settingsFilePath);

                }
                catch (System.IO.DirectoryNotFoundException)
                {
                    //Error code goes here
                    return false;
                }
            }
            // load all the settings file
            if (!File.Exists(settingsFilePath + "AddOns.json"))
            {
                // lets copy the default addons.json to the settings path..
                // probably want to remove this and do the file copy in an installer..
                // string defaultpath = AppDomain.CurrentDomain.BaseDirectory;
                string startupPath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName, "addons.json");
                string sourceFile = startupPath;
                string destinationFile = settingsFilePath + "AddOns.json";
                try
                {
                    File.Copy(sourceFile, destinationFile, true);
                    updateMyStatus("Settings copied");
                    updateMyStatus("Loading Settings");
                }
                catch (IOException iox)
                {
                    // Console.WriteLine(iox.Message);
                    updateMyStatus("Settings error");
                }
            }
            currentProfile.AddAddons(DeserializeAddOns());
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="addOns"></param>
        internal static void SerializeAddons(object addOns)
        {
            var Json = JsonConvert.SerializeObject(addOns, Formatting.Indented, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Objects,
                TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple
            });

            File.WriteAllText(settingsFilePath + "AddOns.json", Json);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal static List<AddOn> DeserializeAddOns()   //read settings to json and load into objects
        {
            var Json = File.ReadAllText(settingsFilePath + "AddOns.json");
            try
            {
                return JsonConvert.DeserializeObject<List<AddOn>>(Json, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Objects,
                    TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple
                });
            }
            catch
            {
                //oops something went wrong
                //updatemystatus("Prefs file corrupt, please delete and re run");
                return null;
            }
        }

    }
}
