using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.IO;

namespace LazyAdmin.DataBase
{
    class FileIOService
    {
        private readonly string PATH;
        public FileIOService(string path)
        {
            PATH = path;
        }
        public ObservableCollection<Asset> LoadData()
        {
            var fileExists = File.Exists(PATH);
            if (!fileExists)
            {
                File.CreateText(PATH).Dispose();
                return new ObservableCollection<Asset>();
            }
            using (var reader = File.OpenText(PATH))
            {
                var fileText = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<ObservableCollection<Asset>>(fileText);
            }
        }
        public void SaveData(object GridOfAssets)
        {
            using (StreamWriter writer = File.CreateText(PATH))
            {
                string output = JsonConvert.SerializeObject(GridOfAssets);
                writer.Write(output);
            }
        }
    }
}
