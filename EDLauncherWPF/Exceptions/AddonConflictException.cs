using EDLauncherWPF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using static EDLauncherWPF.Models.Addons;

namespace EDLauncherWPF.Exceptions
{
    public class AddonConflictException : Exception
    {
        public AddOn ExistingAddon { get;}
        public AddOn IncomingAddon { get;}
        public AddonConflictException(AddOn existingAddon, AddOn incomingAddon)
        {
            ExistingAddon = existingAddon;
            IncomingAddon = incomingAddon;
        }

        public AddonConflictException(string? message, AddOn existingAddon, AddOn incomingAddon) : base(message)
        {
            ExistingAddon = existingAddon;
            IncomingAddon = incomingAddon;
        }

        public AddonConflictException(string? message, Exception? innerException, AddOn existingAddon, AddOn incomingAddon) : base(message, innerException)
        {
            ExistingAddon = existingAddon;
            IncomingAddon = incomingAddon;
        }

        protected AddonConflictException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

    }
}
