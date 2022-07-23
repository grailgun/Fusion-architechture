using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RandomProject
{
    public static class ClientInfo
    {
        public static string Username
        {
            get => PlayerPrefs.GetString("C_Username", "");
            set => PlayerPrefs.SetString("C_Username", value);
        }

        public static string LobbyName
        {
            get => PlayerPrefs.GetString("C_LastLobbyName", "");
            set => PlayerPrefs.SetString("C_LastLobbyName", value);
        }
    }
}
