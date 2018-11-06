using Newtonsoft.Json;
using System;
using System.IO;

namespace CVnHR.Business.Services
{
    public class SettingsService : ISettingsService
    {
        // TODO: DI/IoC for FileSystem to test this class?

        public T GetSettings<T>()
        {
            var fileName = GetFileName<T>();
            if (!File.Exists(fileName))
            {
                var directoryName = Path.GetDirectoryName(fileName);
                if (!Directory.Exists(directoryName))
                {
                    Directory.CreateDirectory(directoryName);
                }

                UpdateSettings(Activator.CreateInstance<T>());
            }
            var item = File.ReadAllText(fileName);
            return JsonConvert.DeserializeObject<T>(item);
        }

        public void UpdateSettings<T>(T newSettings)
        {
            var json = JsonConvert.SerializeObject(newSettings, Formatting.Indented);
            File.WriteAllText(GetFileName<T>(), json);
        }

        private string GetFileName<T>()
        {
            var typeName = typeof(T).Name;
            typeName = $"{char.ToLowerInvariant(typeName[0])}{typeName.Substring(1)}";
            return $"Config/{typeName}.json";
        }
    }
}
