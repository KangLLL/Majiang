using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CommandConsts;
using Common;
using System.Linq;
public class MaJiangManager : MonoBehaviour 
{
	[SerializeField] private PaiFactory m_Factory;
	[SerializeField] private PlayersManagerBehavior m_PlayerManager;
	[SerializeField] private GameObject m_OperationsObject;
	[SerializeField] private GameObject m_SelfShouPaiParent;
	[SerializeField] private ChuPaiButtonBehavior m_ChuButton;
    public ChuPaiButtonBehavior ChuPaiButtonBehavior { get { return m_ChuButton; } }
	[SerializeField] private PengPaiButtonBehavior m_PengButton;
	[SerializeField] private GangPaiButtonBehavior m_GangButton;
	[SerializeField] private HuPaiButtonBehavior m_HuButton;
	[SerializeField] private GuoButtonBehavior m_GuoButton;
	[SerializeField] private StopwatchBehavior m_Stopwatch;
	[SerializeField] private OperationTimeManager m_TimeManager;
    [SerializeField] OperationButtonBehavior m_OperationButtonBehavior;
    [SerializeField] tk2dSprite m_TrusteeshipSprite;
    [SerializeField] PlayerStatus m_PlayerStatus;
    [SerializeField] Transform m_Parent;
    private bool m_IsTrusteeship = false;
	private Dictionary<string, int> m_PlayerPositionDict;
    public Dictionary<string, int> PlayerPositionDict { get { return m_PlayerPositionDict; } }
    private static MaJiangManager m_Singleton;
    public static MaJiangManager Instance { get { return m_Singleton; } }
    public HuaSeType SelfDinQueHuaSe { get; set; }
	void Awake()
	{
        m_Singleton = this;
		this.m_PlayerPositionDict = new Dictionary<string, int>();
		CommunicationUtility.Instance.RegisterServerEventListener(ServerCommandConsts.MA_JIANG_MO_PAI_NOTIFY_SELF_RESPONSE,
		                                                          this, "SelfMoPai", false);
		CommunicationUtility.Instance.RegisterServerEventListener(ServerCommandConsts.MA_JIANG_MO_PAI_NOTIFY_OTHER_RESPONSE,
		                                                          this, "OtherMoPai", false);
		CommunicationUtility.Instance.RegisterServerEventListener(ServerCommandConsts.MA_JIANG_CHU_PAI_NOTIFY_SELF_RESPONSE,
		                                                          this, "SelfChuPai", false);
		CommunicationUtility.Instance.RegisterServerEventListener(ServerCommandConsts.MA_JIANG_CHU_PAI_NOTIFY_OTHER_RESPONSE,
		                                                          this, "OtherChuPai", false);
		CommunicationUtility.Instance.RegisterServerEventListener(ServerCommandConsts.MA_JIANG_PENG_NOTIFY_SELF_RESPONSE,
		                                                          this, "SelfPengPai", false);
		CommunicationUtility.Instance.RegisterServerEventListener(ServerCommandConsts.MA_JIANG_PENG_NOTIFY_OTHER_RESPONSE,
		                                                          this, "OtherPengPai", false);
		CommunicationUtility.Instance.RegisterServerEventListener(ServerCommandConsts.MA_JIANG_GANG_NOTIFY_SELF_RESPONSE,
		                                                          this, "SelfGangPai", false);
		CommunicationUtility.Instance.RegisterServerEventListener(ServerCommandConsts.MA_JIANG_GANG_NOTIFY_OTHER_RESPONSE,
		                                                          this, "OtherGangPai", false);
		CommunicationUtility.Instance.RegisterServerEventListener(ServerCommandConsts.MA_JIANG_HU_NOTIFY_SELF_RESPONSE,
		                                                          this, "SelfHuPai", false);
		CommunicationUtility.Instance.RegisterServerEventListener(ServerCommandConsts.MA_JIANG_HU_NOTIFY_OTHER_RESPONSE, 
		                                                          this, "OtherHuPai", false);
		CommunicationUtility.Instance.RegisterServerEventListener(ServerCommandConsts.MA_JIANG_QIANG_GANG_NOTIFY_SELF_RESPONSE,
		                                                          this, "SelfQiangGang", false);
		CommunicationUtility.Instance.RegisterServerEventListener(ServerCommandConsts.MA_JIANG_QIANG_GANG_NOTIFY_OTHER_RESPONSE,
		                                                          this, "OtherQiangGang", false);
     
	}

