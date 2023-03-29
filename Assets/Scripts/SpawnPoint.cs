using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 public enum Team
    {
        Blue,
        Red
    }


public class SpawnPoint : MonoBehaviour
{

    public Team team = Team.Blue;
    Color teamColor;

    // Start is called before the first frame update
    void Start()
    {
        //siable the collider so we can't walk into it
        GetComponent<Collider>().enabled = false;
        
    }

    void OnDrawGizmos()
    {
        //set the color of the spawnpoint based on the team
        switch (team)
        {
            case Team.Blue:
                teamColor = new Color(0,0,1,0.5f);
                gameObject.tag = "BlueSpawn";
                break;
            case Team.Red:
                gameObject.tag = "RedSpawn";
                teamColor = new Color(1,0,0,0.5f);
                break;
        }
        //draw a sphere at the spawn point
        Gizmos.color = teamColor;
        Gizmos.DrawCube(transform.position, new Vector3(1,2,1));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
