using UnityEngine;
using System.Collections;
using CommandConsts;
using System.Collections.Generic;

public class RoomBehavior : MonoBehaviour 
{
	[SerializeField]
	private tk2dTextMesh m_NoText;
	[SerializeField]
	private GameObject m_RoomPlayerPrefab;
	[SerializeField]
	private GameObject m_PlayingObject;

	[SerializeField]
	private RoomPositionBehavior[] m_Positions;

	private int m_RoomNo;
	private Dictionary<string, GameObject> m_Players;
    public Dictionary<string, GameObject> Player { get { return m_Players; } }
	public RoomInformation RoomInformation { get; set; }
	public int RoomNo { get { return this.m_RoomNo; } }

	void Start () 
	{
        
		this.m_RoomNo = this.RoomInformation.RoomNo;
		this.m_NoText.text = this.m_RoomNo.ToString();
		this.m_Players = new Dictionary<string, GameObject>();
		for(int i = 0; i < this.RoomInformation.Players.Count; i ++)
		{
			CommandConsts.PlayerInformation playerInfo = this.RoomInformation.Players[i];

			GameObject player = GameObject.Instantiate(this.m_RoomPlayerPrefab) as GameObject;

			int position = playerInfo.Position;
			player.transform.parent = this.m_Positions[position].transform;
			player.transform.localPosition = new Vector3(0, 0, -1);

			LobbyPlayerBehavior pb = player.GetComponent<LobbyPlayerBehavior>();
			pb.PlayerId = playerInfo.PlayerId;
            pb.Position = playerInfo.Position;
			this.m_Players.Add(playerInfo.PlayerId, player);

			tk2dSprite sp = player.GetComponentInChildren<tk2dSprite>();
			sp.color = playerInfo.IsReady ? Color.red : Color.white;
		}
         
	}
	
	void Update () 
	{
	}

	private void OnClick()
	{
		for(int i = 0; i < this.m_Positions.Length; i ++)
		{
			if(!this.m_Positions[i].IsOccupied)
			{
				JoinRoomRequestParameter request = new JoinRoomRequestParameter();
				request.RoomNo = this.m_RoomNo;
				request.Position = i;
				PlayerInformation.Instance.CurrentRoomNo = this.m_RoomNo;
				PlayerInformation.Instance.RoomPosition = i;
				CommunicationUtility.Instance.JoinRoom(request, this, "ReceivedJoinResponse");
				break;
			}
		}
	}

	private void ReceivedJoinResponse(Hashtable response)
	{
		JoinRoomResponseParameter param = new JoinRoomResponseParameter();
		param.InitialParameterObjectFromHashtable(response);
		if(param.IsSuccessful)
		{
			PlayerInformation.Instance.InitialRivals = param.Players;
			Application.LoadLevel(ClientConfigConsts.Instance.RoomLevelName);
		}
	}

	public void QuitPlayer(string playerId)
	{
		GameObject.Destroy(this.m_Players[playerId]);
		this.m_Players.Remove(playerId);
	}

	public void JoinPlayer(string playerId, int position)
	{
		GameObject player = GameObject.Instantiate(this.m_RoomPlayerPrefab) as GameObject;

		player.transform.parent = this.m_Positions[position].transform;
		player.transform.localPosition = new Vector3(0, 0, -1);
		
		LobbyPlayerBehavior pb = player.GetComponent<LobbyPlayerBehavior>();
		pb.PlayerId = playerId;
        pb.Position = position; 
		this.m_Players.Add(playerId, player);
	}

	public void ChangeReadyStatus(string playerId, bool newStatus)
	{
        //tk2dSprite sp = this.m_Players[playerId].GetComponentInChildren<tk2dSprite>();
        //sp.color = newStatus ? Color.red : Color.white;
        this.m_Players[playerId].GetComponent<LobbyPlayerBehavior>().SetStatus(newStatus);
	}

	public void StartGame()
	{
		foreach (var p in this.m_Players)
		{
			tk2dSprite sp = p.Value.GetComponentInChildren<tk2dSprite>();
			sp.color = Color.white;
			p.Value.SetActive(false);
		}

		this.m_PlayingObject.SetActive(true);
	}

	public void FinishGame()
	{
		foreach (var p in this.m_Players) 
		{
			p.Value.SetActive(true);
		}

		this.m_PlayingObject.SetActive(false);
	}

}
