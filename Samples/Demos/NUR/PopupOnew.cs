using HoopyGame.UIF;
using static PopupOnew;
public class PopupOnew : BasePopupIncludeData<PopupOneData>
{
    public class PopupOneData : IUIDataBase
    {
        public string name;
    }

    public override void OnStart()
    {
        base.OnStart();
        DebugUtils.Print(data.name);
    }
}
