using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace IUmo.Functions
{
    class IOFunctions
    {
        public void chkAndCreateFolder(string path) {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }

        public void chkAndCreateFile(string path)
        {
            if (!File.Exists(path))
                File.Create(path);
        }

        public void chkFirstStart(string path, string file_name)
        {
            if (!Directory.Exists(path) && !File.Exists(path))
                create_json(file_name);
        }

        public Classes.Class_JSON_Setting openJSONSetting()
        {
            FileStream file = null;
            try
            {
                file = new FileStream($"settings\\settings.json", FileMode.OpenOrCreate);
                byte[] buffer = new byte[file.Length];
                file.Read(buffer, 0, buffer.Length);
                Encoding.Default.GetString(buffer);
                Classes.Class_JSON_Setting settings = JsonSerializer.Deserialize<Classes.Class_JSON_Setting>(buffer);
                return settings;
            }
            catch (Exception ex)
            {
            }
            finally { file?.Close(); }
            return null;
        }

        public void saveJSONSetting(Classes.Class_JSON_Setting json_Setting) {
            string settingsString_ = "";
            if (json_Setting.maximilize_window == Classes.Class_types.WindowState.False)
                settingsString_ = JsonSerializer.Serialize(json_Setting);
            else if (json_Setting.maximilize_window == Classes.Class_types.WindowState.True) 
            {
                settingsString_ = JsonSerializer.Serialize(
                    new Classes.Class_JSON_Setting() { 
                    maximilize_window = json_Setting.maximilize_window,
                    size_window = new List<double>() { 450, 800 }
                    });
            }
            File.WriteAllText("settings\\settings.json", settingsString_);

        }

        public void create_json(string file_name) {
            switch (file_name)
            {
                case "settings.json":
                    Classes.Class_JSON_Setting json_Setting = new Classes.Class_JSON_Setting {
                        maximilize_window = Classes.Class_types.WindowState.False, 
                        size_window = new List<double>() { 450, 800 }
                    };

                    string settingsString = JsonSerializer.Serialize(json_Setting);

                    chkAndCreateFolder("settings");
                    File.WriteAllText($"settings\\{file_name}", settingsString);
                    break;
            }
        }
    }
}
