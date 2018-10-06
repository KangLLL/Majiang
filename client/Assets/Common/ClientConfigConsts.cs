using UnityEngine;
using System.Collections;

public class ClientConfigConsts : MonoBehaviour
{
	private static ClientConfigConsts s_Sigleton;

	public static ClientConfigConsts Instance
	{
		get { return s_Sigleton; }
	}

	void Awake()
	{
		s_Sigleton = this;
		GameObject.DontDestroyOnLoad(this.gameObject);
	}

	void OnDestroy()
	{
		s_Sigleton = null;
	}

	[SerializeField]
	private string m_LobbyLevelName;
	[SerializeField]
	private string m_RoomLevelName;
	[SerializeField]
	private int m_OperationMaxSecond;

	public string LobbyLevelName { get { return this.m_LobbyLevelName; } }
	public string RoomLevelName { get { return this.m_RoomLevelName; } }
	public int OperationMaxSecond { get { return this.m_OperationMaxSecond; } }
}
