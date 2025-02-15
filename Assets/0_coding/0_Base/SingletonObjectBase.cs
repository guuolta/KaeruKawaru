using System;
using UnityEngine;

/// <summary>
/// シングルトンパターンの基底クラス
/// </summary>
/// <typeparam name="T"> 対象のクラス </typeparam>
public class SingletonObjectBase<T> : MonoBehaviour
    where T : MonoBehaviour
{
    private static T _instance;
    public static T Instance
    {
        get
        {
            if (_instance is null)
            {
                _instance = FindObjectOfType<T>();

                if (_instance is null)
                {
                    Debug.LogError(typeof(T) + "is nothing");
                }
            }

            return _instance;
        }
    }

    protected virtual void Awake()
    {
        if (this != Instance)
        {
            Destroy(this);
        }
    }
        
    protected virtual void OnDestroy()
    {
        if (this == Instance)
        {
            _instance = null;
        }
    }
}