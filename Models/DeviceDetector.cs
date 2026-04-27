using System.Collections.Generic;
using GearOS.Models;

namespace GearOS.Utilities
{
    public static class DeviceDetector
    {
        public static void DetectAndFillKeys(DeviceInfo device)
        {
            if (device == null) return;

            device.Keys.Clear();

            if (device.Name.ToUpper().Contains("TARTARUS"))
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
                            Y = 60 + (row * 45)
                        });
                    }
                }

                // Touches latérales
                device.Keys.Add(new DeviceKey { ID = "THUMB", X = 530, Y = 210 });
                device.Keys.Add(new DeviceKey { ID = "WHEEL", X = 530, Y = 140 });
            }
        }
    }
}