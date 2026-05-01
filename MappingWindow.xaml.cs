using System.Windows;
using System.Windows.Controls;
using GearOS.Models;
using GearOS.Utilities;

namespace GearOS
{
    public partial class MappingWindow : Window
    {
        private DeviceInfo _device;
        private DeviceKey _selectedKey;

        public MappingWindow(DeviceInfo device)
        {
            InitializeComponent();
            _device = device;
            InitializeProfileAsync();
        }

        private async void InitializeProfileAsync()
        {
            var savedProfile = ProfileManager.LoadProfile();
            if (savedProfile != null)
                _device.ActiveProfile = savedProfile;
            else
            {
                _device.ActiveProfile = new DeviceProfile { Name = "Layer 4" };
                // Utiliser la détection async pour les specs complètes
                await DeviceDetector.DetectAndFillKeysAsync(_device);
                _device.ActiveProfile.Keys = _device.Keys;
            }

            DeviceNameText.Text = _device.Name.ToUpper();
            KeysItemsControl.ItemsSource = _device.ActiveProfile.Keys;
        }

        private void Key_Click(object sender, RoutedEventArgs e)
        {
            _selectedKey = (sender as Button).DataContext as DeviceKey;
            if (_selectedKey != null)
            {
                MacroInput.Text = _selectedKey.Mapping.TargetAction;
                if (ActionTypeCombo != null)
                    ActionTypeCombo.SelectedIndex = (int)_selectedKey.Mapping.Type;
            }
        }

        private void MacroInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_selectedKey != null)
            {
                _selectedKey.Mapping.TargetAction = MacroInput.Text;
                ProfileManager.SaveProfile(_device.ActiveProfile);
            }
        }

        private void ActionTypeCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_selectedKey != null && ActionTypeCombo != null)
            {
                _selectedKey.Mapping.Type = (MappingType)ActionTypeCombo.SelectedIndex;
                ProfileManager.SaveProfile(_device.ActiveProfile);
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e) => this.Close();
        private void Cancel_Click(object sender, RoutedEventArgs e) => this.Close();
    }
}
