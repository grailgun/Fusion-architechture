using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RandomProject
{
    public class PlayerInteraction : MonoBehaviour
    {
        public float interactionDistance;
        public LayerMask interactionMask;

        public InputActionAsset inputAsset;
        public TMP_Text interactionText;

        [SerializeField]
        private Interactable activeInteractable;

        private Camera cam;

        private void Start()
        {
            cam = Camera.main;
        }

        private void FixedUpdate()
        {
            Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, interactionDistance, interactionMask))
            {
                if (hit.collider == null) return;

                Interactable interactable = hit.collider.GetComponentInParent<Interactable>();
                if (interactable)
                {
                    activeInteractable = interactable;
                    interactionText.text = "Press E to " + interactable.GetDescription();
                }
            }
            else
            {
                activeInteractable = null;
                interactionText.text = "";
            }
        }

        public void Interact()
        {
            if (activeInteractable == null) return;

            switch (activeInteractable.interactionType)
            {
                case InteractionType.Click:
                    activeInteractable.Interact();
                    break;

                case InteractionType.Hold:
                    activeInteractable.Interact();
                    break;
            }
        }
    }
}