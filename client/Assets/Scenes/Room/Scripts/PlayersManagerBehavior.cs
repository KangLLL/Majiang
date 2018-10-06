using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CommandConsts;

public class PlayersManagerBehavior : MonoBehaviour 
{
	public Dictionary<string, RoomPlayerBehavior> Players { get; private set; }
	[SerializeField] private PlayerFactory m_Factory;
    [SerializeField] tk2dButtonSwith m_tk2dButtonSwith;
    
	void Awake()
	{
		this.Players = new Dictionary<string, RoomPlayerBehavior>();
	}

	void Start () 
	{
		CommunicationUtility.Instance.RegisterServerEventListener(ServerCommandConsts.JOIN_ROOM_NOTIFY_ROOM_RESPONSE,
		                                                          this, "PlayerJoinedRoom", false);
		CommunicationUtility.Instance.RegisterServerEventListener(ServerCommandConsts.QUIT_ROOM_NOTIFY_ROOM_RESPONSE,
		                                                          this, "PlayerQuitedRoom", false);
		CommunicationUtility.Instance.RegisterServerEventListener(ServerCommandConsts.READY_STATUS_CHANGE_NOTIFY_ROOM_RESPONSE,
		                                                          this, "PlayerStatusChanged", false);

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
		JoinRoomNotifyParameter param = new JoinRoomNotifyParameter();
		param.InitialParameterObjectFromHashtable(response);

		this.GeneratePlayer(param.NewPlayerId, param.Position, false);
	}

	public void GeneratePlayer(string playerId, int position, bool isReady)
	{
		this.m_Factory.GeneratePlayer(playerId, position, isReady);
	}
	
	private void PlayerQuitedRoom(Hashtable response)
	{
		QuitRoomNotifyParameter param = new QuitRoomNotifyParameter();
		param.InitialParameterObjectFromHashtable(response);

		GameObject.Destroy(this.Players[param.PlayerId].gameObject);
		this.Players.Remove(param.PlayerId);
	}

	private void PlayerStatusChanged(Hashtable response)
	{
		ReadyStatusChangeNotifyParameter param = new ReadyStatusChangeNotifyParameter();
		param.InitialParameterObjectFromHashtable(response);
        this.Players[param.PlayerId].SetStatus(param.ReadyStatus);
        //tk2dSprite sp = this.Players[param.PlayerId].GetComponentInChildren<tk2dSprite>();
        //sp.color = param.ReadyStatus ? Color.red : Color.white;
       
	}
	
	public void RegisterPlayer(RoomPlayerBehavior player, string playerId)
	{
		this.Players.Add(playerId, player);
	}

    public void ResetAllPlayerState()
    {
        foreach (KeyValuePair<string, RoomPlayerBehavior> item in this.Players)
        {
            item.Value.SetStatus(false);
        }
        m_tk2dButtonSwith.FisrtButton = true;
    }


}
