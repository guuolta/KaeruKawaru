using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class Frog : GameObjectBase
{
    [Header("カエルオブジェクトリスト")]
    [SerializeField]
    private List<FlogGameObject> _flogGameObjects = new List<FlogGameObject>();

    private ReactiveProperty<EvolutionaryType> _type = new ReactiveProperty<EvolutionaryType>(EvolutionaryType.Egg);
    /// <summary>
    /// カエルの状態
    /// </summary>
    public ReactiveProperty<EvolutionaryType> Type => _type;


    private GameObject _showObject;
    private Dictionary<EvolutionaryType, GameObject> _flogDict = new Dictionary<EvolutionaryType, GameObject>();

    protected override void Init()
    {
        base.Init();
        SetFlogDictionary();
    }

    protected override void SetEvent()
    {
        base.SetEvent();
        SetEventEvolve();
    }

    /// <summary>
    /// カエルのオブジェクトを辞書に登録
    /// </summary>
    private void SetFlogDictionary()
    {
        foreach (var flog in _flogGameObjects)
        {
            _flogDict.Add(flog.Type, flog.FrogObject);
        }
    }

    /// <summary>
    /// 進化したときのイベント
    /// </summary>
    private void SetEventEvolve()
    {
        _type
            .TakeUntilDestroy(this)
            .DistinctUntilChanged()
            .Subscribe(type =>
            {
                _showObject.SetActive(false);
                if(type == EvolutionaryType.None)
                {
                    _showObject = null;
                    return;
                }

                _showObject = _flogDict[type];
                _showObject.SetActive(true);
            });
    }

    /// <summary>
    /// 次の進化状態へ
    /// </summary>
    public void Evolve()
    {
        _type.Value = (EvolutionaryType)(Mathf.Clamp((int)Type.Value + 1, 0, 3));
    }
}

/// <summary>
/// カエルの進化状態
/// </summary>
public enum EvolutionaryType
{
    None,
    Egg,
    Tadpole,
    Frog
}

public class FlogGameObject
{
    [Header("進化状態")]
    [SerializeField]
    private EvolutionaryType _type;
    /// <summary>
    /// 進化状態
    /// </summary>
    public EvolutionaryType Type => _type;
    [Header("返るオブジェクト")]
    [SerializeField]
    private GameObject _frogObject;
    /// <summary>
    /// カエルオブジェクト
    /// </summary>
    public GameObject FrogObject => _frogObject;
}