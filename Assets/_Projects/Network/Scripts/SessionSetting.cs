using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RandomProject
{
    public class SessionSetting
    {
        public GameMode gameMode = GameMode.Client;
        public string sessionName = "Auto Name";
        public string lobbyID = "Default";
        public int playerLimit = 5;
        public bool isVisible = true;
    }
}