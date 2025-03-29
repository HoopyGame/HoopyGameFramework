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
using HoopyGame.Core;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace HoopyGame.UIF
{
    public abstract class BaseUI : MonoBehaviour, IController
    {
        private IUIDataBase _data;
        protected IUIDataBase Data { get { return _data; } }//����

        protected Button closeBtn;                          //������ҳ�涼�з��ع���
        [HideInInspector]
        public UIType uiType;                               //UI���� �µ�����Ҫ����������

        private Action _closeBtnEvent;                      //�رմ������¼�

        public virtual IHGArchitecture GetHGArchitecture() => UIArchitecture.Instance;

        /// <summary>
        /// �Ƿ��Ѿ���ʼ��
        /// </summary>
        public bool IsInit { get; private set; }
        /// <summary>
        /// Openһ��UI(����дOpenEff�����Զ��嵯��Ч��)
        /// </summary>
        /// <param name="data">����</param>
        public virtual void Open(IUIDataBase data = null)
        {
            //������Ϊ�˱�����˴�ʱ�����data��ԭ����data���ǵ�
            if (data != null) _data = data;
            if (!IsInit) Init();//û�г�ʼ�����Ż��ʼ��
            OnStart();//Start

            gameObject.SetActive(true);
            transform.SetAsLastSibling();
        }
        /// <summary>
        /// ��ʼ��
        /// </summary>
        private void Init()
        {
            closeBtn = transform.FindButtonFrommChilds("Close_Btn");
            //Init�Ժ��ʼ�����
            OnInitComponent();
            //����¼�
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