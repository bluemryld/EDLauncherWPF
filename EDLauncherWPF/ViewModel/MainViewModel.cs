using EDLauncherWPF.Models;
using Microsoft.Xaml.Behaviors.Core;
using System.Collections;
using System.Windows;
using System.Windows.Documents;
using static EDLauncherWPF.Models.Profiles;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Data;
using System.ComponentModel;
using System.Xml.Linq;
using CommunityToolkit.Mvvm.Input;

namespace EDLauncherWPF.ViewModel
{
    public partial class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public MainViewModel()
        {
            this.ProfileNames = CollectionViewSource.GetDefaultView(Profiles.AllProfiles);
        }

        public Settings Settings = new();
        private Profiles profiles = new();
        private Addons addons= new();

        public string CurrentProfile
        {
            get { return profiles.CurrentProfile; }
            set
            {
                profiles.CurrentProfile = value;
                OnChangedAll();
            }
        }

        private void OnChangedAll()
        {
            OnPropertyChanged(nameof(CurrentProfile));
            OnPropertyChanged(nameof(ProfileDescription));
            OnPropertyChanged(nameof(ProfileGame));

        }
        
        public ICollectionView ProfileNames { get; private set; }

        //public IEnumerable<Profile> ProfileNames => _profileNames;

        //{
        //    get => Profiles.AllProfiles;
        //    //set => SetProperty(Profiles.Name, value, Profiles, (u, n) => u.Name = n);
        //}
        
        public string ProfileDescription
        {
            get => profiles.Description;
            set { profiles.Description = value; }
        }

        public string ProfileGame
        {
            get => Profiles.GameName;
            set { Profiles.GameName = value;}
        }

        public IEnumerable GetCurrentAddons
        {    
            get => profiles.GetCurrentAddons();
        }

        private ActionCommand btncmd_NewProfile1;
        public ICommand btncmd_NewProfile => btncmd_NewProfile1 ??= new ActionCommand(Performbtncmd_NewProfile);

        private void Performbtncmd_NewProfile()
        {
            profiles.Add("", "");

            ProfileNames.Refresh(); 
            OnPropertyChanged(nameof(ProfileNames));
        }

        private ActionCommand btncmd_DeleteProfile1;
        public ICommand btncmd_DeleteProfile => btncmd_DeleteProfile1 ??= new ActionCommand(Performbtncmd_DeleteProfile);

        private void Performbtncmd_DeleteProfile()
        {
            profiles.Remove();
            OnPropertyChanged(nameof(CurrentProfile));
            ProfileNames.Refresh();
            OnPropertyChanged(nameof(ProfileNames));
        }

        private ActionCommand btncmd_ExitApp1;
        public ICommand btncmd_ExitApp => btncmd_ExitApp1 ??= new ActionCommand(Performbtncmd_ExitApp);

        private void Performbtncmd_ExitApp()
        {
            profiles.SaveProfiles();
            Application.Current.Shutdown();
        }


        
        [RelayCommand]
        private void RunAddon(CurrentProfileAddon cpAddon)
        {
            addons.RunAddon(cpAddon.FriendlyName);
            OnPropertyChanged(nameof(GetCurrentAddons));

        }


        //public Visibility AddOnRunVisibility
        //{
        //    get { return AddOnRunVisibility ? Visibility.Visible : Visibility.Collapsed }
        //}

    }
}
