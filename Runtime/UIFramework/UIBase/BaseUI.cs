/*
 | | | | ___   ___  _ __  _   _ / ___| __ _ _ __ ___   ___ 
 | |_| |/ _ \ / _ \| '_ \| | | | |  _ / _` | '_ ` _ \ / _ \
 |  _  | (_) | (_) | |_) | |_| | |_| | (_| | | | | | |  __/
 |_| |_|\___/ \___/| .__/ \__, |\____|\__,_|_| |_| |_|\___|
                   |_|    |___/                            
┌──────────────────────────────────────────────┐
│　Copyright(C) 2025 by HoopyGameStudio
│　描   述*：所有UI面板的抽象基类
│　创 建 人*：Hoopy
│　创建时间：2025-01-01 00:00:00
└──────────────────────────────────────────────┘
┌──────────────────────────────────────────────┐
│　修 改 人：
│　修改描述：
└──────────────────────────────────────────────┘
*/
using HoopyGame.Core;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace HoopyGame.UIF
{
    public abstract class BaseUI : MonoBehaviour, IController
    {
        private IUIDataBase _data;
        protected IUIDataBase Data { get { return _data; } }//数据

        protected Button closeBtn;                          //除了主页面都有返回功能
        [HideInInspector]
        public UIType uiType;                               //UI类型 新的类型要在这里增加

        private Action _closeBtnEvent;                      //关闭触发的事件

        public virtual IHGArchitecture GetHGArchitecture() => UIArchitecture.Instance;

        /// <summary>
        /// 是否已经初始化
        /// </summary>
        public bool IsInit { get; private set; }
        /// <summary>
        /// Open一个UI(可重写OpenEff方法自定义弹出效果)
        /// </summary>
        /// <param name="data">数据</param>
        public virtual void Open(IUIDataBase data = null)
        {
            //这里是为了避免回退打开时传入空data将原本的data覆盖掉
            if (data != null) _data = data;
            if (!IsInit) Init();//没有初始化过才会初始化
            OnStart();//Start

            gameObject.SetActive(true);
            transform.SetAsLastSibling();
        }
        /// <summary>
        /// 初始化
        /// </summary>
        private void Init()
        {
            closeBtn = transform.FindButtonFrommChilds("Close_Btn");
            //Init以后初始化组件
            OnInitComponent();
            //添加事件
            OnAddListener();
            IsInit = true;
        }
        /// <summary>
        /// Close 
        /// </summary>
        public virtual void Close(bool isDestroy = false)
        {
            UIMgr.Instance.CloseUI(this, uiType, isDestroy);
        }
        public void InitCloseCallback(Action act)
        {
            if (act != null)
            {
                _closeBtnEvent = act;
            }
        }
        #region Listener
        /// <summary>
        /// 初始化组件(基于Open)
        /// </summary>
        public virtual void OnInitComponent() { }
        /// <summary>
        /// 添加事件(基于Open)
        /// </summary>
        public virtual void OnAddListener() { closeBtn?.OnRegister(OnCloseBtnClick); }
        /// <summary>
        /// 每次打开页面(基于Open其他页面打开)
        /// </summary>
        public virtual void OnStart() { }
        /// <summary>
        /// 关闭页面(基于Close_BtnEvent(False))
        /// </summary>
        public virtual void OnClose() { }
        /// <summary>
        /// 删除页面(基于Close_BtnEvent(True))
        /// </summary>
        public virtual void OnDestory() { }
        /// <summary>
        /// 移除事件(基于Close_BtnEvent(True))
        /// </summary>
        public virtual void OnRemoveListener() { closeBtn?.OnClear(); _closeBtnEvent = null; }
        /// <summary>
        /// 点击Close的事件(不需要Base)
        /// </summary>
        public virtual void OnCloseBtnClick() { _closeBtnEvent?.Invoke(); Close(); }
        #endregion
    }
}