using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace EDLauncherWPF.Models
{
    public class Profiles : ObservableObject
    {
        private static Dictionary<string, Profile> _profiles = new();
        private static string ProfilesFilePath = Settings.SettingsFilePath;
        private static string ProfilesFile = Settings.SettingsFilePath + "profiles.json";

        //todo: add validation
        public static string CurrentProfile { get; set; }

        public static string Description
        {
            get
            {
                return _profiles[CurrentProfile].Description;
            }
            set => _profiles[CurrentProfile].Description = value;
        }

        public static string Name
        {
            get
            {
                return _profiles[CurrentProfile].Name;
            }
            set
            {
                _profiles[CurrentProfile].Name = value;
                CurrentProfile= value;
            }
        }

        public static string GameName
        {
            get
            {
                return _profiles[CurrentProfile].GameName;
            }
            set => _profiles[CurrentProfile].GameName = value;
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
                    _profiles = JsonConvert.DeserializeObject<Dictionary<string, Profile>>(Json2);
                    //    , new JsonSerializerSettings
                    //{
                    //    TypeNameHandling = TypeNameHandling.Objects,
                    //    TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple
                    //});
                    if (_profiles.ContainsKey(Settings.DefaultProfile))
                    {
                        CurrentProfile = Settings.DefaultProfile;
                    }
                    else
                    {
                        var firstElement = _profiles.FirstOrDefault();
                        CurrentProfile = firstElement.Key;
                    }
                    _profiles[CurrentProfile].AddAllAddons();
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

        private bool Add(string name, string description)
        {
            if (!_profiles.ContainsKey(name))
            {
                _profiles.Add(name, new Profile(name, description));
                _profiles[name].AddAllAddons();
                SaveProfiles();
                return true;
            }
            return false;
        }
        
        private bool SaveProfiles()
        {
            var Json = JsonConvert.SerializeObject(_profiles, Formatting.Indented);
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

        public class Profile : ObservableObject
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public string GameName { get; set; }
            public List<ProfileAddon> ProfileAddOns =new();

            public class ProfileAddon : ObservableObject
            {
                public string Name = string.Empty;
                public bool Enabled;

                public ProfileAddon(string name, bool enabled)
                {
                    Name = name;
                    Enabled = enabled;
                }
            }

            public bool AddAllAddons()
            {
                Addons _addons = new();
                foreach (Addons.AddOn a in _addons.GetAddonsList())
                {
                    AddProfileAddon(a.FriendlyName, a.EnabledDefault);
                }
                return false;
            }
                        
            public bool AddProfileAddon(string name, bool enabled)
            {
                if(!ProfileAddonExists(name))
                { 
                    ProfileAddOns.Add(new ProfileAddon(name, enabled));
                    return true;
                }
                return false;
            }

            public bool ProfileAddonExists(string name)
            {
                foreach (ProfileAddon pa in ProfileAddOns)
                {
                    if(pa.Name == name) return true;
                }
                return false;
            }

            public Profile() { }

            public Profile(string name)
            {
                Name = name;
                Description = name;
                GameName = string.Empty;
                ProfileAddOns = new List<ProfileAddon>();
            }

            public Profile(string name, string description) : this(name)
            {
                Name= name;
                Description = description;
            }

            public IEnumerable<ProfileAddon> GetProfileAddonsList()
            {
                return ProfileAddOns; 
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="addOn"></param>
            //public void AddAddon(AddOn addOn)
            //{
            //    _addons.AddAddon(addOn);
            //}
            //public void AddAddons(List<AddOn> incomingAddons)
            //{
            //    _addons.AddAddons(incomingAddons);
            //}
        }
    }
}
