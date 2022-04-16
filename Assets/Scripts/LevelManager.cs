using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class LevelManager : MonoBehaviour
{
    //the player prefab to spawn in
    public PhotonView playerPrefab;

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
        Debug.Log($"Spawning Local Player for {PhotonNetwork.LocalPlayer}");
        Transform t = spawnManager.GetNextSpawn();
        localPlayerGameObject = PhotonNetwork.Instantiate(playerPrefab.name,t.position,t.rotation);
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
