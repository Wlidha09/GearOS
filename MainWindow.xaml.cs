using GearOS.Models;
using GearOS.Utilities;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace GearOS
{
    /// <summary>
    /// Logique principale de l'interface GearOS
    /// </summary>
    public partial class MainWindow : Window
    {
        private GameDetector _gameDetector;
        private List<DeviceProfile> _userProfiles;

        // Service de gestion du matériel (HidSharp)
        private Services.HardwareHandler _hardwareHandler = new Services.HardwareHandler();

        public MainWindow()
        {
            try
            {
                InitializeComponent();
                InitializeSystem();
            }
            catch (Exception ex)
            {
                // Capture les erreurs d'initialisation XAML ou logique
                MessageBox.Show($"Erreur au démarrage :\n{ex.Message}", "GearOS - Crash");
            }
        }

        private void InitializeSystem()
        {
            // 1. Chargement du profil via le manager
            var savedProfile = ProfileManager.LoadProfile();
            _userProfiles = new List<DeviceProfile> { savedProfile ?? new DeviceProfile { Name = "Profil Standard" } };

            // 2. Scan et affichage du matériel connecté
            UpdateHardwareUI();

            // 3. Setup du détecteur de processus (Jeux)
            _gameDetector = new GameDetector(_userProfiles);
            _gameDetector.ProfileActivated += (profile) =>
            {
                // Utilisation du Dispatcher pour mettre à jour l'UI depuis le thread du Timer
                Dispatcher.Invoke(() =>
                {
                    this.Title = $"GearOS - Profil Actif : {profile.Name}";
                });
            };
            _gameDetector.Start();

            // Vue par défaut
            if (EquipementView != null) EquipementView.Visibility = Visibility.Visible;
        }

        private void UpdateHardwareUI()
        {
            try
            {
                var devices = _hardwareHandler.GetAllConnectedDevices();
                DevicesListControl.ItemsSource = devices;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur de détection : {ex.Message}");
            }
        }

        // --- Navigation ---

        private void ShowEquipement(object sender, RoutedEventArgs e)
        {
            EquipementView.Visibility = Visibility.Visible;
            LibraryView.Visibility = Visibility.Collapsed;
        }

        private void ShowLibrary(object sender, RoutedEventArgs e)
        {
            EquipementView.Visibility = Visibility.Collapsed;
            LibraryView.Visibility = Visibility.Visible;
        }

        private void ScanDevices_Click(object sender, RoutedEventArgs e)
        {
            UpdateHardwareUI();
        }

        // --- Interaction avec les périphériques ---

        private void Device_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (sender is Border border && border.DataContext is DeviceInfo device)
            {
                // On passe le périphérique à la fenêtre de mapping
                MappingWindow mappingWin = new MappingWindow(device)
                {
                    Owner = this
                };
                mappingWin.ShowDialog();

                // Rafraîchir après modification
                UpdateHardwareUI();
            }
        }
    }
}