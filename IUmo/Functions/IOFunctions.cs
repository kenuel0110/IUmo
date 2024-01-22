using IUmo.Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Resources;

namespace IUmo.Functions
{
    //Здесь описаны все взаимодействия с файлами и папками
    class IOFunctions
    {

        public string copyTemplate(string path)
        {
           string state = "";
            try 
            {
                if (!File.Exists(path))
                {
                    StreamResourceInfo resourceInfo = Application.GetResourceStream(new Uri("pack://application:,,,/Files/Template.xlsx"));
                    using (Stream resourceStream = resourceInfo.Stream)
                    {
                        using (FileStream fileStream = new FileStream(path, FileMode.Create))
                        {
                            resourceStream.CopyTo(fileStream);
                            resourceStream.Close();
                        }
                    }
                }
                else 
                {
                    File.Delete(path);

                    StreamResourceInfo resourceInfo = Application.GetResourceStream(new Uri("pack://application:,,,/Files/Template.xlsx"));
                    using (Stream resourceStream = resourceInfo.Stream)
                    {
                        using (FileStream fileStream = new FileStream(path, FileMode.Create))
                        {
                            resourceStream.CopyTo(fileStream);
                            resourceStream.Close();
                        }
                    }
                }
            } 
            catch(Exception ex) 
            {
                state = ex.Message;
                return state;
            }
            state = "";
            return state;
        }

        //Сохранение файла recent_files.json
        public void saveJSONRecentFiles(Classes.Class_JSON_RecenFiles last_file)
        {
            ObservableCollection<Classes.Class_JSON_RecenFiles> json_recent = openJSONRecentFiles();
            if (json_recent == null)
                json_recent = new ObservableCollection<Classes.Class_JSON_RecenFiles>();
            json_recent.Add(last_file);   
            string recentString = JsonSerializer.Serialize(json_recent);
            chkAndCreateFolder("settings");
            File.WriteAllText($"settings\\recent_files.json", recentString);
        }

        //Сохранение файла recent_files.json
        public void removeJSONRecentFile(List<Classes.Class_JSON_RecenFiles> new_json)
        {
            string recentString = JsonSerializer.Serialize(new_json);
            chkAndCreateFolder("settings");
            File.WriteAllText($"settings\\recent_files.json", recentString);
        }

        //Открытие файла recent_files.json
        public ObservableCollection<Classes.Class_JSON_RecenFiles> openJSONRecentFiles(Pages.Page_start page = null)
        {
            FileStream file = null;
            try
            {
                file = new FileStream($"settings\\recent_files.json", FileMode.OpenOrCreate);
                byte[] buffer = new byte[file.Length];
                file.Read(buffer, 0, buffer.Length);
                ObservableCollection<Classes.Class_JSON_RecenFiles> files = new ObservableCollection<Classes.Class_JSON_RecenFiles>();
                if (buffer.Length > 0)
                {
                    Encoding.Default.GetString(buffer);
                    files = JsonSerializer.Deserialize<ObservableCollection<Classes.Class_JSON_RecenFiles>>(buffer);
                }
                return files;
            }
            catch (Exception ex)
            {
                if (page != null)
                {
                    page.lbl_info_popup.Text = ex.Message;
                    page.popup_f_info.Visibility = System.Windows.Visibility.Visible;
                }
            }
            finally { file?.Close(); }
            return null;
        }

        //Проверка и автоматическое создание папок
        public void chkAndCreateFolder(string path) {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }
        //Проверка и автоматическое создание файлов
        public void chkAndCreateFile(string path)
        {
            if (!File.Exists(path))
                File.Create(path);
        }
        //Проверка и создание файла настроек
        public void chkFirstStart(string folder, string file_name)
        {
            
            if (!File.Exists($"{folder}\\{file_name}"))
                create_json(file_name);
            else
            {
                chkAndCreateFolder(Path.GetDirectoryName($"{folder}\\{file_name}"));
                chkAndCreateFile($"{folder}\\{file_name}");
            }

        }

        //Создать временый файл, для передачи данных между элементами программы
        public void createTemp()
        {
            chkAndCreateFolder(Path.GetDirectoryName("Temp\\temp.json"));
            Classes.Class_JSON_Temp json_temp = new Classes.Class_JSON_Temp
            {
                tempType = Classes.Class_types.TempType.Temp_none,
                path = "",
                course = 0
            };

            string tempString = JsonSerializer.Serialize(json_temp);
            File.WriteAllText("Temp\\temp.json", tempString);
        }

        public Classes.Class_JSON_Temp openJSONTemp()
        {
            FileStream file = null;
            try
            {
                file = new FileStream("Temp\\temp.json", FileMode.OpenOrCreate);
                byte[] buffer = new byte[file.Length];
                file.Read(buffer, 0, buffer.Length);
                Encoding.Default.GetString(buffer);
                Classes.Class_JSON_Temp temp = JsonSerializer.Deserialize<Classes.Class_JSON_Temp>(buffer);
                return temp;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally { file?.Close(); }
            return null;
        }

        //Удалить временый файл, для передачи данных между элементами программы
        public void removeTemp()
        {
            Directory.Delete("Temp", true);
        }
        
        public void saveTemp(Classes.Class_JSON_Temp new_temp)
        {
            string temp = JsonSerializer.Serialize(new_temp);
            using (StreamWriter sw = new StreamWriter("Temp\\temp.json"))
            {
                sw.Write(temp);
            }
        }

        //Открытие файла настроек
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
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally { file?.Close(); }
            return null;
        }

