using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Common;
using System;
using CommandConsts;
public class WinDinQue : WindowBase {
    [SerializeField] tk2dSprite[] m_PaisSprites;
    [SerializeField] List<PaisName> m_PaisName;

    private bool[] m_EnableDinQue = new bool[3];
	// Use this for initialization
	void Start () {
	
	}

    public void ShowWindow(IEnumerable<int> pais)
    {
        this.Reset();
        List<HuaSeType> huaSeList = new List<HuaSeType>();
        foreach(int item in pais)
        { 
           HuaSeType huase = CommonUtility.GetHuaSeFromId(item);
           if (!huaSeList.Contains(huase)) huaSeList.Add(huase);
        }
        if (huaSeList.Count == 3)
        {
            for (int i = 0; i < m_PaisSprites.Length; i++)
            {
                m_PaisSprites[i].SetSprite(m_PaisName[i].paisNames[1]);
                m_EnableDinQue[i] = true;
            }
        }
        else
        {
            Array enmuArray = Enum.GetValues(typeof(HuaSeType));
            for (int i = 0; i < enmuArray.Length - 1; i++)
            {
                if (!huaSeList.Contains((HuaSeType)enmuArray.GetValue(i)))
                {
                    m_PaisSprites[i].SetSprite(m_PaisName[i].paisNames[1]);
                    m_EnableDinQue[i] = true;
                }
            }
        }
        base.ShowWindow();
    }
 
    public void WanOnDown()
    {
        if (m_EnableDinQue[0]) m_PaisSprites[0].SetSprite(m_PaisName[0].paisNames[2]);
    }

    public void WanOnRelease()
    {
        if (m_EnableDinQue[0]) m_PaisSprites[0].SetSprite(m_PaisName[0].paisNames[1]);
    }

    public void WanOnClick()
    {
        if (m_EnableDinQue[0])
        {
            m_PaisSprites[0].SetSprite(m_PaisName[0].paisNames[1]);
            CommunicationUtility.Instance.DingQue(new MaJiangDingQueRequestParameter() { HuaSe = HuaSeType.Wan, PlayerId = PlayerInformation.Instance.PlayerID });
           StartCoroutine(DelayHideWindow());
        } 
    }
    public void TiaoOnDown()
    {
        if (m_EnableDinQue[1]) m_PaisSprites[1].SetSprite(m_PaisName[1].paisNames[2]);
    }
    public void TiaoOnRelease()
    {
        if (m_EnableDinQue[1]) m_PaisSprites[1].SetSprite(m_PaisName[1].paisNames[1]); 
    }
    public void TiaoOnClick()
    {
        if (m_EnableDinQue[1]) 
        {
            m_PaisSprites[1].SetSprite(m_PaisName[1].paisNames[1]);
            CommunicationUtility.Instance.DingQue(new MaJiangDingQueRequestParameter() { HuaSe = HuaSeType.Tiao, PlayerId = PlayerInformation.Instance.PlayerID });

           StartCoroutine(DelayHideWindow());
        }
        ////
    }
    public void TongOnDown()
    {
        if (m_EnableDinQue[2]) m_PaisSprites[2].SetSprite(m_PaisName[2].paisNames[2]);
    }
    public void TongOnRelease()
    {
        if (m_EnableDinQue[2]) m_PaisSprites[2].SetSprite(m_PaisName[2].paisNames[1]);
    }
    public void TongOnClick()
    {
        if (m_EnableDinQue[2])
        {
            m_PaisSprites[2].SetSprite(m_PaisName[2].paisNames[1]);
            CommunicationUtility.Instance.DingQue(new MaJiangDingQueRequestParameter() { HuaSe = HuaSeType.Tong, PlayerId = PlayerInformation.Instance.PlayerID });
            StartCoroutine(DelayHideWindow());
        }
    }
    private void Reset()
    {
        for (int i = 0; i < 3; i++)
        {
            m_EnableDinQue[i] = false;
            m_PaisSprites[i].SetSprite(m_PaisName[i].paisNames[0]);
        }
    }
    private IEnumerator DelayHideWindow()
    {
        yield return new WaitForSeconds(0.01f);
        base.HideWindow();
    }
}
[Serializable]
public class PaisName
{
    public List<string> paisNames;
}