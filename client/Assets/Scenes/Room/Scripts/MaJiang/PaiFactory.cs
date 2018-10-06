using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PaiFactory : MonoBehaviour
{
    [SerializeField] private GameObject[] m_SelfShouPaiPrefabs;
    [SerializeField] private GameObject[] m_SelfGangPengPrefabs;
    [SerializeField] private GameObject[] m_OtherShouPaiPrefab;
    [SerializeField] private GameObject[] m_DiPaiPrefab;
    [SerializeField] private GameObject[] m_ChuPaiPrefab;
    [SerializeField] private GameObject[] m_ChuPaiPrefab1;
    [SerializeField] private GameObject[] m_ChuPaiPrefab2;
    [SerializeField] private GameObject[] m_ChuPaiPrefab3;

    [SerializeField] private Transform[] m_ShouPaiAnchors;
    [SerializeField] private Transform[] m_ChuPaiAnchors;
    [SerializeField] private Transform[] m_MoPaiAnchors;
    [SerializeField] private Transform[] m_PengPaiAnchors;
    [SerializeField] private Transform[] m_DiPaiAnchors;
    //[SerializeField] private float m_OtherShouPaiDistance0;
    [SerializeField] private float m_OtherShouPaiDistance0;
    [SerializeField] private float m_OtherShouPaiDistance1;
    [SerializeField] private float m_OtherShouPaiDistance2;
    [SerializeField] private float m_OtherShouPaiDistance3;
    [SerializeField] private float m_ChuPaiDistance0;
    [SerializeField] private float m_ChuPaiDistance1;
    [SerializeField] private float m_ChuPaiDistance2;
    [SerializeField] private float m_ChuPaiDistance3;
    //[SerializeField] private float m_PengPaiColumnDistance;
    [SerializeField] private float m_PengPaiRowDistance0;
    [SerializeField] private float m_PengPaiRowDistance1;
    [SerializeField] private float m_PengPaiRowDistance2;
    [SerializeField] private float m_PengPaiRowDistance3;

    [SerializeField] private float m_ChuPaiRowDistance0;
    [SerializeField] private float m_ChuPaiRowDistance1;
    [SerializeField] private float m_ChuPaiRowDistance2;
    [SerializeField] private float m_ChuPaiRowDistance3;
    [SerializeField] private int m_ChuPaiPerRowCount;
    [SerializeField] private PaiSequencer m_Sequencer;
    [SerializeField] private PlayersManagerBehavior m_PlayersManagerBehavior;

    private int m_CurrentChuPaiIndex;
    public GameObject CurrentChuPai { get; set; }

    public GameObject CurrentMoPai { get; set; }
    public GameObject CurrentTempGangPai { get; set; }

    private Dictionary<int, ShouPaiBehavior> m_SelfShouPais;
    public Dictionary<int, ShouPaiBehavior> SelfShouPais { get { return m_SelfShouPais; } }
    private Dictionary<int, int> m_ChuPaiIndexes;
    private Dictionary<int, int> m_PengPaiIndexes;//key = player pos;value = times of gang or peng 
    private Dictionary<int, Dictionary<int, int>> m_PengPais; // key = player pos;key of value = pai type; value of value = pai;
    private Dictionary<int, Dictionary<int, int>> m_PengPairsCount;// key = player pos;key of value = pai type; value of value = pai's count;
    private Queue<GameObject> m_QueueDiPai;
    void Awake()
    {
        this.m_SelfShouPais = new Dictionary<int, ShouPaiBehavior>();
        this.m_ChuPaiIndexes = new Dictionary<int, int>();
        this.m_PengPaiIndexes = new Dictionary<int, int>();
        this.m_PengPairsCount = new Dictionary<int, Dictionary<int, int>>();
        this.m_PengPais = new Dictionary<int, Dictionary<int, int>>();
        this.m_QueueDiPai = new Queue<GameObject>();
        
        //for (int i = 0; i < this.m_ChuPaiAnchors.Length; i++)
        //{
        //    this.m_ChuPaiIndexes.Add(i, 0);
        //}
        //for (int i = 0; i < this.m_PengPaiAnchors.Length; i++)
        //{
        //    this.m_PengPaiIndexes.Add(i, 0);
        //    this.m_PengPais.Add(i, new Dictionary<int, int>());
        //    this.m_PengPairsCount.Add(i, new Dictionary<int, int>());
        //}
        this.InitialDict();
    }
    private void InitialDict()
    {
        this.m_PengPais.Clear();
        this.m_QueueDiPai.Clear();
        this.m_PengPairsCount.Clear();
        this.m_ChuPaiIndexes.Clear();
        this.m_SelfShouPais.Clear();
        this.m_PengPaiIndexes.Clear();
        for (int i = 0; i < this.m_ChuPaiAnchors.Length; i++)
        {
            this.m_ChuPaiIndexes.Add(i, 0);
        }
        for (int i = 0; i < this.m_PengPaiAnchors.Length; i++)
        {
            this.m_PengPaiIndexes.Add(i, 0);
            this.m_PengPais.Add(i, new Dictionary<int, int>());
            this.m_PengPairsCount.Add(i, new Dictionary<int, int>());
        }
        for(int i=0 ;i< m_PengPaiAnchors.Length;i++)
        {
            m_ShouPaiAnchors[i].position = m_PengPaiAnchors[i].position;
        }
        
           
    }
    public void DestoryAllPais()
    {
        this.DestroyAllChildren(this.m_ShouPaiAnchors);
        this.DestroyAllChildren(this.m_PengPaiAnchors);
        this.DestroyAllChildren(this.m_ChuPaiAnchors);
        this.DestroyAllChildren(this.m_MoPaiAnchors);

        this.DestroyAllChildren(this.m_DiPaiAnchors);

        this.CurrentChuPai = null;
        this.CurrentMoPai = null;
        this.CurrentTempGangPai = null;

        //this.m_SelfShouPais.Clear();
        //for (int i = 0; i < this.m_ChuPaiAnchors.Length; i++)
        //{
        //    this.m_ChuPaiIndexes[i] = 0;
        //}
        //for (int i = 0; i < this.m_PengPaiAnchors.Length; i++)
        //{
        //    this.m_PengPaiIndexes[i] = 0;
        //}
        this.InitialDict();

    }

    private void DestroyAllChildren(IEnumerable<Transform> parents)
    {
        foreach (var p in parents)
        {
            //for (int i = p.childCount - 1; i >= 0; i--)
            //{
            //    GameObject.Destroy(p.GetChild(i).gameObject);
            //}
            UnityUtility.DestroyAllChild(p);
        }
    }

    public void CreateAllDiPais( int remainingPaiCount = 0)
    { 
        List<GameObject> m_DiPais = new List<GameObject>();
        int pos = Random.Range(0, 4);
        int random = Random.Range(0, 14) + 2;
        int start = random % 2 != 0 ? Mathf.Clamp(random + 1, 0, 15) : random;
  
       
        int[] p = { 26, 28, 26, 28 };
        for (int i = 0; i < p[3] / 2; i++)
        {
            GameObject paiGo1 = Instantiate(this.m_DiPaiPrefab[3]) as GameObject;
            GameObject paiGo2 = Instantiate(this.m_DiPaiPrefab[3]) as GameObject;
            paiGo1.transform.parent = m_DiPaiAnchors[3];
            paiGo2.transform.parent = m_DiPaiAnchors[3];
            float origin = m_ChuPaiDistance3 * p[3] / 2 / 2;
            Vector3 offset1 = new Vector3(0, -origin + m_ChuPaiDistance3 * i + 7, 0);
            Vector3 offset2 = new Vector3(0, -origin + m_ChuPaiDistance3 * i, 0);
            paiGo1.GetComponentInChildren<tk2dSprite>().SortingOrder = p[3] / 2 - i;
            paiGo2.GetComponentInChildren<tk2dSprite>().SortingOrder = p[3] / 2 - i - 2;
            paiGo1.transform.localPosition = offset1;
            paiGo2.transform.localPosition = offset2;
            m_DiPais.Add(paiGo1);
            m_DiPais.Add(paiGo2);
        }
        for (int i = 0; i < p[2] / 2; i++)
        {
            GameObject paiGo1 = Instantiate(this.m_DiPaiPrefab[2]) as GameObject;
            GameObject paiGo2 = Instantiate(this.m_DiPaiPrefab[2]) as GameObject;
            paiGo1.transform.parent = m_DiPaiAnchors[2];
            paiGo2.transform.parent = m_DiPaiAnchors[2];
            float origin = m_ChuPaiDistance2 * p[2] / 2 / 2;
            Vector3 offset1 = new Vector3(-origin + m_ChuPaiDistance2 * i, 7, 0);
            Vector3 offset2 = new Vector3(-origin + m_ChuPaiDistance2 * i, 0, 0);
            paiGo1.GetComponentInChildren<tk2dSprite>().SortingOrder = 1;
            paiGo2.GetComponentInChildren<tk2dSprite>().SortingOrder = 0;
            paiGo1.transform.localPosition = offset1;
            paiGo2.transform.localPosition = offset2;
            m_DiPais.Add(paiGo1);
            m_DiPais.Add(paiGo2);
        }
        for (int i = 0; i < p[1] / 2; i++)
        {
            GameObject paiGo1 = Instantiate(this.m_DiPaiPrefab[1]) as GameObject;
            GameObject paiGo2 = Instantiate(this.m_DiPaiPrefab[1]) as GameObject;
            paiGo1.transform.parent = m_DiPaiAnchors[1];
            paiGo2.transform.parent = m_DiPaiAnchors[1];
            float origin = m_ChuPaiDistance1 * p[1] / 2 / 2;
            Vector3 offset1 = new Vector3(0, origin - m_ChuPaiDistance1 * i + 7, 0);
            Vector3 offset2 = new Vector3(0, origin - m_ChuPaiDistance1 * i, 0);
            paiGo1.GetComponentInChildren<tk2dSprite>().SortingOrder = i + 2;
            paiGo2.GetComponentInChildren<tk2dSprite>().SortingOrder = i;
            paiGo1.transform.localPosition = offset1;
            paiGo2.transform.localPosition = offset2;
            m_DiPais.Add(paiGo1);
            m_DiPais.Add(paiGo2);
        }
        for (int i = 0; i < p[0] / 2; i++)
        {
            GameObject paiGo1 = Instantiate(this.m_DiPaiPrefab[0]) as GameObject;
            GameObject paiGo2 = Instantiate(this.m_DiPaiPrefab[0]) as GameObject;
            paiGo1.transform.parent = m_DiPaiAnchors[0];
            paiGo2.transform.parent = m_DiPaiAnchors[0]; 
            float origin = m_ChuPaiDistance2 * p[0]/ 2 / 2; 
            Vector3 offset1 = new Vector3(origin - m_ChuPaiDistance2 * i, 7, 0);
            Vector3 offset2 = new Vector3(origin - m_ChuPaiDistance2 * i, 0, 0);
            paiGo1.GetComponentInChildren<tk2dSprite>().SortingOrder = 1;
            paiGo2.GetComponentInChildren<tk2dSprite>().SortingOrder = 0;
            paiGo1.transform.localPosition = offset1;
            paiGo2.transform.localPosition = offset2;
            m_DiPais.Add(paiGo1);
            m_DiPais.Add(paiGo2);
        } 
        int indext = 0;
        for (int i = 0;i < pos ; i++)
        { 
            indext += p[i];
        }
        indext += start;
        int destroyPaiCount = remainingPaiCount == 0 ? 13 * m_PlayersManagerBehavior.Players.Count : 108 - remainingPaiCount;
        for (int i = 0; i < destroyPaiCount; i++)
        { 
           Destroy(m_DiPais[(indext + i) % m_DiPais.Count]);
        }
        for (int i = 0; i < 108 - destroyPaiCount; i++)
        {
            this.m_QueueDiPai.Enqueue(m_DiPais[(indext + destroyPaiCount + i) % m_DiPais.Count]);
        }
  
    }
    public void MoDiPai()
    {
        if (this.m_QueueDiPai.Count > 0)
        {
            Destroy(this.m_QueueDiPai.Dequeue());
        }
    }
    public void CreateSelfShouPai(int[] pais)
    {
        foreach (var pai in pais)
        {
            this.ConstructSelfShouPai(pai);
        }
        this.m_Sequencer.Layout();
    }

    public void CreateOtherShouPai(int id, int count)
    {
        Vector3 offset = Vector3.zero;
        //float start = 0;
        for (int i = 0; i < count; i++)
        {
            GameObject shouPai = this.ConstructOtherShouPai(id, i.ToString());

            switch (id)
            {
                //case 0:
                //    offset = new Vector3(i * this.m_OtherShouPaiDistance0, 0, 0);
                //    break;
                case 1:
                   // start = this.m_OtherShouPaiDistance1 * (count / -2f);
                    offset = new Vector3(0, /*start +*/ i * this.m_OtherShouPaiDistance1, 0);
                    shouPai.GetComponentInChildren<tk2dSprite>().SortingOrder = count - i;
                    break;
                case 2:
                   // start = m_OtherShouPaiDistance2 * (count / 2f);
                    offset = new Vector3(/*start*/ - i * this.m_OtherShouPaiDistance2, 0, 0);
                    break;
                case 3:
                   // start = this.m_OtherShouPaiDistance3 * (count / 2f);
                    offset = new Vector3(0, /*start */- i * this.m_OtherShouPaiDistance3, 0);
                    shouPai.GetComponentInChildren<tk2dSprite>().SortingOrder = i;
                    break;
            }
            shouPai.transform.localPosition = offset;
        }
    }

    public void CreateSelfMoPai(int pai)
    {
        this.CurrentTempGangPai = null;
        this.MoDiPai();
        GameObject shouPai = this.ConstructSelfShouPai(pai);
     
        //shouPai.transform.position = this.m_MoPaiAnchors[0].position;
        //Vector3 pos = this.m_MoPaiAnchors[0].position; 

        //shouPai.transform.localPosition = new Vector3((m_SelfShouPais.Count - 1) * m_OtherShouPaiDistance0 / 2f + m_OtherShouPaiDistance0, 0, 0);
        shouPai.transform.localPosition = new Vector3(m_SelfShouPais.Count * m_OtherShouPaiDistance0 , 0, 0);
        this.m_MoPaiAnchors[0].position = shouPai.transform.position;
        this.CurrentMoPai = shouPai;
    }

    public void CreateOtherMoPai(int id)
    {
        this.CurrentTempGangPai = null;
        this.MoDiPai();
        GameObject shouPai = this.ConstructOtherShouPai(id, this.m_ShouPaiAnchors[id].transform.childCount.ToString());
        switch (id)
        {
            case 1:
                shouPai.transform.localPosition = new Vector3(0, m_ShouPaiAnchors[1].childCount * m_OtherShouPaiDistance1, 0);
                this.m_MoPaiAnchors[1].position = shouPai.transform.position;
                break;
            case 2:
                shouPai.transform.localPosition = new Vector3(-m_ShouPaiAnchors[2].childCount * m_OtherShouPaiDistance2, 0, 0);
                this.m_MoPaiAnchors[2].position = shouPai.transform.position;
                break;
            case 3:
                shouPai.transform.localPosition = new Vector3(0, -m_ShouPaiAnchors[3].childCount * m_OtherShouPaiDistance3, 0);
                this.m_MoPaiAnchors[3].position = shouPai.transform.position;
                break;
        }
       // shouPai.transform.position = this.m_MoPaiAnchors[id].position;

        this.CurrentMoPai = shouPai;
    }

    public void CreateSelfChuPai(int pai)
    {
        this.ConstructChuPai(0, pai);
        if (CurrentPlayerLogicBehavior.Instance.ShouPaiBehavior != null)
            CurrentPlayerLogicBehavior.Instance.ShouPaiBehavior.Reset();
        GameObject.DestroyImmediate(this.m_SelfShouPais[pai].gameObject);
        this.m_SelfShouPais.Remove(pai);
        this.m_Sequencer.Layout();

        this.CurrentMoPai = null;
    }

    public void CreateOtherChuPai(int id, int pai)
    {
        this.ConstructChuPai(id, pai);
        GameObject.DestroyImmediate(this.m_ShouPaiAnchors[id].GetChild(this.m_ShouPaiAnchors[id].childCount - 1).gameObject);

        this.CurrentMoPai = null;
    }

    public void CreateSelfPengPai(List<int> shouPais)
    {
        print("CreateSelfPengPai ========>>>>>>>>>>????>>>>>><<<<<<<<<<<<");
        if (CurrentPlayerLogicBehavior.Instance.ShouPaiBehavior != null)
            CurrentPlayerLogicBehavior.Instance.ShouPaiBehavior.Reset();
        foreach (var pai in shouPais)
        {
            GameObject.DestroyImmediate(this.m_SelfShouPais[pai].gameObject);
            print("CreateSelfPengPai  pai=" + pai);
            this.m_SelfShouPais.Remove(pai);
        }
        shouPais.Add(CurrentPlayerLogicBehavior.Instance.CurrentPai);
        this.ConstructPengPai(0, shouPais);
        this.m_Sequencer.Layout();
    }

    public void CreateOtherPengPai(int id)
    {
        List<int> pais = new List<int>();
		int type = CurrentPlayerLogicBehavior.Instance.CurrentPai / 4;
        for (int i = 0; i < 3; i++)
        {
            pais.Add(type * 4 + i);
        }
        this.ConstructPengPai(id, pais);
        for (int i = 0; i < 2; i++)
        {
            GameObject.DestroyImmediate(this.m_ShouPaiAnchors[id].GetChild(this.m_ShouPaiAnchors[id].childCount - 1).gameObject);
        }
    }

    public void CreateSelfHuPai(int pai)
    {
        this.ConstructHuPai(0, pai);
    }

    public void CreateOtherHuPai(int id, int pai)
    {
        this.ConstructHuPai(id, pai);
    }

    public void CreateSelfTempGangPai(int pai)
    {
        GameObject gangPai = GameObject.Instantiate(this.m_SelfGangPengPrefabs[pai]) as GameObject;
        gangPai.transform.parent = this.m_PengPaiAnchors[0];
        int type = pai /4;
        float length = 0;
        float start = 0;
        int indext = 0;
        foreach (KeyValuePair<int, int> item in this.m_PengPairsCount[0])
        {
            if (type == item.Key)
                break;
            else
                length += this.m_OtherShouPaiDistance0 * item.Value;
            indext++;
        }
        start = length + indext * this.m_PengPaiRowDistance0;
        gangPai.transform.localPosition = new Vector3(start +  this.m_OtherShouPaiDistance0, 12, 0);
        gangPai.GetComponentInChildren<tk2dSprite>().SortingOrder = 100;
        //this.m_PengPairsCount[id] // key = player pos;key of value = pai type; value of value = pai's count;
        //m_PengPais // key = player pos;key of value = pai type; value of value = pai;
        //gangPai.transform.localPosition = new Vector3(3 * this.m_OtherShouPaiDistance0, this.m_PengPais[0][pai / 4] * this.m_PengPaiRowDistance0, 0);
//#region dangerouse
      
//        UnityUtility.DestroyAllChild(this.m_PengPaiAnchors[0]);
//        int type = pai /4;
//        this.m_PengPairsCount[0][type] = 4; 
//        this.m_PengPaiIndexes[0] = 0;
//        int p = this.m_PengPais[0][type] * 4;
//        List<int> pais = new List<int>();
//        for(int i=0;i<4;i++)
//        {
//            pais.Add(p+i);
//        }
//        this.SetGangPosition(0, pais);
//#endregion 
        GameObject.DestroyImmediate(this.m_SelfShouPais[pai].gameObject);
        this.m_SelfShouPais.Remove(pai);
        this.CurrentTempGangPai = gangPai;

        this.m_Sequencer.Layout();
    }

    public void CreateOtherTempGangPai(int id, int pai)
    {

        GameObject gangPai = null;
        int type = pai / 4;
        float length = 0;
        float start = 0;
        int indext = 0;
        switch (id)
        {
            //case 0:
            //    gangPai = GameObject.Instantiate(this.m_SelfGangPengPrefabs[pai]) as GameObject;
            //    gangPai.transform.parent = this.m_PengPaiAnchors[id];
            //    gangPai.transform.localPosition = new Vector3(3 * this.m_OtherShouPaiDistance0 / 2, this.m_PengPais[id][pai / 4] * this.m_PengPaiRowDistance0, 0);
            //    break;
            case 1:
                gangPai = GameObject.Instantiate(this.m_ChuPaiPrefab1[pai]) as GameObject;
                gangPai.transform.parent = this.m_PengPaiAnchors[id];
                //gangPai.transform.localPosition = new Vector3(this.m_PengPais[id][pai / 4] * this.m_PengPaiRowDistance1, 3 * this.m_ChuPaiDistance1 / 2, 0);

                foreach (KeyValuePair<int, int> item in this.m_PengPairsCount[1])
                {
                    if (type == item.Key)
                        break;
                    else
                        length += this.m_ChuPaiDistance1 * item.Value;
                    indext++;
                }
                start = length + indext * this.m_PengPaiRowDistance1;
                gangPai.transform.localPosition = new Vector3(0 , start + this.m_ChuPaiDistance1 + 7, 0);
                gangPai.GetComponentInChildren<tk2dSprite>().SortingOrder = 100;
                break;
            case 2:
                gangPai = GameObject.Instantiate(this.m_ChuPaiPrefab2[pai]) as GameObject;
                gangPai.transform.parent = this.m_PengPaiAnchors[id];
                //gangPai.transform.localPosition = new Vector3(3 * this.m_ChuPaiDistance2 / 2, this.m_PengPais[id][pai / 4] * this.m_PengPaiRowDistance2, 0);
                foreach (KeyValuePair<int, int> item in this.m_PengPairsCount[2])
                {
                    if (type == item.Key)
                        break;
                    else
                        length += this.m_ChuPaiDistance2 * item.Value;
                    indext++;
                }
                start = length + indext * this.m_PengPaiRowDistance2;
                gangPai.transform.localPosition = new Vector3(-start - this.m_ChuPaiDistance2, 7, 0);
                gangPai.GetComponentInChildren<tk2dSprite>().SortingOrder = 100;
                break;
            case 3:
                 gangPai = GameObject.Instantiate(this.m_ChuPaiPrefab3[pai]) as GameObject;
                 gangPai.transform.parent = this.m_PengPaiAnchors[id];
                //gangPai.transform.localPosition = new Vector3(this.m_PengPais[id][pai / 4] * this.m_PengPaiRowDistance3, 3 * this.m_ChuPaiDistance3 / 2, 0);
    
                foreach (KeyValuePair<int, int> item in this.m_PengPairsCount[3])
                {
                    if (type == item.Key)
                        break;
                    else
                        length += this.m_ChuPaiDistance3 * item.Value;
                    indext++;
                }
                start = length + indext * this.m_PengPaiRowDistance3;
                gangPai.transform.localPosition = new Vector3(0 , -start - this.m_ChuPaiDistance3 + 7, 0);
                gangPai.GetComponentInChildren<tk2dSprite>().SortingOrder = 100;
                break;
        }
         
      //gangPai.transform.localPosition = new Vector3(3 * this.m_OtherShouPaiDistance0, this.m_PengPais[0][pai / 4] * this.m_PengPaiRowDistance0, 0);
 
    
        GameObject.DestroyImmediate(this.m_ShouPaiAnchors[id].GetChild(this.m_ShouPaiAnchors[id].childCount - 1).gameObject);
        this.CurrentTempGangPai = gangPai;
    }

    public void CreateSelfGangPai(List<int> shouPais, int pai)
    {
        if (CurrentPlayerLogicBehavior.Instance.ShouPaiBehavior != null)
            CurrentPlayerLogicBehavior.Instance.ShouPaiBehavior.Reset();
 
        foreach (var p in shouPais)
        { 
            GameObject.DestroyImmediate(this.m_SelfShouPais[p].gameObject);
            this.m_SelfShouPais.Remove(p);
        }
        if (shouPais.Count == 3)
        {
            shouPais.Add(CurrentPlayerLogicBehavior.Instance.CurrentPai);
            this.ConstructPengPai(0, shouPais);
        }
        else if (shouPais.Count == 4)
        {
            this.ConstructPengPai(0, shouPais);
        }
        else if (shouPais.Count == 0)
        {
            if (this.m_PengPairsCount[0].ContainsKey(pai / 4))
                this.m_PengPairsCount[0][pai / 4] = 4;
            this.LayoutShouPaiAnchor(0);
        }
        this.m_Sequencer.Layout();
    }

    public void CreateOtherGangPai(int id, int pai)
    {
        if (this.m_ShouPaiAnchors[id].childCount % 3 == 2)
        {
            List<int> pais = new List<int>();
			int type = pai / 4;
            for (int i = 0; i < 4; i++)
            {
                pais.Add(type * 4 + i);
            }
           // print("pai=============" + pai);
            this.ConstructPengPai(id, pais);

            for (int i = 0; i < 4; i++)
            {
                GameObject.DestroyImmediate(this.m_ShouPaiAnchors[id].GetChild(this.m_ShouPaiAnchors[id].childCount - 1).gameObject);
            }
        }
        else if (this.CurrentTempGangPai == null)
        {
            List<int> pais = new List<int>();
            for (int i = 0; i < 4; i++)
            {
                pais.Add(pai);
            }
            this.ConstructPengPai(id, pais);

            for (int i = 0; i < 3; i++)
            {
                GameObject.DestroyImmediate(this.m_ShouPaiAnchors[id].GetChild(this.m_ShouPaiAnchors[id].childCount - 1).gameObject);
            }
        }
    }

    public GameObject ConstructChuPai(int id, int pai)
    {
        GameObject chuPai = null;
        int row = this.m_ChuPaiIndexes[id] / this.m_ChuPaiPerRowCount;
        int column = this.m_ChuPaiIndexes[id] % this.m_ChuPaiPerRowCount;
        switch (id)
        {
            case 0:
                chuPai = GameObject.Instantiate(this.m_ChuPaiPrefab[pai]) as GameObject;
                chuPai.transform.parent = this.m_ChuPaiAnchors[id];
                chuPai.transform.localPosition = new Vector3(column * this.m_ChuPaiDistance0, -row * m_ChuPaiRowDistance0, 0);
                chuPai.GetComponentInChildren<tk2dSprite>().SortingOrder = row;
                break;
            case 1:
                chuPai = GameObject.Instantiate(this.m_ChuPaiPrefab1[pai]) as GameObject;
                chuPai.transform.parent = this.m_ChuPaiAnchors[id];
                chuPai.transform.localPosition = new Vector3(row * m_ChuPaiRowDistance1, column * this.m_ChuPaiDistance1, 0);
                //chuPai.GetComponentInChildren<tk2dSprite>().SortingOrder = -this.m_ChuPaiIndexes[id];
                chuPai.GetComponentInChildren<tk2dSprite>().SortingOrder = -column;
                break;
            case 2:
                chuPai = GameObject.Instantiate(this.m_ChuPaiPrefab2[pai]) as GameObject;
                chuPai.transform.parent = this.m_ChuPaiAnchors[id];
                chuPai.transform.localPosition = new Vector3(-column * this.m_ChuPaiDistance2, row * m_ChuPaiRowDistance2, 0);
                chuPai.GetComponentInChildren<tk2dSprite>().SortingOrder = -row;
                break;
            case 3:
                chuPai = GameObject.Instantiate(this.m_ChuPaiPrefab3[pai]) as GameObject;
                chuPai.transform.parent = this.m_ChuPaiAnchors[id];
                chuPai.transform.localPosition = new Vector3(-row * m_ChuPaiRowDistance3, -column * this.m_ChuPaiDistance3, 0);
                //chuPai.GetComponentInChildren<tk2dSprite>().SortingOrder = this.m_ChuPaiIndexes[id];
                chuPai.GetComponentInChildren<tk2dSprite>().SortingOrder = column;
                break;


        }

        this.m_ChuPaiIndexes[id]++;

        this.m_CurrentChuPaiIndex = id;
        this.CurrentChuPai = chuPai;
        return chuPai;
    }

    private GameObject ConstructOtherShouPai(int id, string name)
    {
        GameObject shouPai = GameObject.Instantiate(this.m_OtherShouPaiPrefab[id]) as GameObject;
 

        shouPai.transform.parent = this.m_ShouPaiAnchors[id];
        shouPai.name = name;
        return shouPai;
    }

    private GameObject ConstructSelfShouPai(int pai)
    {
        GameObject shouPai = GameObject.Instantiate(this.m_SelfShouPaiPrefabs[pai]) as GameObject;
        shouPai.transform.parent = this.m_ShouPaiAnchors[0];
        ShouPaiBehavior behavior = shouPai.GetComponent<ShouPaiBehavior>();
        behavior.Pai = pai;
        //Debug.Log(pai);
        this.m_SelfShouPais.Add(pai, behavior);//Bug
        return shouPai;
    }

    public void ConstructPengPai(int id, List<int> pais)
    {
        this.ConstructPengPai(id, pais, true);
    }

    public Dictionary<int, GameObject> ConstructPengPai(int id, List<int> pais, bool isNeedDestroyChuPai)
    {
        int type = pais[0] / 4;
        this.CurrentTempGangPai = null;
        Dictionary<int, GameObject> result = new Dictionary<int, GameObject>();
        
        // this.m_PengPaiAnchors[id].childCount * this.m_PengPaiColumnDistance
        GameObject pengPai = null;
        for (int i = 0; i < pais.Count; i++)
        {
           
            float length = 0;
            float start = 0;
            switch (id)
            {
                case 0:
                    pengPai = GameObject.Instantiate(this.m_SelfGangPengPrefabs[pais[i]]) as GameObject;
                    pengPai.transform.parent = this.m_PengPaiAnchors[id];  
                    foreach (KeyValuePair<int, int> item in this.m_PengPairsCount[id])
                    {
                        length += this.m_OtherShouPaiDistance0 * item.Value;
                    }
                    start = length + this.m_PengPaiIndexes[id] * this.m_PengPaiRowDistance0;
                    pengPai.transform.localPosition = new Vector3(start + i * this.m_OtherShouPaiDistance0, 0, 0);
           
                    break;
                case 1:
                    pengPai = GameObject.Instantiate(this.m_ChuPaiPrefab1[pais[i]]) as GameObject;
                    pengPai.transform.parent = this.m_PengPaiAnchors[id]; 
        
                    foreach (KeyValuePair<int,int> item in this.m_PengPairsCount[id])
                    {
                        length += this.m_ChuPaiDistance1 * item.Value; 
                    }
  
                     start = length + this.m_PengPaiIndexes[id] * this.m_PengPaiRowDistance1; 
                    
                    pengPai.transform.localPosition = new Vector3(0, start + i * this.m_ChuPaiDistance1, 0);
                    pengPai.GetComponentInChildren<tk2dSprite>().SortingOrder = -i - this.m_PengPaiIndexes[id] * 4 + 26;
              
                    break; 
                case 2:
                    pengPai = GameObject.Instantiate(this.m_ChuPaiPrefab2[pais[i]]) as GameObject;
                    pengPai.transform.parent = this.m_PengPaiAnchors[id]; 
                    foreach (KeyValuePair<int, int> item in this.m_PengPairsCount[id])
                    {
                        length += this.m_ChuPaiDistance2 * item.Value;
                    }
                    start = length + this.m_PengPaiIndexes[id] * this.m_PengPaiRowDistance2;
                    pengPai.transform.localPosition = new Vector3(-start - i * this.m_ChuPaiDistance2, 0, 0);
                
                    break;
                case 3:
                    pengPai = GameObject.Instantiate(this.m_ChuPaiPrefab3[pais[i]]) as GameObject;
                    pengPai.transform.parent = this.m_PengPaiAnchors[id];
                    foreach (KeyValuePair<int, int> item in this.m_PengPairsCount[id])
                    {
                        length += this.m_ChuPaiDistance3 * item.Value;
                    }
                    start = length + this.m_PengPaiIndexes[id] * this.m_PengPaiRowDistance3;
                    pengPai.transform.localPosition = new Vector3(0, -start - i * this.m_ChuPaiDistance3, 0);
                    pengPai.GetComponentInChildren<tk2dSprite>().SortingOrder =  i + this.m_PengPaiIndexes[id] * 4 - 26;
                 
                    break;
            }
           // result.Add(pais[i], pengPai);
            result.Add(type * 4 + i, pengPai);
        }
        print("type =" + type + "   key " + id  + "   value =" + this.m_PengPaiIndexes[id]);
        this.m_PengPais[id].Add(type, this.m_PengPaiIndexes[id]);
        this.m_PengPairsCount[id].Add(type, pais.Count);

        this.LayoutShouPaiAnchor(id);
        this.m_PengPaiIndexes[id]++;

        if (isNeedDestroyChuPai && this.CurrentChuPai != null)
        {
            GameObject.Destroy(this.CurrentChuPai);
            this.m_ChuPaiIndexes[this.m_CurrentChuPaiIndex]--;
        }
        return result;
    }
    //private void SetGangPosition(int id, List<int> pais)
    //{
    //    GameObject pengPai = null;
    //    for (int i = 0; i < pais.Count; i++)
    //    {

    //        float length = 0;
    //        float start = 0;
    //        switch (id)
    //        {
    //            case 0:
    //                pengPai = GameObject.Instantiate(this.m_SelfGangPengPrefabs[pais[i]]) as GameObject;
    //                pengPai.transform.parent = this.m_PengPaiAnchors[id];
    //                foreach (KeyValuePair<int, int> item in this.m_PengPairsCount[id])
    //                {
    //                    length += this.m_OtherShouPaiDistance0 * item.Value;
    //                }
    //                start = length + this.m_PengPaiIndexes[id] * this.m_PengPaiRowDistance0;
    //                pengPai.transform.localPosition = new Vector3(start + i * this.m_OtherShouPaiDistance0, 0, 0);

    //                break;
    //            case 1:
    //                pengPai = GameObject.Instantiate(this.m_ChuPaiPrefab1[pais[i]]) as GameObject;
    //                pengPai.transform.parent = this.m_PengPaiAnchors[id];

    //                foreach (KeyValuePair<int, int> item in this.m_PengPairsCount[id])
    //                {
    //                    length += this.m_ChuPaiDistance1 * item.Value;
    //                }

    //                start = length + this.m_PengPaiIndexes[id] * this.m_PengPaiRowDistance1;

    //                pengPai.transform.localPosition = new Vector3(0, start + i * this.m_ChuPaiDistance1, 0);
    //                pengPai.GetComponentInChildren<tk2dSprite>().SortingOrder = -i - this.m_PengPaiIndexes[id] * 4 + 26;

    //                break;
    //            case 2:
    //                pengPai = GameObject.Instantiate(this.m_ChuPaiPrefab2[pais[i]]) as GameObject;
    //                pengPai.transform.parent = this.m_PengPaiAnchors[id];
    //                foreach (KeyValuePair<int, int> item in this.m_PengPairsCount[id])
    //                {
    //                    length += this.m_ChuPaiDistance2 * item.Value;
    //                }
    //                start = length + this.m_PengPaiIndexes[id] * this.m_PengPaiRowDistance2;
    //                pengPai.transform.localPosition = new Vector3(-start - i * this.m_ChuPaiDistance2, 0, 0);

    //                break;
    //            case 3:
    //                pengPai = GameObject.Instantiate(this.m_ChuPaiPrefab3[pais[i]]) as GameObject;
    //                pengPai.transform.parent = this.m_PengPaiAnchors[id];
    //                foreach (KeyValuePair<int, int> item in this.m_PengPairsCount[id])
    //                {
    //                    length += this.m_ChuPaiDistance3 * item.Value;
    //                }
    //                start = length + this.m_PengPaiIndexes[id] * this.m_PengPaiRowDistance3;
    //                pengPai.transform.localPosition = new Vector3(0, -start - i * this.m_ChuPaiDistance3, 0);
    //                pengPai.GetComponentInChildren<tk2dSprite>().SortingOrder = i + this.m_PengPaiIndexes[id] * 4 - 26;

    //                break;
    //        } 
    //    }
    //    this.m_PengPairsCount[id].Add(pais[0]/4, pais.Count);

    //    this.LayoutShouPaiAnchor(id);
    //    this.m_PengPaiIndexes[id]++;
    //}
    private void LayoutShouPaiAnchor(int id)
    {
        float length = 0;
        float start = 0;
        Vector3 pos = Vector3.zero;
        switch (id)
        {
            case 0:
                foreach (KeyValuePair<int, int> item in this.m_PengPairsCount[id])
                {
                    length += this.m_OtherShouPaiDistance0 * item.Value;
                }
                start = this.m_PengPaiAnchors[id].position.x + length + this.m_PengPaiIndexes[id] * this.m_PengPaiRowDistance0 + this.m_PengPaiRowDistance0;
                pos = this.m_ShouPaiAnchors[0].position;
                this.m_ShouPaiAnchors[0].position = new Vector3(start, pos.y, pos.z);
                break;
            case 1:
                foreach (KeyValuePair<int, int> item in this.m_PengPairsCount[id])
                {
                    length += this.m_ChuPaiDistance1 * item.Value;
                }
                start = this.m_PengPaiAnchors[id].position.y + length + this.m_PengPaiIndexes[id] * this.m_PengPaiRowDistance1 + this.m_PengPaiRowDistance1;
                pos = this.m_ShouPaiAnchors[1].position;
                this.m_ShouPaiAnchors[1].position = new Vector3(pos.x, start, pos.z);
                break;
            case 2:
                foreach (KeyValuePair<int, int> item in this.m_PengPairsCount[id])
                {
                    length += this.m_ChuPaiDistance2 * item.Value;
                }
                start = this.m_PengPaiAnchors[id].position.x - length - this.m_PengPaiIndexes[id] * this.m_PengPaiRowDistance2 - this.m_PengPaiRowDistance2;
                pos = this.m_ShouPaiAnchors[2].position;
                this.m_ShouPaiAnchors[2].position = new Vector3(start, pos.y, pos.z);
                break;
            case 3:
                foreach (KeyValuePair<int, int> item in this.m_PengPairsCount[id])
                {
                    length += this.m_ChuPaiDistance3 * item.Value;
                }
                start = this.m_PengPaiAnchors[id].position.y - length - this.m_PengPaiIndexes[id] * this.m_PengPaiRowDistance3 - this.m_PengPaiRowDistance3;
                pos = this.m_ShouPaiAnchors[3].position;
                this.m_ShouPaiAnchors[3].position = new Vector3(pos.x, start, pos.z);
                break;
        }
    }
    public void ConstructHuPai(int id, int pai)
    {
        this.ConstructHuPai(id, pai, true);
    }

    public void ConstructHuPai(int id, int pai, bool isNeedDestroyPai)
    {
        if (isNeedDestroyPai && this.CurrentMoPai != null)
        {
            GameObject.Destroy(this.CurrentMoPai);
            this.CurrentMoPai = null;
        }
        if (isNeedDestroyPai && this.CurrentTempGangPai != null)
        {
            GameObject.Destroy(this.CurrentTempGangPai);
            this.CurrentTempGangPai = null;
        }

        GameObject huPai = null;
        switch (id)
        {
            case 0:
                huPai = GameObject.Instantiate(this.m_SelfGangPengPrefabs[pai]) as GameObject;
                break;
            case 1:
                huPai = GameObject.Instantiate(this.m_ChuPaiPrefab1[pai]) as GameObject;
                break;
            case 2:
                huPai = GameObject.Instantiate(this.m_ChuPaiPrefab2[pai]) as GameObject;
                break;
            case 3:
                huPai = GameObject.Instantiate(this.m_ChuPaiPrefab3[pai]) as GameObject;
                break;
        }
        huPai.transform.parent = this.m_MoPaiAnchors[id];
        huPai.transform.localPosition = Vector3.zero;
    }
    public void ShowAllPais(Dictionary<string,List<int>> playerShouPais)
    {
        this.DestoryShouPai();
        List<int> pais = new List<int>();
        Vector3 offset = Vector3.zero;
        foreach (var item in MaJiangManager.Instance.PlayerPositionDict)
        {
            switch (item.Value)
            {
                case 0:
                    break;
                case 1:
                     pais = playerShouPais[item.Key];
                     pais.Sort();
                     for (int i = 0; i < pais.Count; i++)
                     {
                         GameObject shouPai = Instantiate(m_ChuPaiPrefab1[pais[i]]) as GameObject;
                         shouPai.transform.parent = this.m_ShouPaiAnchors[1];
                         offset = new Vector3(0, /*start +*/ i * this.m_ChuPaiDistance1, 0);
                         shouPai.GetComponentInChildren<tk2dSprite>().SortingOrder = pais.Count - i;
                         shouPai.transform.localPosition = offset;
                     } 
                    break;
                case 2:
                     pais = playerShouPais[item.Key];
                     pais.Sort();
                     for (int i = 0; i < pais.Count; i++)
                     {
                         GameObject shouPai = Instantiate(m_ChuPaiPrefab2[pais[i]]) as GameObject;
                         shouPai.transform.parent = this.m_ShouPaiAnchors[2];
                         offset = new Vector3(/*start*/ -i * this.m_ChuPaiDistance2, 0, 0);
                         shouPai.transform.localPosition = offset;
                     } 
                    break;
                case 3:
                     pais = playerShouPais[item.Key];
                     pais.Sort();
                     for (int i = 0; i < pais.Count; i++)
                     {
                         GameObject shouPai = Instantiate(m_ChuPaiPrefab3[pais[i]]) as GameObject;
                         shouPai.transform.parent = this.m_ShouPaiAnchors[3];
                         offset = new Vector3(0, /*start */-i * this.m_ChuPaiDistance3, 0);
                         shouPai.GetComponentInChildren<tk2dSprite>().SortingOrder = i;
                         shouPai.transform.localPosition = offset;
                     } 
                    break;
            }
        }
    }
    private void DestoryShouPai()
    {
        for (int i = 1; i < m_ShouPaiAnchors.Length; i++)
        {
            UnityUtility.DestroyAllChild(m_ShouPaiAnchors[i]); 
        }
    }
}
