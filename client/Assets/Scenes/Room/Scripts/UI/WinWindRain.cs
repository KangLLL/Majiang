using UnityEngine;
using System.Collections;
using CommandConsts;

public class WinWindRain : WindowBase {
    [SerializeField] SettlementPlayerItem3 m_SettlementPlayerItem3;
	// Use this for initialization
	void Start () {
	
	}
    public void ShowWindow(MaJiangGangPaiNotifySelfParameter param)
    {
        m_SettlementPlayerItem3.SetSelfGangPaiParameter(param);
        base.ShowWindow();
        StartCoroutine(DelayHideWindow());
  
    }
    public void ShowWindow(MaJiangGangPaiNotifyOtherParameter param)
    {
        m_SettlementPlayerItem3.SetOtherGangPaiParameter(param);
        base.ShowWindow();
        StartCoroutine(DelayHideWindow());
    }
    //public void ShowWindow(MaJiangQiangGangNotifySelfParameter param)
    //{ 
    //    base.ShowWindow();
    //}
    public void ShowWindow(MaJiangHuPaiNotifySelfParameter param)
    { 
        m_SettlementPlayerItem3.SetSelfHuPaiParameter(param);
        base.ShowWindow();
        StartCoroutine(DelayHideWindow());
    }
    public void ShowWindow(MaJiangHuPaiNotifyOtherParameter param)
    { 
        m_SettlementPlayerItem3.SetOtherHuPaiParameter(param);
        base.ShowWindow();
        StartCoroutine(DelayHideWindow());
    }
 
    public override void HideWindow()
    {
        base.HideWindow();
    }
    private IEnumerator  DelayHideWindow()
    {
        yield return new WaitForSeconds(5f);
        base.HideWindow();
    }
 
}
