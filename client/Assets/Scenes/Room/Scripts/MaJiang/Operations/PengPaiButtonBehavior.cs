using UnityEngine;
using System.Collections;
using CommandConsts;

public class PengPaiButtonBehavior : OperationButtonBehavior 
{
	protected override void Process ()
	{
        base.SetAllUnselectable(false, true);
        base.StopCount();
		MaJiangPengPaiRequestParameter request = new MaJiangPengPaiRequestParameter();
		request.Pai = CurrentPlayerLogicBehavior.Instance.CurrentPai;
        print("request.Pai ======" + request.Pai);
		CommunicationUtility.Instance.PengPai(request);
	}
}
