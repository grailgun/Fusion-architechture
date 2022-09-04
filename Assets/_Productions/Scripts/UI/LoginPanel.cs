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
    public class LoginPanel : Menu<LoginPanel>
    {
        [Title("Login Panel")]
        public TMP_InputField idNameField;

        public void LoginPlayer()
        {
            var request = new LoginWithCustomIDRequest
            {
                CustomId = idNameField.text,
                CreateAccount = true,
                InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
                {
                    GetPlayerProfile = true
                }
            };
            PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnError);
        }

        private void OnLoginSuccess(LoginResult obj)
        {
            Debug.Log("Success Login");

            //Get username
            string displayName = null;
            if (obj.InfoResultPayload.PlayerProfile != null)
            {
                displayName = obj.InfoResultPayload.PlayerProfile.DisplayName;
                ClientInfo.Username = displayName;

                bool isEmpty = string.IsNullOrEmpty(displayName);
                if (isEmpty)
                {
                    Close();
                    SetProfilePanel.Open();
                }
                else
                {
                    Close();
                }
            } 
            else
            {
                Close();
                SetProfilePanel.Open();
            }

            GameEvent.Trigger("SuccessLogin");
        }

        private void OnError(PlayFabError obj)
        {
            Debug.Log(obj.ErrorMessage);
        }
    }
}