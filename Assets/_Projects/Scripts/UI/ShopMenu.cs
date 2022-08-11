using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace RandomProject
{
    public class ShopMenu : Menu<ShopMenu>
    {
        [Title("Shop Item")]
        public ShopItemUI shopItemUIPrefab;
        [SerializeField]
        private Transform shopItemParent;

        [Title("Shop Addressable")]
        [SerializeField]
        private bool deleteBundleCache = false;
        public AssetReference shopItemCollectionAssetRef;
        public ItemCollection cachedItemCollection;

        private AsyncOperationHandle<long> downloadSizeBundle;
        private AsyncOperationHandle shopCollectionHandle;
        private AsyncOperationHandle itemShowHandle;

        private void OnEnable()
        {
            if(deleteBundleCache)
                Addressables.ClearDependencyCacheAsync(shopItemCollectionAssetRef);

            downloadSizeBundle = Addressables.GetDownloadSizeAsync(shopItemCollectionAssetRef);
            downloadSizeBundle.Completed += OnCheckDownloadCollectionCompleted;
        }

        private void OnCheckDownloadCollectionCompleted(AsyncOperationHandle<long> obj)
        {
            Debug.Log($"Shop has to download Bundle Update : {obj.Result}");

            if (obj.Status == AsyncOperationStatus.Succeeded && obj.Result > 0)
            {
                shopCollectionHandle = Addressables.LoadAssetAsync<ItemCollection>(shopItemCollectionAssetRef);
                shopCollectionHandle.Completed += OnDownloadCollectionCompleted;
            }
            else
            {
                ShowItemShop();
            }

            downloadSizeBundle.Completed -= OnCheckDownloadCollectionCompleted;
        }

        private void OnDownloadCollectionCompleted(AsyncOperationHandle obj)
        {
            var remoteCollection = obj.Result as ItemCollection;
            cachedItemCollection.itemDataReferences = remoteCollection.itemDataReferences;
            ShowItemShop();

            shopCollectionHandle.Completed -= OnDownloadCollectionCompleted;
        }

        private void ShowItemShop()
        {
            itemShowHandle = Addressables.LoadAssetsAsync<ItemData>(cachedItemCollection.itemDataReferences, (item) =>
            {
                CreateShopItem(item);
            }, Addressables.MergeMode.Union);
        }

        private void CreateShopItem(ItemData item)
        {
            var shopItem = Instantiate(shopItemUIPrefab, shopItemParent);
            shopItem.SetShopItem(item);
        }
    }
}