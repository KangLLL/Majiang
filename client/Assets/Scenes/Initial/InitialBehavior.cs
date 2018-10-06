using UnityEngine;
using System.Collections;
using CommandConsts;

public class InitialBehavior : MonoBehaviour 
{
	[SerializeField]
	private string m_ServerIP;
	// Use this for initialization
	void Start () 
	{
		CommunicationUtility.Instance.SetServerIP(this.m_ServerIP);
		CommunicationUtility.Instance.ConnectToServer();

		this.StartCoroutine("Initialize");
	}

	IEnumerator Initialize()
	{
		while(!CommunicationUtility.Instance.IsConnectedToServer)
		{
			yield return null;
		}


		MaJiangResumeRequestParameter param = new MaJiangResumeRequestParameter();
		param.PlayerId = PlayerInformation.Instance.PlayerID;

		CommunicationUtility.Instance.ResumeGame(param, this, "ReceivedResumeResponse");
	}

	private void ReceivedResumeResponse(Hashtable response)
	{
		if(response == null || response.Count == 0)
		{
			Application.LoadLevel(ClientConfigConsts.Instance.LobbyLevelName);
		}
		else
		{
			MaJiangResumeResponseParameter param = new MaJiangResumeResponseParameter();
			param.InitialParameterObjectFromHashtable(response);
			PlayerInformation.Instance.ResumeResponse = param;

			PlayerInformation.Instance.CurrentRoomNo = param.RoomNo;
			foreach (var player in param.Players) 
			{
				if(player.PlayerId == PlayerInformation.Instance.PlayerID)
				{
					PlayerInformation.Instance.RoomPosition = player.Position;
				}
			}
			Application.LoadLevel(ClientConfigConsts.Instance.RoomLevelName);
		}
	}
}
