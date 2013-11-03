using System;
using System.Windows.Forms;

namespace BattlePetLeveler.GUI
{
    public partial class BattlePetLevelerGUI : Form
    {
        public BattlePetLevelerGUI()
        {
            InitializeComponent();
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
     }
}
