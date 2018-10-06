using UnityEngine;
using System.Collections;
using CommandConsts;
using System.Linq;
using System.Collections.Generic;
public class OperationTimeManager : MonoBehaviour 
{
	[SerializeField] private Transform m_SelfShouPaiParent;
	[SerializeField] private GameObject m_ButtonGroup;
    [SerializeField] ChuPaiButtonBehavior m_ChuPaiButtonBehavior;
    public List<OperatePaiParameter> OperatePaiParameterList { get; set; }
	private float m_StartSecond;
	private SelfOperationType m_Type;
	private bool m_IsProcessed;

	public void StartCount(SelfOperationType operationType, float currentSecond)
	{
		this.m_Type = operationType;
		this.m_StartSecond = currentSecond;
		this.m_IsProcessed = false;
	}

	public void StopCount()
	{
		this.m_IsProcessed = true;
	}

	void Start()
	{
		this.m_IsProcessed = true;
	}
	
	void Update () 
	{
		if(!this.m_IsProcessed)
		{
			float elapsedSeconds = Time.realtimeSinceStartup - this.m_StartSecond;
			if(elapsedSeconds >= ClientConfigConsts.Instance.OperationMaxSecond)
			{
				this.TimeUpProcess();
			}
		}
	}

	private void TimeUpProcess()
	{
		this.m_IsProcessed = true;
		this.m_ButtonGroup.SetActive(false);

		switch(this.m_Type)
		{
			case SelfOperationType.SelfMoPai:
			{
				var shouPais = this.m_SelfShouPaiParent.GetComponentsInChildren<ShouPaiBehavior>();
				foreach (var item in shouPais)
				{
					item.IsSelectable = false;
				}
				
                //ShouPaiBehavior maxPai = shouPais[0];
                //for(int i = 1; i < shouPais.Length; i ++)
                //{
                //    if(shouPais[i].Pai > maxPai.Pai)
                //    {
                //        maxPai =  shouPais[i];
                //    }
                //}
                //MaJiangChuPaiRequestParameter request = new MaJiangChuPaiRequestParameter();
                //request.Pai = maxPai.Pai;
                //CommunicationUtility.Instance.ChuPai(request);
                print("Time ======>");
                m_ChuPaiButtonBehavior.AutoChuPai(this.OperatePaiParameterList);
			}
			break;
			case SelfOperationType.OtherChuPai:
			{
				CommunicationUtility.Instance.Guo();
			}
			break;
		}
	}
}
