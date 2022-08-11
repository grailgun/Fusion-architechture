using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace RandomProject
{
    public class PackageLoader : MonoBehaviour
    {
        [Title("UI")]
        public TextMeshProUGUI downloadText;
        public GameObject downloadPrompPanel;

        [Title("Asset Addressable References")]
        public bool deleteBundleCache = false;
        public AssetLabelReference labelReference;

        private void OnEnable()
        {
            if (deleteBundleCache)
                Addressables.ClearDependencyCacheAsync(labelReference);

            StartCoroutine(CheckDownloadSize());
        }

        private IEnumerator CheckDownloadSize()
        {
            AsyncOperationHandle<long> downloadSize = Addressables.GetDownloadSizeAsync(labelReference);
            yield return downloadSize;

            if (downloadSize.Status == AsyncOperationStatus.Failed)
            {
                Debug.LogError("Failed to check download size");
            }

            if (downloadSize.Result > 0)
            {
                downloadPrompPanel.SetActive(true);
                downloadText.text = $"You have to download file {downloadSize.Result / 1000f} KB";
            }
        }

        public void StartDownloadDependencies()
        {
            downloadPrompPanel.SetActive(false);
            Debug.Log("Start Download");
            StartCoroutine(DownloadDependenciesProgress());
        }

        IEnumerator DownloadDependenciesProgress()
        {
            AsyncOperationHandle downloadShopdependencies = Addressables.DownloadDependenciesAsync(labelReference);
            yield return downloadShopdependencies;
        }
    }
}