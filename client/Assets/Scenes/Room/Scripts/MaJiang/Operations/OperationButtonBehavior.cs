using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Common;
public class OperationButtonBehavior : MonoBehaviour 
{
	[SerializeField] private tk2dBaseSprite m_Sprite;
	[SerializeField] private GameObject m_ButtonsGroup;
	[SerializeField] private GameObject m_SelfShouPaiParent;     
    [SerializeField] protected PaiFactory m_PaiFactory;
    [SerializeField] protected OperationTimeManager m_TimeManager;
    private bool m_IsOperatable;
    public bool IsOperatable { get { return m_IsOperatable; } set { m_IsOperatable = value; this.m_Sprite.color = m_IsOperatable ? Color.white : new Color(0, 0, 0, 0); } }

 

	public void OnClick()
	{
		if(this.IsOperatable)
		{ 
			this.Process();
		}
	}
    public void StopCount()
    {
        this.m_TimeManager.StopCount();
    }
	protected virtual void Process()
	{
	}

	/*
	protected virtual bool IsSetUnselectableOnClick
	{
		get { return true; }
	}
	*/

	public void SetAllUnselectable(bool enableInput ,bool enableHightlight, bool acitveButtonGroup = false)
	{
        this.m_ButtonsGroup.SetActive(acitveButtonGroup);

		var shouPais = this.m_SelfShouPaiParent.GetComponentsInChildren<ShouPaiBehavior>();
		foreach (var item in shouPais)
		{
            item.IsSelectable = enableInput;
            item.Hightlight(enableHightlight);
            item.IsGangSelectable = false;
		}
	}
    public void SetPaiSeletable(int pai, bool enableInput, bool enableHightlight)
    {
        var shouPais = this.m_SelfShouPaiParent.GetComponentsInChildren<ShouPaiBehavior>();
        foreach (var item in shouPais)
        {
            if (pai == item.Pai)
            {
                item.IsSelectable = enableInput;
                item.Hightlight(enableHightlight);
            }
        }
    }
    public void SetGangSelectable(bool enableInput, bool enableHightlight, List<ShouPaiBehavior> listShouPaiBehavior, List<int> canGangPais)
    {
        this.m_ButtonsGroup.SetActive(false);
        foreach (var item in listShouPaiBehavior)
        {
            item.IsGangSelectable = enableInput;
            item.Hightlight(enableHightlight);
        }

        foreach (int item in canGangPais)
        {
            foreach (ShouPaiBehavior spb in listShouPaiBehavior)
            {
                if (spb.Pai / 4 == item / 4)
                {
                    spb.GangPai = item;
                }
            }
        }
    }
    public void Reset()
    {
        var shouPais = this.m_SelfShouPaiParent.GetComponentsInChildren<ShouPaiBehavior>();
        foreach (var spb in shouPais)
       // foreach (KeyValuePair<int, ShouPaiBehavior> spb in m_PaiFactory.SelfShouPais)
        {
            //spb.Value.Reset();
            spb.Reset();
        }
    }
 
    //public void SetDinQueSelecable(HuaSeType huaSeType)
    //{
    //    var shouPais = this.m_SelfShouPaiParent.GetComponentsInChildren<ShouPaiBehavior>();
    //    foreach (var item in shouPais)
    //    {
    //        bool isDinQue = CommonUtility.GetHuaSeFromId(item.Pai) == huaSeType;

    //        item.IsSelectable = isDinQue;
    //        item.Hightlight(isDinQue);
          
    //    }
    //}
}
