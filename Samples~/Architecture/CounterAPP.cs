/*
 | | | | ___   ___  _ __  _   _ / ___| __ _ _ __ ___   ___ 
 | |_| |/ _ \ / _ \| '_ \| | | | |  _ / _` | '_ ` _ \ / _ \
 |  _  | (_) | (_) | |_) | |_| | |_| | (_| | | | | | |  __/
 |_| |_|\___/ \___/| .__/ \__, |\____|\__,_|_| |_| |_|\___|
                   |_|    |___/                            
┌──────────────────────────────────────────────┐
│　Copyright(C) 2025 by HoopyGameStudio
│　描   述*：使用框架的案例
│　创 建 人*：Hoopy
│　创建时间：2025-02-28 17:31:59
└──────────────────────────────────────────────┘
┌──────────────────────────────────────────────┐
│　修 改 人：
│　修改描述：
└──────────────────────────────────────────────┘
*/

using UnityEngine;
using UnityEngine.UI;
using TMPro;
using HoopyGame.Core;

namespace HoopyGame
{
    //ViewController Model

    //ViewController
    public class CounterAPP : MonoBehaviour, IController
    {
        private Button _add_Btn;
        private Button _sub_Btn;
        private TextMeshProUGUI _content_Txt;
        private CountModel _model;

        public IHGArchitecture GetHGArchitecture()
        {
            return CountArchitecture.Instance;
        }
        private void Awake()
        {
            _add_Btn = transform.FindButtonFrommChilds("Add_Btn");
            _sub_Btn = transform.FindButtonFrommChilds("Sub_Btn");
            _content_Txt = transform.FindTextMeshProUGUIFromChilds("Content_Txt");
        }
        private void Start()
        {
            _model = this.GetModel<CountModel>();
            _add_Btn.OnRegister(() =>
            {
                //逻辑
                this.SendCommand<AddCommand>();
            });
            _sub_Btn.OnRegister(() =>
            {
                this.SendCommand<SubCommand>();
            });

            _model.count.RegisterWithInitValue(args =>
            {
                UpdateView();
            }).UnRegisterWhenGameObjectDestroyed(gameObject);
        }

        public void UpdateView()
        {
            _content_Txt.text = _model.count.Value.ToString();
        }

    }

    public class CountOverallArchitecture : HGArchitecture<CountOverallArchitecture>
    {
        public override IOCContainer AppointParentIOC() => null;
        
        protected override void Init()
        {
            this.RegisterModel(new CountModel());
            //Utility
            this.RegisterUtility(new CountUtility());
        }

    }

    public class CountArchitecture : HGArchitecture<CountArchitecture>
    {
        public override IOCContainer AppointParentIOC() => CountOverallArchitecture.Instance.IOCContainer;
        
        protected override void Init()
        {
            //System
            this.RegisterSystem(new CountSystem());
        }
    }

    public class CountChangeEvent
    {

    }
    public class CountModel : AbstractModel
    {
        public BindableProperty<int> count = new BindableProperty<int>();

        protected override void OnInit()
        {
            count.SetValueWithoutEvent(this.GetUtility<CountUtility>().GetValue(nameof(count)));

            count.Register(value =>
            {
                this.GetUtility<CountUtility>().SetValue(nameof(count), count.Value);
            });
        }
    }

    public class CountSystem : AbstractSystem
    {
        protected override void OnInit()
        {
            var countModel = this.GetModel<CountModel>();

            this.RegisterEvent<CountChangeEvent>(args =>
            {
                switch (countModel.count.Value)
                {
                    case 10:
                        Debug.Log("完成成就10");
                        break;
                    case -10:
                        Debug.Log("完成成就-10");
                        break;
                }
            });
        }
    }

    public class CountUtility : IUtility
    {
        public void SetValue(string key, int value)
        {
            PlayerPrefs.SetInt(key, value);
        }
        public int GetValue(string key) => PlayerPrefs.GetInt(key);
    }

    public class AddCommand : AbstractCommand
    {
        protected override void OnExecute()
        {
            this.GetModel<CountModel>().count.Value ++;
            this.SendEvent<CountChangeEvent>();
        }

        protected override void OnUnExecute()
        {

        }
    }

    public class SubCommand : AbstractCommand
    {
        protected override void OnExecute()
        {
            this.GetModel<CountModel>().count.Value--;
            this.SendEvent<CountChangeEvent>();
        }

        protected override void OnUnExecute()
        {

        }
    }

}