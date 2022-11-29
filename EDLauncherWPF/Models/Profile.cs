using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDLauncherWPF.Models
{
    public class Profile
    {
        private readonly AddonSet _addonSet;
        public string Name { get; set; }
        public string Description { get; set; }
        public bool VRMmode { get; set; }
        public Profile(string name)
        {
            Name = name;
            Description = name;

            _addonSet = new AddonSet();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<AddOn> GetAddonsList()
        {
            return _addonSet.GetAddonsList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="addOn"></param>
        public void AddAddon(AddOn addOn)
        {
            _addonSet.AddAddon(addOn);
        }
        public void AddAddons(List<AddOn> incomingAddons)
        {
            _addonSet.AddAddons(incomingAddons);
        }
    }
}
