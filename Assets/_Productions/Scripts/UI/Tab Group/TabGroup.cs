using System;
using System.Collections.Generic;
using UnityEngine;

namespace CustomCode.UI.TabSystem
{
    public class TabGroup : MonoBehaviour
    {
        public bool isKeepLastOpened = false;
        public List<TabConfig> tabs = new List<TabConfig>();

        private void Start()
        {
            SetupTabs();
            OpenTab(0);
        }

        private void OnEnable() 
        {
            if(!isKeepLastOpened) OpenTab(0);
        }

        private void SetupTabs()
        {
            for (int i = 0; i < tabs.Count; i++)
            {
                var idx = i;
                tabs[i].button.AddListener(() => OpenTab(idx));
            }
        }

        private void OpenTab(int index)
        {
            foreach (var tab in tabs)
            {
                tab.button.Inactive();
                tab.container.Inactive();
            }
            
            tabs[index].container.Active();
            tabs[index].button.Active();
        }
    }
}