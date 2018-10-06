using UnityEngine;
using System.Collections;

public class PlayerFactory : MonoBehaviour 
{
	[SerializeField] private GameObject m_PlayerPrefab;
	[SerializeField] private Transform[] m_Positions;
	[SerializeField] private PlayersManagerBehavior m_Manager;
	[SerializeField] private ReadyButtonBehavior m_ReadyButton;

	private bool m_IsInitialConstructed;

	private int GetIndexFromPosition(int position)
	{
		int result = position - PlayerInformation.Instance.RoomPosition;
		return result < 0 ? result + 4 : result;
	}
	
	void Start () 
	{
		if(!this.m_IsInitialConstructed)
		{
			this.GeneratePlayer(PlayerInformation.Instance.PlayerID, PlayerInformation.Instance.RoomPosition, false);

			foreach (var rival in PlayerInformation.Instance.InitialRivals) 
			{
				this.GeneratePlayer(rival.PlayerId, rival.Position, rival.IsReady);
			}
		}
	}

	public void GeneratePlayer(string playerId, int position, bool isReady)
	{
		GameObject player = GameObject.Instantiate(this.m_PlayerPrefab) as GameObject;
		int roomPositionIndex = this.GetIndexFromPosition(position);
		player.transform.parent = this.m_Positions[roomPositionIndex];
		player.transform.localPosition = new Vector3(0, 0, -1);
		RoomPlayerBehavior pb = player.GetComponent<RoomPlayerBehavior>();
		pb.PlayerId = playerId;
		pb.RoomPositionIndex = roomPositionIndex;
		tk2dSprite sp = player.GetComponentInChildren<tk2dSprite>();
		sp.color = isReady ? Color.red : Color.white;
		this.m_Manager.RegisterPlayer(pb, playerId);
		if(playerId == PlayerInformation.Instance.PlayerID)
		{
			this.m_ReadyButton.PlayerSprite = sp;
		}
		this.m_IsInitialConstructed = true;
	}
}
