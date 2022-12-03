using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace EDLauncherWPF.Models
{
    public class Addons : ObservableObject
    {
        //the list of addons we know about
        private static List<AddOn> _addons = new();

        private static string AddonsFilePath = Settings.SettingsFilePath;
        private static string AddonsFile = Settings.SettingsFilePath + "addons.json";

        public Addons()
        {
            //if there are no addons loaded attempt to load them
            if (_addons.Count == 0)
            {
                LoadAddons();
            }
        }

        //TODO:Delete Addon


        public bool LoadAddons()
        {
            // load the AddOns file
            if (!File.Exists(AddonsFile))
            {
                //There is no settings file so attempt to load the default addons from the app folder
                var Json = File.ReadAllText(Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName, "addons.json"));
                try
                {
                    //TODO: add some validation - maybe convert to temp variable?
                    _addons = JsonConvert.DeserializeObject<List<AddOn>>(Json, new JsonSerializerSettings
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

                SaveAddons();
            }
            var Json2 = File.ReadAllText(AddonsFile);
            try
            {
                _addons = JsonConvert.DeserializeObject<List<AddOn>>(Json2, new JsonSerializerSettings
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
            return true;

        }

        internal static bool SaveAddons()
        {
            var Json = JsonConvert.SerializeObject(_addons, Formatting.Indented);
            //    , new JsonSerializerSettings
            //{
            //    TypeNameHandling = TypeNameHandling.Objects,
            //    TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple
            //});

            //look for settings path, create if needed
            if (!Path.Exists(AddonsFilePath))
            {
                try
                {
                    System.IO.Directory.CreateDirectory(AddonsFilePath);

                }
                catch (System.IO.DirectoryNotFoundException)
                {
                    //Error code goes here
                    return false;
                }
            }
            try
            {
                File.WriteAllText(AddonsFile, Json);
            }
            catch
            {
                return false;
            }
            return true;
        }


        public IEnumerable<AddOn> GetAddonsList()
        {
            return _addons;
        }

        public void AddAddon(AddOn addon)
        {
            //foreach (AddOn existingAddon in _addons)
            //{
            //    if(existingAddon.Conflicts(addon))
            //    {
            //        throw new AddonConflictException(existingAddon, addon);
            //    }
            //}


            _addons.Add(addon);
        }

        public void AddAddons(List<AddOn> incomingAddons)
        {
            foreach (AddOn newAddon in incomingAddons)
            {
                AddAddon(newAddon);
            }
        }

        public class AddOn
        {
            

            public AddOn(string friendlyName, bool enabled, string programDirectory, string executableName, bool installable, string autoDiscoverPath, string scripts, string url, string webApp)
            {
                FriendlyName = friendlyName;
                EnabledDefault = enabled;
                ProgramDirectory = programDirectory;
                ExecutableName = executableName;
                Installable = installable;
                AutoDiscoverPath = autoDiscoverPath;
                Scripts = scripts;
                Url = url;
                WebApp = webApp;
            }

            public AddOn()
            {
                
            }

            public AddOn(string friendlyName)
            {
                FriendlyName = friendlyName;
            }

            /// <summary>
            /// Human readable name for displaying on screen etc
            /// </summary>
            public string FriendlyName { get; set; } = string.Empty;
            /// <summary>
            /// Add on for if default enabled (box checked)
            /// </summary>
            public bool EnabledDefault { get; set; } = false;
            /// <summary>
            /// String to file location on computer
            /// </summary>
            public string ProgramDirectory { get; set; } = string.Empty;
            /// <summary>
            /// Name of the executable
            /// </summary>
            public string ExecutableName { get; set; } = string.Empty;
            /// <summary>
            /// If install button should show
            /// </summary>
            public bool Installable { get; set; } = false;
            /// <summary>
            /// Path for auto discovery.
            /// </summary>
            public string AutoDiscoverPath { get; set; } = string.Empty;
            /// <summary>
            /// Path for extras e.g. TARGET scripts.
            /// </summary>
            public string Scripts { get; set; } = string.Empty;
            /// <summary>
            /// Path for extras e.g. TARGET scripts.
            /// </summary>
            public string Url { get; set; } = string.Empty;
            /// <summary>
            /// Path for webapps, eg inara.
            /// </summary>
            public string WebApp { get; set; } = string.Empty;
            public string Args { get; set; } = string.Empty;
            
            [JsonIgnore]
            public Process Proc = new Process();
            public ProcessStartInfo ProcStartInfo = new();

            private void LaunchAddon()                           // function to launch enabled applications
            {
                
                // TARGET requires a path to a script, if that path has spaces, we need to quote them - set a string called quote we can use to top and tail
                const string quote = "\"";
                
                var path = $"{ProgramDirectory}/{ExecutableName}";
                
                if (File.Exists(path))      // worth checking the app we want to launch actually exists...
                {
                    try
                    {
                        ProcStartInfo.Arguments = Args;
                        ProcStartInfo.UseShellExecute = true;
                        ProcStartInfo.WorkingDirectory = ProgramDirectory;
                        Proc = Process.Start(ProcStartInfo);
                        Proc.EnableRaisingEvents = true;
                        //TODO update the running icon proc.Exited += new EventHandler(ProcessExitHandler);

//                        System.Threading.Thread.Sleep(50);
//                        proc.Refresh();


                    }
                    catch
                    {
                        // oh dear, something want horribluy wrong..
                     }


                }
                //else
                //{
                //    // yeah, that path didnt exist...
                //    //are we launching a web app?
                //    if (addOn.WebApp != String.Empty)
                //    {
                //        //ok lets launch it in default browser
                //        updatemystatus("Launching " + addOn.FriendlyName);
                //        string target = addOn.WebApp;
                //        Process.Start(new ProcessStartInfo(target) { UseShellExecute = true });
                //        //System.Diagnostics.Process.Start(target);
                //    }
                //    else
                //    {
                //        updatemystatus($"Unable to launch {addOn.FriendlyName}..");
                //    }

                //}
                //updatemystatus("All apps launched, waiting for EDLaunch Exit..");


            }

            public bool Conflicts(AddOn addon)
            {
                // TODO: add better conflict detection
                // change to a eq
                if (addon.FriendlyName != FriendlyName)
                {
                    return false;
                }

                return true;
            }
        }
    }
}
