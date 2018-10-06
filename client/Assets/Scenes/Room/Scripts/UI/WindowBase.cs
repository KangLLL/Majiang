using UnityEngine;
using System.Collections;

public class WindowBase : MonoBehaviour {

    public virtual void ShowWindow()
    {
        gameObject.SetActive(true);
    }

    public virtual void HideWindow()
    {
        gameObject.SetActive(false);
    }
}
