using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RandomProject
{
    public abstract class Interactable : NetworkBehaviour
    {
        public InteractionType interactionType = InteractionType.Click;
        public abstract void Interact();
        public abstract string GetDescription();
    }

    public enum InteractionType
    {
        Click, Hold
    }
}