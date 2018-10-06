using UnityEngine;
using System.Collections;

public class CurrentPlayerLogicBehavior : MonoBehaviour 
{
	private static CurrentPlayerLogicBehavior s_Sigleton;

	public int CurrentSelectPai { get;set; }
	public int CurrentPai { get;set; }
    public ShouPaiBehavior ShouPaiBehavior { get; set; }
	public static CurrentPlayerLogicBehavior Instance
	{
		get
		{
			return s_Sigleton;
		}
	}

	void Awake()
	{
		s_Sigleton = this;
	}

	void OnDestroy()
	{
		s_Sigleton = null;
	}

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	}
}
