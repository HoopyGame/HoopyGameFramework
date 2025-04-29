/*
 | | | | ___   ___  _ __  _   _ / ___| __ _ _ __ ___   ___ 
 | |_| |/ _ \ / _ \| '_ \| | | | |  _ / _` | '_ ` _ \ / _ \
 |  _  | (_) | (_) | |_) | |_| | |_| | (_| | | | | | |  __/
 |_| |_|\___/ \___/| .__/ \__, |\____|\__,_|_| |_| |_|\___|
                   |_|    |___/                            
������������������������������������������������������������������������������������������������
����Copyright(C) 2025 by HoopyGameStudio
������   ��*������UI���ĳ������
������ �� ��*��Hoopy
��������ʱ�䣺2025-01-01 00:00:00
������������������������������������������������������������������������������������������������
������������������������������������������������������������������������������������������������
������ �� �ˣ�
�����޸�������
������������������������������������������������������������������������������������������������
*/
using HoopyGame.Manager;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace HoopyGame.UIF
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class BaseUI : MonoBehaviour
    {
        private IUIDataBase _data;
        protected IUIDataBase Data { get { return _data; } }//����

        protected Button closeBtn;                          //������ҳ�涼�з��ع���
        [HideInInspector]
        public UIType uiType;                               //UI���� �µ�����Ҫ����������

        private Action _closeBtnEvent;                      //�رմ������¼�
        private CanvasGroup _cg;                            //���ڹ������UI����ʾ����

        /// <summary>
        /// �Ƿ��Ѿ���ʼ��
        /// </summary>
        public bool IsInit { get; private set; }
        /// <summary>
        /// Openһ��UI(�����Ч������д�˷���)
        /// </summary>
        /// <param name="data">����</param>
        public virtual void Open(IUIDataBase data = null)
        {
            //������Ϊ�˱�����˴�ʱ�����data��ԭ����data���ǵ�
            if (data != null) _data = data;
            if (!IsInit) Init();//û�г�ʼ�����Ż��ʼ��
            
            transform.SetAsLastSibling();
            _cg.alpha = 1;
            _cg.blocksRaycasts = true;
            OnStart();//Start
        }
        /// <summary>
        /// ��ʼ��
        /// </summary>
        private void Init()
        {
            _cg = GetComponent<CanvasGroup>();
            closeBtn = transform.FindButtonFrommChilds("Close_Btn");
            //Init�Ժ��ʼ�����
            OnInitComponent();
            //����¼�
            OnAddListener();
            IsInit = true;
        }
        /// <summary>
        /// Closeһ��UI(����ر�Ч������д�˷�����Ч��������base.Close����)
        /// </summary>
        public virtual void Close(bool isDestroy = false)
        {
            if (isDestroy)
            {
                OnRemoveListener();
                OnDestory();
                //DestroyImmediate(baseUI.gameObject); --��
                DestroyImmediate(gameObject);
            }
            else
            {
                OnClose();
                _cg.alpha = 0;
                _cg.blocksRaycasts = false;
            }

            LSMgr.Instance.GetFromeGLS<UIMgr>().CloseUIJustAutoUsed(this, uiType, isDestroy);
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
        /// ��ʼ�����(����Open)
        /// </summary>
        public virtual void OnInitComponent() { }
        /// <summary>
        /// ����¼�(����Open)
        /// </summary>
        public virtual void OnAddListener() { closeBtn?.OnRegister(OnCloseBtnClick); }
        /// <summary>
        /// ÿ�δ�ҳ��(����Open����ҳ���)
        /// </summary>
        public virtual void OnStart() { }
        /// <summary>
        /// �ر�ҳ��(����Close_BtnEvent(False))
        /// </summary>
        public virtual void OnClose() { }
        /// <summary>
        /// ɾ��ҳ��(����Close_BtnEvent(True))
        /// </summary>
        public virtual void OnDestory() { }
        /// <summary>
        /// �Ƴ��¼�(����Close_BtnEvent(True))
        /// </summary>
        public virtual void OnRemoveListener() { closeBtn?.OnClear(); _closeBtnEvent = null; }
        /// <summary>
        /// ���Close���¼�(����ҪBase)
        /// </summary>
        public virtual void OnCloseBtnClick() { _closeBtnEvent?.Invoke(); Close(); }
        #endregion
    }
}