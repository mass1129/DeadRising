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
        Ray ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));



        if (Physics.Raycast(ray, out RaycastHit hitInfo,float.MaxValue, Mask))
        {

            

            transform.position = hitInfo.point;
            //Debug.Log(hitInfo.transform.tag);
            
        }
        else
        {
            //Debug.Log(hitInfo.transform.tag);
            transform.position = ray.origin + ray.direction * 1000.0f;
            //transform.position = ray.GetPoint(75);
        }
    }


    void DorayCast()
    {
        //ray.origin = mainCamera.transform.position;
        //ray.direction = mainCamera.transform.forward;
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));


        if (Physics.Raycast(ray, out RaycastHit hitInfo, Mask))
        {
            
            

            transform.position = hitInfo.point;
            //Debug.Log(hitInfo.transform.tag);
            //if (Mask.value.Equals(9))
            //{
            //    Debug.Log(hitInfo.transform.tag);
            //    transform.position = ray.origin + ray.direction * 1000.0f;
            //    Debug.Log(hitInfo.transform.tag);
            //}
        }
        else
        {
            Debug.Log(hitInfo.transform.tag);
            transform.position = ray.origin + ray.direction * 1000.0f;
            //transform.position = ray.GetPoint(75);
        }
    }
}
