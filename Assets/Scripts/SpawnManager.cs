using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Photon.Pun;

public class SpawnManager : MonoBehaviourPun
{

    public List<Transform> spawnPoints;

    // THis just populates it at runtime before the game starts
    void OnValidate()
    {
        //get all the spawnpoints as transforms
        spawnPoints.AddRange(GameObject.FindGameObjectsWithTag("Respawn").Select((item) => item.transform));
    }

    public Transform GetNextSpawn()
    {
        //return the next spawnpoint- be careaful to only call this on the master-client!
        int r = Random.Range(0,spawnPoints.Count-1);
        Debug.Log($"Spawning at point {r}");
        return spawnPoints[Random.Range(0,spawnPoints.Count-1)];        
    }    
}
