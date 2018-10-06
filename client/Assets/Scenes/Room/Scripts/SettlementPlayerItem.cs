using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CommandConsts;
using System.Linq;
using Common;
public class SettlementPlayerItem : MonoBehaviour {
    [SerializeField] tk2dSprite m_BonusOrPunishment1;
    [SerializeField] tk2dSprite m_BonusOrPunishment2;
    [SerializeField] TextMesh m_SelfName;
    [SerializeField] TextMesh m_OtherName;
    [SerializeField] TextMesh m_TextFan;
    [SerializeField] TextMesh m_TextBonus;

    private void SetHuPai(HuPaiParameter param)
    {
        this.m_SelfName.text = param.HuPaiPlayerId;
        this.m_BonusOrPunishment1.SetSprite(StringConsts.ICON_HU);
        this.m_OtherName.text = param.FangPaoPlayerIds[0];
        this.m_BonusOrPunishment2.gameObject.SetActive(true);
        this.m_BonusOrPunishment2.SetSprite(StringConsts.ICON_FANG_PAO);
        this.m_TextBonus.text = string.Format(StringConsts.SCORING, param.Bouns);
        print("SetHuPai param.Bouns =" + param.Bouns);
        this.m_TextFan.text = this.CalculateFan(param);
    }
    private void SetZhiMo(HuPaiParameter param)
    {
        this.m_SelfName.text = param.HuPaiPlayerId;
        this.m_BonusOrPunishment1.SetSprite(StringConsts.ICON_ZI_MO);
        this.m_OtherName.text = GuanPlayerName(param);
        this.m_BonusOrPunishment2.gameObject.SetActive(false);
        this.m_TextBonus.text = string.Format(StringConsts.SCORING, param.Bouns * param.FangPaoPlayerIds.Count);
        this.m_TextFan.text = this.CalculateFan(param);
        print("SetZhiMo param.Bouns=" + param.Bouns);
    }
    private void SetPeiJiao(RemainingPlayerParameter param ,string playerId )
    {
        this.m_SelfName.text = playerId;
        this.m_BonusOrPunishment1.SetSprite(StringConsts.ICON_PEI_JIAO);
        this.m_OtherName.text = PeiPlayerName(param);
        this.m_BonusOrPunishment2.gameObject.SetActive(false);
        this.m_TextBonus.text = string.Format(StringConsts.SCORING, param.MaxBouns);
        print("       <<<<<    >>>>> playerId =" + playerId);
        this.m_TextFan.text = CalculatePeiJiao(param);
        print("SetPeiJiao param.MaxBouns=" + param.MaxBouns);
    }
    public void SetHuPaiParameter(HuPaiParameter param)
    {
        gameObject.SetActive(true);
        if (this.IsBonusType(param,BounsType.ZiMo))
            this.SetZhiMo(param);
        else
            this.SetHuPai(param);

    }
    public void SetPunishmentParameter(RemainingPlayerParameter param ,string selfPlayerID)
    {
        gameObject.SetActive(true);
        this.SetPeiJiao(param, selfPlayerID);
    }
    public string CalculateFan(HuPaiParameter param)
    {
        if (param.Bouns == SystemConsts.Di)
        { 
           return StringConsts.PIN_HU;
        }
        if (param.BounsTypes.Count != 0)
        {
            List<string> stringList = new List<string>();
            if (IsBonusType(param, BounsType.QingYiSe)) stringList.Add(StringConsts.QING_YI_SE);
            if (IsBonusType(param, BounsType.QiDu)) stringList.Add(StringConsts.QI_DUI[BonusTypeCount(param, BounsType.Gou) + BonusTypeCount(param, BounsType.Gang)]);
            if (IsBonusType(param, BounsType.DuiZiHu)) stringList.Add(StringConsts.DUI_ZI_HU);
            if (IsBonusType(param, BounsType.JinGouDiao)) stringList.Add(StringConsts.JIN_GOU_DIAO);
            if (IsBonusType(param, BounsType.QiangGang)) stringList.Add(StringConsts.QIANG_GANG);
            if (IsBonusType(param, BounsType.HaiDi)) stringList.Add(StringConsts.Hai_Di);

            if (IsBonusType(param, BounsType.GangShang))
                if (IsBonusType(param, BounsType.ZiMo))
                    stringList.Add(StringConsts.GANG_SHANG_HUA);
                else
                    stringList.Add(StringConsts.GANG_SHANG_PAO);
            if (!IsBonusType(param, BounsType.QiDu))
            {
                int count = BonusTypeCount(param, BounsType.Gou) + BonusTypeCount(param, BounsType.Gang);
                if (count > 0) stringList.Add(string.Format(StringConsts.DAI_GEN_COUNT, count));
            }

            return string.Join(StringConsts.SPACING, stringList.ToArray());
        }

        return StringConsts.PIN_HU;
    }
    public string CalculatePeiJiao(RemainingPlayerParameter param)
    {
        if (param.MaxBouns == SystemConsts.Di)
        {
            return StringConsts.PIN_HU;
        }
        if (param.BounsTypes.Count != 0)
        {
            List<string> stringList = new List<string>();
            if (IsBonusType(param, BounsType.QingYiSe)) stringList.Add(StringConsts.QING_YI_SE);
            if (IsBonusType(param, BounsType.QiDu)) stringList.Add(StringConsts.QI_DUI[BonusTypeCount(param, BounsType.Gou) + BonusTypeCount(param, BounsType.Gang)]);
            if (IsBonusType(param, BounsType.DuiZiHu)) stringList.Add(StringConsts.DUI_ZI_HU);
            if (IsBonusType(param, BounsType.JinGouDiao)) stringList.Add(StringConsts.JIN_GOU_DIAO);
            if (IsBonusType(param, BounsType.QiangGang)) stringList.Add(StringConsts.QIANG_GANG);
            if (IsBonusType(param, BounsType.GangShang)) stringList.Add(StringConsts.GANG_SHANG_HUA);
            if (!IsBonusType(param, BounsType.QiDu))
            {
                int count = BonusTypeCount(param, BounsType.Gou) + BonusTypeCount(param, BounsType.Gang);
                if (count > 0)
                    stringList.Add(string.Format(StringConsts.DAI_GEN_COUNT, count));
            }
            return string.Join(StringConsts.SPACING, stringList.ToArray());      
        }
        return StringConsts.PIN_HU;
    }
    public string GuanPlayerName(HuPaiParameter param)
    { 
      return string.Format(StringConsts.GUANG_JIA, string.Join(StringConsts.COMMA, param.FangPaoPlayerIds.ToArray()), param.FangPaoPlayerIds.Count);
    }
    public string PeiPlayerName(RemainingPlayerParameter param)
    {
        return string.Format(StringConsts.PEI_JIA, param.PlayerId);
    }
    public bool IsBonusType(HuPaiParameter param, BounsType bounsType)
    {
        return param.BounsTypes.Contains(bounsType);
    }
    public bool IsBonusType(RemainingPlayerParameter param, BounsType bounsType)
    {
        return param.BounsTypes.Contains(bounsType);
    }
    public int BonusTypeCount(HuPaiParameter param, BounsType bounsType)
    {
        return param.BounsTypes.Count(a => a == bounsType);
    }
    public int BonusTypeCount(RemainingPlayerParameter param, BounsType bounsType)
    {
        return param.BounsTypes.Count(a => a == bounsType);
    }
}
