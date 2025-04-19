/*
 | | | | ___   ___  _ __  _   _ / ___| __ _ _ __ ___   ___ 
 | |_| |/ _ \ / _ \| '_ \| | | | |  _ / _` | '_ ` _ \ / _ \
 |  _  | (_) | (_) | |_) | |_| | |_| | (_| | | | | | |  __/
 |_| |_|\___/ \___/| .__/ \__, |\____|\__,_|_| |_| |_|\___|
                   |_|    |___/                            
┌──────────────────────────────────────────────┐
│　Copyright(C) 2025 by HoopyGameStudio
│　描   述*：依赖注入，使用VContainer来进行IOC
│　创 建 人*：Hoopy
│　创建时间：2025-01-01 00:00:00
└──────────────────────────────────────────────┘
┌──────────────────────────────────────────────┐
│　修 改 人：
│　修改描述：
└──────────────────────────────────────────────┘
*/
using VContainer;
using VContainer.Unity;

namespace HoopyGame
{
    public class GameLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {

            //--不需要Mono的单例
            //事件系统
            builder.Register<EventMgr>(Lifetime.Singleton);
            //对象池系统
            builder.Register<ObjectPoolMgr>(Lifetime.Singleton);
            //资源管理系统
            builder.Register<AssetMgr>(Lifetime.Singleton);

            //--需要Mono的单例
            builder.Register<AudioMgr>(Lifetime.Singleton);
        }
    }
}