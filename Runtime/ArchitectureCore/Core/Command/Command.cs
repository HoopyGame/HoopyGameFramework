/*
 | | | | ___   ___  _ __  _   _ / ___| __ _ _ __ ___   ___ 
 | |_| |/ _ \ / _ \| '_ \| | | | |  _ / _` | '_ ` _ \ / _ \
 |  _  | (_) | (_) | |_) | |_| | |_| | (_| | | | | | |  __/
 |_| |_|\___/ \___/| .__/ \__, |\____|\__,_|_| |_| |_|\___|
                   |_|    |___/                            
┌──────────────────────────────────────────────┐
│　Copyright(C) 2025 by HoopyGameStudio
│　描   述*：命令层(表现逻辑层) 分离控制层的代码量
│　创 建 人*：Hoopy
│　创建时间：2025-02-25 09:27:02
└──────────────────────────────────────────────┘
┌──────────────────────────────────────────────┐
│　修 改 人：
│　修改描述：
└──────────────────────────────────────────────┘
*/

namespace HoopyGame.Core
{
    public interface ICommand:IBelongToHGArchitecture,ICanSetHGArchitecture,ICanGetSystem,ICanGetModel,ICanGetUtility,
        ICanSendCommand,ICanSendEvent,ICanSendQuery
	{
		void Execute();

        void UnExecute();
	}
    public interface ICommand<TResult>: IBelongToHGArchitecture, ICanSetHGArchitecture, ICanGetSystem, ICanGetModel, ICanGetUtility,
        ICanSendCommand, ICanSendEvent, ICanSendQuery
    {
        TResult Execute();
    }

    public abstract class AbstractCommand : ICommand
    {
        private IHGArchitecture _hgArchitecture;
        public IHGArchitecture GetHGArchitecture() => _hgArchitecture;
        public void SetHGArichitecture(IHGArchitecture hgArchitecture) => _hgArchitecture = hgArchitecture;

        void ICommand.Execute() => OnExecute();
        protected abstract void OnExecute();
        void ICommand.UnExecute() => OnUnExecute();
        protected abstract void OnUnExecute();
    }
    public abstract class AbstractCommand<TResult> : ICommand<TResult>
    {
        private IHGArchitecture _hgArchitecture;
        public IHGArchitecture GetHGArchitecture() => _hgArchitecture;
        public void SetHGArichitecture(IHGArchitecture hgArchitecture) => _hgArchitecture= hgArchitecture;

        TResult ICommand<TResult>.Execute() => OnExecute();
        protected abstract TResult OnExecute();

    }
}