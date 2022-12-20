using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimLookAt : MonoBehaviour
{
    Camera mainCamera;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = mainCamera.transform.position+ mainCamera.transform.forward*10;
    }
}
