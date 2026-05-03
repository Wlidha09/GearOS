using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using GearOS.Models;

namespace GearOS.Services
{
    public class GameLibrary
    {
        private readonly string _libraryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "game-library.json");
        private List<Game> _games;

        public GameLibrary()
        {
            LoadLibrary();
        }

        private void LoadLibrary()
        {
            _games = new List<Game>();
            try
            {
                if (File.Exists(_libraryPath))
                {
                    var json = File.ReadAllText(_libraryPath);
                    _games = JsonSerializer.Deserialize<List<Game>>(json) ?? new List<Game>();
                }
            }
            catch { }
        }

        public void SaveLibrary()
        {
            try
            {
                var dirPath = Path.GetDirectoryName(_libraryPath);
                if (!Directory.Exists(dirPath))
                    Directory.CreateDirectory(dirPath);

                var json = JsonSerializer.Serialize(_games, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(_libraryPath, json);
            }
            catch { }
        }

        public List<Game> GetAllGames() => _games;

        public void AddGame(Game game)
        {
            if (!_games.Any(g => g.ExecutablePath == game.ExecutablePath))
            {
                _games.Add(game);
                SaveLibrary();
            }
        }

        public void RemoveGame(string gameID)
        {
            _games.RemoveAll(g => g.ID == gameID);
            SaveLibrary();
        }

        public Game GetGameByExecutable(string exeName)
        {
            return _games.FirstOrDefault(g => g.ExecutableName.Equals(exeName, StringComparison.OrdinalIgnoreCase));
        }

        public void AssignProfileToGame(string gameID, DeviceProfile profile)
        {
            var game = _games.FirstOrDefault(g => g.ID == gameID);
            if (game != null)
            {
                game.AssignedProfile = profile;
                SaveLibrary();
            }
        }

        public async Task SyncWithPlatformsAsync()
        {
            var detector = new PlatformDetector();
            var detectedGames = await detector.DetectInstalledGamesAsync();

            foreach (var game in detectedGames)
            {
                if (!_games.Any(g => g.ExecutablePath == game.ExecutablePath))
                {
                    AddGame(game);
                }
            }
        }
    }
}
