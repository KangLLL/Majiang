using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CommandConsts;
using Common;

public class SettlementPlayerItem3 : MonoBehaviour {
    [SerializeField] TextMesh m_SelfName;
    [SerializeField] TextMesh m_OtherName;
    [SerializeField] tk2dSprite m_SelfIcon;
    [SerializeField] tk2dSprite m_OtherIcon;
    [SerializeField] TextMesh m_SelftBonus;
    [SerializeField] TextMesh m_OtherBonus;

    [SerializeField] tk2dSprite m_Title;

    public void SetSelfHuPaiParameter(MaJiangHuPaiNotifySelfParameter param)
    {
        m_SelfName.text = PlayerInformation.Instance.PlayerID;
        bool isZiMo = this.IsBonusType(param.BounsTypes, BounsType.ZiMo); 
        m_OtherName.text =  isZiMo?  string.Format(StringConsts.GUANG_JIA,  string.Join(StringConsts.SPACING, param.FangPaoPlayerIds.ToArray()) ,param.FangPaoPlayerIds.Count) : param.FangPaoPlayerIds[0];
        
        m_SelfIcon.SetSprite(isZiMo ? "Settlement_ZhiMo" : "Settlement_Hu");
        m_OtherIcon.gameObject.SetActive(!isZiMo);
        m_OtherIcon.SetSprite("Settlement_FangPao");
        m_SelftBonus.text = string.Empty;
        m_OtherBonus.text = string.Empty;
        m_Title.SetSprite(isZiMo ? "TitleZiMo" : "TitleHu");
    }
    public void SetOtherHuPaiParameter(MaJiangHuPaiNotifyOtherParameter param)
    {
        m_SelfName.text = param.PlayerId;
        bool isZiMo = this.IsBonusType(param.BounsTypes, BounsType.ZiMo);
        m_OtherName.text = isZiMo ? string.Format(StringConsts.GUANG_JIA, string.Join(StringConsts.SPACING, param.FangPaoPlayerIds.ToArray()), param.FangPaoPlayerIds.Count) : param.FangPaoPlayerIds[0];
       
        m_SelfIcon.SetSprite(isZiMo ? "Settlement_ZhiMo" : "Settlement_Hu");
        m_OtherIcon.gameObject.SetActive(!isZiMo);
        m_OtherIcon.SetSprite("Settlement_FangPao");
        m_SelftBonus.text = string.Empty;
        m_OtherBonus.text = string.Empty;
        m_Title.SetSprite(isZiMo ? "TitleZiMo" : "TitleHu");
    }
    public void SetSelfGangPaiParameter(MaJiangGangPaiNotifySelfParameter param)
    {
        m_SelfName.text = PlayerInformation.Instance.PlayerID;
        m_OtherName.text = string.Join(StringConsts.SPACING, param.DianGangPlayers.ToArray());
        switch (param.ShouPai.Count)
        {
            case 0:  //0 ba gang
                m_OtherIcon.gameObject.SetActive(false);
                m_SelfIcon.SetSprite("Settlement_BaGang");
                break;
            case 3:  //3 diang gang
                    m_OtherIcon.gameObject.SetActive(true);
                    m_SelfIcon.SetSprite("Settlement_YinGang");
                    m_OtherIcon.SetSprite("Settlement_DianGang");
                break;
            case 4: //4 an gang
                m_OtherIcon.gameObject.SetActive(false);
                m_SelfIcon.SetSprite("Settlement_AnGang");
                break;
        } 
        m_SelftBonus.text = ((param.ShouPai.Count == 0 ? SystemConsts.Di : SystemConsts.Di * 2) * param.DianGangPlayers.Count).ToString();
        m_OtherBonus.text = (-(param.ShouPai.Count == 0 ? SystemConsts.Di : SystemConsts.Di * 2) ).ToString();
        m_Title.SetSprite("TitleRainWind");
    }
    public void SetOtherGangPaiParameter(MaJiangGangPaiNotifyOtherParameter param)
    {
        m_SelfName.text = param.PlayerId;
        m_OtherName.text = string.Join(StringConsts.SPACING, param.DianGangPlayers.ToArray());

        switch (param.ShouPai.Count)
        {
            case 0:  //0 ba gang
                m_OtherIcon.gameObject.SetActive(false);
                m_SelfIcon.SetSprite("Settlement_BaGang");
                break;
            case 3:  //3 diang gang
                m_OtherIcon.gameObject.SetActive(true);
                m_SelfIcon.SetSprite("Settlement_YinGang");
                m_OtherIcon.SetSprite("Settlement_DianGang");
                break;
            case 4: //4 an gang
                m_OtherIcon.gameObject.SetActive(false);
                m_SelfIcon.SetSprite("Settlement_AnGang");
                break;
        } 

        m_SelftBonus.text = ((param.ShouPai.Count == 0 ? SystemConsts.Di : SystemConsts.Di * 2) * param.DianGangPlayers.Count).ToString();
        m_OtherBonus.text = (-(param.ShouPai.Count == 0 ? SystemConsts.Di : SystemConsts.Di * 2)).ToString();
        m_Title.SetSprite("TitleRainWind");
    }
    private bool IsBonusType( List<BounsType> bonusList, BounsType bounsType)
    {
        return bonusList.Contains(bounsType);
    }
}
