/*
 | | | | ___   ___  _ __  _   _ / ___| __ _ _ __ ___   ___ 
 | |_| |/ _ \ / _ \| '_ \| | | | |  _ / _` | '_ ` _ \ / _ \
 |  _  | (_) | (_) | |_) | |_| | |_| | (_| | | | | | |  __/
 |_| |_|\___/ \___/| .__/ \__, |\____|\__,_|_| |_| |_|\___|
                   |_|    |___/                            
������������������������������������������������������������������������������������������������
����Copyright(C) 2025 by HoopyGameStudio
������   ��*������ע�룬ʹ��VContainer������IOC
������ �� ��*��Hoopy
��������ʱ�䣺2025-01-01 00:00:00
������������������������������������������������������������������������������������������������
������������������������������������������������������������������������������������������������
������ �� �ˣ�
�����޸�������
������������������������������������������������������������������������������������������������
*/
using VContainer;
using VContainer.Unity;

namespace HoopyGame
{
    public class GameLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {

            //--����ҪMono�ĵ���
            //�¼�ϵͳ
            builder.Register<EventMgr>(Lifetime.Singleton);
            //�����ϵͳ
            builder.Register<ObjectPoolMgr>(Lifetime.Singleton);
            //��Դ����ϵͳ
            builder.Register<AssetMgr>(Lifetime.Singleton);

            //--��ҪMono�ĵ���
            builder.Register<AudioMgr>(Lifetime.Singleton);
        }
    }
}