using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RandomProject
{
    [CreateAssetMenu(menuName = "Mission Database")]
    public class MissionList : ScriptableObject
    {
        [SerializeField] private List<Mission> missions = new List<Mission>();

        public List<string> GetMissionNameList()
        {
            return missions.Select(mission => mission.missionName).ToList();
        }

        public Mission GetMissionByName(string name)
        {
            return missions.FirstOrDefault(mission => mission.missionName == name);
        }

        public List<Mission> GetAllMission()
        {
            return missions;
        }

        public List<Mission> GetMissionListByRegion(MissionRegion region)
        {
            return missions.Where(mission => mission.missionRegion == region).ToList();
        }

        public List<string> GetMissionNameListByRegion(MissionRegion region)
        {
            var missionByRegions = GetMissionListByRegion(region);
            return missionByRegions.Select(mission => mission.missionName).ToList();
        }

        public int GetMissionIndexByRegion(MissionRegion region, string missionName)
        {
            var missionNameList = GetMissionNameListByRegion(region);
            return missionNameList.IndexOf(missionName);
        }
    }
}