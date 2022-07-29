using Fusion;
using GameLokal.Toolkit;

namespace RandomProject
{
    public struct ConnectionEvent
    {
        public StartGameResult Result;

        public ConnectionEvent(StartGameResult Result)
        {
            this.Result = Result;
        }

        public static ConnectionEvent e;
        public static void TriggerEvent(StartGameResult Result)
        {
            e.Result = Result;
            EventManager.TriggerEvent(e);
        }
    }
}