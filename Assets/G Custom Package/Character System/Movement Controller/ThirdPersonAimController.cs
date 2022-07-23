using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ThirdPersonMoveController))]
public class ThirdPersonAimController : MonoBehaviour
{
    [SerializeField]
    private bool isAiming = false;

    [SerializeField]
    private bool lockAim = true;

    [SerializeField]
    private LayerMask aimColliderMask;
    private Camera mainCam;

    private ThirdPersonMoveController thirdPersonMoveController;

    private void Awake()
    {
        thirdPersonMoveController = GetComponent<ThirdPersonMoveController>();
    }

    private void Start()
    {
        mainCam = Camera.main;
        thirdPersonMoveController.SetRotateOnMove(lockAim);
    }

    private void Update()
    {
        Vector3 worldAimPosition = Vector2.zero;
        
        Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = mainCam.ScreenPointToRay(screenCenter);

        if (Physics.Raycast(ray, out RaycastHit hit, 999f, aimColliderMask))
        {
            worldAimPosition = hit.point;
        }

        if (lockAim && thirdPersonMoveController.IsMoving)
        {
            Vector3 worldAimTarget = worldAimPosition;
            worldAimTarget.y = transform.position.y;
            Vector3 aimDirection = (worldAimTarget - transform.position).normalized;

            transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20f);
        }
    }
}
