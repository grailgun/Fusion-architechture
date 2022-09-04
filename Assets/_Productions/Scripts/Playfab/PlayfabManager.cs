using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System;

public class PlayfabManager : Singleton<PlayfabManager>
{
    public void LoginPlayer(string customID)
    {
        var request = new LoginWithCustomIDRequest
        {
            CustomId = customID,
            CreateAccount = true
        };
        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnError);
    }

    private void OnLoginSuccess(LoginResult obj)
    {
        Debug.Log("Success Login");
    }

    private void OnError(PlayFabError obj)
    {
        Debug.Log(obj.ErrorMessage);
    }


}