using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RandomProject
{
    [CreateAssetMenu(menuName = "Player Data")]
    public class PlayerData : ScriptableObject
    {
        public List<Mission> UnlockedMission = new List<Mission>();
    }
}