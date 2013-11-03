#region Using
using System.IO;
using Styx.Common;
using Styx.Helpers;
#endregion

namespace BattlePetLeveler.GUI
{
    public class BattlePetLevelerSettings : Settings
    {
        #region Variables
        public static readonly BattlePetLevelerSettings Instance = new BattlePetLevelerSettings();
        public BattlePetLevelerSettings()
            : base(Path.Combine(Utilities.AssemblyDirectory, string.Format(@"Settings\{0}\{0}.xml", "BattlePetLeveler")))
        {
            Load();
        }
        #endregion

        #region Character Type Selection
        [Setting, DefaultValue("")]
        public string BPLCharacterTypeComboBox { get; set; }
        #endregion
    }
}
