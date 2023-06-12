using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainSceneUserInputHandler : MonoBehaviour
{
    public Button submit;
    public InputField userNameIF;
    public InputField roomNameIF;

    
    public void HandleIF()
    {
        if(string.IsNullOrEmpty(userNameIF.text) || string.IsNullOrEmpty(roomNameIF.text))
        {
            submit.interactable = false;
        }
        else
        {
            
            submit.interactable = true;

        }
    }

    public void Submit()
    {
        PlayerPrefs.SetString("userName", userNameIF.text);
        PlayerPrefs.SetString("roomName", roomNameIF.text);
        SceneManager.LoadScene(1);
    } 
}
