using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RandomProject
{
    [CreateAssetMenu(menuName = "Item/Data")]
    public class ItemData : ScriptableObject
    {
        public Sprite icon;
        public int price;
    }
}