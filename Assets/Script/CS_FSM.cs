using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;



[Serializable]
public class Para{
    public float health;

    public float maxHealth;
    public float moveSpeed;
    public float chaseSpeed;

    public float maxMoveSpeed;
    public float maxChaseSpeed;
    public float idleTime;
    public Transform[] patrolPoints;
    public Transform[] chasePoints;  
    public Animator animator;

    public Transform target;

    public CS_SkeletonParent parent;

    public LayerMask targetLayer;
    public Transform attackPoint;
    public float attackArea;
    public bool isHit;

    public float deathTime;

    public float hitSpeed;

    public Rigidbody2D myRigidbody2D;

    public int maxHitCount;

    public int currentHitCount;

    public float countTime;

    public float countTimeLeft;

    public bool resetCountIsBegin;

    public float dashSpeed;

    public bool dashIsRight;
    public bool canBeHit;

    

}
public class CS_FSM : MonoBehaviour
{
    public Para para;
    private CS_IState currentState;
    public Dictionary<StateType,CS_IState> states= new Dictionary<StateType, CS_IState>();
    // Start is called before the first frame update
    void Awake()
    {   
        para.parent= transform.GetComponentInParent<CS_SkeletonParent>();
        para.myRigidbody2D =GetComponent<Rigidbody2D>();
        para.animator=GetComponent<Animator>();
        states.Add(StateType.Idle,new CS_IdleState(this));
        states.Add(StateType.Attack,new CS_AttackState(this));
        states.Add(StateType.Chase,new CS_ChaseState(this));
        states.Add(StateType.Dash,new CS_DashState(this));
        states.Add(StateType.Patrol,new CS_PatrolState(this));
        states.Add(StateType.React,new CS_ReactState(this));
        states.Add(StateType.Death,new CS_DeathState(this));
        states.Add(StateType.Hit,new CS_HitState(this));     
        ResetPara();  
    }


    public void ResetPara(){
        para.canBeHit=true;
        para.health=para.maxHealth;
        para.target=null;
        para.moveSpeed=UnityEngine.Random.Range(para.maxMoveSpeed-1,para.maxMoveSpeed);
        para.chaseSpeed=UnityEngine.Random.Range(para.maxChaseSpeed-1,para.maxChaseSpeed);
        transform.position=para.parent.transform.position;

        TransitionState(StateType.Idle);
    }

    // Update is called once per frame
    void Update()
    {
       currentState.OnUpdate();
    }

    public void TransitionState(StateType type){
        if(currentState!=null){
            currentState.OnExit();
        }
        currentState=states[type];
        currentState.OnEnter();
    }

    public void FlipTo(Transform target){
         if (target != null)
        {
            if (transform.position.x > target.position.x)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else if (transform.position.x < target.position.x)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
        }

    }

    public void BeHit(float damage){
        if(!para.canBeHit)return;
        CS_UI.count+=(int)damage;
        CS_ParitclePool.instance.GetFromPool(transform);
        para.countTimeLeft=para.countTime;
        para.health = para.health-damage;
        if(para.health<=0){
            para.health=0;
            if(currentState!=states[StateType.Death])
            TransitionState(StateType.Death);
        }else{
            if(!para.resetCountIsBegin)StartCoroutine(resetHitCount());
            if(para.currentHitCount<para.maxHitCount){
            TransitionState(StateType.Hit);
            }
            else{
           // para.canBeHit=false;
            para.dashIsRight = para.target.position.x>transform.position.x;
            TransitionState(StateType.Dash);
            }
        }

    }

    IEnumerator resetHitCount(){
        para.resetCountIsBegin=true;
        while(para.countTimeLeft>=0){
            para.countTimeLeft-=Time.deltaTime;
            yield return null;
        }
        para.currentHitCount=0;
        para.resetCountIsBegin=false;
        
    }

    public void DestroySelf(){
        
        para.parent.DestroySK();

    }

    public void Dash(){
        if(para.dashIsRight){
        para.myRigidbody2D.velocity=new Vector2(para.dashSpeed,0);
        }else{
        para.myRigidbody2D.velocity=new Vector2(-para.dashSpeed,0);
        }
    }
    public void DashOver(){
        para.myRigidbody2D.velocity=new Vector2(0,0);

    }
    private void OnDrawGizmos(){
        Gizmos.DrawWireSphere(para.attackPoint.position,para.attackArea);
        
    }
    
}
