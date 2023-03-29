using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class LevelManager : MonoBehaviour
{
    //the player prefab to spawn in
    public PhotonView redPlayerPrefab;
    public PhotonView bluePlayerPrefab;

    [HideInInspector]
    public GameObject localPlayerGameObject;
    private PlayerHealth localPlayerHealthScript;
    SpawnManager spawnManager;

    //play the ausiosource when we die!
    AudioSource audioSource;

    // Use OnEnable in case it is also diabled
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        spawnManager = GetComponent<SpawnManager>();
        SpawnLocalPlayer();        
    }

    //Spawn the local player across the network.
    private void SpawnLocalPlayer()
    {
        var teamNumber = (byte)PhotonNetwork.LocalPlayer.CustomProperties["_pt"];
        Debug.Log($"SpawnLocalPlayer for team {teamNumber}");
        //team will be 1 for blue, 2 for red- set the right player prefab name so we can load it
        var prefabName = teamNumber == 1 ? bluePlayerPrefab.name : redPlayerPrefab.name;
        Debug.Log($"Spawning the prefab {prefabName}");
        Debug.Log($"Spawning Local Player for {PhotonNetwork.LocalPlayer}");
        //todo get the local player team and spawn the right player!
        Transform t = spawnManager.GetNextSpawn(teamNumber == 1 ? Team.Blue : Team.Red);
        localPlayerGameObject = PhotonNetwork.Instantiate(prefabName,t.position,t.rotation);
        localPlayerHealthScript =  localPlayerGameObject.GetComponent<PlayerHealth>();
        localPlayerHealthScript.playerKilled += OnLocalPlayerKilled;
        //make sure it is not paused in the levelmenumanager
        GetComponent<LevelMenuManager>().SetMenuEnabled(false);
    }


    public void OnLocalPlayerKilled(int actorNumber)
    {
        //Get the nickname from the actor number- this would be better cached but oh well.
        string nickname = "";
        foreach (var player in PhotonNetwork.PlayerList)
        {
            if (player.ActorNumber == actorNumber)
            {
                nickname = player.NickName;
            }
        }
        Debug.Log($"I was killed by {nickname}");
        //respawn the local player with a coroutine
        StartCoroutine(RespawnCoroutine());
        audioSource.Play();
    }

    IEnumerator RespawnCoroutine()
    {
        Debug.Log($"respawing {gameObject.name}");
        //PhotonNetwork.Destroy(localPlayerGameObject);
        yield return new WaitForSeconds(2);
        SpawnLocalPlayer();
    }





}