	void OnDestroy()
	{
		CommunicationUtility.Instance.RemoveInvalidReceiver();
	}

	public void FinishGame()
	{
		this.m_PlayerPositionDict.Clear();
		this.m_Factory.DestoryAllPais();
        this.m_Parent.gameObject.SetActive(false);
	}

	public void InitialGame(int[] selfShouPais)
	{
		this.m_Factory.CreateSelfShouPai(selfShouPais);
		foreach (var otherPlayer in this.m_PlayerManager.Players) 
		{
			if(otherPlayer.Key != PlayerInformation.Instance.PlayerID)
			{
				this.m_Factory.CreateOtherShouPai(otherPlayer.Value.RoomPositionIndex, selfShouPais.Length);
			}
		}
		this.InitialPlayerPositionDict();
        print("InitialGame>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>");

        this.m_Factory.CreateAllDiPais();
	}

	public void InitialPlayerPositionDict()
	{
		foreach (var otherPlayer in this.m_PlayerManager.Players) 
		{
			if(otherPlayer.Key != PlayerInformation.Instance.PlayerID)
			{
				this.m_PlayerPositionDict.Add(otherPlayer.Key, otherPlayer.Value.RoomPositionIndex);
			}
		}
	}

	private void SelfMoPai(Hashtable response)
	{
		MaJiangMoPaiNotifySelfParameter param = new MaJiangMoPaiNotifySelfParameter();
		param.InitialParameterObjectFromHashtable(response);

		this.m_OperationsObject.SetActive(true);
		this.m_GangButton.IsOperatable = false;
		this.m_HuButton.IsOperatable = false;
		this.m_GuoButton.IsOperatable = false;
		this.m_PengButton.IsOperatable = false;
		this.m_GangButton.CanGangPais.Clear();
        this.m_Factory.CreateSelfMoPai(param.Pai);
        m_OperationButtonBehavior.SetAllUnselectable(false, false,true);
    
		foreach (var op in param.Operations) 
		{
			if(op.OperateType == OperationType.Gang)
			{
				this.m_GangButton.IsOperatable = true;
				this.m_GangButton.CanGangPais.Add(op.OperatePai);
			}
			if(op.OperateType == OperationType.Hu)
			{
				this.m_HuButton.IsOperatable = true; 
			}
            if (op.OperateType == OperationType.Chu)
            {
                m_OperationButtonBehavior.SetPaiSeletable(op.OperatePai, true, true);
                
            }
		}
        m_TimeManager.OperatePaiParameterList = param.Operations; 
		this.m_ChuButton.IsOperatable = true;
	
		CurrentPlayerLogicBehavior.Instance.CurrentPai = param.Pai;
        print("param.Pai; =" + param.Pai);
        print("CurrentPlayerLogicBehavior.Instance.CurrentPai =" + CurrentPlayerLogicBehavior.Instance.CurrentPai);
		CurrentPlayerLogicBehavior.Instance.CurrentSelectPai = param.Pai;
         
      
		float currentSecond = Time.realtimeSinceStartup;
		this.m_Stopwatch.ShowCountdown(0, currentSecond);
		this.m_TimeManager.StartCount(SelfOperationType.SelfMoPai, currentSecond);

        if (this.m_IsTrusteeship)
            this.ChuPaiButtonBehavior.AutoChuPai(param.Operations);
        
        this.m_PlayerStatus.SetBanker(true, m_PlayerManager.Players[PlayerInformation.Instance.PlayerID].RoomPositionIndex);
       // m_OperationButtonBehavior.SetDinQueSelecable(this.SelfDinQueHuaSe);
	}

	private void OtherMoPai(Hashtable response)
	{
		MaJiangMoPaiNotifyOtherParameter param = new MaJiangMoPaiNotifyOtherParameter();
		param.InitialParameterObjectFromHashtable(response);

		this.m_Factory.CreateOtherMoPai(this.m_PlayerPositionDict[param.PlayerId]);
		this.m_Stopwatch.ShowCountdown(this.m_PlayerPositionDict[param.PlayerId], Time.realtimeSinceStartup);
        this.m_PlayerStatus.SetBanker(true, m_PlayerManager.Players[param.PlayerId].RoomPositionIndex);
	}

