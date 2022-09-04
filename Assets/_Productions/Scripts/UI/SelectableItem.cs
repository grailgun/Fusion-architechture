using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace RandomProject
{
    public class SelectableItem : MonoBehaviour
    {
        private int index;
        public int Index
        {
            get => index;
            set
            {
                index = value;
                OnValueChange?.Invoke(index);
            }
        }
        public int itemAmount;

        public UnityEvent<int> OnValueChange;

        public void SetItemAmount(int amount)
        {
            itemAmount = amount;
        }

        public void NextItem()
        {
            index++;

            if (index > itemAmount - 1)
                index = 0;

            Index = index;
        }

        public void PrevItem()
        {
            index--;

            if (index < 0)
                index = itemAmount - 1;

            Index = index;
        }
    }
}