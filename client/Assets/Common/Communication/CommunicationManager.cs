using UnityEngine;
using System.Collections;
using ExitGames.Client.Photon;
using System.Collections.Generic;

public class CommunicationManager : MonoBehaviour, IPhotonPeerListener
{
	private const string SERVER_ADDRESS = "{0}:5055";
	private const string SERVER_APPLICATION_NAME = "MaJiangServer";
	
	private PhotonPeer m_Peer;
	private bool m_IsConnected;

	private string m_ServerIP;

	private bool m_IsBackFromBackground;

	[SerializeField]
	private GameObject m_DisconnectDialog;
	[SerializeField]
	private float m_ServiceIntervalSeconds;
	[SerializeField]
	private float m_BackgroundDonotDisconnectSeconds;

	private float m_EnterBackgroundTime;
	private GameObject m_CurrentDisconnectDialog;
	private float m_PreviousServiceTime;
	
	public bool IsConnectedToServer
	{
		get
		{
			return this.m_IsConnected;
		}
	}

	void OnApplicationPause(bool isPaused)
	{
		if(Application.platform == RuntimePlatform.IPhonePlayer)
		{
			if(isPaused)
			{
				this.m_IsBackFromBackground = true;
				this.m_EnterBackgroundTime = Time.realtimeSinceStartup;
			}
			else
			{
				if(Time.realtimeSinceStartup - this.m_EnterBackgroundTime > this.m_BackgroundDonotDisconnectSeconds)
				{
					this.DisconnectToServer();
					this.ShowDisconnectDialog(false);
				}
				this.m_IsBackFromBackground = false;
			}
		}
	}
	
	void Awake()
	{
		this.m_IsConnected = false;
		this.m_IsBackFromBackground = false;
	    //this.ConnectToServer();
	}
	
	void Update()
	{
		if(this.m_Peer != null)
		{
			float currentTime = Time.realtimeSinceStartup;
			if(currentTime - this.m_PreviousServiceTime >= this.m_ServiceIntervalSeconds)
			{
				this.m_Peer.Service();
				this.m_PreviousServiceTime = currentTime;
			}
		}
	}
	
	public void SetServerIP(string serverIP)
	{
		this.m_ServerIP  = serverIP;
	}
	
	public void ConnectToServer()
	{
		if(!this.m_IsConnected)
		{
			this.m_Peer = new PhotonPeer(this,ConnectionProtocol.Udp);
			string serverAddress = string.Format(SERVER_ADDRESS, this.m_ServerIP);
			this.m_Peer.Connect(serverAddress, SERVER_APPLICATION_NAME);
		}
	}
	
	public void DisconnectToServer()
	{
		if(this.m_IsConnected)
		{
			this.m_IsConnected = false;
			this.m_Peer.Disconnect();
			this.m_Peer = null;
		}
	}
	
	public void Communicate(byte requestCode, Hashtable parameters)
	{
		if(this.m_IsConnected)
		{
			print(requestCode);
			if(parameters == null)
			{
				this.m_Peer.OpCustom(requestCode, null, true);
			}
			else
			{
				Dictionary<byte, object> wrapParameters = new Dictionary<byte, object>();
				wrapParameters[CommandConsts.CommunicationConsts.EXTERNAL_SURFACE_KEY] = parameters;
				this.m_Peer.OpCustom(requestCode, wrapParameters, true);
			}
		}
	}
	
	void OnDestroy()
	{
		if(this.m_Peer != null)
		{
			this.m_Peer.Disconnect();
		}
	}
	
	#region IPhotonPeerListener implementation
	void IPhotonPeerListener.DebugReturn (DebugLevel level, string message)
	{
		Debug.Log(message);
		Debug.Log(level.ToString());
	}

	void IPhotonPeerListener.OnOperationResponse (OperationResponse operationResponse)
	{
		Debug.Log("ruizi!");
		Application.Quit();
		//throw new System.NotImplementedException ();
	}

	void IPhotonPeerListener.OnStatusChanged (StatusCode statusCode)
	{
		if(statusCode == StatusCode.Connect)
		{
			Debug.Log("@@@@@@@@@@@@@@@@@@@@@@@@");
			this.m_IsConnected = true;
		}
		else if(statusCode == StatusCode.Disconnect || statusCode == StatusCode.Exception 
			|| statusCode == StatusCode.TimeoutDisconnect)
		{
			Debug.Log(statusCode.ToString());
			this.m_IsConnected = false;
			this.m_Peer = null;

			this.ShowDisconnectDialog(statusCode == StatusCode.TimeoutDisconnect);
			//Application.LoadLevel(ClientStringConstants.LOADING_SCENE_LEVEL_NAME);
		}
	}

	void IPhotonPeerListener.OnEvent (EventData eventData)
	{
		//print("==========================");
		print(eventData.Code);
		CommunicationUtility.Instance.SendMessage("EventReceiver", eventData);
		//throw new System.NotImplementedException ();
	}
	#endregion

	private void ShowDisconnectDialog(bool isTimeOut)
	{
		/*
		if(this.m_CurrentDisconnectDialog == null)
		{
			AudioController.Play("WindowShow");
			GameObject dialog = GameObject.Instantiate(this.m_DisconnectDialog) as GameObject;
			dialog.GetComponent<DisconnectDialog>().DisconnectType =  isTimeOut ?
				DisconnectType.CannotConnect : this.m_IsBackFromBackground ? DisconnectType.BackFromBackground : 
					DisconnectType.Normal;
			
			GameObject rootObject = GameObject.Find(ClientStringConstants.UI_ROOT_OBJECT_NAME);
			Transform parent = rootObject.transform.GetChild(0).FindChild(ClientStringConstants.UI_ANCHOR_OBJECT_NAME);
			dialog.transform.parent = parent;
			dialog.transform.localPosition = new Vector3(0, 0, -4000);
			dialog.transform.localScale = new Vector3(0.3f, 0.3f, 1);
			iTween.ScaleTo(dialog, iTween.Hash(iT.ScaleTo.scale, Vector3.one, iT.ScaleTo.easetype, iTween.EaseType.easeOutBack, iT.ScaleTo.time, 0.2f, "ignoretimescale", true));
			this.m_CurrentDisconnectDialog = dialog;			
		}
		*/
	}
}
