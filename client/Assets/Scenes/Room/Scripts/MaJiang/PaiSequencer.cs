using UnityEngine;
using System.Collections;
using System.Collections.Generic;
// using System.Linq;

public class PaiSequencer : MonoBehaviour 
{
	[SerializeField] private Transform m_SelfAnchor;
	[SerializeField] private float m_SelfShouPaiDistance;

	public void Layout()
	{
        List<ShouPaiBehavior> shouPaiList = new List<ShouPaiBehavior>(this.m_SelfAnchor.GetComponentsInChildren<ShouPaiBehavior>());         
        shouPaiList.Sort((a,b) => a.Pai - b.Pai);
        //float startX = m_SelfShouPaiDistance * (shouPaiList.Count / -2f);
        for (int i = 0; i < shouPaiList.Count;i++ )
        {
            shouPaiList[i].transform.localPosition = new Vector3(/*startX + */i * this.m_SelfShouPaiDistance, 0, 0);
            shouPaiList[i].SetOrignPos();
          
        }
        //var ordered = shouPaiList.OrderBy(sp=>sp.Pai);
        //int index = 0;
        //foreach (var pai in ordered)
        //{
        //    Vector3 offset = new Vector3(index * this.m_SelfShouPaiDistance, 0, 0);
        //    pai.transform.localPosition = offset;
        //    index ++;
        //}
	}
}
