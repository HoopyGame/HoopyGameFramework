/*
 | | | | ___   ___  _ __  _   _ / ___| __ _ _ __ ___   ___ 
 | |_| |/ _ \ / _ \| '_ \| | | | |  _ / _` | '_ ` _ \ / _ \
 |  _  | (_) | (_) | |_) | |_| | |_| | (_| | | | | | |  __/
 |_| |_|\___/ \___/| .__/ \__, |\____|\__,_|_| |_| |_|\___|
                   |_|    |___/                            
┌──────────────────────────────────────────────┐
│　Copyright(C) 2025 by HoopyGameStudio
│　描   述*：IOC层，符合依赖倒置原则
│　创 建 人*：Hoopy
│　创建时间：2025-02-27 09:17:07
└──────────────────────────────────────────────┘
┌──────────────────────────────────────────────┐
│　修 改 人：
│　修改描述：
└──────────────────────────────────────────────┘
*/
using System;
using System.Collections.Generic;
using System.Linq;

namespace HoopyGame.Core
{
    /// <summary>
    /// IOC层，管理所有单例
    /// </summary>
	public class IOCContainer
    {
        private readonly Dictionary<Type, object> _instanceContaner = new();
        //用于做数据隔离
        private IOCContainer _parentIOC;

        public IOCContainer(IOCContainer parentIOC = null)
        {
            _parentIOC = parentIOC;
        }

        public void Register<T>(T instanceType)
        {
            Type type = typeof(T);
            if (_instanceContaner.ContainsKey(type))
                _instanceContaner[type] = instanceType;
            else 
                _instanceContaner.Add(type, instanceType);
        }

        public T Get<T>() where T : class
        {
            if(_instanceContaner.TryGetValue(typeof(T), out var instance))
            {
                return instance as T;
            }
            else if (_parentIOC != null && _parentIOC._instanceContaner.TryGetValue(typeof(T), out var topInstance))
            {
                return topInstance as T;
            }
            return null;
        }

        public IEnumerable<T> GetInstancesByType<T>()
        {
            Type type = typeof(T);
            return _instanceContaner.Values.Where(instance => type.IsInstanceOfType(instance)).Cast<T>();
        }

        public void Claer()
        {
            _instanceContaner.Clear();
            _parentIOC = null;
        }

	}
}