using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Rendering;

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
        InitFrog();
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
    /// カエルの初期化
    /// </summary>
    private void InitFrog()
    {
        foreach (var flog in _flogGameObjects)
        {
            flog.FrogObject.SetActive(false);
        }
    }

    protected override void SetEvent()
    {
        base.SetEvent();
        SetEventEvolve();
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
                if(_showObject != null)
                {
                    _showObject.SetActive(false);
                }

                if(type == EvolutionaryType.None)
                {
                    _showObject = null;
                    return;
                }

                _showObject = _flogDict[type];
                _showObject.SetActive(true);
            });

        _type
            .Skip(1)
           .TakeUntilDestroy(this)
           .DistinctUntilChanged()
           .Subscribe(type =>
           {
               switch (type)
               {
                   case EvolutionaryType.Egg:
                       AudioManager.Instance.PlayOneShotSE(SEType.Evo1);
                       break;
                   case EvolutionaryType.Tadpole:
                       AudioManager.Instance.PlayOneShotSE(SEType.Evo2);
                       break;
                   case EvolutionaryType.Frog:
                       AudioManager.Instance.PlayOneShotSE(SEType.Evo3);
                       break;
               }
           });
    }

    /// <summary>
    /// 次の進化状態へ
    /// </summary>
    public void Evolve()
    {
        int value = (int)Type.Value + 1; 
        int nextType = value <= 0 || value > 3 ? 1 : value;
        _type.Value = (EvolutionaryType)nextType;
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

[System.Serializable]
public class FlogGameObject
{
    [Header("進化状態")]
    [SerializeField]
    private EvolutionaryType _type;
    /// <summary>
    /// 進化状態
    /// </summary>
    public EvolutionaryType Type => _type;
    [Header("カエルオブジェクト")]
    [SerializeField]
    private GameObject _frogObject;
    /// <summary>
    /// カエルオブジェクト
    /// </summary>
    public GameObject FrogObject => _frogObject;
}