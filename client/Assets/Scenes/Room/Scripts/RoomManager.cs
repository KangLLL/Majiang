using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CommandConsts;
using Common;

public class RoomManager : MonoBehaviour 
{
	[SerializeField] private GameObject m_RoomObject;
	[SerializeField] private GameObject m_MaJiangObject;
	[SerializeField] private PlayersManagerBehavior m_PlayerManager;
	[SerializeField] private tk2dTextMesh m_ReadyButtonLable;
	[SerializeField] private MaJiangManager m_Manager;
    [SerializeField] private PlayerStatus m_PlayerStatus;
    [SerializeField] private PaiFactory m_PaiFactory;

	// Use this for initialization
	void Start () 
	{
		CommunicationUtility.Instance.RegisterServerEventListener(ServerCommandConsts.MA_JIANG_START_NOTIFY_RESPONSE,
		                                                          this, "GameStart", false);
		CommunicationUtility.Instance.RegisterServerEventListener(ServerCommandConsts.FINISH_GAME_NOTIFY_ROOM_RESPONSE,
		                                                          this, "GameFinish", false);
        CommunicationUtility.Instance.RegisterServerEventListener(ServerCommandConsts.DISCONNECT_NOTIFY_ROOM_RESPONSE,
                                                                  this, "Disconnect", false);
        CommunicationUtility.Instance.RegisterServerEventListener(ServerCommandConsts.RESUME_NOTIFY_ROOM_RESPONSE,
                                                                  this, "Resume", false);
        CommunicationUtility.Instance.RegisterServerEventListener(ServerCommandConsts.MA_JIANG_DING_QUE_NOTIFY_RESPONSE,
                                                                  this, "DinQue", false);
        CommunicationUtility.Instance.RegisterServerEventListener(ServerCommandConsts.MA_JIANG_SUMMARY_NOTIFY_RESPONSE,
                                                                  this, "Summary", false);

	}

	private void GameStart(Hashtable response)
	{
		PlayerInformation.Instance.IsReady = false;
		//this.m_ReadyButtonLable.text = "Ready";
		foreach (var p in this.m_PlayerManager.Players) 
		{
			tk2dSprite sp = p.Value.GetComponentInChildren<tk2dSprite>();
			sp.color = Color.white;
		}

		this.m_RoomObject.SetActive(false);
		this.m_MaJiangObject.SetActive(true);

		MaJiangStartNotifyParameter param = new MaJiangStartNotifyParameter();
		param.InitialParameterObjectFromHashtable(response);

		this.m_Manager.InitialGame(param.Pais);

        this.m_PlayerStatus.ResetAll();
        WinManager.Instance.WinDinQue.ShowWindow(param.Pais);
        SystemConsts.Di = param.Di;
	}

	private void GameFinish(Hashtable response)
	{
        //this.m_RoomObject.SetActive(true);
        //this.m_Manager.FinishGame();
        //this.m_MaJiangObject.SetActive(false);
        m_PlayerManager.ResetAllPlayerState();

	}
  
    private void Disconnect(Hashtable response)
    {
         DisconnectNotifyParameter param = new DisconnectNotifyParameter(); 
         param.InitialParameterObjectFromHashtable(response);
         print("Disconnect ->>>>>>>>>>>>>>>>>");
         m_PlayerStatus.SetOffline(true, this.m_PlayerManager.Players[param.PlayerId].RoomPositionIndex); 
    }
    private void Resume(Hashtable response)
    {
        ResumeNotifyParameter param = new ResumeNotifyParameter();
        param.InitialParameterObjectFromHashtable(response);
        m_PlayerStatus.SetOffline(false, this.m_PlayerManager.Players[param.PlayerId].RoomPositionIndex); 
    }

    private void DinQue(Hashtable response)
    {
        print("DinQue ->>>>>>>>>>>>>>>>>>>");
        MaJiangDingQueResponseParameter param = new MaJiangDingQueResponseParameter();
        param.InitialParameterObjectFromHashtable(response);
        foreach (KeyValuePair<string, HuaSeType> item in param.DingQues)
        {
            this.m_PlayerStatus.SetDinQueType(item.Value, this.m_PlayerManager.Players[item.Key].RoomPositionIndex);
        }
        this.m_Manager.SelfDinQueHuaSe = param.DingQues[PlayerInformation.Instance.PlayerID];
        WinManager.Instance.WinDinQue.HideWindow();
    }
    private void Summary(Hashtable response)
    {
        MaJiangFinishGameNotifyParameter param = new MaJiangFinishGameNotifyParameter();
        param.InitialParameterObjectFromHashtable(response);
        WinManager.Instance.WinSettlement.ShowWindow(param);
        this.m_PaiFactory.ShowAllPais(param.PlayerShouPais);
     }
}
