using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;

public class PanelManagerBase<T> : SingletonObjectBase<T>
    where T : ObjectBase
{
    /// <summary>
    /// 最初に開くパネル
    /// </summary>
    private IPresenter _firstPanel;
    private IPresenter _targetPanel;
    /// <summary>
    /// 操作するパネル
    /// </summary>
    protected IPresenter TargetPanel => _targetPanel;

    /// <summary>
    /// 最初のパネルを設定
    /// </summary>
    /// <param name="panel"></param>
    protected void SetFirstPanel(IPresenter panel)
    {
        _firstPanel = panel;
    }

    /// <summary>
    /// 最初のパネルを開く
    /// </summary>
    /// <param name="ct"></param>
    /// <returns></returns>
    public virtual async UniTask OpenFirstPanelAsync(CancellationToken ct)
    {
        await OpenPanelAsync(_firstPanel, ct);
    }

    /// <summary>
    /// パネルを開く(現在開いているパネルは閉じられる)
    /// </summary>
    /// <param name="panel"> 開くパネル </param>
    /// <param name="ct"></param>
    /// <returns></returns>
    public async UniTask OpenPanelAsync(IPresenter panel, CancellationToken ct)
    {
        var tasks = new List<UniTask>();

        // 現在開いているパネルを閉じる
        if (_targetPanel != null)
        {
            tasks.Add(_targetPanel.HideAsync(ct));
        }

        // これから開くパネルを設定
        _targetPanel = panel;
        tasks.Add(_targetPanel.ShowAsync(ct));

        await UniTask.WhenAll(tasks);
    }

    /// <summary>
    /// パネルを閉じる
    /// </summary>
    /// <param name="ct"></param>
    /// <returns></returns>
    public virtual async UniTask ClosePanelAsync(CancellationToken ct)
    {
        // 閉じるパネルがない場合は終了
        if (_targetPanel == null)
        {
            return;
        }
        
        var tasks = new List<UniTask>();
        tasks.Add(_targetPanel.HideAsync(ct));
        
        // 最初のパネルがあれば開く
        if (_targetPanel != _firstPanel)
        {
            tasks.Add(_firstPanel.ShowAsync(ct));
            _targetPanel = _firstPanel;
        }
        else
        {
            _targetPanel = null;
        }

        await UniTask.WhenAll(tasks);
    }

    /// <summary>
    /// 閉じるボタンのイベント設定
    /// </summary>
    /// <param name="closeButton"> 閉じるボタン
    /// </param>
    public void SetEventCloseButton(ButtonBase closeButton, CancellationToken ct)
    {
        // ボタンをクリックしたら、現在開いているパネルを閉じる
        closeButton.OnClickCallback += async () =>
        {
            await ClosePanelAsync(ct);
        };
    }
}
