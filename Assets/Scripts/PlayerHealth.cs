using System;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviourPunCallbacks
{
    public GameObject deadPrefab;

    //the level manager subscribes to this event
    public Action<int> playerKilled;

    public int health = 100;

    public AudioClip takeDamageAudioClip;
    AudioSource audioSource;


    public override void OnEnable()
    {
        base.OnEnable();
        audioSource = GetComponent<AudioSource>();
    }


    //this is called by the laser gun script by a local player when it detects a hit with a clone
    //it runs the RPC so the clones actually registers damage for this connected player
    //open to serious abuse but it works.
    [PunRPC]
    public void TakeDamageRPC(int amount, int shooterActorNumber)
    {
        Debug.Log($"TakeDamage called on {photonView.Owner.NickName} shot by {shooterActorNumber}");
        health -= amount;
        if(health <= 0)
        {
            //we dead
            Debug.Log($"{gameObject.name} is Dead");
            Instantiate(deadPrefab,transform.position,transform.rotation);
            //invoke the action so the levelmanager can spawn the local player
            if(photonView.IsMine)//if we are the local player
            {
                //if i am the local player- tell the level manager we are dead and let it respawn us
                playerKilled?.Invoke(shooterActorNumber);                
                //destroy me on the network
                PhotonNetwork.Destroy(photonView);
            }
            //dend message to messagestream
            Debug.Log($"player {photonView.Owner.NickName} was killed by player with id {GetPlayerNickname(shooterActorNumber)}");
        }
        else
        {
            if(photonView.IsMine)
            {
                audioSource.PlayOneShot(takeDamageAudioClip);
            }
        }
    }

    string GetPlayerNickname(int actorNumber)
    {
        foreach (var player in PhotonNetwork.PlayerList)
        {
            if(player.ActorNumber == actorNumber)
            {
                return player.NickName;
            }
        }
        return "No Name";
    }



}
