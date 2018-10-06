using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CommandConsts;
using ExitGames.Client.Photon;

public class CommunicationUtility : MonoBehaviour 
{
	[SerializeField]
	private CommunicationManager m_Manager;
	
	private Dictionary<byte, List<ReceiverInformation>> m_ReceiverDict;
	
	public bool IsConnectedToServer
	{
		get
		{
			return m_Manager.IsConnectedToServer;
		}
	}
	
	private static CommunicationUtility s_instance;
	public static CommunicationUtility Instance
	{
		get
		{
			return s_instance;
		}
	}
	
	void Awake()
	{
		s_instance = this;
		GameObject.DontDestroyOnLoad(gameObject);
		this.m_ReceiverDict = new Dictionary<byte, List<ReceiverInformation>>();
	}
	
	void Start()
	{
		//this.m_ReceiverDict = new Dictionary<byte, List<ReceiverInformation>>();
	}
	
	public void ConnectToServer()
	{
		this.m_Manager.ConnectToServer();
	}
	
	public void SetServerIP(string serverIP)
	{
		this.m_Manager.SetServerIP(serverIP);
	}
	
	public void DisconnectToServer()
	{
		this.m_Manager.DisconnectToServer();
	}

	public void ResumeGame(MaJiangResumeRequestParameter parameter, Component receiver, string methodName)
	{
		this.CommunicateWithServer(receiver, methodName, true, parameter.GetHashtableFromParameter(),
		                           ClientCommandConsts.MA_JIANG_RESUME_REQUEST, ServerCommandConsts.MA_JIANG_RESUME_RESPONSE);
	}

	public void JoinLobby(JoinLobbyRequestParameter parameter, Component receiver, string methodName) 
	{
		this.CommunicateWithServer(receiver, methodName, true, parameter.GetHashtableFromParameter(), 
		                           ClientCommandConsts.JOIN_LOBBY_REQUEST, ServerCommandConsts.JOIN_LOBBY_RESPONSE);
	}

	public void JoinRoom(JoinRoomRequestParameter parameter, Component receiver, string methodName)
	{
		this.CommunicateWithServer(receiver, methodName, true, parameter.GetHashtableFromParameter(),
		                           ClientCommandConsts.JOIN_ROOM_REQUEST, ServerCommandConsts.JOIN_ROOM_RESPONSE);
	}

	public void QuitRoom()
	{
		this.CommunicateWithServer(null, ClientCommandConsts.QUIT_ROOM_REQUEST);
	}

	public void GetReady()
	{
		this.CommunicateWithServer(null, ClientCommandConsts.GET_READY_REQUEST);
	}

	public void CancelReady()
	{
		this.CommunicateWithServer(null, ClientCommandConsts.CANCEL_READY_REQUEST);
	}


	public void ChuPai(MaJiangChuPaiRequestParameter parameter)
	{
		this.CommunicateWithServer(parameter.GetHashtableFromParameter(), ClientCommandConsts.MA_JIANG_CHU_PAI_REQUEST);
	}

	public void PengPai(MaJiangPengPaiRequestParameter parameter)
	{
        print("Communication PengPai =" + parameter.Pai);
		this.CommunicateWithServer(parameter.GetHashtableFromParameter(), ClientCommandConsts.MA_JIANG_PENG_PAI_REQUEST);
	}

	public void GangPai(MaJiangGangPaiRequestParameter parameter)
	{
		this.CommunicateWithServer(parameter.GetHashtableFromParameter(), ClientCommandConsts.MA_JIANG_GANG_PAI_REQUEST);
	}

	public void HuPai(MaJiangHuPaiRequestParameter parameter)
	{
		this.CommunicateWithServer(parameter.GetHashtableFromParameter(), ClientCommandConsts.MA_JIANG_HU_PAI_REQUEST);
	}

	public void Guo()
	{
		this.CommunicateWithServer(null, ClientCommandConsts.MA_JIANG_GUO_REQUEST);
	}

	public void DingQue(MaJiangDingQueRequestParameter parameter)
	{
		this.CommunicateWithServer(parameter.GetHashtableFromParameter(), ClientCommandConsts.MA_JIANG_DING_QUE_REQUEST);
	}

	public void RegisterServerEventListener(byte serverEventCode, Component receiver, string methodName, bool isListenOnce)
	{
		ReceiverInformation info = new ReceiverInformation() 
		{ Receiver = receiver, MethodName = methodName, IsListenOnce = isListenOnce };
		if(!this.m_ReceiverDict.ContainsKey(serverEventCode))
		{
			this.m_ReceiverDict.Add(serverEventCode, new List<ReceiverInformation>());
		}
		if(!this.m_ReceiverDict[serverEventCode].Contains(info))
		{
			this.m_ReceiverDict[serverEventCode].Add(info);
		}
	}
	
	public void RemoveInvalidReceiver()
	{
		List<byte> invalidReceiverList = new List<byte>();
		foreach(byte eventCode in this.m_ReceiverDict.Keys)
		{
			List<ReceiverInformation> receivers = this.m_ReceiverDict[eventCode];
			for(int i = receivers.Count - 1; i >= 0; i --)
			{
				ReceiverInformation receiver = receivers[i];
				if(receiver.Receiver == null)
				{
					receivers.Remove(receiver);
				}
			}
			
			if(receivers.Count == 0)
			{
				invalidReceiverList.Add(eventCode);
			}
		}
		
		foreach(byte invalidReceiver in invalidReceiverList)
		{
			this.m_ReceiverDict.Remove(invalidReceiver);
		}
	}
	
	private void CommunicateWithServer(Component receiver, string methodName, bool isListenOnce,  Hashtable parameter, 
		byte clientCommandCode, byte serverEventCode)
	{
		this.m_Manager.Communicate(clientCommandCode, parameter);
		this.RegisterServerEventListener(serverEventCode, receiver, methodName, isListenOnce);
	}
	
	private void CommunicateWithServer(Hashtable parameter, byte clientCommandCode)
	{
		this.m_Manager.Communicate(clientCommandCode, parameter);
	}
	
	public void EventReceiver(EventData data)
	{
		if(this.m_ReceiverDict.ContainsKey(data.Code))
		{
			List<ReceiverInformation> receivers = this.m_ReceiverDict[data.Code];
			for(int i = receivers.Count - 1; i >= 0; i --)
			{
				ReceiverInformation receiver = receivers[i];
				if(receiver.Receiver == null)
				{
					receivers.Remove(receiver);
				}
				else
				{
					if(data.Parameters == null || data.Parameters.Count == 0)
					{
						receiver.Receiver.SendMessage(receiver.MethodName, new Hashtable(), SendMessageOptions.DontRequireReceiver);
					}
					else
					{
						receiver.Receiver.SendMessage(receiver.MethodName, 
							(Hashtable)data.Parameters[CommunicationConsts.EXTERNAL_SURFACE_KEY], SendMessageOptions.DontRequireReceiver);
					}
					if(receiver.IsListenOnce)
					{
						receivers.Remove(receiver);
					}
				}
			}
		}
	}
}
