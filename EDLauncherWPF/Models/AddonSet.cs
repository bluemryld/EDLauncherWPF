using EDLauncherWPF.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDLauncherWPF.Models
{
    public class AddonSet
    {
        private readonly List<AddOn> _addons;

        public AddonSet()
        {
            _addons= new List<AddOn>();
        }

        public IEnumerable<AddOn> GetAddonsList()
        {
            return _addons;
        }

        public void AddAddon(AddOn addon)
        {
            foreach (AddOn existingAddon in _addons)
            {
                if(existingAddon.Conflicts(addon))
                {
                    throw new AddonConflictException(existingAddon, addon);
                }
            }


            _addons.Add(addon);
        }

        public void AddAddons(List<AddOn> incomingAddons)
        {
            foreach (AddOn newAddon in incomingAddons)
            {
                AddAddon(newAddon);
            }
        }
    }
}
