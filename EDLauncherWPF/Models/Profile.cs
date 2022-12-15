using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDLauncherWPF.Models
{
    public class Profile
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string GameName { get; set; }
        public string GamePath { get; set; }
        public List<ProfileAddon> ProfileAddOns = new();

        public class ProfileAddon
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
            if (!ProfileAddonExists(name))
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
                if (pa.Name == name) return true;
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
            Name = name;
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
