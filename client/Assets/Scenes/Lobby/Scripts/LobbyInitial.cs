using UnityEngine;
using System.Collections;
using CommandConsts;
using System.Collections.Generic;

public class LobbyInitial : MonoBehaviour
{
	[SerializeField] private GameObject m_RoomPrefab; 
	[SerializeField] private int m_RoomsPerRow;
    [SerializeField] private int m_RoomsPerColumn;
	[SerializeField] private float m_RowDistance;
	[SerializeField] private float m_ColumnDistance;
	[SerializeField] private Vector3 m_InitialPosition;
	[SerializeField] private RoomsManagerBehavior m_RoomManager;
    [SerializeField] private tk2dUIScrollableArea scrollableArea;


    List<RoomInformation> allItems = new List<RoomInformation>();
    List<Transform> cachedContentItems = new List<Transform>();
    List<Transform> unusedContentItems = new List<Transform>();
    int firstCachedItem = -1;
    int maxVisibleItems = 0;
    void OnEnable()
    {
        scrollableArea.OnScroll += OnScroll;
    }

    void OnDisable()
    {
        scrollableArea.OnScroll -= OnScroll;
    }
	// Use this for initialization
	void Start () 
	{
		if(!CommunicationUtility.Instance.IsConnectedToServer)
		{
			CommunicationUtility.Instance.ConnectToServer();
		}
		this.StartCoroutine("Initialize");
	}
	
	// Update is called once per frame
	void Update () 
	{
	}

	IEnumerator Initialize()
	{
		while(!CommunicationUtility.Instance.IsConnectedToServer)
		{
			yield return null;
		}
		JoinLobbyRequestParameter request = new JoinLobbyRequestParameter();
		request.PlayerId = PlayerInformation.Instance.PlayerID;
		CommunicationUtility.Instance.JoinLobby(request, this, "ReceivedJoinResponse");
	}

    //private void ReceivedJoinResponse(Hashtable response)
    //{
    //    JoinLobbyResponseParameter param = new JoinLobbyResponseParameter();
    //    param.InitialParameterObjectFromHashtable(response);

    //    for (int i = 0; i < param.Rooms.Count; i++)
    //    {
    //        RoomInformation roomInfo = param.Rooms[i];

    //        int row = i / this.m_RoomsPerRow;
    //        int column = i % this.m_RoomsPerRow;

    //        Vector3 offset = new Vector3(column * this.m_ColumnDistance, -row * this.m_RowDistance, 0);
    //        GameObject room = GameObject.Instantiate(this.m_RoomPrefab) as GameObject;

    //        room.transform.position = this.m_InitialPosition + offset;
    //        RoomBehavior rb = room.GetComponent<RoomBehavior>();
    //        rb.RoomInformation = roomInfo;
    //        this.m_RoomManager.RegisterRoom(rb, roomInfo.RoomNo);
    //    }
    //}

