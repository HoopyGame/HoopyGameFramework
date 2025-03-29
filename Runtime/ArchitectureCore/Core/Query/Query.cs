/*
 | | | | ___   ___  _ __  _   _ / ___| __ _ _ __ ___   ___ 
 | |_| |/ _ \ / _ \| '_ \| | | | |  _ / _` | '_ ` _ \ / _ \
 |  _  | (_) | (_) | |_) | |_| | |_| | (_| | | | | | |  __/
 |_| |_|\___/ \___/| .__/ \__, |\____|\__,_|_| |_| |_|\___|
                   |_|    |___/                            
┌──────────────────────────────────────────────┐
│　Copyright(C) 2025 by HoopyGameStudio
│　描   述*：查询层
│　创 建 人*：Hoopy
│　创建时间：2025-02-28 13:04:00
└──────────────────────────────────────────────┘
┌──────────────────────────────────────────────┐
│　修 改 人：
│　修改描述：
└──────────────────────────────────────────────┘
*/

namespace HoopyGame.Core
{
    public interface IQuery<TResult> : IBelongToHGArchitecture, ICanSetHGArchitecture, ICanGetModel, ICanGetSystem
        , ICanSendQuery
    {
        TResult Do();
    }
    public abstract class Query<TResult> : IQuery<TResult>
    {
        private IHGArchitecture _hgArchitecture;
        public IHGArchitecture GetHGArchitecture() => _hgArchitecture;
        public void SetHGArichitecture(IHGArchitecture hgArchitecture)=>_hgArchitecture = hgArchitecture;

        public TResult Do() => OnDo();
        protected abstract TResult OnDo();
    }

}