using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RandomProject
{
    public class FPSCamera : MonoBehaviour
    {
        public CinemachineVirtualCamera cinemachineVirtualCamera;

        public void SetTransform(Transform follow)
        {
            cinemachineVirtualCamera.Follow = follow;
        }
    }
}