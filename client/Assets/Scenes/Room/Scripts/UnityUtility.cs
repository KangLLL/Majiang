using UnityEngine;
using System.Collections;

public class UnityUtility 
{
    public static void DestroyAllChild(Transform tran)
    {
        while (tran.childCount > 0)
        {
            Transform t = tran.GetChild(0);
            t.parent = null;
            GameObject.Destroy(t.gameObject);
        }
    }
}
