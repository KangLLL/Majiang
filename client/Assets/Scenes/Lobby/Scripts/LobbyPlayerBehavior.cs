using UnityEngine;
using System.Collections;

public class LobbyPlayerBehavior : MonoBehaviour 
{
	public string PlayerId { get;set; }
	public int Position { get;set; }
    [SerializeField] tk2dSprite m_PlayerIcon;
    [SerializeField] tk2dSprite m_ReadyIcon;
    [SerializeField] Vector2[] m_ReadyIconPos;
	[SerializeField] private tk2dTextMesh m_NameLable;
    [SerializeField] tk2dSprite m_Offline;
	// Use this for initialization
	void Start () 
	{ 
		this.m_NameLable.text = this.PlayerId;
	}
    public void SetStatus(bool isReady)
    {
        m_PlayerIcon.color = isReady ? Color.red : Color.white;
        m_ReadyIcon.gameObject.SetActive(isReady);
        m_ReadyIcon.transform.localPosition = new Vector3(m_ReadyIconPos[this.Position].x, m_ReadyIconPos[this.Position].y, m_ReadyIcon.transform.localPosition.z);
        
    }
    public void SetOffline(bool isOffline)
    {
        m_Offline.gameObject.SetActive(isOffline);
    }
}
