using System.Collections.Generic;
using System.Threading.Tasks;
using GearOS.Models;
using GearOS.Services;

namespace GearOS.Utilities
{
    public static class DeviceDetector
    {
        private static DeviceInfoFetcher _fetcher;

        static DeviceDetector()
        {
            _fetcher = new DeviceInfoFetcher();
        }

        // Version synchrone (compatibilité)
        public static void DetectAndFillKeys(DeviceInfo device)
        {
            DetectAndFillKeysAsync(device).Wait();
        }

        // Version asynchrone (dynamique)
        public static async Task DetectAndFillKeysAsync(DeviceInfo device)
        {
            if (device == null) return;

            device.Keys.Clear();

            // Récupérer les infos du périphérique
            var spec = await _fetcher.FetchDeviceInfoAsync(device);

            if (spec == null)
                return;

            // Générer les touches selon le type
            switch (spec.Type.ToUpper())
            {
                case "TARTARUS":
                    GenerateTartarusKeys(device, spec);
                    break;
                case "MOUSE":
                    GenerateMouseKeys(device, spec);
                    break;
                case "KEYBOARD":
                    GenerateKeyboardKeys(device, spec);
                    break;
                case "GAMEPAD":
                    GenerateGamepadKeys(device, spec);
                    break;
                default:
                    GenerateGenericKeys(device, spec);
                    break;
            }
        }

        private static void GenerateTartarusKeys(DeviceInfo device, DeviceDatabase.DeviceSpec spec)
        {
            // Grille principale 20 touches (4 lignes x 5 colonnes)
            for (int row = 0; row < 4; row++)
            {
                for (int col = 0; col < 5; col++)
                {
                    int idNum = (row * 5) + col + 1;
                    device.Keys.Add(new DeviceKey
                    {
                        ID = idNum.ToString("D2"),
                        X = 280 + (col * 45),
                        Y = 60 + (row * 45),
                        Name = $"Key_{idNum}",
                        DisplayName = $"K{idNum}"
                    });
                }
            }

            // Touches latérales
            device.Keys.Add(new DeviceKey { ID = "THUMB", X = 530, Y = 210, Name = "Thumb", DisplayName = "Thumb" });
            device.Keys.Add(new DeviceKey { ID = "WHEEL", X = 530, Y = 140, Name = "Wheel", DisplayName = "Wheel" });
        }

        private static void GenerateMouseKeys(DeviceInfo device, DeviceDatabase.DeviceSpec spec)
        {
            // Disposition verticale pour souris
            int[] positions = { 0, 40, 80, 120, 160, 200, 240, 280, 320, 360, 400 };
            string[] names = { "LMB", "RMB", "MMB", "SideF", "SideB", "Wheel+", "Wheel-", "Btn7", "Btn8", "Btn9", "Btn10" };

            for (int i = 0; i < spec.KeyCount && i < positions.Length; i++)
            {
                device.Keys.Add(new DeviceKey
                {
                    ID = (i + 1).ToString(),
                    X = 50,
                    Y = positions[i],
                    Name = names[i],
                    DisplayName = names[i]
                });
            }
        }

        private static void GenerateKeyboardKeys(DeviceInfo device, DeviceDatabase.DeviceSpec spec)
        {
            // Layout QWERTY simplifié (20 touches principales)
            string[] qwertyKeys = { "Q", "W", "E", "R", "T", "A", "S", "D", "F", "G", 
                                   "Z", "X", "C", "V", "B", "1", "2", "3", "4", "5" };

            int x = 50, y = 50;
            for (int i = 0; i < spec.KeyCount && i < qwertyKeys.Length; i++)
            {
                device.Keys.Add(new DeviceKey
                {
                    ID = qwertyKeys[i],
                    X = x,
                    Y = y,
                    Name = qwertyKeys[i],
                    DisplayName = qwertyKeys[i]
                });

                x += 45;
                if ((i + 1) % 5 == 0)
                {
                    x = 50;
                    y += 45;
                }
            }
        }

        private static void GenerateGamepadKeys(DeviceInfo device, DeviceDatabase.DeviceSpec spec)
        {
            // Layout Xbox 14 boutons
            string[] buttons = { "A", "B", "X", "Y", "LB", "RB", "Back", "Start", 
                                "LS", "RS", "LT", "RT", "DPadU", "DPadD" };

            for (int i = 0; i < spec.KeyCount && i < buttons.Length; i++)
            {
                device.Keys.Add(new DeviceKey
                {
                    ID = buttons[i],
                    X = 50 + (i % 4) * 60,
                    Y = 50 + (i / 4) * 60,
                    Name = buttons[i],
                    DisplayName = buttons[i]
                });
            }
        }

        private static void GenerateGenericKeys(DeviceInfo device, DeviceDatabase.DeviceSpec spec)
        {
            // Grille générique 2x4
            for (int i = 0; i < spec.KeyCount && i < 8; i++)
            {
                device.Keys.Add(new DeviceKey
                {
                    ID = (i + 1).ToString(),
                    X = 50 + (i % 4) * 50,
                    Y = 50 + (i / 4) * 50,
                    Name = $"Key_{i + 1}",
                    DisplayName = $"K{i + 1}"
                });
            }
        }
    }
}
