using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LogicRoom : MonoBehaviour 
{
	public LogicRoomStatus Status { get;set; }
	private Dictionary<string, LogicRoomPlayer> m_Players;
	public List<LogicRoomPlayer> AllPlayers
	{
		get
		{
			return new List<LogicRoomPlayer>(this.m_Players.Values);
		}
	}
}
