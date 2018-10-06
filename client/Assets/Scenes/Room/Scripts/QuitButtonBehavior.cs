using UnityEngine;
using System.Collections;
using CommandConsts;

public class QuitButtonBehavior : MonoBehaviour 
{

	void OnClick()
	{
		CommunicationUtility.Instance.QuitRoom();

		Application.LoadLevel(ClientConfigConsts.Instance.LobbyLevelName);
	}	
}
