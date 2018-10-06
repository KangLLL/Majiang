using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ReceiverManager  
{
	private List<ReceiverInformation> m_Receivers;
	
	public ReceiverManager()
	{
		this.m_Receivers = new List<ReceiverInformation>();
	}
	
	public void AddReceiver(ReceiverInformation receiver)
	{
		this.m_Receivers.Add(receiver);
	}
	
	public void RemoveReceiver(ReceiverInformation receiver)
	{
		this.m_Receivers.Remove(receiver);
	}
	
	public void Invoke(object data)
	{
		for(int i = this.m_Receivers.Count - 1; i >= 0; i --)
		{
			ReceiverInformation receiver = this.m_Receivers[i];
			if(receiver.Receiver == null)
			{
				this.RemoveReceiver(receiver);
			}
			else if(receiver.IsListenOnce)
			{
				if(data == null)
				{
					receiver.Receiver.SendMessage(receiver.MethodName, SendMessageOptions.DontRequireReceiver);
				}
				else
				{
					receiver.Receiver.SendMessage(receiver.MethodName, data, SendMessageOptions.DontRequireReceiver);
				}
				this.RemoveReceiver(receiver);
			}
			else
			{
				if(data == null)
				{
					receiver.Receiver.SendMessage(receiver.MethodName, SendMessageOptions.DontRequireReceiver);
				}
				else
				{
					receiver.Receiver.SendMessage(receiver.MethodName, data, SendMessageOptions.DontRequireReceiver);
				}
			}
		}
	}
	
	public void RemoveInvalidReceiver()
	{
		for(int i = this.m_Receivers.Count - 1; i >= 0; i --)
		{
			ReceiverInformation receiver = this.m_Receivers[i];
			if(receiver.Receiver == null)
			{
				this.RemoveReceiver(receiver);
			}
		}
	}
}
