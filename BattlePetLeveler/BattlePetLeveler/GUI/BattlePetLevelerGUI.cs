using System;
using System.Windows.Forms;

namespace BattlePetLeveler.GUI
{
    public partial class BattlePetLevelerGUI : Form
    {
        public BattlePetLevelerGUI()
        {
            InitializeComponent();
            LevelingTypeComboBox.Items.Add("Character Leveling");
            LevelingTypeComboBox.Items.Add("Pet Leveling");
            LevelingTypeComboBox.DataBindings.Add(new Binding("SelectedItem",
                BattlePetLevelerSettings.Instance, "BPLLevelingTypeComboBox",
                false, DataSourceUpdateMode.OnPropertyChanged));
            // Populate the second combobox after the first one has been databound (so value from settings has been selected)
            LevelingTypeComboBox_SelectedIndexChanged(null, null);
            CharacterTypeComboBox.DataBindings.Add(new Binding("SelectedItem",
                BattlePetLevelerSettings.Instance, "BPLCharacterTypeComboBox", 
                false, DataSourceUpdateMode.OnPropertyChanged));
            
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("BPL Settings have been saved.", "Save");
            BattlePetLevelerSettings.Instance.Save();
            BattlePetLeveler.BPLlog("Settings Saved");
            Close();
        }

        private void LevelingTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            CharacterTypeComboBox.Items.Clear();
            string[] values;
            if (LevelingTypeComboBox.SelectedIndex == 0)
            {
                values = new[] { "Winner", "Loser", "Win Trade" };
            }
            else if (LevelingTypeComboBox.SelectedIndex == 1)
            {
                values = new[] { "Win Trade" };
            }
            else
                values = new string[0];
            CharacterTypeComboBox.Items.AddRange(values);
        }
     }
}
