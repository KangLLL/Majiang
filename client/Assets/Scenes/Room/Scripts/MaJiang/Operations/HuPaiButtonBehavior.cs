using UnityEngine;
using System.Collections;
using CommandConsts;

public class HuPaiButtonBehavior : OperationButtonBehavior 
{
	protected override void Process ()
	{
        base.SetAllUnselectable(false, true);
        base.StopCount();
		MaJiangHuPaiRequestParameter request = new MaJiangHuPaiRequestParameter();
		request.Pai = CurrentPlayerLogicBehavior.Instance.CurrentPai;
        print("CurrentPai = " + CurrentPlayerLogicBehavior.Instance.CurrentPai);
        print("request.Pai =" + CurrentPlayerLogicBehavior.Instance.CurrentPai);
		CommunicationUtility.Instance.HuPai(request);
	}
}
