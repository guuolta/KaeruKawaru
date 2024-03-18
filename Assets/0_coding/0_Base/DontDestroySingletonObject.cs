public class DontDestroySingletonObject<T> : SingletonObjectBase<T>
    where T : ObjectBase
{
    protected override void Init()
    {
        base.Init();
        DontDestroyOnLoad(gameObject);
    }
}
