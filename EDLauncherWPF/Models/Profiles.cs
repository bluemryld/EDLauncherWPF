using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace EDLauncherWPF.Models
{
    public class Profiles
    {
        private static string ProfilesFilePath = Settings.SettingsFilePath;
        private static string ProfilesFile = Settings.SettingsFilePath + "profiles.json";
                
        public static Dictionary<string, Profile> AllProfiles = new();
       
        private static string currentProfile = string.Empty;
        public string CurrentProfile
        {
            //TODO: add validation
            get { return currentProfile; }
            set { currentProfile = value; }
        }
        public string Name
        {
            get { return AllProfiles[currentProfile].Name; }
            set 
            {
                AllProfiles[currentProfile].Name = value;
                currentProfile = value;
            }
        }        
        public string Description
        {
            get { return AllProfiles[currentProfile].Description; }
            set => AllProfiles[currentProfile].Description = value; 
        }
        public static string GameName
        {
            get { return AllProfiles[currentProfile].GameName; }
            set => AllProfiles[currentProfile].GameName = value;
        }
        public static string GamePath
        {
            get { return AllProfiles[currentProfile].GamePath; }
            set => AllProfiles[currentProfile].GamePath = value;
        }
        public Profiles()
        {
            LoadProfiles();
        }

        private bool LoadProfiles()
        {
            {
                // load the file
                if (!File.Exists(ProfilesFile))
                {
                    //There is no file so attempt to create a default
                    Add("Default", "Automatically Created Profile");
                    Settings.DefaultProfile = "Default";
                    CurrentProfile= "Default";
                    SaveProfiles();
                }
                var Json2 = File.ReadAllText(ProfilesFile);
                try
                {
                    AllProfiles = JsonConvert.DeserializeObject<Dictionary<string, Profile>>(Json2);
                    //    , new JsonSerializerSettings
                    //{
                    //    TypeNameHandling = TypeNameHandling.Objects,
                    //    TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple
                    //});
                    if (AllProfiles.ContainsKey(Settings.DefaultProfile))
                    {
                        CurrentProfile = Settings.DefaultProfile;
                    }
                    else
                    {
                        var firstElement = AllProfiles.FirstOrDefault();
                        CurrentProfile = firstElement.Key;
                    }
                    AllProfiles[CurrentProfile].AddAllAddons();
                    SaveProfiles();
                 }
                catch
                {
                    //oops something went wrong
                    return false;
                }
                return true;
            }
        }

        public bool SaveProfiles()
        {
            var Json = JsonConvert.SerializeObject(AllProfiles, Formatting.Indented);
            //    , new JsonSerializerSettings
            //{
            //    TypeNameHandling = TypeNameHandling.Objects,
            //    TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple
            //});

            //look for settings path, create if needed
            if (!System.IO.Path.Exists(ProfilesFilePath))
            {
                try
                {
                    System.IO.Directory.CreateDirectory(ProfilesFilePath);

                }
                catch (System.IO.DirectoryNotFoundException)
                {
                    //Error code goes here
                    return false;
                }
            }
            try
            {
                File.WriteAllText(ProfilesFile, Json);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool Add(string name, string description)
        {
            if (name=="")
            {
                name = "New";

            }

            byte i = 0;
            string nametofind = name;

            while (AllProfiles.ContainsKey(nametofind) && i <255)
            {
                i++;
                nametofind = name + " #" + i.ToString();
            }
            if (i == 255) return false;
            AllProfiles.Add(nametofind, new Profile(nametofind, description));
            AllProfiles[nametofind].AddAllAddons();
            SaveProfiles();
            currentProfile = nametofind;
            return true;
        }
        public bool Remove()
        {
            //TODO: Validation
            if (AllProfiles.Count > 1)
            {
                AllProfiles.Remove(currentProfile);
                currentProfile = AllProfiles.FirstOrDefault().Key;
                SaveProfiles();
                return true;
            }
            else return false;
            
        }
        
        public List<CurrentProfileAddon> GetCurrentAddons()
        {
            Addons _addons = new();
            List<CurrentProfileAddon> output = new(); 
            CurrentProfileAddon currentProfileAddon= new();
            foreach (Addons.AddOn a in _addons.GetAddonsList())
            {
                currentProfileAddon = new();
                currentProfileAddon.FriendlyName= a.FriendlyName;
                currentProfileAddon.Url= a.Url;
                currentProfileAddon.AutoDiscoverPath= a.AutoDiscoverPath;
                currentProfileAddon.ExecutableName= a.ExecutableName;
                currentProfileAddon.WebApp= a.WebApp;
                currentProfileAddon.Installable= a.Installable;
                currentProfileAddon.Scripts= a.Scripts;
                currentProfileAddon.ProgramDirectory= a.ProgramDirectory;
                currentProfileAddon.Running= a.Running;
                //currentProfileAddon.Enabled= _profiles[CurrentProfile].ProfileAddOns.
                output.Add(currentProfileAddon);
            }
            return output;
        }

        public class CurrentProfileAddon : Addons.AddOn
        {
            public bool Enabled;
            public CurrentProfileAddon()
            { }
        } 

        
    }
}
