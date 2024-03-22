using System;

/// <summary>
/// シングルトンパターンのオブジェクトベース
/// </summary>
/// <typeparam name="T"> 対象のクラス </typeparam>
public class SingletonObjectBase<T> : ObjectBase
    where T : ObjectBase
{
    private static T _instance;
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                Type type = typeof(T);

                _instance = (T)FindObjectOfType(type);
            }

            return _instance;
        }
    }

    protected override void Init()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }
    }
}