	private void SelfChuPai(Hashtable response)
	{
        print("SelfChuPai ????????????");
		MaJiangChuPaiNotifySelfParameter param = new MaJiangChuPaiNotifySelfParameter();
		param.InitialParameterObjectFromHashtable(response);

		CurrentPlayerLogicBehavior.Instance.CurrentPai = param.Pai;
		this.m_Factory.CreateSelfChuPai(param.Pai);

		this.m_Stopwatch.HideCountdown();
	}

	private void OtherChuPai(Hashtable response)
	{
        print("OtherChuPai ????????????");
		MaJiangChuPaiNotifyOtherParameter param = new MaJiangChuPaiNotifyOtherParameter();
		param.InitialParameterObjectFromHashtable(response);

		this.m_Stopwatch.HideCountdown();
		CurrentPlayerLogicBehavior.Instance.CurrentPai = param.Pai;
		if(param.Operations.Count > 0)
		{
			this.m_OperationsObject.SetActive(true);
			this.m_GangButton.CanGangPais.Clear();
			this.m_GangButton.IsOperatable = false;
			this.m_ChuButton.IsOperatable = false;
			this.m_HuButton.IsOperatable = false;
			this.m_PengButton.IsOperatable = false;
            //print("Other chu pai  ===" + param.Pai);
			foreach (var operationType in param.Operations) 
			{
				if(operationType == OperationType.Peng)
				{
					this.m_PengButton.IsOperatable = true;
				}
				if(operationType == OperationType.Gang)
				{
					this.m_GangButton.IsOperatable = true;
					this.m_GangButton.CanGangPais.Add(param.Pai);
				}
				if(operationType == OperationType.Hu)
				{
					this.m_HuButton.IsOperatable = true;
				}
			}
			this.m_GuoButton.IsOperatable = true;

			float currentSecond = Time.realtimeSinceStartup;
			this.m_Stopwatch.ShowCountdown(0, currentSecond);
			this.m_TimeManager.StartCount(SelfOperationType.OtherChuPai, currentSecond);
		}

		this.m_Factory.CreateOtherChuPai(this.m_PlayerPositionDict[param.PlayerId], param.Pai);
        if (m_IsTrusteeship) CommunicationUtility.Instance.Guo();
	}

	private void SelfPengPai(Hashtable response)
	{
		MaJiangPengPaiNotifySelfParameter param = new MaJiangPengPaiNotifySelfParameter();
		param.InitialParameterObjectFromHashtable(response);
 
		//this.m_Factory.CreateSelfPengPai(param.ShouPai);
        this.m_Factory.CreateSelfPengPai(param.ShouPai);
		this.m_OperationsObject.SetActive(true);
		
		this.m_ChuButton.IsOperatable = true;
		this.m_GuoButton.IsOperatable = false;
		this.m_PengButton.IsOperatable = false;
		this.m_GangButton.IsOperatable = false;
		this.m_HuButton.IsOperatable = false;

		var shouPais = this.m_SelfShouPaiParent.GetComponentsInChildren<ShouPaiBehavior>();
		CurrentPlayerLogicBehavior.Instance.CurrentSelectPai = -1;
		foreach (var item in shouPais)
		{ 
			item.IsSelectable = true;
			if(item.Pai > CurrentPlayerLogicBehavior.Instance.CurrentSelectPai)
			{
				CurrentPlayerLogicBehavior.Instance.CurrentSelectPai = item.Pai;
			}
		}
        m_OperationButtonBehavior.SetAllUnselectable(false, false, true);
       // print("SelfPengPai======================>>>>>>>>>>>>>>>");
        foreach (OperatePaiParameter item in param.Operations)
        {
          //  print("param.Operations = " + item.OperatePai);
            m_OperationButtonBehavior.SetPaiSeletable(item.OperatePai, true, true);
        }
       
	}

	private void OtherPengPai(Hashtable response)
	{
		MaJiangPengPaiNotifyOtherParameter param = new MaJiangPengPaiNotifyOtherParameter();
		param.InitialParameterObjectFromHashtable(response);

		this.m_Factory.CreateOtherPengPai(this.m_PlayerPositionDict[param.PlayerId]);
	}

