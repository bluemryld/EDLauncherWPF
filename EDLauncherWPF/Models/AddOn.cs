using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace EDLauncherWPF.Models
{
    public class AddOn
    {
        public AddOn()
        {

        }
        public AddOn(string friendlyName, bool enabled, string programDirectory, string executableName, bool installable, string autoDiscoverPath, string scripts, string url, string webApp)
        {
            FriendlyName = friendlyName;
            Enabled = enabled;
            ProgramDirectory = programDirectory;
            ExecutableName = executableName;
            Installable = installable;
            AutoDiscoverPath = autoDiscoverPath;
            Scripts = scripts;
            Url = url;
            WebApp = webApp;
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
        /// Add on for if enabled (box checked)
        /// </summary>
        public bool Enabled { get; set; } = false;

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

        public bool Conflicts(AddOn addon)
        {
            // TO DO: add better conflict detection
            if(addon.FriendlyName != FriendlyName)
            {
                return false;
            }

            return true;
        }
    }
}
