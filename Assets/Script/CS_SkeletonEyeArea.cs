using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_SkeletonEyeArea : MonoBehaviour
{
    public CS_FSM parent;
    public bool isEnter;

    public float missTime;
    private float missTimeLeft;
    // Start is called before the first frame update
    void Start()
    {
        parent = transform.GetComponentInParent<CS_FSM>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!isEnter){
            if(missTimeLeft>=0){
                missTimeLeft-=Time.deltaTime;
            }else{
                parent.para.target=null;
            }
        }  
    }

    private void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("Player")){
            isEnter=true;
            parent.para.target=other.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D other){
        if(other.CompareTag("Player")){
            isEnter=false;
            missTimeLeft=missTime;
        }
    }
}
