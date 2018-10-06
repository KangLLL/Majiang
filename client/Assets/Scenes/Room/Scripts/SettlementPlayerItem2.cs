using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class SettlementPlayerItem2 : MonoBehaviour {
    [SerializeField] TextMesh m_PlayerName1;
    [SerializeField] TextMesh m_PlayerName2;
    [SerializeField] TextMesh m_WindRain;
    [SerializeField] TextMesh m_ZhiMoJiaDi;
    [SerializeField] TextMesh m_ChaHuaZhu;
    [SerializeField] TextMesh m_Sum;

    public void SetItemData(string playerID , SettlementParameter param)
    {
        gameObject.SetActive(true);
        m_PlayerName1.text = playerID;
        m_PlayerName2.text = playerID;
        m_WindRain.text = param.WindRain.ToString();
        m_ZhiMoJiaDi.text = param.ZiMoJiaDi.ToString();
        m_ChaHuaZhu.text = param.ChaHuaZhu.ToString();
        m_Sum.text = param.Sum.ToString();
    }
}
