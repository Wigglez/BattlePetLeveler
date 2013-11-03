#region Using
using System.IO;
using Styx.Common;
using Styx.Helpers;
#endregion

namespace BattlePetLeveler.GUI {
    public class BattlePetLevelerSettings : Settings {
        #region Variables

        public static readonly BattlePetLevelerSettings Instance = new BattlePetLevelerSettings();

        public BattlePetLevelerSettings()
            : base(Path.Combine(Utilities.AssemblyDirectory, string.Format(@"Settings\{0}\{0}-{1}.xml", "BattlePetLeveler", Styx.StyxWoW.Me.Name))) {
            Load();
        }

        #endregion

        #region Combo Boxes

        [Setting, DefaultValue("")]
        public string BPLLevelingTypeComboBox { get; set; }

        [Setting, DefaultValue("")]
        public string BPLCharacterTypeComboBox { get; set; }

        #endregion
    }
}
