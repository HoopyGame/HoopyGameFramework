using UnityEngine;
using HoopyGame.UIF;

public struct PanelOneData : IUIDataBase
{
    public string name;
    public int id;
    public float hight;
}
public class PanelOnew : BasePanel<PanelOneData>
{
    public override void OnStart()
    {
        base.OnStart();
        Debug.Log("Start" + data.name);
    }
}
