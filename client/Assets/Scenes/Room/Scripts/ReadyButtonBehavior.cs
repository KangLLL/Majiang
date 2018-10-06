using UnityEngine;
using System.Collections;

public class ReadyButtonBehavior : MonoBehaviour 
{
	//[SerializeField] private tk2dTextMesh m_ButtonLable;
    //[SerializeField] tk2dSprite m_tk2dSpriteReady;
	public tk2dSprite PlayerSprite { get; set; }
    [SerializeField] tk2dButtonSwith m_tk2dButtonSwith;
	void OnClick()
	{
        print("OnClick======================");
		if(PlayerInformation.Instance.IsReady)
		{
			CommunicationUtility.Instance.CancelReady();
			//this.PlayerSprite.color = Color.white; 
		}
		else
		{
			CommunicationUtility.Instance.GetReady();
			//this.PlayerSprite.color = Color.red; 
		}
		PlayerInformation.Instance.IsReady = !PlayerInformation.Instance.IsReady;
        m_tk2dButtonSwith.FisrtButton = !PlayerInformation.Instance.IsReady;
         
	}


}
