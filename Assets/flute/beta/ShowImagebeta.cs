using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class ShowImagebeta : MonoBehaviour
{
    public Timerbeta m_gameTimer;
    public Image m_imgTarget;
    //public float m_fTargetTime;

    //public bool m_bIncremental = true;

    void Start()
    {
        Observable.EveryUpdate()
            .Subscribe(_ => 
                m_imgTarget.fillAmount = m_gameTimer.CurrentTime / m_gameTimer.maxtime);
    }
    /*void Update()
    {
        m_imgTarget.fillAmount = m_gameTimer.CurrentTime / m_gameTimer.maxtime;
        /*if (m_bIncremental)
        {
            m_imgTarget.fillAmount = m_gameTimer.CurrentTime / m_fTargetTime;
        }
        else
        {
            m_imgTarget.fillAmount = (m_fTargetTime - m_gameTimer.CurrentTime) / m_fTargetTime;
        }
    }/**/
}
