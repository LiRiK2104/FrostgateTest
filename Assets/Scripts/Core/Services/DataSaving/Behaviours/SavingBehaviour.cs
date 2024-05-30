using Cysharp.Threading.Tasks;

namespace Core.Services.DataSaving.Behaviours
{
    public abstract class SavingBehaviour
    {
        internal abstract void Init();
        
        internal abstract UniTask<T> Load<T>(string key, T defaultValue);
    
        internal abstract UniTask Save<T>(string key, T data);
    }
}
