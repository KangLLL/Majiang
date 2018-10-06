using UnityEngine;
using System.Collections;
using CommandConsts;

public class GangSecondSelectorButtonBehavior : MonoBehaviour 
{
	public int Pai { get;set; }
	public GangPaiButtonBehavior FirstButton { get;set; }

	[SerializeField]
	private tk2dTextMesh m_Text;

	void Start()
	{
		int type = this.Pai / 36;
		int k = (this.Pai % 36) / 4 + 1;
		string typeString = type == 0 ? "wan" : type == 1 ? "tong" : "tiao";

		this.m_Text.text = k.ToString() + typeString;
	}

	void OnClick()
	{
		MaJiangGangPaiRequestParameter request = new MaJiangGangPaiRequestParameter();
		request.Pai = this.Pai;
		CommunicationUtility.Instance.GangPai(request);

        this.FirstButton.SetAllUnselectable(false, true);
		for(int i = this.transform.parent.childCount - 1; i >= 0; i --)
		{
			GameObject.Destroy(this.transform.parent.GetChild(i).gameObject);
		}
	}
}
