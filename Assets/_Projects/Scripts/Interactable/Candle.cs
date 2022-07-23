using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RandomProject
{
    public class Candle : Interactable
    {
        [Networked(OnChanged = nameof(OnCandleToggle))]
        public bool IsOn { get; set; }

        public string whenOnDescription;
        public string whenOffDescription;
        private string interactableDescription;

        public GameObject[] toggleObjects;

        public override void Spawned()
        {
            ToggleCandle();
        }

        private static void OnCandleToggle(Changed<Candle> changed) => changed.Behaviour.ToggleCandle();
        private void ToggleCandle()
        {
            interactableDescription = IsOn ? whenOffDescription : whenOnDescription;

            foreach (var go in toggleObjects)
            {
                go.SetActive(IsOn);
            }
        }

        public override string GetDescription()
        {
            return interactableDescription;
        }

        public override void Interact()
        {
            IsOn = !IsOn;
        }
    }
}