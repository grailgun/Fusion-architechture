using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace RandomProject
{
    [CreateAssetMenu(menuName = "Item/Collection")]
    public class ItemCollection : ScriptableObject
    {
        public AssetReference[] itemDataReferences;
    }
}