
using CommandConsts;
using Common;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
public class SettlementParameter
{
    //public string PlayerID { get; set; }
    public int WindRain { get; set; }
    public int ZiMoJiaDi { get; set; }
    public int ChaHuaZhu { get; set; }
    public int Sum { get; set; }
}
public class SettlementPlayer
{
    private Dictionary<string, SettlementParameter> m_SettlementPlayerDict = new Dictionary<string, SettlementParameter>();//Key = playerID;
    public Dictionary<string, SettlementParameter> SettlementPlayerDict { get { return m_SettlementPlayerDict; } }
    public SettlementPlayer(List<string> players, 
                                 int Di ,
                                 List<GangPaiParameter> gangPais, 
                                 List<string> huaPlayers,  
                                 List<HuPaiParameter> huPai,
                                 List<string> ntXiaJiaoPlayers,
                                 List<RemainingPlayerParameter> remainingPlayers)
    {
        foreach (string playerID in players)
        {
            int windRain =  CalculateWindRain(playerID, gangPais);
            int ziMoJiaDi = CalculateZiMoJiaDi(playerID, huPai, Di);
            int chaHuaZhu = CalculateChaHuaZhu(playerID, players, huaPlayers, Di);
            int sum = Sum(playerID, huPai, ntXiaJiaoPlayers, remainingPlayers) + windRain + ziMoJiaDi + chaHuaZhu;
            SettlementParameter sp = new SettlementParameter() { WindRain = windRain, ZiMoJiaDi = ziMoJiaDi, ChaHuaZhu = chaHuaZhu, Sum = sum };
            m_SettlementPlayerDict.Add(playerID, sp);

            UnityEngine.Debug.Log("playerId = " + playerID + "  windRain = " + windRain + "  ziMoJiaDi = " + ziMoJiaDi + "  chaHuaZhu = " + chaHuaZhu + "  sum = " + sum);
        }
        #region Debug
        foreach (HuPaiParameter item in huPai)
        {
            UnityEngine.Debug.Log("HuPai PlayerID = " + item.HuPaiPlayerId + "  Bouns = " + item.Bouns);
            foreach(BounsType item2 in item.BounsTypes)
                UnityEngine.Debug.Log("BounsType = " + item2.ToString());
        }
        foreach (GangPaiParameter item in gangPais)
        {
            UnityEngine.Debug.Log("GangPai PlayerID = " + item.GangPaiPlayerId + "  Bounts =" + item.Bouns);
            UnityEngine.Debug.Log("Dian gang PlayerID =" + string.Join(StringConsts.SPACING, item.DianGangPlayerIds.ToArray()));
        }
        #endregion End Debug
    }


    private int CalculateWindRain(string playerID, List<GangPaiParameter> gangPais)
    {
        int sum = 0;
        foreach (GangPaiParameter g in gangPais)
        {
            if (g.GangPaiPlayerId == playerID) sum += g.Bouns * g.DianGangPlayerIds.Count; 
            if (g.DianGangPlayerIds.Contains(playerID)) sum -= g.Bouns; 
        }
        return sum;
    }
    private int CalculateZiMoJiaDi(string playerID, List<HuPaiParameter> huPai ,int di)
    {
        int sum = 0;
        foreach (HuPaiParameter hpp in huPai)
        {
            if (hpp.HuPaiPlayerId == playerID && this.IsBonusType(hpp.BounsTypes, BounsType.ZiMo)) 
                sum += hpp.FangPaoPlayerIds.Count * di; 
            else 
                if (hpp.FangPaoPlayerIds.Contains(playerID) && this.IsBonusType(hpp.BounsTypes, BounsType.ZiMo)) sum -= di;   
        }
        return sum;
    }
    private int CalculateChaHuaZhu(string playerID, List<string> players, List<string> huaPlayers,int di)
    {
        int sum = 0;
        int count = players.Count - huaPlayers.Count;
        if(huaPlayers.Contains(playerID))
        {
            sum -= 8 * di * count;
        }
        return sum;
    }
    private int Sum(string playerID, List<HuPaiParameter> huPai, List<string> ntXiaJiaoPlayers, List<RemainingPlayerParameter> remainingPlayers)
    {
        int sum = 0;
        //HuPaiParameter hpp = huPai.First_(a => a.HuPaiPlayerId == playerID);
        //if (hpp != null) sum += hpp.Bouns;// *hpp.FangPaoPlayerIds.Count;
        foreach (HuPaiParameter hpp in huPai)
        {
            if (hpp.HuPaiPlayerId == playerID)
                sum += hpp.Bouns * hpp.FangPaoPlayerIds.Count;
            else
                if (hpp.FangPaoPlayerIds.Contains(playerID)) sum -= hpp.Bouns; 
        }
        //foreach (string noJiaoPlayer in ntXiaJiaoPlayers)
        //{
        //    if (noJiaoPlayer == playerID)
        //    {
        if (ntXiaJiaoPlayers.Contains(playerID))
        {
            foreach (RemainingPlayerParameter rpp in remainingPlayers)
            {
                sum -= rpp.MaxBouns;
            }
        }
        else 
        {
            foreach (RemainingPlayerParameter rpp in remainingPlayers)
            {
                if (rpp.PlayerId == playerID)
                {
                    sum += rpp.MaxBouns * ntXiaJiaoPlayers.Count;
                    break;
                }
            }
        }
        //    }
        //}
        return sum;
    }
    private bool IsBonusType(List<BounsType> bonusList, BounsType bounsType)
    {
        return bonusList.Contains(bounsType);
    }
}