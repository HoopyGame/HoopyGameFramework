/*
 | | | | ___   ___  _ __  _   _ / ___| __ _ _ __ ___   ___ 
 | |_| |/ _ \ / _ \| '_ \| | | | |  _ / _` | '_ ` _ \ / _ \
 |  _  | (_) | (_) | |_) | |_| | |_| | (_| | | | | | |  __/
 |_| |_|\___/ \___/| .__/ \__, |\____|\__,_|_| |_| |_|\___|
                   |_|    |___/                            
┌──────────────────────────────────────────────┐
│　Copyright(C) 2025 by HoopyGameStudio
│　描   述*：命令模式，将所有操作作为命令存储（撤回，反撤回操作）
│　创 建 人*：Hoopy
│　创建时间：2025-01-01 00:00:00
└──────────────────────────────────────────────┘
┌──────────────────────────────────────────────┐
│　修 改 人：
│　修改描述：
└──────────────────────────────────────────────┘
*/
using System.Collections.Generic;

public class CommandInvoke : SingleBase<CommandInvoke>
{
    private Stack<ICommand> _executedCommandStack;//已经执行过的命令，可以撤回
    private Stack<ICommand> _withdrawCommandStack;//撤回的命令，可以撤销撤回

    protected override void Init()
    {
        base.Init();
        _executedCommandStack = new Stack<ICommand>();
        _withdrawCommandStack = new Stack<ICommand>();
    }
    /// <summary>
    /// 执行一个命令
    /// </summary>
    /// <param name="command">一个命令</param>
    public void ExcuteCommand(ICommand command)
    {
        _executedCommandStack.Push(command);
        command.Execute();
    }
    /// <summary>
    /// 撤回一条命令
    /// </summary>
    public void WithdrawCommand()
    {
        if( _executedCommandStack.Count > 0)
        {
            ICommand command = _executedCommandStack.Pop();
            command.Undo();
            _withdrawCommandStack.Push(command);
        }
        else
        {
            DebugUtils.Print("当前没有可撤回的命令了", DebugType.Error);
        }
    }
    /// <summary>
    /// 撤销撤回命令
    /// </summary>
    public void RevokeWithdrawCommand()
    {
        if (_withdrawCommandStack.Count > 0)
        {
            ICommand command = _withdrawCommandStack.Pop();
            command.Execute();
            _withdrawCommandStack.Push(command);
        }
        else 
        {
            DebugUtils.Print("当前没有可撤销撤回的命令了", DebugType.Error);
        }
    }

    /// <summary>
    /// 获取可以撤回的命令的数量
    /// </summary>
    /// <returns></returns>
    public int GetCanWithdrawCommandCount()
    {
        return _executedCommandStack.Count;
    }
    /// <summary>
    /// 可以撤销撤回的命令的数量
    /// </summary>
    /// <returns></returns>
    public int GetCanRevokeWithdrawCommandCount()
    {
        return _withdrawCommandStack.Count;
    }

    /// <summary>
    /// 删除所有触发过的命令
    /// </summary>
    public void ClearCommand()
    {
        _executedCommandStack.Clear();
        _withdrawCommandStack.Clear();
    }

}
