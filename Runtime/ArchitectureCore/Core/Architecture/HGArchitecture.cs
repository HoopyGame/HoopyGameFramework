/*
 | | | | ___   ___  _ __  _   _ / ___| __ _ _ __ ___   ___ 
 | |_| |/ _ \ / _ \| '_ \| | | | |  _ / _` | '_ ` _ \ / _ \
 |  _  | (_) | (_) | |_) | |_| | |_| | (_| | | | | | |  __/
 |_| |_|\___/ \___/| .__/ \__, |\____|\__,_|_| |_| |_|\___|
                   |_|    |___/                            
┌──────────────────────────────────────────────┐
│　Copyright(C) 2025 by HoopyGameStudio
│　描   述*：HG的总架构
│　创 建 人*：Hoopy
│　创建时间：2025-02-23 11:35:46
└──────────────────────────────────────────────┘
┌──────────────────────────────────────────────┐
│　修 改 人：
│　修改描述：
└──────────────────────────────────────────────┘
*/

using System;
using System.Linq;

namespace HoopyGame.Core
{
    public interface IHGArchitecture
    {
        //注册系统层，模型层，工具层
        void RegisterSystem<T>(T system) where T : ISystem;
        void RegisterModel<T>(T model) where T : IModel;
        void RegisterUtility<T>(T utility) where T : IUtility;
        //获取系统层，模型层，工具层
        T GetSystem<T>() where T : class, ISystem;
        T GetModel<T>() where T : class, IModel;
        T GetUtility<T>() where T : class, IUtility;
        //发送事件、命令和查询
        void SendCommand<T>(T command) where T : ICommand;
        TResult SendCommand<TResult>(ICommand<TResult> command);
        TResult SendQuery<TResult>(IQuery<TResult> query);
        void SendEvent<T>() where T : new();
        void SendEvent<T>(T e);
        //注册和注销事件
        IUnRegister RegisterEvent<T>(Action<T> onEvnet);
        void UnRegisterEvent<T>(Action<T> onEvent);

        IOCContainer AppointParentIOC();
        IOCContainer IOCContainer { get; }

        void DeInit();
    }

    public abstract class HGArchitecture<T> : IHGArchitecture where T: HGArchitecture<T>, new()
    {
        //是否已经初始化
        private bool _inited = false;
        public static Action<T> OnRegisterPatch = architecture => { };
        protected static T _hgArchitecture;

        public static IHGArchitecture Instance
        {
            get
            {
                if (_hgArchitecture == null) MakeSureArchitecture();
                return _hgArchitecture;
            }
        } 
        static void MakeSureArchitecture()
        {
            if (_hgArchitecture == null)
            {
                _hgArchitecture = new T();
                _hgArchitecture.InitIOC(_hgArchitecture.AppointParentIOC());
                _hgArchitecture.Init();

                OnRegisterPatch?.Invoke(_hgArchitecture);
                foreach (var model in _hgArchitecture._container.GetInstancesByType<IModel>().Where(m => !m.Initialized))
                {
                    model.Init();
                    model.Initialized = true;
                }
                foreach (var system in _hgArchitecture._container.GetInstancesByType<ISystem>()
                             .Where(m => !m.Initialized))
                {
                    system.Init();
                    system.Initialized = true;
                }
                _hgArchitecture._inited = true;
            }
        }

        //IOC 控制反转 将脚本生命周期交给框架管理
        private IOCContainer _container;
        public IOCContainer IOCContainer => _container;
        public abstract IOCContainer AppointParentIOC();
        protected void InitIOC(IOCContainer iOCContainer) => _container = new IOCContainer(iOCContainer);
        protected abstract void Init();
       
        public void DeInit()
        {
            OnDeInit();
            foreach (var system in _container.GetInstancesByType<ISystem>().Where(s => s.Initialized)) system.DeInit();
            foreach (var model in _container.GetInstancesByType<IModel>().Where(m => m.Initialized)) model.DeInit();
            _container.Claer();
            _container = null;
        }
        protected virtual void OnDeInit() { }

        //Register
        public void RegisterSystem<TSystem>(TSystem system) where TSystem : ISystem
        {
            system.SetHGArichitecture(this);
            _container.Register<TSystem>(system);

            if (_inited)
            {
                system.Init();
                system.Initialized = true;
            }
        }
        public void RegisterModel<TModel>(TModel model) where TModel : IModel
        {
            model.SetHGArichitecture(this);
            _container.Register<TModel>(model);

            if (_inited)
            {
                model.Init();
                model.Initialized = true;
            }
        }
        public void RegisterUtility<TUtility>(TUtility utility) where TUtility : IUtility =>
            _container.Register<TUtility>(utility);


        public TSystem GetSystem<TSystem>() where TSystem : class, ISystem => _container.Get<TSystem>();

        public TModel GetModel<TModel>() where TModel : class, IModel => _container.Get<TModel>();

        public TUtility GetUtility<TUtility>() where TUtility : class, IUtility => _container.Get<TUtility>();

        public TResult SendCommand<TResult>(ICommand<TResult> command) => ExecuteCommand(command);

        public void SendCommand<TCommand>(TCommand command) where TCommand : ICommand => ExecuteCommand(command);

        protected virtual TResult ExecuteCommand<TResult>(ICommand<TResult> command)
        {
            command.SetHGArichitecture(this);
            return command.Execute();
        }

        protected virtual void ExecuteCommand(ICommand command)
        {
            command.SetHGArichitecture(this);
            command.Execute();
        }

        public TResult SendQuery<TResult>(IQuery<TResult> query) => DoQuery<TResult>(query);

        protected virtual TResult DoQuery<TResult>(IQuery<TResult> query)
        {
            query.SetHGArichitecture(this);
            return query.Do();
        }

        private TypeEventSystem _typeEventSystem = new ();

        public void SendEvent<TEvent>() where TEvent : new() => _typeEventSystem.Send<TEvent>();

        public void SendEvent<TEvent>(TEvent e) => _typeEventSystem.Send<TEvent>(e);

        public IUnRegister RegisterEvent<TEvent>(Action<TEvent> onEvent) => _typeEventSystem.Register<TEvent>(onEvent);

        public void UnRegisterEvent<TEvent>(Action<TEvent> onEvent) => _typeEventSystem.UnRegister<TEvent>(onEvent);
    }

    public interface IOnEvent<T>
    {
        void OnEvent(T e);
    }
    public static class OnGlobalEventExtension
    {
        public static IUnRegister RegisterEvent<T>(this IOnEvent<T> self) where T : struct =>
            TypeEventSystem.Global.Register<T>(self.OnEvent);

        public static void UnRegisterEvent<T>(this IOnEvent<T> self) where T : struct =>
            TypeEventSystem.Global.UnRegister<T>(self.OnEvent);
    }
}