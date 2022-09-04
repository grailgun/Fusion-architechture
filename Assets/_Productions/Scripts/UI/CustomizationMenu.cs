using GameLokal.Toolkit;
using PlayFab;
using PlayFab.ClientModels;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RandomProject
{
    public class CustomizationMenu : MonoBehaviour, IEventListener<GameEvent>
    {
        [Title("Character")]
        public SkinnedMeshRenderer meshRenderer;

        [Title("Selection")]
        public Mesh[] bodyShape;
        public Material[] eyes;

        [Title("Selection UI")]
        public SelectableItem bodyShapeChanger;
        public SelectableItem eyeChanger;

        private void Start()
        {
            bodyShapeChanger.SetItemAmount(bodyShape.Length);
            eyeChanger.SetItemAmount(eyes.Length);
        }

        private void OnEnable()
        {
            bodyShapeChanger.OnValueChange.AddListener(SetBodyShape);
            eyeChanger.OnValueChange.AddListener(SetEyeShape);

            EventManager.AddListener(this);
        }

        private void OnDisable()
        {
            bodyShapeChanger.OnValueChange.RemoveListener(SetBodyShape);
            eyeChanger.OnValueChange.RemoveListener(SetEyeShape);

            EventManager.RemoveListener(this);
        }

        private void SaveCharacterData()
        {
            var request = new UpdateUserDataRequest()
            {
                Data = new Dictionary<string, string>()
                {
                    { "CharacterShape", $"{bodyShapeChanger.Index}" },
                    { "CharacterEye", $"{eyeChanger.Index}" }
                }
            };
            PlayFabClientAPI.UpdateUserData(request, OnUpdateSend, OnError);
        }

        private void OnUpdateSend(UpdateUserDataResult obj)
        {
            Debug.Log("Saved");
        }

        private void GetCharacterData()
        {
            PlayFabClientAPI.GetUserData(new GetUserDataRequest(), OnCharacterDataReceived, OnError);
        }

        private void OnCharacterDataReceived(GetUserDataResult obj)
        {
            if (obj.Data == null) return;

            Dictionary<string, UserDataRecord> data = obj.Data;
            
            if (data.TryGetValue("CharacterShape", out var value))
            {
                bodyShapeChanger.Index = int.Parse(value.Value);
            }

            if (data.TryGetValue("CharacterEye", out var eyeValue))
            {
                eyeChanger.Index = int.Parse(eyeValue.Value);
            }
        }

        private void OnError(PlayFabError obj)
        {
            Debug.Log(obj.ErrorMessage);
        }

        private void SetEyeShape(int arg0)
        {
            Material[] materials = meshRenderer.materials;
            materials[1] = eyes[arg0];

            meshRenderer.materials = materials;
            SaveCharacterData();
        }

        private void SetBodyShape(int arg0)
        {
            meshRenderer.sharedMesh = bodyShape[arg0];
            SaveCharacterData();
        }

        public void OnEvent(GameEvent e)
        {
            if(e.EventName == "SuccessLogin" || e.EventName == "SuccessSetUsername")
            {
                GetCharacterData();
            }
        }
    }
}