using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RandomProject
{
    public abstract class RunnerCallback : MonoBehaviour
    {
        protected Launcher launcher;

        public void Init(Launcher launcher)
        {
            this.launcher = launcher;
        }
    }
}