using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_Particle : MonoBehaviour
{
    // Start is called before the first frame update

    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!this.enabled){
             CS_ParitclePool.instance.RetrunPool(this.gameObject);
        }
    }
}
