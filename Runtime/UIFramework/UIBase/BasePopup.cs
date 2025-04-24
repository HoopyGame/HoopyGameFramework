/*
 | | | | ___   ___  _ __  _   _ / ___| __ _ _ __ ___   ___ 
 | |_| |/ _ \ / _ \| '_ \| | | | |  _ / _` | '_ ` _ \ / _ \
 |  _  | (_) | (_) | |_) | |_| | |_| | (_| | | | | | |  __/
 |_| |_|\___/ \___/| .__/ \__, |\____|\__,_|_| |_| |_|\___|
                   |_|    |___/                            
������������������������������������������������������������������������������������������������
����Copyright(C) 2025 by HoopyGameStudio
������   ��*��������������
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
using UnityEngine.UI;

namespace HoopyGame.UIF
{
    public abstract class BasePopup : BaseUI
    {
        protected Button sureBtn;
        private Action _sureBtnEvent;

        public override void OnInitComponent()
        {
            base.OnInitComponent();
            uiType = UIType.Popup;
            sureBtn = transform.FindComponentFromChild<Button>("Sure_Btn");
        }

        public override void OnAddListener()
        {
            base.OnAddListener();
            sureBtn?.onClick.AddListener(OnSureBtnClick);
        }
        public override void OnRemoveListener()
        {
            base.OnRemoveListener();
            sureBtn?.onClick.RemoveListener(OnSureBtnClick);
            _sureBtnEvent = null;
        }

        public override void OnStart()
        {
            LSMgr.Instance.GetFromeGLS<UIMgr>().ShieldControl(true);
            base.OnStart();
        }
        public override void Close(bool isDestory)
        {
            LSMgr.Instance.GetFromeGLS<UIMgr>().ClosePopupToCheckShield(name);
            base.Close(isDestory);
        }
        /// <summary>
        /// ���ȷ�ϰ�ť
        /// </summary>
        public virtual void OnSureBtnClick()
        {
            _sureBtnEvent?.Invoke();
            _sureBtnEvent = null;
        }

        public void InitSureCallback(Action act)
        {
            if (act != null)
            {
                _sureBtnEvent = act;
            }

        }
    }
}