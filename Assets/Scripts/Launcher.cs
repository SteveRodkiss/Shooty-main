using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;

public class Launcher : MonoBehaviourPunCallbacks
{

    public GameObject buttonPanelGameObject;
    private string nickName = "Frank";

    //the level to load
    [Tooltip("The name of the level that you want to play")]
    public string LevelToLoad = "SampleScene";


    // Start is called before the first frame update
    void Start()
    {
        //try to connect
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.AutomaticallySyncScene = true;
        buttonPanelGameObject.SetActive(false);
    }

    public void OnPlayButtonClicked()
    {
        PhotonNetwork.NickName = nickName;
        //called by the button click event
        PhotonNetwork.JoinRandomOrCreateRoom();
    }

    public void OnQuitButtonClicked()
    {
        Application.Quit();
    }


    public void NicknameValueChanged(string newNickName)
    {
        nickName = newNickName;
    }


#region PUN Callbacks

    public override void OnConnectedToMaster()
    {
        //we connected
        Debug.Log("Connected to Master");
        buttonPanelGameObject.SetActive(true);
        //PhotonNetwork.JoinRandomOrCreateRoom();
    }

    public override void OnJoinedRoom()
    {
        
        Debug.Log($"Joined a room successfully! Room Name: {PhotonNetwork.CurrentRoom.Name}");
        //enable the Play Button
        if(PhotonNetwork.IsMasterClient)
        {
            //pick/load a room
            PhotonNetwork.LoadLevel(LevelToLoad);
        } 
    }

#endregion

}
