using UnityEngine;
using System.Collections;
using CommandConsts;
using System.Collections.Generic;
using Common;

public class WinSettlement : WindowBase {
    [SerializeField] private GameObject m_RoomObject;
    //[SerializeField] private GameObject m_MaJiangObject;  
    [SerializeField] private MaJiangManager m_Manager;
    [SerializeField] private SettlementPlayerItem[] m_SettlementPlayerItem;
    [SerializeField] private SettlementPlayerItem2[] m_SettlementPlayerItem2;
    [SerializeField] private PlayersManagerBehavior m_PlayersManagerBehavior;
	// Use this for initialization
	void Start () {
	
	}
    public void ShowWindow(MaJiangFinishGameNotifyParameter param)
    {
        int i = 0;
        foreach (HuPaiParameter hp in param.HuPais)
        {
            m_SettlementPlayerItem[i].SetHuPaiParameter(hp); 
            i++;
        }
   
        //foreach (PunishmentParameter pp in param.Punishments)
        //{ 
        //    foreach(RemainingPlayerParameter rpp in pp.RemainingPlayers)
        //    {
        //        m_SettlementPlayerItem[i].SetPunishmentParameter(rpp, pp.PlayerId);
        //        i++;
        //    } 
        //}
         
        foreach (string noJiaoPlayer in param.NotXiaJiaoPlayers)
        {
            foreach (RemainingPlayerParameter rpp in param.RemainingPlayers)
            {
                m_SettlementPlayerItem[i].SetPunishmentParameter(rpp, noJiaoPlayer);
                i++;
            }
        } 
        List<string> players = new List<string>(m_PlayersManagerBehavior.Players.Keys);

        SettlementPlayer sp = new SettlementPlayer(players, SystemConsts.Di, param.GangPais, param.HuaPlayers, param.HuPais,param.NotXiaJiaoPlayers,param.RemainingPlayers );
        for (int j = 0; j < players.Count; j++)
        {
            m_SettlementPlayerItem2[j].SetItemData(players[j], sp.SettlementPlayerDict[players[j]]);
        }
        base.ShowWindow();
    }
    public override void HideWindow()
    {
        this.m_RoomObject.SetActive(true);
        this.m_Manager.FinishGame();
        //this.m_MaJiangObject.SetActive(false);
        base.HideWindow();

        foreach (SettlementPlayerItem spi in m_SettlementPlayerItem)
        {
            spi.gameObject.SetActive(false);
        }
        foreach (SettlementPlayerItem2 spi in m_SettlementPlayerItem2)
        {
            spi.gameObject.SetActive(false);
        }
    }



}
