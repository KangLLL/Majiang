using UnityEngine;
using System.Collections;

public class StopwatchBehavior : MonoBehaviour 
{
	//[SerializeField] private GameObject[] m_PositionTips;
    [SerializeField] string[] m_PositionTips;
    [SerializeField] tk2dSprite m_Sprite;
	[SerializeField] private tk2dTextMesh m_TimeLable;

	private float m_StartCountdownSecond;

	public void ShowCountdown(int position, float currentSecond)
	{
		//this.m_PositionTips[position].SetActive(true);
       
        m_Sprite.SetSprite(m_PositionTips[position]);
		this.m_TimeLable.text = ClientConfigConsts.Instance.OperationMaxSecond + "s";
		this.gameObject.SetActive(true);
		this.m_StartCountdownSecond = currentSecond;
	}

	public void HideCountdown()
	{
        //foreach(GameObject pt in this.m_PositionTips)
        //{
        //    pt.SetActive(false);
        //}
		this.gameObject.SetActive(false);
	}

	void Update () 
	{
		float elapsedSeconds = Time.realtimeSinceStartup - this.m_StartCountdownSecond;
		if(elapsedSeconds < ClientConfigConsts.Instance.OperationMaxSecond)
		{
			int remainingSeconds  = Mathf.RoundToInt(ClientConfigConsts.Instance.OperationMaxSecond - elapsedSeconds);
			this.m_TimeLable.text = remainingSeconds + "s";
		}
	}
}
