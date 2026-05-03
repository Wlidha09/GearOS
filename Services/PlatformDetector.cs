using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using GearOS.Models;
using Microsoft.Win32;

namespace GearOS.Services
{
    public class PlatformDetector
    {
        public async Task<List<Game>> DetectInstalledGamesAsync()
        {
            var games = new List<Game>();

            // Détecter depuis Steam
            games.AddRange(await DetectSteamGamesAsync());

            // Détecter depuis Epic Games
            games.AddRange(await DetectEpicGamesAsync());

            // Détecter depuis Battle.net
            games.AddRange(await DetectBattleNetGamesAsync());

            // Détecter depuis EA Play
            games.AddRange(await DetectEAGamesAsync());

            // Détecter depuis Ubisoft+
            games.AddRange(await DetectUbisoftGamesAsync());

            // Détecter depuis Rockstar
            games.AddRange(await DetectRockstarGamesAsync());

            return games.DistinctBy(g => g.ExecutablePath).ToList();
        }

        private async Task<List<Game>> DetectSteamGamesAsync()
        {
            var games = new List<Game>();
            try
            {
                string steamPath = (string)Registry.GetValue(@"HKEY_CURRENT_USER\Software\Valve\Steam", "SteamPath", null);
                if (string.IsNullOrEmpty(steamPath))
                    return games;

                string libraryFoldersPath = Path.Combine(steamPath, "steamapps", "libraryfolders.json");
                if (!File.Exists(libraryFoldersPath))
                    return games;

                var json = await File.ReadAllTextAsync(libraryFoldersPath);
                using (JsonDocument doc = JsonDocument.Parse(json))
                {
                    var root = doc.RootElement.GetProperty("libraryfolders");
                    foreach (var folder in root.EnumerateObject())
                    {
                        string libraryPath = folder.Value.GetProperty("path").GetString();
                        string appsPath = Path.Combine(libraryPath, "steamapps");

                        foreach (var dir in Directory.GetDirectories(appsPath))
                        {
                            string manifestFile = Path.Combine(dir, "app.vdf");
                            if (File.Exists(manifestFile))
                            {
                                var gameDir = Directory.GetDirectories(dir).FirstOrDefault();
                                if (gameDir != null)
                                {
                                    games.Add(new Game
                                    {
                                        Name = Path.GetFileName(gameDir),
                                        Platform = "Steam",
                                        PlatformID = Path.GetFileName(dir),
                                        ExecutablePath = gameDir,
                                        ExecutableName = Path.GetFileNameWithoutExtension(gameDir)
                                    });
                                }
                            }
                        }
                    }
                }
            }
            catch { }

            return games;
        }

        private async Task<List<Game>> DetectEpicGamesAsync()
        {
            var games = new List<Game>();
            try
            {
                string epicPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "Epic Games");
                if (!Directory.Exists(epicPath))
                    return games;

                foreach (var gameDir in Directory.GetDirectories(epicPath))
                {
                    var exes = Directory.GetFiles(gameDir, "*.exe", SearchOption.AllDirectories).FirstOrDefault();
                    if (!string.IsNullOrEmpty(exes))
                    {
                        games.Add(new Game
                        {
                            Name = Path.GetFileName(gameDir),
                            Platform = "Epic",
                            ExecutablePath = exes,
                            ExecutableName = Path.GetFileNameWithoutExtension(exes)
                        });
                    }
                }
            }
            catch { }

            return games;
        }

        private async Task<List<Game>> DetectBattleNetGamesAsync()
        {
            var games = new List<Game>();
            try
            {
                string bnPath = (string)Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\Blizzard Entertainment\Battle.net\Capabilities", "ApplicationPath", null);
                if (string.IsNullOrEmpty(bnPath))
                    return games;

                string gamesPath = Path.Combine(Path.GetDirectoryName(bnPath), "Games");
                if (Directory.Exists(gamesPath))
                {
                    foreach (var gameDir in Directory.GetDirectories(gamesPath))
                    {
                        var exes = Directory.GetFiles(gameDir, "*.exe").FirstOrDefault();
                        if (!string.IsNullOrEmpty(exes))
                        {
                            games.Add(new Game
                            {
                                Name = Path.GetFileName(gameDir),
                                Platform = "Battle.net",
                                ExecutablePath = exes,
                                ExecutableName = Path.GetFileNameWithoutExtension(exes)
                            });
                        }
                    }
                }
            }
            catch { }

            return games;
        }

        private async Task<List<Game>> DetectEAGamesAsync()
        {
            var games = new List<Game>();
            try
            {
                string eaPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "EA Games");
                if (!Directory.Exists(eaPath))
                    return games;

                foreach (var gameDir in Directory.GetDirectories(eaPath))
                {
                    var exes = Directory.GetFiles(gameDir, "*.exe", SearchOption.AllDirectories).FirstOrDefault();
                    if (!string.IsNullOrEmpty(exes))
                    {
                        games.Add(new Game
                        {
                            Name = Path.GetFileName(gameDir),
                            Platform = "EA",
                            ExecutablePath = exes,
                            ExecutableName = Path.GetFileNameWithoutExtension(exes)
                        });
                    }
                }
            }
            catch { }

            return games;
        }

        private async Task<List<Game>> DetectUbisoftGamesAsync()
        {
            var games = new List<Game>();
            try
            {
                string ubiPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "Ubisoft\\Ubisoft Game Launcher\\games");
                if (!Directory.Exists(ubiPath))
                    return games;

                foreach (var gameDir in Directory.GetDirectories(ubiPath))
                {
                    var exes = Directory.GetFiles(gameDir, "*.exe", SearchOption.AllDirectories).FirstOrDefault();
                    if (!string.IsNullOrEmpty(exes))
                    {
                        games.Add(new Game
                        {
                            Name = Path.GetFileName(gameDir),
                            Platform = "Ubisoft",
                            ExecutablePath = exes,
                            ExecutableName = Path.GetFileNameWithoutExtension(exes)
                        });
                    }
                }
            }
            catch { }

            return games;
        }

        private async Task<List<Game>> DetectRockstarGamesAsync()
        {
            var games = new List<Game>();
            try
            {
                string rockstarPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "Rockstar Games");
                if (!Directory.Exists(rockstarPath))
                    return games;

                foreach (var gameDir in Directory.GetDirectories(rockstarPath))
                {
                    var exes = Directory.GetFiles(gameDir, "*.exe", SearchOption.AllDirectories).FirstOrDefault();
                    if (!string.IsNullOrEmpty(exes))
                    {
                        games.Add(new Game
                        {
                            Name = Path.GetFileName(gameDir),
                            Platform = "Rockstar",
                            ExecutablePath = exes,
                            ExecutableName = Path.GetFileNameWithoutExtension(exes)
                        });
                    }
                }
            }
            catch { }

            return games;
        }
    }
}
