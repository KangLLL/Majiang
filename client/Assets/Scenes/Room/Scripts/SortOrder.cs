using UnityEngine;
using System.Collections;

public class SortOrder : MonoBehaviour {
    [SerializeField] int m_SortOrder;
    void Awake()
    {
        MeshRenderer mr = GetComponent<MeshRenderer>();
        if (mr != null)
        {
            mr.sortingOrder = this.m_SortOrder;
        }
    }
}
