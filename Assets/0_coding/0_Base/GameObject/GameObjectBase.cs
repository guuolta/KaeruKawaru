using UnityEngine;

/// <summary>
/// ゲームオブジェクトのベース
/// </summary>
public class GameObjectBase : ObjectBase
{
    private GameObject _gameObject;
    /// <summary>
    /// ゲームオブジェクト
    /// </summary>
    public GameObject GameObject
    {
        get
        {
            if (_gameObject == null)
                _gameObject = gameObject;

            return _gameObject;
        }
    }

    private Transform _transform;
    /// <summary>
    /// トランスフォーム
    /// </summary>
    public Transform Transform
    {
        get
        {
            if (_transform == null)
                _transform = transform;

            return _transform;
        }
    }
}
