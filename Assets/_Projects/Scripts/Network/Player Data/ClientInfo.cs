using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RandomProject
{
    public static class ClientInfo
    {
        private static string username;
        private static int level;

        public static string Username
        {
            get => username;
            set => username = value;
        }

        public static int Level
        {
            get => level;
            set => level = value;
        }
    }
}
