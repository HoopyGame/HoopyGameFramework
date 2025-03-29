/*
 | | | | ___   ___  _ __  _   _ / ___| __ _ _ __ ___   ___ 
 | |_| |/ _ \ / _ \| '_ \| | | | |  _ / _` | '_ ` _ \ / _ \
 |  _  | (_) | (_) | |_) | |_| | |_| | (_| | | | | | |  __/
 |_| |_|\___/ \___/| .__/ \__, |\____|\__,_|_| |_| |_|\___|
                   |_|    |___/                            
┌──────────────────────────────────────────────┐
│　Copyright(C) 2025 by HoopyGameStudio
│　描   述*：系统层
│　创 建 人*：Hoopy
│　创建时间：2025-02-24 19:08:27
└──────────────────────────────────────────────┘
┌──────────────────────────────────────────────┐
│　修 改 人：
│　修改描述：
└──────────────────────────────────────────────┘
*/

namespace HoopyGame.Core
{
    public interface ISystem:IBelongToHGArchitecture,ICanSetHGArchitecture,ICanGetModel, ICanGetSystem, ICanGetUtility,
        ICanRegisterEvent,ICanSendEvent, ICanInit
    {

	}
    public abstract class AbstractSystem : ISystem
    {
        private IHGArchitecture _hgArchitecture;
        public IHGArchitecture GetHGArchitecture() => _hgArchitecture;
        public void SetHGArichitecture(IHGArchitecture hgArchitecture) => _hgArchitecture = hgArchitecture;


        public bool Initialized { get; set; }

        void ICanInit.Init() => OnInit();
        protected abstract void OnInit();

        public void DeInit() => OnDeInit();
        protected virtual void OnDeInit() { }
    }
}