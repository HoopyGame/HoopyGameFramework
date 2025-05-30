﻿/*
 | | | | ___   ___  _ __  _   _ / ___| __ _ _ __ ___   ___ 
 | |_| |/ _ \ / _ \| '_ \| | | | |  _ / _` | '_ ` _ \ / _ \
 |  _  | (_) | (_) | |_) | |_| | |_| | (_| | | | | | |  __/
 |_| |_|\___/ \___/| .__/ \__, |\____|\__,_|_| |_| |_|\___|
                   |_|    |___/                            
┌──────────────────────────────────────────────┐
│　Copyright(C) 2025 by HoopyGameStudio
│　描   述*：
│　创 建 人*：Hoopy
│　创建时间：2025-01-01 00:00:00
└──────────────────────────────────────────────┘
┌──────────────────────────────────────────────┐
│　修 改 人：
│　修改描述：
└──────────────────────────────────────────────┘
*/
using System;
using System.Collections.Generic;
using VContainer;
using VContainer.Unity;

namespace HoopyGame.Manager
{
    public class LSMgr : SingleBaseMono<LSMgr>
    {
        private Dictionary<Type, LifetimeScope> _lifetimeScopes;

        public override void OnInit()
        {
            base.OnInit();
            _lifetimeScopes = new Dictionary<Type, LifetimeScope>();
        }
        /// <summary>
        /// 直接获取当前ICO容器里的某个类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public T Get<T>(Type t) where T : class
            => GetLifetimeScopeContainer(t).Resolve<T>();
        /// <summary>
        /// 直接从GameLifetimeScope里寻找
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetFromeGLS<T>() 
            => GetLifetimeScopeContainer(typeof(GameLifetimeScope)).Resolve<T>();
        /// <summary>
        /// 获取当类型作用域
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public IObjectResolver GetLifetimeScopeContainer(Type t)
            => GetLifetimeScope(t).Container;
        /// <summary>
        /// 获取当类型作用于
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public LifetimeScope GetLifetimeScope<T>(T t) where T : Type
        {
            if (!_lifetimeScopes.TryGetValue(t, out var lifetimeScope))
            {
                lifetimeScope = gameObject.AddComponent(t) as LifetimeScope;
                _lifetimeScopes.Add(t, lifetimeScope);
            }
            return lifetimeScope;
        }
    }

    public class UILifetimeScope : LifetimeScope
    {

    }
}