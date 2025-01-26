using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

/// <summary>
/// カエルのオブジェクトにする処理
/// </summary>
public class Frog : GameObjectBase
{
    [Header("カエルオブジェクトリスト")]
    [SerializeField]
    private List<FlogData> _flogGameObjects = new List<FlogData>();
    [Header("進化時のパーティクルシステム")]
    [SerializeField]
    private ParticleSystem _evoParticleSystem;
    [Header("クリア時のパーティクルシステム")]
    [SerializeField]
    private ParticleSystem _clearParticleSystem;
    [Header("アニメーションの時間")]
    [SerializeField]
    private float _animationTime = 1.0f;

    private ReactiveProperty<EvolutionaryType> _type = new ReactiveProperty<EvolutionaryType>(EvolutionaryType.Egg);
    /// <summary>
    /// カエルの進化状態
    /// </summary>
    public ReactiveProperty<EvolutionaryType> Type => _type;

    // 現在表示中のカエルの進化系
    private GameObject _showObject;
    // カエルの進化系とそれに対応したオブジェクト
    private Dictionary<EvolutionaryType, GameObject> _flogDict = new Dictionary<EvolutionaryType, GameObject>();

    protected override void Init()
    {
        base.Init();
        InitFlogDictionary();
        InitFrog();
    }

    /// <summary>
    /// カエルのオブジェクトを辞書に登録
    /// </summary>
    private void InitFlogDictionary()
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
        // すべての進化系のオブジェクトを非表示にする
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
        // 進化したら対応するオブジェクトを表示する
        _type
            .TakeUntilDestroy(this)
            .DistinctUntilChanged()
            .Subscribe(type =>
            {
                // 表示しているオブジェクトを非表示
                if(_showObject != null)
                {
                    _showObject.SetActive(false);
                }

                // 次の進化が何もない状態の場合は終了
                if(type == EvolutionaryType.None)
                {
                    _showObject = null;
                    return;
                }

                _showObject = _flogDict[type];
                _showObject.SetActive(true);
            });

        // 進化したときに対応したSEを鳴らす
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
        // 卵->オタマジャクシ->カエル->卵->・・・の順
        int value = (int)Type.Value + 1; 
        int nextType = value <= 0 || value > 3 ? 1 : value;
        _type.Value = (EvolutionaryType)nextType;
        
        // 進化のパーティクル再生
        _evoParticleSystem.Play();
    }

    /// <summary>
    /// お題クリアアニメーションの再生
    /// </summary>
    public async UniTask StartClearAnimationAsync()
    {
        // すでに再生中の場合はなにもしない
        if (_clearParticleSystem.isPlaying)
        {
            return;
        }

        // 再生終了まで待つ
        _clearParticleSystem.Play();
        await UniTask.WaitForSeconds(_animationTime);
        _clearParticleSystem.Stop();
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

/// <summary>
/// カエルのオブジェクトデータ
/// </summary>
[System.Serializable]
public class FlogData
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