        //Открытие файла с настройками курсов
        public List<Class_JSON_Courses_Settings> openJSONCourses()
        {
            FileStream file = null;
            try
            {
                file = new FileStream("settings\\courses.json", FileMode.OpenOrCreate);
                byte[] buffer = new byte[file.Length];
                file.Read(buffer, 0, buffer.Length);
                Encoding.Default.GetString(buffer);
                List<Class_JSON_Courses_Settings> courses = JsonSerializer.Deserialize<List<Class_JSON_Courses_Settings>>(buffer);
                return courses;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally { file?.Close(); }
            return null;
        }

        //Сохранение файла настроек
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

        //Создание файла JSON
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
                case "recent_files.json":
                    
                    break;

                case "courses.json":
                    List<Class_JSON_Courses_Settings> default_Courses = new List<Class_JSON_Courses_Settings>()
                    {
                        new Class_JSON_Courses_Settings()
                        {
                            number =1,
                            groups = new List<KeyValuePair<String, String>>()
                            {
                                new KeyValuePair<String, String>("231Р11", "Инф"),
                                new KeyValuePair<String, String>("231Р21", "Арх"),
                                new KeyValuePair<String, String>("231Р31", "Эк"),
                                new KeyValuePair<String, String>("231Р41", "Диз"),
                                new KeyValuePair<String, String>("231Р51", "УВТС"),
                                new KeyValuePair<String, String>("231Р71", "ПГС"),
                                new KeyValuePair<String, String>("231Р91", "ТМС"),
                                new KeyValuePair<String, String>("231Р101", "УЗС"),
                                new KeyValuePair<String, String>("231Р111", "НТТС"),
                                new KeyValuePair<String, String>("231Р51", "Эн")
                            },
                        },
                        new Class_JSON_Courses_Settings()
                        {
                            number =2,
                            groups = new List<KeyValuePair<String, String>>()
                            {
                                new KeyValuePair<String, String>("221Р01", "Инф"),
                                new KeyValuePair<String, String>("221Р11", "Арх"),
                                new KeyValuePair<String, String>("221Р21", "Эк"),
                                new KeyValuePair<String, String>("221Р31", "Диз"),
                                new KeyValuePair<String, String>("221Р41", "УВТС"),
                                new KeyValuePair<String, String>("221Р101", "ПГС"),
                                new KeyValuePair<String, String>("221Р51", "ТМС"),
                                new KeyValuePair<String, String>("221Р61", "НТТС"),
                                new KeyValuePair<String, String>("221Р71", "УЗС"),
                                new KeyValuePair<String, String>("221Р91", "Эн"),
                                new KeyValuePair<String, String>("221Р111", "ГС")
                            },
                        },
                        new Class_JSON_Courses_Settings()
                        {
                            number =3,
                            groups = new List<KeyValuePair<String, String>>()
                            {
                                new KeyValuePair<String, String>("211Р01", "Инф"),
                                new KeyValuePair<String, String>("211Р11", "Арх"),
                                new KeyValuePair<String, String>("211Р21", "Эк"),
                                new KeyValuePair<String, String>("211Р31", "УВТС"),
                                new KeyValuePair<String, String>("211Р41", "ГС"),
                                new KeyValuePair<String, String>("211Р61", "ПГС"),
                                new KeyValuePair<String, String>("211Р71", "УЗС"),
                                new KeyValuePair<String, String>("211Р81", "Эн"),
                                new KeyValuePair<String, String>("211Р91", "Диз"),
                                new KeyValuePair<String, String>("211Р111", "ТМС"),
                                new KeyValuePair<String, String>("211Р51", "Ар")
                            },
                        },
                        new Class_JSON_Courses_Settings()
                        {
                            number =4,
                            groups = new List<KeyValuePair<String, String>>()
                            {
                                new KeyValuePair<String, String>("219Р", "Инф")
                            },
                        },
                        new Class_JSON_Courses_Settings()
                        {
                            number =5,
                            groups = new List<KeyValuePair<String, String>>()
                            {
                                new KeyValuePair<String, String>("218Р", "Инф")
                            },
                        },
                        new Class_JSON_Courses_Settings()
                        {
                            number =6,
                            groups = new List<KeyValuePair<String, String>>()
                            {
                                new KeyValuePair<String, String>("217Р", "Инф")
                            },
                        },
                        new Class_JSON_Courses_Settings()
                        {
                            number =7,
                            groups = new List<KeyValuePair<String, String>>()
                            {
                                new KeyValuePair<String, String>("216Р", "Инф")
                            },
                        }
                    };

                    string coursesString = JsonSerializer.Serialize(default_Courses);

                    chkAndCreateFolder("settings");
                    File.WriteAllText($"settings\\{file_name}", coursesString);
                    break;
            }
        }
    }
}
