using UnityEngine;

public class ConfirmPanelView : PanelViewBase
{
    [Header("はいボタン")]
    [SerializeField]
    private ButtonBase _yesButton;
    /// <summary>
    /// はいボタン
    /// </summary>
    public ButtonBase YesButton => _yesButton;
    [Header("いいえボタン")]
    [SerializeField]
    private ButtonBase _noButton;
    /// <summary>
    /// いいえボタン
    /// </summary>
    public ButtonBase NoButton => _noButton;
}
