using UnityEngine;
using System.Collections;
using CommandConsts;

public class RoomPositionBehavior : MonoBehaviour 
{
	[SerializeField]
	private RoomBehavior m_Room;
	[SerializeField]
	private int m_Position;

	public bool IsOccupied { get { return this.GetComponentInChildren<LobbyPlayerBehavior>() != null; } }
	

	void OnClick()
	{
		if(!this.IsOccupied)
		{
			JoinRoomRequestParameter request = new JoinRoomRequestParameter();
			request.RoomNo = this.m_Room.RoomNo;
			request.Position = this.m_Position;
			CommunicationUtility.Instance.JoinRoom(request, this, "ReceivedJoinResponse");
		}
	}

	private void ReceivedJoinResponse(Hashtable response)
	{
		JoinRoomResponseParameter param = new JoinRoomResponseParameter();
		param.InitialParameterObjectFromHashtable(response);
		if(param.IsSuccessful)
		{
			PlayerInformation.Instance.CurrentRoomNo = this.m_Room.RoomNo;
			PlayerInformation.Instance.RoomPosition = this.m_Position;
			PlayerInformation.Instance.InitialRivals = param.Players;
			Application.LoadLevel(ClientConfigConsts.Instance.RoomLevelName);
		}
	}
}
