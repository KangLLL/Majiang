using UnityEngine;
using System.Collections;

public class tk2dButtonSwith : MonoBehaviour {
    [SerializeField]private tk2dUIItem uiItem;
    [SerializeField]private bool useOnReleaseInsteadOfOnUp = false;
    [SerializeField]private tk2dSprite m_Button;
    [SerializeField]private string m_Btn1Up;
    [SerializeField]private string m_Btn1Dwon;
    [SerializeField]private string m_Btn2Up;
    [SerializeField]private string m_Btn2Down;
    public bool UseOnReleaseInsteadOfOnUp
    {
        get { return useOnReleaseInsteadOfOnUp; }
    }

    private bool isDown = false;
    private bool isFirstButton = true;
    public bool FisrtButton { get { return isFirstButton; } set { isFirstButton = value; this.SetState(); } }
    void Start()
    {
        SetState();
    }

    void OnEnable()
    {
        if (uiItem)
        {
            uiItem.OnDown += ButtonDown;
            if (useOnReleaseInsteadOfOnUp)
            {
                uiItem.OnRelease += ButtonUp;
            }
            else
            {
                uiItem.OnUp += ButtonUp;
            }
        }
    }

    void OnDisable()
    {
        if (uiItem)
        {
            uiItem.OnDown -= ButtonDown;
            if (useOnReleaseInsteadOfOnUp)
            {
                uiItem.OnRelease -= ButtonUp;
            }
            else
            {
                uiItem.OnUp -= ButtonUp;
            }
        }
    }

    private void ButtonUp()
    {
        isDown = false;
        SetState();
    }

    private void ButtonDown()
    {
        isDown = true;
        SetState();
    }

    private void SetState()
    {
        if (isFirstButton)
        {
            m_Button.SetSprite(isDown ? m_Btn1Dwon : m_Btn1Up);
        }
        else
        {                             
            m_Button.SetSprite(isDown ? m_Btn2Down : m_Btn2Up);
        }
    }

    /// <summary>
    /// Internal do not use
    /// </summary>
    public void InternalSetUseOnReleaseInsteadOfOnUp(bool state)
    {
        useOnReleaseInsteadOfOnUp = state;
    }
}
