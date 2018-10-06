using UnityEngine;
using System.Collections;
using Common;

public class PlayerStatus : MonoBehaviour {
    [SerializeField] tk2dSprite[] m_Banker;
    [SerializeField] tk2dSprite[] m_DinQueSprite;
    [SerializeField] string[] m_DinQueSpriteName;
    [SerializeField] tk2dSprite[] m_Offline;
    private int m_BankerPostion = -1;
    
    public void SetOffline(bool isOffline,int room)
    {
        m_Offline[room].gameObject.SetActive(isOffline);
    }
    public void SetDinQueType(HuaSeType type, int room)
    { 
        m_DinQueSprite[room].SetSprite(this.m_DinQueSpriteName[(int)type]);
        m_DinQueSprite[room].gameObject.SetActive(true);
    }
    public void SetBanker(bool isBanker, int room)
    {
        if (this.m_BankerPostion < 0)
        {
            this.m_BankerPostion = room;
            this.m_Banker[room].gameObject.SetActive(isBanker);
        }
    }
    public void Reset(int room)
    {
        m_Banker[room].gameObject.SetActive(false);
        m_DinQueSprite[room].gameObject.SetActive(false);
        m_Offline[room].gameObject.SetActive(false);
    }
    public void ResetAll()
    {
        for (int i = 0; i < 4; i++)
        {
            m_Banker[i].gameObject.SetActive(false);
            m_DinQueSprite[i].gameObject.SetActive(false);
            m_Offline[i].gameObject.SetActive(false);
            this.m_BankerPostion = -1;
        }
    }
}
