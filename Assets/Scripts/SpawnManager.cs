using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Photon.Pun;

public class SpawnManager : MonoBehaviourPun
{

    public List<Transform> blueSpawnPoints;
    public List<Transform> redSpawnPoints;

    //start function
    void Start()
    {
        //debug log the players team number 1 for blue, 2 for red
        Debug.Log($"Player is on team {PhotonNetwork.LocalPlayer.CustomProperties["_pt"]}");
    }



    // THis just populates it at runtime before the game starts
    void OnValidate()
    {
        //get all the spawnpoints as transforms
        blueSpawnPoints = new List<Transform>();
        blueSpawnPoints.AddRange(GameObject.FindGameObjectsWithTag("BlueSpawn").Select((item) => item.transform));
        redSpawnPoints = new List<Transform>();
        redSpawnPoints.AddRange(GameObject.FindGameObjectsWithTag("RedSpawn").Select((item) => item.transform));
    }

    public Transform GetNextSpawn(Team team)
    {
        switch (team)
        {
            case Team.Blue:
                int r = Random.Range(0, blueSpawnPoints.Count - 1);
                Debug.Log($"Spawning at point {r}");
                return blueSpawnPoints[Random.Range(0, blueSpawnPoints.Count - 1)];
            case Team.Red:
                int r2 = Random.Range(0, redSpawnPoints.Count - 1);
                Debug.Log($"Spawning at point {r2}");
                return redSpawnPoints[Random.Range(0, redSpawnPoints.Count - 1)];
        }
        return null;
    }
}
