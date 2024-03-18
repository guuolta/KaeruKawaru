using UnityEngine.UI;

public class ScrollViewBase : UIBase
{
    private ScrollRect _scrollRect;
    public ScrollRect ScrollRect
    {
        get
        {
            if(_scrollRect == null)
            {
                _scrollRect = GetComponent<ScrollRect>();
            }

            return _scrollRect;
        }
    }

    private Scrollbar _scrollbar;
    public Scrollbar Scrollbar
    {
        get
        {
            if(_scrollbar == null)
            {
                _scrollbar = ScrollRect.verticalScrollbar;
            }

            return _scrollbar;
        }
    }
}
