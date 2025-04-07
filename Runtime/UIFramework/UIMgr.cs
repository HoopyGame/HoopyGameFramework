/*
 | | | | ___   ___  _ __  _   _ / ___| __ _ _ __ ___   ___ 
 | |_| |/ _ \ / _ \| '_ \| | | | |  _ / _` | '_ ` _ \ / _ \
 |  _  | (_) | (_) | |_) | |_| | |_| | (_| | | | | | |  __/
 |_| |_|\___/ \___/| .__/ \__, |\____|\__,_|_| |_| |_|\___|
                   |_|    |___/                            
┌──────────────────────────────────────────────┐
│　Copyright(C) 2025 by HoopyGameStudio
│　描   述*：UI管理器，统筹管理所有UI
│　创 建 人*：Hoopy
│　创建时间：2025-01-01 00:00:00
└──────────────────────────────────────────────┘
┌──────────────────────────────────────────────┐
│　修 改 人：
│　修改描述：
└──────────────────────────────────────────────┘
*/

using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using HoopyGame.Core;

namespace HoopyGame.UIF
{
    public class UIMgr : SingleBaseMono<UIMgr>, IController
    {
        public RectTransform UIRoot { get; private set; }

        /// <summary>
        /// 获取Canvas的Rect数据
        /// </summary>
        public Rect GetCanvasRect => UIRoot.rect;
  
        public RectTransform PanelRoot { get; private set; }                          //PanelParent
        public RectTransform PopupRoot { get; private set; }                          //PopupParent

        public Image Shield { get; private set; }                                     //遮罩

        private readonly float _shieldHight = .8f;                                    //遮罩的暗度
        private readonly float _shieldHideDuration = .2f;                             //隐藏或显示持续时间

        private Dictionary<UIType, Dictionary<string, BaseUI>> _totalUIMap;           //所有UI的存放
        private Dictionary<string, BasePopup> _openingPopupUI;                        //目前打开着的Popup

        private LeastResentlyUsedUtility _notUsedResentlyUI;                            //最久未使用算法
        private UIFactory _uiFactory;

        public IHGArchitecture GetHGArchitecture() => UIArchitecture.Instance;

        private void Awake()
        {
            //Tentative：创建Lazy的优先级好似比较高 这里暂定不修改
            //           内部写了_inited来规避二次初始化，后续若有问题，再来修改这里
            InitSelf(this);
        }

        /// <summary>
        /// 初始化操作
        /// </summary>
        public override void OnInit()
        {
            InitUIMap();
            _notUsedResentlyUI = this.GetUtility<LeastResentlyUsedUtility>();
            //实例工厂
            _uiFactory = new UIFactory();
            try
            {
                //这里默认是RootUI
                UIRoot = GameObject.Find("UIRoot").GetComponent<RectTransform>();
                PanelRoot = UIRoot.Find("PanelRoot").GetComponent<RectTransform>();
                PopupRoot = UIRoot.Find("PopupRoot").GetComponent<RectTransform>();
                Shield = PopupRoot.Find("Shield").GetComponent<Image>();
                Shield.gameObject.SetActive(false);
                AdaptToSafeArea();
            }
            catch
            {
                throw new MissingReferenceException("没有找到UIRoot或内容，请使用UIF的UIRoot或追踪此错误信息修改try内查找的名字");
            }

            DebugUtils.Print("UIManager初始化完成！");
        }

        /// <summary>
        /// 初始化UIMap
        /// </summary>
        private void InitUIMap()
        {
            _totalUIMap = new Dictionary<UIType, Dictionary<string, BaseUI>>();
            foreach (UIType uiType in Enum.GetValues(typeof(UIType)))
            {
                _totalUIMap.Add(uiType, new Dictionary<string, BaseUI>());
            }
            //打开着的UI
            _openingPopupUI = new Dictionary<string, BasePopup>();
        }

