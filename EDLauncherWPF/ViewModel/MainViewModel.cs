using CommunityToolkit.Mvvm.ComponentModel;
using EDLauncherWPF.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDLauncherWPF.ViewModel
{
    public class MainViewModel : ObservableObject
    {
        private Profile currentProfile = new Profile("Default");

        // setup some variables
        static readonly string settingsFilePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Elite Add On Helper wpf\\";
        // Current Objects


        public MainViewModel()
        {
            Load_addons();
        }


        public string ProfileName
        {
            get => currentProfile.Name;
            set => SetProperty(currentProfile.Name, value, currentProfile, (u, n) => u.Name = n);
        }

        
        public IEnumerable<AddOn> ListAddons
        {
            get => currentProfile.GetAddonsList();

         }




        /// <summary>
        /// Loads preferences from addons.json in settings folder
        /// if not found copies defautl file to folder
        /// </summary>
        private void Load_addons()                                       //load preferences
        {
            //look for settings path, create if needed
            updateMyStatus("Checking Folder Exists");
            if (!Path.Exists(settingsFilePath))
            {
                updateMyStatus("Path Not Found");
                try
                {
                    updateMyStatus("Trying to create");
                    System.IO.Directory.CreateDirectory(settingsFilePath);

                }
                catch (System.IO.DirectoryNotFoundException)
                {
                    //Error code goes here
                }
            }
            // load all the textboxes with values from settings file
            updateMyStatus("Checking file exists");
            if (!File.Exists(settingsFilePath + "AddOns.json"))
            {
                updateMyStatus("File not found");
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

        private void updateMyStatus(string status)                      // function to update the status bar
        {

            //toolStripStatusLabel1.Text = status;
            Debug.WriteLine(status);
        }

        
    }
}
