using GameLokal.Toolkit;
using PlayFab;
using PlayFab.ClientModels;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace RandomProject
{
    public class SetProfilePanel : Menu<SetProfilePanel>
    {
        [Title("Input Username Panel")]
        public TMP_InputField usernameInput;

        public void SubmitNewUsername()
        {
            var request = new UpdateUserTitleDisplayNameRequest
            {
                DisplayName = usernameInput.text
            };
            PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnSubmitUsernameSuccess, OnError);
        }

        private void OnSubmitUsernameSuccess(UpdateUserTitleDisplayNameResult obj)
        {
            Debug.Log("Updated display name");
            ClientInfo.Username = obj.DisplayName;

            GameEvent.Trigger("SuccessSetUsername");
            Close();
        }

        private void OnError(PlayFabError obj)
        {
            Debug.Log(obj.ErrorMessage);
        }
    }
}