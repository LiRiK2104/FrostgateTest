using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using Unity.Services.CloudSave;
using Unity.Services.CloudSave.Models;

namespace Core.Services.DataSaving.Behaviours
{
    public class CloudSavingBehaviour : SavingBehaviour
    {
        private Dictionary<string, Item> _data;
        
        
        internal override async void Init()
        {
            _data = await CloudSaveService.Instance.Data.Player.LoadAllAsync();
        }

        internal override async UniTask<T> Load<T>(string key, T defaultValue)
        {
            if (_data.TryGetValue(key, out Item value) == false)
            {
                _data = await CloudSaveService.Instance.Data.Player.LoadAsync(new HashSet<string> { key });
            }

            if (_data.TryGetValue(key, out value) == false)
            {
                return defaultValue;
            }

            string json = value.Value.GetAs<string>();

            return JsonConvert.DeserializeObject<T>(json);
        }

        internal override async UniTask Save<T>(string key, T data)
        {
            string json = JsonConvert.SerializeObject(data);

            Dictionary<string, object> stringData = new() { { key, json } };

            await CloudSaveService.Instance.Data.Player.SaveAsync(stringData);
            
            _data = await CloudSaveService.Instance.Data.Player.LoadAllAsync();
        }
    }
}