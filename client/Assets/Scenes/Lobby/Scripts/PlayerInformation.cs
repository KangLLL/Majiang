using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CommandConsts;

public class PlayerInformation : MonoBehaviour 
{
	[SerializeField]
	private string m_PlayerID;

	private static PlayerInformation s_Sigleton;
	public static PlayerInformation Instance
	{
		get {  return s_Sigleton; }
	}

	private MaJiangResumeResponseParameter m_CurrentResumeResponse;

	public string PlayerID { get { return this.m_PlayerID; } }
	public int CurrentRoomNo { get; set; }
	public int RoomPosition { get; set; }
	public bool IsReady { get; set; }
	public List<CommandConsts.PlayerInformation> InitialRivals { get; set; }
	public MaJiangResumeResponseParameter ResumeResponse
	{
		get
		{
			MaJiangResumeResponseParameter result = this.m_CurrentResumeResponse;
			this.m_CurrentResumeResponse = null;
			return result;
		}
		set
		{
			this.m_CurrentResumeResponse = value;
		}
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
}
