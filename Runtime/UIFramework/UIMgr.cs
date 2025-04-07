/*
 | | | | ___   ___  _ __  _   _ / ___| __ _ _ __ ___   ___ 
 | |_| |/ _ \ / _ \| '_ \| | | | |  _ / _` | '_ ` _ \ / _ \
 |  _  | (_) | (_) | |_) | |_| | |_| | (_| | | | | | |  __/
 |_| |_|\___/ \___/| .__/ \__, |\____|\__,_|_| |_| |_|\___|
                   |_|    |___/                            
������������������������������������������������������������������������������������������������
����Copyright(C) 2025 by HoopyGameStudio
������   ��*��UI��������ͳ���������UI
������ �� ��*��Hoopy
��������ʱ�䣺2025-01-01 00:00:00
������������������������������������������������������������������������������������������������
������������������������������������������������������������������������������������������������
������ �� �ˣ�
�����޸�������
������������������������������������������������������������������������������������������������
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
        /// ��ȡCanvas��Rect����
        /// </summary>
        public Rect GetCanvasRect => UIRoot.rect;
  
        public RectTransform PanelRoot { get; private set; }                          //PanelParent
        public RectTransform PopupRoot { get; private set; }                          //PopupParent

        public Image Shield { get; private set; }                                     //����

        private readonly float _shieldHight = .8f;                                    //���ֵİ���
        private readonly float _shieldHideDuration = .2f;                             //���ػ���ʾ����ʱ��

        private Dictionary<UIType, Dictionary<string, BaseUI>> _totalUIMap;           //����UI�Ĵ��
        private Dictionary<string, BasePopup> _openingPopupUI;                        //Ŀǰ���ŵ�Popup

        private LeastResentlyUsedUtility _notUsedResentlyUI;                            //���δʹ���㷨
        private UIFactory _uiFactory;

        public IHGArchitecture GetHGArchitecture() => UIArchitecture.Instance;

        private void Awake()
        {
            //Tentative������Lazy�����ȼ����ƱȽϸ� �����ݶ����޸�
            //           �ڲ�д��_inited����ܶ��γ�ʼ���������������⣬�����޸�����
            InitSelf(this);
        }

        /// <summary>
        /// ��ʼ������
        /// </summary>
        public override void OnInit()
        {
            InitUIMap();
            _notUsedResentlyUI = this.GetUtility<LeastResentlyUsedUtility>();
            //ʵ������
            _uiFactory = new UIFactory();
            try
            {
                //����Ĭ����RootUI
                UIRoot = GameObject.Find("UIRoot").GetComponent<RectTransform>();
                PanelRoot = UIRoot.Find("PanelRoot").GetComponent<RectTransform>();
                PopupRoot = UIRoot.Find("PopupRoot").GetComponent<RectTransform>();
                Shield = PopupRoot.Find("Shield").GetComponent<Image>();
                Shield.gameObject.SetActive(false);
                AdaptToSafeArea();
            }
            catch
            {
                throw new MissingReferenceException("û���ҵ�UIRoot�����ݣ���ʹ��UIF��UIRoot��׷�ٴ˴�����Ϣ�޸�try�ڲ��ҵ�����");
            }

            DebugUtils.Print("UIManager��ʼ����ɣ�");
        }

        /// <summary>
        /// ��ʼ��UIMap
        /// </summary>
        private void InitUIMap()
        {
            _totalUIMap = new Dictionary<UIType, Dictionary<string, BaseUI>>();
            foreach (UIType uiType in Enum.GetValues(typeof(UIType)))
            {
                _totalUIMap.Add(uiType, new Dictionary<string, BaseUI>());
            }
            //���ŵ�UI
            _openingPopupUI = new Dictionary<string, BasePopup>();
        }

        /// <summary>
        /// ����Shield���ֵ���ʾ������
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
        /// ����PanelBase�ĸ߶ȣ���֤��Ļ����������
        /// </summary>
        public void AdaptToSafeArea()
        {
            //��ȡһ�����ڰ�ȫ��Χ��С��ê��
            Rect safeArea = Screen.safeArea;

            Vector2 safeAreaAnchorMin = safeArea.position;
            Vector2 safeAreaAnchorMax = safeArea.position + safeArea.size;

            //���������Ӧ��Canvas�ϵ�ê��
            safeAreaAnchorMin.x /= Screen.width;
            safeAreaAnchorMin.y /= Screen.height;

            safeAreaAnchorMax.x /= Screen.width;
            safeAreaAnchorMax.y /= Screen.height;

            //��panel��ê������Ϊ��  Popup�ǵ�������ҪTopbar
            PanelRoot.anchorMin = safeAreaAnchorMin;
            PanelRoot.anchorMax = safeAreaAnchorMax;
        }

        #region FindUI
        /// <summary>
        /// ��������һ��UI�Ƿ��Ѿ�������
        /// </summary>
        /// <param name="uiName">UI����</param>
        /// <param name="uiType">UI����</param>
        /// <returns>�Ƿ��ҵ�</returns>
        public bool FindUIIsLoaded(string uiName, Dictionary<string, BaseUI> uiDic)
        {
            return uiDic.ContainsKey(uiName);
        }
        /// <summary>
        /// ����һ��UI�Ƿ��Ѿ����ز�Out��
        /// </summary>
        /// <param name="uiName">UI����</param>
        /// <param name="uiType">UI����</param>
        /// <param name="findUIOut">����BaseUI��û�ҵ���ΪNull</param>
        /// <returns>�Ƿ��ҵ�</returns>
        public bool FindUIIsLoaded(string uiName, Dictionary<string, BaseUI> uiDic, out BaseUI findUIOut)
        {
            return uiDic.TryGetValue(uiName, out findUIOut);
        }
        #endregion
        #region UIController
        /// <summary>
        /// ��һ��UI
        /// </summary>
        /// <param name="uiName">UI����</param>
        /// <param name="uiType">UI������</param>
        /// <param name="data">Ҫ�������ݣ��ɿգ�</param>
        /// <param name="closeEvent">�ر��¼�(�ɿ�)</param>
        /// <param name="sureEvent">ȷ���¼�(�ɿ�)</param>
        /// <returns>�������UI</returns>
        /// <exception cref="MissingFieldException"></exception>
        public BaseUI OpenUI(string uiName, UIType uiType, IUIDataBase data = null, Action closeEvent = null, Action sureEvent = null)
        {
            if (string.IsNullOrEmpty(uiName)) throw new MissingFieldException("Ҫ���ص�UI����Ϊ��");
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
            if (string.IsNullOrEmpty(uiName)) throw new MissingFieldException("Ҫ���ص�UI����Ϊ��");
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
        /// ���أ����ã���رգ������ã�һ��UI
        /// </summary>
        /// <param name="uiName">UI����</param>
        /// <param name="uiType">UI����</param>
        /// <param name="isDestory">�Ƿ�ɾ��</param>
        public void CloseUI(string uiName, UIType uiType, bool isDestory = false)
        {
            if (string.IsNullOrEmpty(uiName)) return;
            if (!FindUIIsLoaded(uiName, _totalUIMap[uiType], out BaseUI baseUI))
                throw new ArgumentException("���Թر���һ�������ڻ�ײ��UI�����Ƿ���Ȼ��������ص��ע������ѯ���ߣ�UIName:" + uiName);

            baseUI.Close(isDestory);
        }
        /// <summary>
        /// ���أ����ã���رգ������ã�һ��UI
        /// </summary>
        /// <param name="baseUI">UI</param>
        /// <param name="uiType">UI����</param>
        /// <param name="isDestory">�Ƿ�ɾ��</param>
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
        /// ��һ��UI��UIMap���Ƴ�
        /// </summary>
        /// <param name="uiName"></param>
        /// <param name="uiType"></param>
        public void RemoveUIFromUIMap(string uiName, UIType uiType)
        {
            _totalUIMap[uiType].Remove(uiName);
        }
        /// <summary>
        /// �������UI, ͨ������ת������ʹ��
        /// </summary>
        /// <param name="automatic">�Ƿ����Close</param>
        public void ClaerAllUI(bool automatic = true)
        {
            if (!automatic)
            {
                //��������foreachҲû��ϵ����Ϊ�����ÿռ任ʱ����
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
            //�����ǰû�е���
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
        /// ͨ��TypeѰ�Ҵ�UIӦ�����Ǹ���������
        /// </summary>
        /// <param name="uiType">UI����</param>
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
                //����¼�
                BasePopup basePopup = baseUI.GetComponent<BasePopup>();
                //������Ŀ����Ϊ�˱�֤�򿪵�˳�� ��ֹ����A��B��B��C��C�ٴ�A�����ȹر�A����ʾ����B������
                if (_openingPopupUI.ContainsKey(baseUI.name))
                    _openingPopupUI.Remove(baseUI.name);
                _openingPopupUI.Add(baseUI.name, basePopup);
                basePopup.InitSureCallback(sureEvent);
            }
        }

    }
}