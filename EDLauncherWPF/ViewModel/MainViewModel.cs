using CommunityToolkit.Mvvm.ComponentModel;
using EDLauncherWPF.Models;
using System.Collections;
using System.Windows.Documents;
using static EDLauncherWPF.Models.Profiles;

namespace EDLauncherWPF.ViewModel
{
    public class MainViewModel : ObservableObject
    {
        public Settings Settings = new();

        private Profiles Profiles = new();

        static class ProfileComboList
        {
            public static string Name
            {
                    get;
            }
        }

        public class CurrentAddon : CurrentProfileAddon
        {

        }

        
                
                
        public string GetProfileNames()
        {
            return ProfileComboList.Name;
        }

        public string ProfileName
        {
            get => Profiles.Name;
            //set => SetProperty(Profiles.Name, value, Profiles, (u, n) => u.Name = n);
        }
        
        public string ProfileDescription
        {
            get => Profiles.Description;
        }

        public string ProfileGame
        {
            get => Profiles.GameName;
        }

        public IEnumerable GetCurrentAddons
        {    
            get => Profiles.GetCurrentAddons();
        }

        

    }
}
