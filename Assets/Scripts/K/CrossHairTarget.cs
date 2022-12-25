using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossHairTarget : MonoBehaviour
{
    Camera mainCamera;
    //Ray ray;
    //RaycastHit hitInfo;
    [SerializeField]
    private LayerMask Mask;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
    }
    
    // Update is called once per frame
    void Update()
    {
        DorayCast();
    }


    void DorayCast()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        if (Physics.Raycast(ray, out RaycastHit hitInfo, 1000, Mask))
        {
            transform.position = hitInfo.point;
        }
        else
        {
            //Debug.Log(hitInfo.transform.tag);
            transform.position = ray.origin + ray.direction * 1000.0f;
        }
    }
}
