using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCam : MonoBehaviour
{
    public Transform cameraPos;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = cameraPos.position;
    }
}