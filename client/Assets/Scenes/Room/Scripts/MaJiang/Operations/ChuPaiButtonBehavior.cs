using UnityEngine;
using System.Collections;
using CommandConsts;
using System.Collections.Generic;

public class ChuPaiButtonBehavior : OperationButtonBehavior 
{


	protected override void Process ()
	{
        base.SetAllUnselectable(false, true);
        base.StopCount();
        base.Reset();
		MaJiangChuPaiRequestParameter request = new MaJiangChuPaiRequestParameter();
		request.Pai = CurrentPlayerLogicBehavior.Instance.CurrentSelectPai;
        print("CurrentPlayerLogicBehavior.Instance.CurrentSelectPai =" + CurrentPlayerLogicBehavior.Instance.CurrentSelectPai);
		CommunicationUtility.Instance.ChuPai(request);
	}
    public void ChuPai(int pai)
    {
        print("ChuPai  =======" + pai);
        base.SetAllUnselectable(false, true);
        base.StopCount();
        base.Reset();
        MaJiangChuPaiRequestParameter request = new MaJiangChuPaiRequestParameter();
        request.Pai = pai;
        CommunicationUtility.Instance.ChuPai(request);
    }
    public void AutoChuPai(List< OperatePaiParameter> param )
    {
        List<int> paramList = new List<int>();
        foreach (OperatePaiParameter item in param)
        {
            if (item.OperateType == Common.OperationType.Chu)
                paramList.Add(item.OperatePai);
        }
        paramList.Sort((a, b) => b - a);
        this.ChuPai(paramList[0]);
       
    }
}