    private void ReceivedJoinResponse(Hashtable response)
    {
        JoinLobbyResponseParameter param = new JoinLobbyResponseParameter();
        param.InitialParameterObjectFromHashtable(response);
        //maxVisibleItems = m_RoomsPerColumn * m_RoomsPerRow + m_RoomsPerRow;
        maxVisibleItems = param.Rooms.Count;
        for (int i = 0; i < param.Rooms.Count; ++i)
        {
            #region test
            RoomInformation roomInfo = param.Rooms[i];
            GameObject room = Instantiate(m_RoomPrefab) as GameObject;
            RoomBehavior rb = room.GetComponent<RoomBehavior>();
            rb.RoomInformation = roomInfo; 
            this.m_RoomManager.RegisterRoom(rb, roomInfo.RoomNo);


            #endregion

            room.transform.parent = scrollableArea.contentContainer.transform;
            int row = i / this.m_RoomsPerRow;
            int column = i % this.m_RoomsPerRow;
            room.transform.localPosition = new Vector3(column * this.m_ColumnDistance, -row * this.m_RowDistance, 0); 

            //DoSetActive(room.transform, false);
            //unusedContentItems.Add(room.transform);
        }
        // SetItemCount(param.Rooms.Count, param.Rooms);
        #region  test
        int itemPerPage = m_RoomsPerRow * m_RoomsPerColumn;
        int pages = Mathf.CeilToInt((float)maxVisibleItems / itemPerPage);
        scrollableArea.ContentLength = pages * this.m_RowDistance * m_RoomsPerColumn;
       
  
        #endregion
    }
    void SetItemCount(int numItems, List<RoomInformation> roomList)
    {
        if (numItems < allItems.Count)
        {
            allItems.RemoveRange(numItems, allItems.Count - numItems);
        }
        else
        {
            for (int j = allItems.Count; j < numItems; j++)
            {
                allItems.Add(roomList[j]);
            }
        }
        UpdateListGraphics();
    }
    void UpdateListGraphics()
    {
        float previousOffset = scrollableArea.Value * (scrollableArea.ContentLength - scrollableArea.VisibleAreaLength);
        int firstVisibleItem = Mathf.FloorToInt(previousOffset / this.m_RowDistance); //Mathf.FloorToInt(previousOffset / itemStride);
  
        int itemPerPage = m_RoomsPerRow * m_RoomsPerColumn;
        int pages = Mathf.CeilToInt((float)allItems.Count / itemPerPage);
 
        float newContentLength = pages * this.m_RowDistance * m_RoomsPerColumn;//allItems.Count * itemStride;
        //print("newContentLength =" + newContentLength);
        if (!Mathf.Approximately(newContentLength, scrollableArea.ContentLength))
        { 
            if (newContentLength < scrollableArea.VisibleAreaLength) {
             
                scrollableArea.Value = 0; // no more scrolling
                for (int i = 0; i < cachedContentItems.Count; ++i)
                { 
                    DoSetActive(cachedContentItems[i], false);
                    unusedContentItems.Add(cachedContentItems[i]); // clear whole list
                }
                cachedContentItems.Clear();
                firstCachedItem = -1;
                firstVisibleItem = 0;
            }
            // The total size required to display all elements
            scrollableArea.ContentLength = newContentLength;

            // Rescale the previousOffset so it remains constant
            if (scrollableArea.ContentLength > 0)
            { 
                scrollableArea.Value = previousOffset / (scrollableArea.ContentLength - scrollableArea.VisibleAreaLength);
            } 
        }
        ////////////////////////////////////////////////////////
        int lastVisibleItem = Mathf.Min(firstVisibleItem + maxVisibleItems, allItems.Count);
       // print("lastVisibleItem =" + lastVisibleItem);
        // If any items are visible that shouldn't need to be visible, get rid of them
        while (firstCachedItem >= 0 && firstCachedItem < firstVisibleItem )
        { 
            firstCachedItem++;
            DoSetActive(cachedContentItems[0], false);
            unusedContentItems.Add(cachedContentItems[0]);
            cachedContentItems.RemoveAt(0);
            if (cachedContentItems.Count == 0)
            { 
                firstCachedItem = -1;
            }
        }
        // Ditto for end of list
        while (firstCachedItem >= 0 && (firstCachedItem + cachedContentItems.Count) > lastVisibleItem)
        { 
            DoSetActive(cachedContentItems[cachedContentItems.Count - 1], false);
            unusedContentItems.Add(cachedContentItems[cachedContentItems.Count - 1]);
            cachedContentItems.RemoveAt(cachedContentItems.Count - 1);
            if (cachedContentItems.Count == 0)
            { 
                firstCachedItem = -1;
            }
        }
        // Nothing visible, simply fill as needed
        if (firstCachedItem < 0)
        { 
            firstCachedItem = firstVisibleItem;
            int maxToAdd = Mathf.Min(firstCachedItem + maxVisibleItems, allItems.Count);
            for (int i = firstCachedItem; i < maxToAdd; ++i)
            { 
                Transform t = unusedContentItems[0];
                cachedContentItems.Add(t);
                unusedContentItems.RemoveAt(0);
                CustomizeListObject(t, i);
                DoSetActive(t, true);
            }
        }
        else
        { 
            // Fill in items that should be visible but aren't
            while (firstCachedItem > firstVisibleItem)
            { 
                --firstCachedItem;
                Transform t = unusedContentItems[0];
                unusedContentItems.RemoveAt(0);
                cachedContentItems.Insert(0, t);
                CustomizeListObject(t, firstCachedItem);
                DoSetActive(t, true);

            }
            while (firstCachedItem + cachedContentItems.Count < lastVisibleItem)
            { 
                Transform t = unusedContentItems[0];
                unusedContentItems.RemoveAt(0);
                CustomizeListObject(t, firstCachedItem + cachedContentItems.Count);
                cachedContentItems.Add(t);
                DoSetActive(t, true);

            }
        }
    }

    void CustomizeListObject(Transform contentRoot, int itemId)
    {
        int row = itemId / this.m_RoomsPerRow;
        int column = itemId % this.m_RoomsPerRow;

        contentRoot.localPosition = new Vector3(column * this.m_ColumnDistance, -row * this.m_RowDistance, 0);
       
    }
    void OnScroll(tk2dUIScrollableArea scrollableArea)
    {
       // UpdateListGraphics();
    }
    protected void DoSetActive(Transform t, bool state)
    {
#if UNITY_3_5
		t.gameObject.SetActiveRecursively(state);
#else
        t.gameObject.SetActive(state);
#endif
    }
}
