using UnityEngine;
using System.Collections;

public class WinManager : MonoBehaviour {
    [SerializeField] WinDinQue m_WindDinQue;
    public WinDinQue WinDinQue { get { return m_WindDinQue; } }

    [SerializeField] WinWindRain m_WindWindRain;
    public WinWindRain WinWindRain { get { return m_WindWindRain; } }
    [SerializeField] WinSettlement m_WindSettlement;
    public WinSettlement WinSettlement { get { return m_WindSettlement; } }
    private static WinManager m_Singleton;
    public static WinManager Instance { get { return m_Singleton; } }
    void Awake()
    {
       m_Singleton = this;
    }
 
 
}
