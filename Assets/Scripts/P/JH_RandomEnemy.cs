using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JH_RandomEnemy : MonoBehaviour
{
    SkinnedMeshRenderer rendererMtl;
    // Start is called before the first frame update
    void Start()
    {   float random = Random.Range(1f,2f);
        rendererMtl = GetComponent<SkinnedMeshRenderer>();
        rendererMtl.material.SetTextureOffset("_Texture",new Vector2(random,1));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
