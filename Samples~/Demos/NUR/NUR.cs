using UnityEngine;
using HoopyGame.Manager;

public class NUR : MonoBehaviour
{
    //private PQManager<PQBase> pqManager;

    //private readonly List<int> ints = new() { 2,4,2,1,2,2,1 };

    private void Awake()
    {
        //pqManager = new PQManager<PQBase>(5);

    }
    void Start()
    {
        //foreach (var item in ints)
        //{
        //    pqManager.Enqueue(new CallBackPQ(item, () =>
        //    {
        //        Debug.Log("�ҵ����ȼ���:" + item);
        //    }));
        //}

        //for (int i = 0; i < ints.Count; i++)
        //{
        //    PQBase pQBase = pqManager.DeQueue();
        //    pQBase.Execute();
        //}
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            LSMgr.Instance.GetFromeGLS<UIMgr>().OpenUI("pan", UIType.Panel, new PanelOneData { name = "����" }, () =>
            {
                Debug.Log("�ر����UI");
            });
        }
        if(Input.GetKeyUp(KeyCode.B))
        {
            LSMgr.Instance.GetFromeGLS<UIMgr>().OpenUI("pop", UIType.Popup, new PopupOnew.PopupOneData { name = "����"}, () =>
            {
                Debug.Log("�ر����UI");
            });
        }
    }
}
