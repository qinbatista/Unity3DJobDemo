
using UnityEngine;
public abstract class Singleton<T> : Singleton where T : Singleton<T>
{
    static T _instance;
    public static T Instance { get => _instance; set => _instance = value; }
    public virtual void Awake()
    {
        if (_instance != null) { Destroy(gameObject); } else { _instance = (T)this; }
    }
    public virtual void OnDestroy()
    {
        _instance = null;
    }
}
public abstract class Singleton : MonoBehaviour
{

}