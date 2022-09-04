using Sirenix.OdinInspector;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace CustomCode.UI.TabSystem
{
    [RequireComponent(typeof(Button))]
    public class TabButton : MonoBehaviour
    {
        [Title("Image Settings")]
        public Color activeColor;
        public Color inactiveColor;
        public Image image;

        public bool isUsedTextColor = false;
        [ShowIf("isUsedTextColor"), Title("Text Settings")]
        public Color activeTextColor;
        [ShowIf("isUsedTextColor")]
        public Color inactiveTextColor;
        [ShowIf("isUsedTextColor")]
        public TextMeshProUGUI text;

        public Button Button => button;
        private Button button;

        public void AddListener(Action onClickCallback)
        {
            button.onClick.AddListener(() => onClickCallback.Invoke());
        }

        private void OnDestroy()
        {
            button.onClick.RemoveAllListeners();
        }

        private void Awake()
        {
            button = GetComponent<Button>();

            if (image == null)
                image = GetComponent<Image>();
        }
        
        public void Active()
        {
            if (image != null)
            {
                image.color = activeColor;
            }

            if(text != null)
            {
                text.color = activeTextColor;
            }
        }

        public void Inactive()
        {
            if (image != null)
            {
                image.color = inactiveColor;
            }

            if (text != null)
            {
                text.color = inactiveTextColor;
            }
        }
    }
}