	private void SelfHuPai(Hashtable response)
	{
		MaJiangHuPaiNotifySelfParameter param = new MaJiangHuPaiNotifySelfParameter();
		param.InitialParameterObjectFromHashtable(response);

        //print("SelfHuPai =  "+param.Pai);
        //Debug.Log(param.Bouns);
        //foreach(BounsType type in param.BounsTypes)
        //{
        //    Debug.Log(type.ToString());
        //}
        //if(!string.IsNullOrEmpty(param.FangPaoPlayerId))
        //{
        //    Debug.Log(param.FangPaoPlayerId);
        //}
		this.m_Factory.CreateSelfHuPai(param.Pai);
        if (!param.IsTheLastOne)
            WinManager.Instance.WinWindRain.ShowWindow(param);
        
	}

	private void OtherHuPai(Hashtable response)
	{
		MaJiangHuPaiNotifyOtherParameter param = new MaJiangHuPaiNotifyOtherParameter();
		param.InitialParameterObjectFromHashtable(response);
        //Debug.Log(param.PlayerId);
        //Debug.Log(param.Bouns);
        //foreach(BounsType type in param.BounsTypes)
        //{
        //    Debug.Log(type.ToString());
        //}
        //if(!string.IsNullOrEmpty(param.FangPaoPlayerId))
        //{
        //    Debug.Log(param.FangPaoPlayerId);
        //}
        this.m_Factory.CreateOtherHuPai(this.m_PlayerPositionDict[param.PlayerId], param.Pai);
        if (!param.IsTheLastOne)
            WinManager.Instance.WinWindRain.ShowWindow(param);
        
	}

	private void SelfGangPai(Hashtable response)
	{
		MaJiangGangPaiNotifySelfParameter param = new MaJiangGangPaiNotifySelfParameter();
		param.InitialParameterObjectFromHashtable(response);

		this.m_Factory.CreateSelfGangPai(param.ShouPai, param.Pai);
        if (param.CanGetBouns)
            WinManager.Instance.WinWindRain.ShowWindow(param);
        
        
	}

	private void OtherGangPai(Hashtable response)
	{
		MaJiangGangPaiNotifyOtherParameter param = new MaJiangGangPaiNotifyOtherParameter();
        param.InitialParameterObjectFromHashtable(response);

        this.m_Factory.CreateOtherGangPai(this.m_PlayerPositionDict[param.PlayerId], param.Pai);
        if (param.CanGetBouns)
            WinManager.Instance.WinWindRain.ShowWindow(param);
	}

	private void SelfQiangGang(Hashtable response)
	{
        print("SelfQiangGang ????????????");
		MaJiangQiangGangNotifySelfParameter param = new MaJiangQiangGangNotifySelfParameter();
		param.InitialParameterObjectFromHashtable(response);
		CurrentPlayerLogicBehavior.Instance.CurrentPai = param.Pai; 
		this.m_Factory.CreateSelfTempGangPai(param.Pai);
       // WinManager.Instance.WinWindRain.ShowWindow(param);
	}

	private void OtherQiangGang(Hashtable response)
	{
        print("OtherQiangGang ????????????");
		MaJiangQiangGangNotifyOtherParameter param = new MaJiangQiangGangNotifyOtherParameter();
		param.InitialParameterObjectFromHashtable(response);

		if(param.CanQiang)
		{
			this.m_OperationsObject.SetActive(true);

			CurrentPlayerLogicBehavior.Instance.CurrentPai = param.Pai;
			this.m_ChuButton.IsOperatable = false;
			this.m_GuoButton.IsOperatable = true;
			this.m_PengButton.IsOperatable = false;
			this.m_GangButton.IsOperatable = false;
			this.m_HuButton.IsOperatable = true;
		}
		this.m_Factory.CreateOtherTempGangPai(this.m_PlayerPositionDict[param.PlayerId], param.Pai);
	}
    private void Trusteeship()
    {
        this.m_IsTrusteeship = !this.m_IsTrusteeship;
        this.m_TrusteeshipSprite.SetSprite(this.m_IsTrusteeship ? "CancelTrusteeship" : "trusteeship");
    }
}
