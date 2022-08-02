using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RandomProject
{
    public class SelectableItem : MonoBehaviour
    {
        [Title("Items")]
        public Sprite[] items;
        private int index;

        [Title("Component")]
        public Image previewImage;

        private void Start()
        {
            index = 0;
            ShowItemAtIndex(index);
        }

        public void SetItem(Sprite[] items)
        {
            this.items = items;
        }

        public void NextItem()
        {
            index++;

            if (index > items.Length - 1)
                index = 0;

            ShowItemAtIndex(index);
        }

        public void PrevItem()
        {
            index--;

            if (index < 0)
                index = items.Length - 1;

            ShowItemAtIndex(index);
        }

        private void ShowItemAtIndex(int index)
        {
            previewImage.sprite = items[index];
        }
    }
}