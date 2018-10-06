using UnityEngine;
using System.Collections;
using CommandConsts;

public class RoomPlayerBehavior : MonoBehaviour 
{
	public string PlayerId { get; set; }
	public int RoomPositionIndex { get; set; } 
	[SerializeField] private tk2dTextMesh m_NameLable;
    [SerializeField] tk2dSprite m_PlayerIcon;
    [SerializeField] tk2dSprite m_ReadyIcon;
	
	void Start ()
	{
		this.m_NameLable.text = this.PlayerId;
	}

    public void SetStatus(bool isReady)
    {
        m_PlayerIcon.color = isReady ? Color.red : Color.white;
        m_ReadyIcon.gameObject.SetActive(isReady);
    }

    //public void SetReadyStatus(ReadyStatusChangeNotifyParameter param)
    //{
    //    int pos = this.GetIndexFromPosition(this.RoomPositionIndex);
    //    m_ReadySprite[pos].gameObject.SetActive(param.ReadyStatus);
    //}
    //private int GetIndexFromPosition(int position)
    //{
    //    int result = position - PlayerInformation.Instance.RoomPosition;
    //    return result < 0 ? result + 4 : result;
    //}
}
