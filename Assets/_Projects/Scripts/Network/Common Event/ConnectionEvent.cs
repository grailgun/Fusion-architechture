using GameLokal.Toolkit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RandomProject
{
    public enum ConnectionType
    {
        Failed, Session, Lobby
    }

    public struct ConnectionEvent
    {
        public bool isSuccess;
        public ConnectionType status;

        public ConnectionEvent(bool isSuccess, ConnectionType connectionStatus)
        {
            this.isSuccess = isSuccess;
            this.status = connectionStatus;
        }

        public static ConnectionEvent e;
        public static void TriggerEvent(bool isSuccess, ConnectionType connectionStatus)
        {
            e.isSuccess = isSuccess;
            e.status = connectionStatus;
            EventManager.TriggerEvent(e);
        }
    }
}