        /// <summary>
        /// 控制Shield遮罩的显示与隐藏
        /// </summary>
        /// <param name="isShow"></param>
        public void ShieldControl(bool isShow)
        {
            Shield.DOKill(true);
            if (isShow)
            {
                if (!Shield.gameObject.activeSelf)
                {
                    Shield.gameObject.SetActive(true);
                    Shield.DOFade(_shieldHight, _shieldHideDuration)
                        .SetEase(Ease.Linear);
                }
                else
                {
                    Shield.transform.SetAsLastSibling();
                }
            }
            else
            {
                Shield.DOFade(0, _shieldHideDuration)
                    .SetEase(Ease.Linear)
                    .OnComplete(() =>
                    {
                        Shield.gameObject.SetActive(false);
                    });
            }

        }

        /// <summary>
        /// 控制PanelBase的高度，保证屏幕适配异形屏
        /// </summary>
        public void AdaptToSafeArea()
        {
            //获取一个对于安全范围大小的锚点
            Rect safeArea = Screen.safeArea;

            Vector2 safeAreaAnchorMin = safeArea.position;
            Vector2 safeAreaAnchorMax = safeArea.position + safeArea.size;

            //用来计算对应到Canvas上的锚点
            safeAreaAnchorMin.x /= Screen.width;
            safeAreaAnchorMin.y /= Screen.height;

            safeAreaAnchorMax.x /= Screen.width;
            safeAreaAnchorMax.y /= Screen.height;

            //将panel的锚点设置为此  Popup是弹窗不需要Topbar
            PanelRoot.anchorMin = safeAreaAnchorMin;
            PanelRoot.anchorMax = safeAreaAnchorMax;
        }

        #region FindUI
        /// <summary>
        /// 仅仅查找一个UI是否已经加载了
        /// </summary>
        /// <param name="uiName">UI名字</param>
        /// <param name="uiType">UI类型</param>
        /// <returns>是否找到</returns>
        public bool FindUIIsLoaded(string uiName, Dictionary<string, BaseUI> uiDic)
        {
            return uiDic.ContainsKey(uiName);
        }
        /// <summary>
        /// 查找一个UI是否已经加载并Out它
        /// </summary>
        /// <param name="uiName">UI名字</param>
        /// <param name="uiType">UI类型</param>
        /// <param name="findUIOut">返回BaseUI，没找到则为Null</param>
        /// <returns>是否找到</returns>
        public bool FindUIIsLoaded(string uiName, Dictionary<string, BaseUI> uiDic, out BaseUI findUIOut)
        {
            return uiDic.TryGetValue(uiName, out findUIOut);
        }
        #endregion
        #region UIController
        /// <summary>
        /// 打开一个UI
        /// </summary>
        /// <param name="uiName">UI名字</param>
        /// <param name="uiType">UI的类型</param>
        /// <param name="data">要传的数据（可空）</param>
        /// <param name="closeEvent">关闭事件(可空)</param>
        /// <param name="sureEvent">确认事件(可空)</param>
        /// <returns>返回这个UI</returns>
        /// <exception cref="MissingFieldException"></exception>
        public BaseUI OpenUI(string uiName, UIType uiType, IUIDataBase data = null, Action closeEvent = null, Action sureEvent = null)
        {
            if (string.IsNullOrEmpty(uiName)) throw new MissingFieldException("要加载的UI名字为空");
            Transform uiParent = FindUIParent(uiType);
            if (FindUIIsLoaded(uiName, _totalUIMap[uiType], out BaseUI baseUI))
            {
                baseUI.Open(data);
            }
            else
            {
                baseUI = _uiFactory.LoadUI(uiName, uiParent);
                baseUI.Open(data);
                _totalUIMap[uiType].Add(uiName, baseUI);
            }
            BindDefaultEvent(baseUI, uiType, closeEvent, sureEvent);
            return baseUI;
        }

