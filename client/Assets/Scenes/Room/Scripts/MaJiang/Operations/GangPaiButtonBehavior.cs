using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CommandConsts;
using System.Linq;

public class GangPaiButtonBehavior : OperationButtonBehavior 
{
	[SerializeField] private Transform m_SecondSelectorParent;
	[SerializeField] private GameObject m_SecondSelectorPrefab;
	[SerializeField] private float m_SecondSelectorDistance;
    
	private List<int> m_CanGangPais = new List<int>();

	public List<int> CanGangPais { get { return this.m_CanGangPais; } }

 
    //protected override void Process ()
    //{
    //    print("ggggggggggggggg");
    //    if(this.m_SecondSelectorParent.childCount == 0)
    //    {
    //        print("ggggggggggggggg1");
    //        if(this.m_CanGangPais.Count == 1)
    //        {
    //            print("ggggggggggggggg2");
    //            MaJiangGangPaiRequestParameter request = new MaJiangGangPaiRequestParameter();
    //            request.Pai = this.m_CanGangPais[0];
    //            CommunicationUtility.Instance.GangPai(request);
    //        }
    //        else
    //        {
    //            this.ConstructSecondSelector();
    //        }
    //    }
    //}

    protected override void Process()
    {
        if (this.m_SecondSelectorParent.childCount == 0)
        {
            switch (this.m_CanGangPais.Count)
            {
                case 1:
                    base.StopCount();
                    MaJiangGangPaiRequestParameter request = new MaJiangGangPaiRequestParameter();
                    request.Pai = this.m_CanGangPais[0];
                    CommunicationUtility.Instance.GangPai(request);
                    break;
                default:
                  
                        List<KeyValuePair<int, ShouPaiBehavior>> gangPaiList = new List<KeyValuePair<int, ShouPaiBehavior>>();
                        foreach (int item in m_CanGangPais)
                        {
                            gangPaiList.AddRange(m_PaiFactory.SelfShouPais.Where(a => a.Value.Pai / 4 == item / 4));
                        }

                        List<ShouPaiBehavior> ganList = new List<ShouPaiBehavior>();
                        gangPaiList.ForEach(a => ganList.Add(a.Value));
                        base.SetAllUnselectable(false, false);
                        base.SetGangSelectable(true, true, ganList ,this.m_CanGangPais);

                    break;
               
            }
        }
    }
	/*
	protected override bool IsSetUnselectableOnClick 
	{
		get 
		{
			return this.m_CanGangPais.Count == 1;
		}
	}
	*/

	private void ConstructSecondSelector()
	{
		for(int i = 0; i < this.m_CanGangPais.Count; i ++)
		{
			GameObject selector = GameObject.Instantiate(this.m_SecondSelectorPrefab) as GameObject;
			selector.transform.parent = this.m_SecondSelectorParent;
			selector.transform.localPosition = new Vector3(0, - i * this.m_SecondSelectorDistance, 0);

			GangSecondSelectorButtonBehavior behavior = selector.GetComponent<GangSecondSelectorButtonBehavior>();
			behavior.Pai = this.m_CanGangPais[i];
			behavior.FirstButton = this;
		}
	}
}
