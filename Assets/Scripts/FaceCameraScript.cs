using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCameraScript : MonoBehaviour
{

    Transform cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main.transform;
        Debug.Log(cam.name);
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(cam);
    }
}
