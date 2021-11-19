using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinRun.UserWindowSizes
{
    public class SizeRepository
    {
        private const string FileName = "UserSizes.json";

        private Dictionary<string, SizeModel> Load()
        {
            if (File.Exists(FileName))
            {
                string json = File.ReadAllText(FileName);
                return JsonConvert.DeserializeObject<Dictionary<string, SizeModel>>(json);
            }

            return new Dictionary<string, SizeModel>();
        }

        private void Save(Dictionary<string, SizeModel> storage)
        {
            string json = JsonConvert.SerializeObject(storage);
            File.WriteAllText(FileName, json);
        }

        public void Set(string name, SizeModel model)
        {
            var storage = Load();
            storage[name] = model;
            Save(storage);
        }

        public IReadOnlyCollection<string> GetNames() 
            => Load().Keys;

        public SizeModel Find(string name)
        {
            var storage = Load();
            if (storage.TryGetValue(name, out var model))
                return model;

            return null;
        }
    }
}
