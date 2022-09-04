using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomCode.UI.TabSystem
{
    public class TabContainer : MonoBehaviour
    {
        public void Active()
        {
            gameObject.SetActive(true);
        }

        public void Inactive()
        {
            gameObject.SetActive(false);
        }
    }
}