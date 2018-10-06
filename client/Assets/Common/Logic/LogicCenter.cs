using UnityEngine;
using System.Collections;

public class LogicCenter : MonoBehaviour
{
	private static LogicCenter s_Sigleton;

	public static LogicCenter Instance
	{
		get
		{
			return s_Sigleton;
		}
	}

	private LogicLobby m_Lobby;

	void Start () 
	{
		this.m_Rooms = new Dictionary<int, RoomBehavior>();
		
		CommunicationUtility.Instance.RegisterServerEventListener(ServerCommandConsts.JOIN_ROOM_NOTIFY_LOBBY_RESPONSE,
		                                                          this, "PlayerJoinedRoom", false);
		CommunicationUtility.Instance.RegisterServerEventListener(ServerCommandConsts.QUIT_ROOM_NOTIFY_LOBBY_RESPONSE,
		                                                          this, "PlayerQuitedRoom", false);
		CommunicationUtility.Instance.RegisterServerEventListener(ServerCommandConsts.READY_STATUS_CHANGE_NOTIFY_LOBBY_RESPONSE,
		                                                          this, "PlayerStatusChanged", false);
		CommunicationUtility.Instance.RegisterServerEventListener(ServerCommandConsts.START_GAME_NOTIFY_LOBBY_RESPONSE,
		                                                          this, "GameStart", false);
		CommunicationUtility.Instance.RegisterServerEventListener(ServerCommandConsts.FINISH_GAME_NOTIFY_LOBBY_RESPONSE,
		                                                          this, "GameOver", false);
		CommunicationUtility.Instance.RegisterServerEventListener(ServerCommandConsts.DISCONNECT_NOTIFY_LOBBY_RESPONSE,
		                                                          this, "Disconnect", false);
		CommunicationUtility.Instance.RegisterServerEventListener(ServerCommandConsts.RESUME_NOTIFY_LOBBY_RESPONSE,
		                                                          this, "Resume", false);
	}
	
	void Update () 
	{
		
	}
	
	void OnDestroy()
	{
		CommunicationUtility.Instance.RemoveInvalidReceiver();
	}
	
	private void PlayerJoinedRoom(Hashtable response)
	{
		JoinRoomNotifyLobbyParameter param = new JoinRoomNotifyLobbyParameter();
		param.InitialParameterObjectFromHashtable(response);
		
		this.m_Rooms[param.RoomNo].JoinPlayer(param.PlayerId, param.Position);
	}
	
	private void PlayerQuitedRoom(Hashtable response)
	{
		QuitRoomNotifyLobbyParameter param = new QuitRoomNotifyLobbyParameter();
		param.InitialParameterObjectFromHashtable(response);
		
		this.m_Rooms[param.RoomNo].QuitPlayer(param.PlayerId);
	}
	
	private void PlayerStatusChanged(Hashtable response)
	{
		ReadyStatusChangeNotifyLobbyParameter param = new ReadyStatusChangeNotifyLobbyParameter();
		param.InitialParameterObjectFromHashtable(response);
		
		this.m_Rooms[param.RoomNo].ChangeReadyStatus(param.PlayerId, param.ReadyStatus);
		
	}
	
	private void GameStart(Hashtable response)
	{
		StartGameNotifyLobbyParameter param = new StartGameNotifyLobbyParameter();
		param.InitialParameterObjectFromHashtable(response);
		
		this.m_Rooms[param.RoomNo].StartGame();
	}
	
	private void GameOver(Hashtable response)
	{
		FinishGameNotifyLobbyParameter param = new FinishGameNotifyLobbyParameter();
		param.InitialParameterObjectFromHashtable(response);
		
		this.m_Rooms[param.RoomNo].FinishGame();
	}
	
	public void RegisterRoom(RoomBehavior room, int roomNo)
	{
		this.m_Rooms.Add(roomNo, room);
		room.transform.parent = this.transform;
	}
	private void Disconnect(Hashtable response)
	{
		print("Lobby Disconnect>>>>>>>>>>>");
		DisconnectNotifyLobbyParameter param = new DisconnectNotifyLobbyParameter();
		param.InitialParameterObjectFromHashtable(response);
		m_Rooms[param.RoomNo].Player[param.PlayerId].GetComponent<LobbyPlayerBehavior>().SetOffline(true);
	}
	private void Resume(Hashtable response)
	{
		ResumeNotifyLobbyParameter param = new ResumeNotifyLobbyParameter();
		param.InitialParameterObjectFromHashtable(response);
		m_Rooms[param.RoomNo].Player[param.PlayerId].GetComponent<LobbyPlayerBehavior>().SetOffline(false);
	}
}
