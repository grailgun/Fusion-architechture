using Fusion;
using GameLokal.Toolkit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomCode.FusionNetwork
{
    public struct SessionListEvent
    {
        public List<SessionInfo> sessionList;
        public SessionListEvent(List<SessionInfo> list)
        {
            sessionList = list;
        }

        public static SessionListEvent e;
        public static void Trigger(List<SessionInfo> list)
        {
            e.sessionList = list;
            EventManager.TriggerEvent(e);
        }
    }
}