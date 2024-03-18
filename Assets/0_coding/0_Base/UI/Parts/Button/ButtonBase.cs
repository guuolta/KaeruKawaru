using Cysharp.Threading.Tasks;
using UnityEngine;

public class ButtonBase : AnimationPartBase
{
    [Header("SE")]
    [SerializeField]
    private SEType _seType = SEType.Posi;

    protected override void Init()
    {
        ChangeInteractive(true);
    }

    protected override void SetEvent()
    {
        SetEventPlaySe();
        SetEventDobleClickPrevention();
    }

    /// <summary>
    /// SEを鳴らす
    /// </summary>
    protected void SetEventPlaySe()
    {
        if (_seType == SEType.None)
            return;

        OnClickCallback += () =>
        {
            AudioManager.Instance.PlayOneShotSE(_seType);
        };
    }

    /// <summary>
    /// 連打防止
    /// </summary>
    protected virtual void SetEventDobleClickPrevention()
    {
        OnClickCallback += async () =>
        {
            ChangeInteractive(false);
            await UniTask.WaitForSeconds(0.1f, cancellationToken: Ct);
            ChangeInteractive(true);
        };
    }
}
