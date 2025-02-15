using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

/// <summary>
/// UIの基底クラス
/// </summary>
[RequireComponent(typeof(RectTransform), typeof(CanvasGroup))]
public class UIBase: UIBehaviour
{
    /* コンポーネント */
    [SerializeField, HideInInspector]
    private GameObject _gameObject;
    public GameObject GameObject => _gameObject;
    [SerializeField, HideInInspector]
    private Transform _transform;
    public Transform Transform => _transform;
    
    [SerializeField, HideInInspector]
    private RectTransform _rectTransform;
    public RectTransform RectTransform => _rectTransform;

    [SerializeField, HideInInspector]
    private CanvasGroup _canvasGroup;
    public CanvasGroup CanvasGroup => _canvasGroup;

    /* 数値 */
    [Header("押せないときの透明度")]
    [Range(0f, 1f)]
    [SerializeField]
    private float _disInteractiveAlpha = 0.8f;
    /// <summary>
    /// アニメーションの時間
    /// </summary>
    [FormerlySerializedAs("AnimationTime")]
    [Header("アニメーションの時間(秒)")]
    [Range(0f, 10f)]
    [SerializeField]
    protected float AnimationSec = 0.1f;
    
#if UNITY_EDITOR
    protected virtual void OnValidate()
    {
        _gameObject ??= gameObject;
        _transform ??= transform;
        _rectTransform ??= GetComponent<RectTransform>();
        _canvasGroup ??= GetComponent<CanvasGroup>();
    }
#endif

    /// <summary>
    /// 初期化
    /// </summary>
    public virtual void Init()
    {
        SetEvent();
    }

    /// <summary>
    /// イベント設定
    /// </summary>
    protected virtual void SetEvent()
    {
        
    }

    /// <summary>
    /// サイズを徐々に変える
    /// </summary>
    /// <param name="size">最終サイズ</param>
    /// <param name="ease"></param>
    protected async UniTask DoScaleAsync(float size, Ease ease)
    {
        // アニメーションをしていたら即座に最終状態に
        Transform.DOComplete();
        
        await Transform
            .DOScale(size, AnimationSec)
            .SetEase(ease)
            .ToUniTask(cancellationToken: destroyCancellationToken);
    }

    /// <summary>
    /// 透明度を徐々に変える
    /// </summary>
    /// <param name="alpha">最終透明度</param>
    /// <param name="ease"></param>
    protected async UniTask DoFadeAsync(float alpha, Ease ease)
    {
        // アニメーションをしていたら即座に最終状態に
        CanvasGroup.DOComplete();
        
        await CanvasGroup.DOFade(alpha, AnimationSec)
            .SetEase(ease)
            .ToUniTask(cancellationToken: destroyCancellationToken);
    }
    
    /// <summary>
    /// UIを触れるようにするか設定
    /// </summary>
    /// <param name="isInteractive">押せるか</param>
    public void ChangeInteractive(bool isInteractive)
    {
        CanvasGroup.interactable = isInteractive;
        CanvasGroup.blocksRaycasts = isInteractive;
        CanvasGroup.alpha = isInteractive ? 1f : _disInteractiveAlpha;
    }
}