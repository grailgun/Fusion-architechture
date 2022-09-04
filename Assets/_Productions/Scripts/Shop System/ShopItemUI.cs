using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RandomProject
{
    public class ShopItemUI : MonoBehaviour
    {
        public Image itemIcon;
        public TextMeshProUGUI itemPrice;

        public void SetShopItem(ItemData itemData)
        {
            itemIcon.sprite = itemData.icon;
            itemPrice.text = itemData.price.ToString();
        }
    }
}