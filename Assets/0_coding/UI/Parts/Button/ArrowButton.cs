using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;

public class ArrowButton : ButtonBase
{
    private bool _isHide;
    protected override void SetEventDobleClickPrevention()
    {
        OnClickCallback += async () =>
        {
            if(_isHide)
            {
                return;
            }

            ChangeInteractive(false);
            await UniTask.WaitForSeconds(0.1f, cancellationToken: Ct);

            if (Ct.IsCancellationRequested) return;
            ChangeInteractive(true);
        };
    }
    public void SetisHide(bool ishide)
    {
        _isHide =ishide;
        ChangeInteractive(ishide);
    }
}
