using System;
using System.Windows;
using System.Windows.Controls;

namespace GearOS.Services
{
    // Lightweight UI helper for the right panel (can be expanded)
    public class KeyMappingPanel : StackPanel
    {
        public KeyMappingPanel()
        {
            Orientation = Orientation.Vertical;
            Margin = new Thickness(8);

            var title = new TextBlock { Text = "Configuration de la touche", Foreground = System.Windows.Media.Brushes.White, FontSize = 16 };
            Children.Add(title);

            var recordBtn = new Button { Content = "Record", Margin = new Thickness(0,8,0,0) };
            Children.Add(recordBtn);

            var mouseDropdown = new ComboBox { Margin = new Thickness(0,8,0,0) };
            mouseDropdown.Items.Add("Click Left");
            mouseDropdown.Items.Add("Click Right");
            mouseDropdown.Items.Add("Scroll Up");
            mouseDropdown.Items.Add("Scroll Down");
            Children.Add(mouseDropdown);

            var macroArea = new TextBox { AcceptsReturn = true, Height = 80, Margin = new Thickness(0,8,0,0), Text = "# Enter macro commands with delays in ms\n" };
            Children.Add(macroArea);

            var save = new Button { Content = "Save Mapping", Background = new System.Windows.Media.SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#FF7A00")), Foreground = System.Windows.Media.Brushes.White, Margin = new Thickness(0,8,0,0) };
            Children.Add(save);
        }
    }
}
