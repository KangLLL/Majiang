using UnityEngine;
using System.Collections;

public class GuoButtonBehavior : OperationButtonBehavior
{
	protected override void Process ()
	{
        base.SetAllUnselectable(false, true);
        base.StopCount();
		CommunicationUtility.Instance.Guo();
	}
}
