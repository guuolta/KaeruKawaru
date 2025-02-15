using UnityEngine;

/// <summary>
/// シーン移動で壊れないシングルトンの基底クラス
/// </summary>
/// <typeparam name="T"></typeparam>
public class DontDestroySingletonObject<T> : SingletonObjectBase<T>
    where T : MonoBehaviour
{
    protected override void Awake()
    {
        if (this != Instance)
        {
            Destroy(this);
        }
        else
        {
            DontDestroyOnLoad(gameObject);   
        }
    }
}
