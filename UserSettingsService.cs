using System;
using System.Drawing;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NotePadWF_CS
{
    public class UserSettings
    {
        // Default values should be provided for a good first-run experience
        public Point MyLocation { get; set; } = new Point(100, 100); // Default location
        public Size MySize { get; set; } = new Size(800, 600);     // Default size
        public string FontName { get; set; } = "Microsoft Sans Serif"; // Store Font properties separately for JSON
        public float FontSize { get; set; } = 8.25f;
        public FontStyle FontStyle { get; set; } = FontStyle.Regular;
        public int MyTextColorArgb { get; set; } = Color.Black.ToArgb(); // Store Color as ARGB
        public int MyBackgroundColorArgb { get; set; } = Color.White.ToArgb();
        public bool MyStatusBarVisible { get; set; } = true;
        public bool MyWordWrap { get; set; } = true;
        public bool MyAutoSaveEnabled { get; set; } = false;

        // Non-serialized properties to get Font/Color objects
        [JsonIgnore]
        public Font MyFont
        {
            get { return new Font(FontName, FontSize, FontStyle); }
            set
            {
                FontName = value.Name;
                FontSize = value.Size;
                FontStyle = value.Style;
            }
        }

        [JsonIgnore]
        public Color MyTextColor
        {
            get { return Color.FromArgb(MyTextColorArgb); }
            set { MyTextColorArgb = value.ToArgb(); }
        }

        [JsonIgnore]
        public Color MyBackgroundColor
        {
            get { return Color.FromArgb(MyBackgroundColorArgb); }
            set { MyBackgroundColorArgb = value.ToArgb(); }
        }
    }

    public class UserSettingsService
    {
        private static readonly string _settingsFilePath;
        private static readonly JsonSerializerOptions _jsonOptions;

        public UserSettings Settings { get; private set; }

        static UserSettingsService()
        {
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string appFolderPath = Path.Combine(appDataPath, ".NETpad");
            _settingsFilePath = Path.Combine(appFolderPath, "usersettings.json");

            _jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                // Converters will be added here in a later step if the simplified Font/Color storage isn't enough.
                // For now, we are storing Font parts and Color ARGB directly.
                // Point and Size are value types that System.Text.Json might handle out-of-the-box
                // or might require simple converters if they have non-public setters/getters not compatible by default.
            };
            // Ensure Point and Size can be serialized/deserialized.
            // System.Text.Json can handle simple structs with public get/set properties.
            // Point and Size have these.
        }

        public UserSettingsService()
        {
            Settings = Load();
        }

        public UserSettings Load()
        {
            try
            {
                if (File.Exists(_settingsFilePath))
                {
                    string json = File.ReadAllText(_settingsFilePath);
                    var settings = JsonSerializer.Deserialize<UserSettings>(json, _jsonOptions);
                    return settings ?? new UserSettings(); // Return default if deserialization results in null
                }
            }
            catch (Exception ex)
            {
                // Log error or handle (e.g., by returning default settings)
                Console.WriteLine($"Error loading settings: {ex.Message}");
            }
            return new UserSettings(); // Return default if file doesn't exist or error occurs
        }

        public void Save()
        {
            try
            {
                // Ensure the directory exists
                string? directoryName = Path.GetDirectoryName(_settingsFilePath);
                if (directoryName != null && !Directory.Exists(directoryName))
                {
                    Directory.CreateDirectory(directoryName);
                }

                string json = JsonSerializer.Serialize(Settings, _jsonOptions);
                File.WriteAllText(_settingsFilePath, json);
            }
            catch (Exception ex)
            {
                // Log error or handle
                Console.WriteLine($"Error saving settings: {ex.Message}");
            }
        }

        // Optional: Provide a static instance for easy access if preferred (Singleton-like)
        private static UserSettingsService? _instance;
        public static UserSettingsService Instance => _instance ??= new UserSettingsService();
    }
}