        public async UniTask<BaseUI> OpenUIAsync(string uiName, UIType uiType, IUIDataBase data = null, Action closeEvent = null, Action sureEvent = null)
        {
            if (string.IsNullOrEmpty(uiName)) throw new MissingFieldException("要加载的UI名字为空");
            Transform uiParent = FindUIParent(uiType);
            if (FindUIIsLoaded(uiName, _totalUIMap[uiType], out BaseUI baseUI))
            {
                baseUI.Open(data);
            }
            else
            {
                baseUI = await _uiFactory.LoadUIAsync(uiName, uiParent);
                baseUI.Open(data);
                _totalUIMap[uiType].Add(uiName, baseUI);
            }
            BindDefaultEvent(baseUI, uiType, closeEvent, sureEvent);
            return baseUI;
        }
        /// <summary>
        /// 隐藏（复用）或关闭（不复用）一个UI
        /// </summary>
        /// <param name="uiName">UI名字</param>
        /// <param name="uiType">UI类型</param>
        /// <param name="isDestory">是否删除</param>
        public void CloseUI(string uiName, UIType uiType, bool isDestory = false)
        {
            if (string.IsNullOrEmpty(uiName)) return;
            if (!FindUIIsLoaded(uiName, _totalUIMap[uiType], out BaseUI baseUI))
                throw new ArgumentException("尝试关闭了一个不存在或底层的UI，这是非自然情况，需重点关注，或咨询作者！UIName:" + uiName);

            baseUI.Close(isDestory);
        }
        /// <summary>
        /// 隐藏（复用）或关闭（不复用）一个UI
        /// </summary>
        /// <param name="baseUI">UI</param>
        /// <param name="uiType">UI类型</param>
        /// <param name="isDestory">是否删除</param>
        public void CloseUI(BaseUI baseUI, UIType uiType, bool isDestory = false)
        {
            if (isDestory)
            {
                RemoveUIFromUIMap(baseUI.name, uiType);
                _notUsedResentlyUI.RemoveFromTotalHideUIList(baseUI.name);
                baseUI.OnRemoveListener();
                baseUI.OnDestory();
                DestroyImmediate(baseUI.gameObject);
            }
            else
            {
                _notUsedResentlyUI.AddToTotalHideUIList(baseUI);
                baseUI.OnClose();
                baseUI.gameObject.SetActive(false);
            }
        }
        /// <summary>
        /// 将一个UI从UIMap中移除
        /// </summary>
        /// <param name="uiName"></param>
        /// <param name="uiType"></param>
        public void RemoveUIFromUIMap(string uiName, UIType uiType)
        {
            _totalUIMap[uiType].Remove(uiName);
        }
        /// <summary>
        /// 清空所有UI, 通常在跳转场景会使用
        /// </summary>
        /// <param name="automatic">是否逐个Close</param>
        public void ClaerAllUI(bool automatic = true)
        {
            if (!automatic)
            {
                //这里两层foreach也没关系，因为这里用空间换时间了
                foreach (var uiMapValues in _totalUIMap.Values)
                {
                    foreach (var ui in uiMapValues.Values)
                    {
                        ui.Close(true);
                    }
                    uiMapValues.Clear();
                }
            }
            _openingPopupUI.Clear();
        }
        public void ClosePopupToCheckShield(string uiName)
        {
            //如果当前没有弹窗
            _openingPopupUI.Remove(uiName);
            if (_openingPopupUI.Count > 0)
            {
                _openingPopupUI.Last().Value.Open();
            }
            else
            {
                ShieldControl(false);
            }
        }

        #endregion

        /// <summary>
        /// 通过Type寻找此UI应该在那个父类下面
        /// </summary>
        /// <param name="uiType">UI类型</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        private Transform FindUIParent(UIType uiType)
        {
            return uiType switch
            {
                UIType.Panel => PanelRoot,
                UIType.Popup => PopupRoot,
                _ => throw new NotImplementedException(),
            };
        }
        private void BindDefaultEvent(BaseUI baseUI, UIType uiType, Action closeEvent = null, Action sureEvent = null)
        {
            baseUI.InitCloseCallback(closeEvent);
            if (uiType == UIType.Popup)
            {
                //添加事件
                BasePopup basePopup = baseUI.GetComponent<BasePopup>();
                //这样的目的是为了保证打开的顺序 防止出现A打开B，B打开C，C再打开A导致先关闭A后显示的是B的问题
                if (_openingPopupUI.ContainsKey(baseUI.name))
                    _openingPopupUI.Remove(baseUI.name);
                _openingPopupUI.Add(baseUI.name, basePopup);
                basePopup.InitSureCallback(sureEvent);
            }
        }

    }
}