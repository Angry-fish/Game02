using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_SkeletonParent : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform startPoint;
    void OnEnable(){
        GetComponentInChildren<CS_FSM>().ResetPara();
    }

    
    public void DestroySK(){
        CS_SkPool.instance.RetrunPool(this.gameObject);
    }
}
