using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System;
using PlayFab.Json;
using TMPro;

namespace RandomProject
{
    public class CloudScriptMenu : Menu<CloudScriptMenu>
    {
        public string id = "sinatrya";

        public TMP_Text messageText;

        private void Start()
        {
            LoginPlayer();
        }

        public void LoginPlayer()
        {
            var request = new LoginWithCustomIDRequest
            {
                CustomId = id,
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
            Debug.Log("Logged In");
        }

        public void TryToExecuteCloundScript()
        {
            var request = new ExecuteCloudScriptRequest()
            {
                FunctionName = "sayJoke",
                FunctionParameter = new
                {
                    
                },
            };

            PlayFabClientAPI.ExecuteCloudScript(request, OnExecuteSuccess, OnError);
        }

        private void OnExecuteSuccess(ExecuteCloudScriptResult result)
        {
            
            foreach (LogStatement log in result.Logs)
            {
                Debug.Log(log.Message);
            }
        }

        private void OnError(PlayFabError obj)
        {
            Debug.Log(obj.ErrorMessage);
        }
    }
}