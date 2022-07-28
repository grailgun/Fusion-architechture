using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RandomProject
{
    public enum MissionType
    {
        Escort,
        Defense,
        Destroy
    }

    public enum MissionRegion
    {
        Jakarta,
        Tokyo,
        Citayem,
        Depok
    }

    public enum MissionDifficulty
    {
        Ez,
        Medium,
        Hard
    }

    [CreateAssetMenu(menuName = "Mission")]
    public class Mission : ScriptableObject
    {
        public string missionName;
        public MissionRegion missionRegion;
        public MissionType missionType;
    }
}