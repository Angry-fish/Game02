using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//敌人的状态机类型
public enum StateType{
    Idle,Chase,Patrol,React,Attack,Dash,Death,Hit
}
public class CS_Manager : MonoBehaviour
{
    private static CS_Manager instance;
    public static CS_Manager Instance{
        get{
            if(instance==null){
                instance=Transform.FindObjectOfType<CS_Manager>();
            }
            return instance;
        }
    }

    public float hitScale;
    public float hitWaitTime;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HitAnima(Animator hit,Animator beHit){
        StartCoroutine(HitAnimaIE(hit,beHit));    
    }


    IEnumerator HitAnimaIE(Animator hit,Animator beHit){ 
        hit.speed=hitScale;
        beHit.speed=hitScale;
        yield return new WaitForSecondsRealtime(hitWaitTime);
        hit.speed=1;
        beHit.speed=1;
    }
}
