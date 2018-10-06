using UnityEngine;
using System.Collections;
using CommandConsts;
public class ShouPaiBehavior : MonoBehaviour 
{
	public int Pai { get; set; }
	public bool IsSelectable { get; set; }
	//[SerializeField] private GameObject m_SelectedObject;
    [SerializeField] tk2dSprite m_Tk2dSprite;
    private Vector3 m_Orign;
    private Vector3 m_Offset = new Vector3(0, 15, 0);
    public bool IsGangSelectable { get; set; }
    public int GangPai { get; set; }
	// Use this for initialization
	void Start () 
	{ 
        this.SetOrignPos();
	}
	
	// Update is called once per frame
	void Update () 
	{
        //if(this.IsSelectable)
        //{
        //    this.m_SelectedObject.SetActive(CurrentPlayerLogicBehavior.Instance.CurrentSelectPai == this.Pai);
        //}
	}

	void OnClick()
	{
        if (this.IsSelectable)
        {
            CurrentPlayerLogicBehavior.Instance.CurrentSelectPai = this.Pai;
            if (CurrentPlayerLogicBehavior.Instance.ShouPaiBehavior != this)
            {
                if (CurrentPlayerLogicBehavior.Instance.ShouPaiBehavior != null)
                    CurrentPlayerLogicBehavior.Instance.ShouPaiBehavior.Reset();

                CurrentPlayerLogicBehavior.Instance.ShouPaiBehavior = this;
                base.transform.localPosition = this.m_Orign + this.m_Offset;
            }
            else 
            {
                MaJiangManager.Instance.ChuPaiButtonBehavior.OnClick();
            }
        }
        this.SelectGangPai();
	}
    public void Reset()
    {
        base.transform.localPosition = this.m_Orign;
        CurrentPlayerLogicBehavior.Instance.ShouPaiBehavior = null;
        //this.Hightlight(true);
        this.IsGangSelectable = false;
    }
    public void SetOrignPos()
    {
        this.m_Orign = base.transform.localPosition;
    }
 
 
    public void Hightlight(bool active)
    {
        m_Tk2dSprite.color = active ? Color.white : Color.gray;
    }

    private void SelectGangPai()
    { 
        if (this.IsGangSelectable)
        {
            MaJiangManager.Instance.ChuPaiButtonBehavior.StopCount();
            this.IsGangSelectable = false;
            MaJiangGangPaiRequestParameter request = new MaJiangGangPaiRequestParameter();
            request.Pai = this.GangPai;
            CommunicationUtility.Instance.GangPai(request);
        }
    }
}
