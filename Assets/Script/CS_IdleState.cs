using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_IdleState : CS_IState
{
    private CS_FSM FSM;
    private Para myPara;
    private float timer;



    public CS_IdleState(CS_FSM fsm){
        this.FSM=fsm;
        this.myPara=fsm.para;
    }
    public void OnExit(){
        timer=0;
    }
    public void OnUpdate(){
    
        if(myPara.target!=null && myPara.target.position.x<myPara.chasePoints[1].position.x
        && myPara.target.position.x>myPara.chasePoints[0].position.x){
            FSM.TransitionState(StateType.React);
        }
        timer+=Time.deltaTime;
        if(timer>=myPara.idleTime){
            FSM.TransitionState(StateType.Patrol);

        }

    }
    public void OnEnter(){
        myPara.animator.Play("EnemyIdle");  
    }
}


public class CS_DashState : CS_IState
{
    private CS_FSM FSM;
    private Para myPara;
    private AnimatorStateInfo info;
    public CS_DashState(CS_FSM fsm){
        this.FSM=fsm;
        this.myPara=fsm.para;
        

    }
    public void OnExit(){
        myPara.canBeHit=true;
    }
    public void OnUpdate(){
        info = myPara.animator.GetCurrentAnimatorStateInfo(0);
        if(info.normalizedTime>=0.95f){
            FSM.TransitionState(StateType.Chase);
        }

    }
    public void OnEnter(){
        FSM.FlipTo(myPara.target);
        myPara.canBeHit=false;
        myPara.currentHitCount=0;
        myPara.countTimeLeft=0;
        myPara.animator.Play("EnemyDash");
    }
}

public class CS_PatrolState : CS_IState
{
    private CS_FSM FSM;
    private Para myPara;
    private int patrolPoint;
    public CS_PatrolState(CS_FSM fsm){
        this.FSM=fsm;
        this.myPara=fsm.para;
        patrolPoint=Random.Range(0,myPara.patrolPoints.Length);
    }
    public void OnExit(){
        patrolPoint++;
        if(patrolPoint>=myPara.patrolPoints.Length)patrolPoint=0;
    }
    public void OnUpdate(){
        Vector2 myPoint = new Vector2(myPara.patrolPoints[patrolPoint].position.x,
        FSM.transform.position.y);

        FSM.transform.position=Vector2.MoveTowards(FSM.transform.position,
        myPoint,myPara.moveSpeed*Time.deltaTime);

        if(Mathf.Abs(FSM.transform.position.x-myPara.patrolPoints[patrolPoint].position.x)<0.1f){
            FSM.TransitionState(StateType.Idle);
        }

        if(myPara.target!=null && myPara.target.position.x<myPara.chasePoints[1].position.x
        && myPara.target.position.x>myPara.chasePoints[0].position.x){
            FSM.TransitionState(StateType.React);
        }
    }
    public void OnEnter(){
        FSM.FlipTo(myPara.patrolPoints[patrolPoint]);
        myPara.animator.Play("EnemyWalk");
    }
}

public class CS_ChaseState : CS_IState
{
    private CS_FSM FSM;
    private Para myPara;
    public CS_ChaseState(CS_FSM fsm){
        this.FSM=fsm;
        this.myPara=fsm.para;
    }
    public void OnExit(){

    }
    public void OnUpdate(){
        
        if(myPara.target && myPara.target.position.x<myPara.chasePoints[1].position.x
        && myPara.target.position.x>myPara.chasePoints[0].position.x){
            FSM.FlipTo(myPara.target);
            Vector2 targetPoint =new Vector2(myPara.target.position.x,FSM.transform.position.y);
            FSM.transform.position=Vector2.MoveTowards(FSM.transform.position,
            targetPoint, myPara.chaseSpeed*Time.deltaTime );
        }else{
            FSM.TransitionState(StateType.Idle);
        }
        if(Physics2D.OverlapCircle(myPara.attackPoint.position,myPara.attackArea,myPara.targetLayer)){
            FSM.TransitionState(StateType.Attack);
        }

    }
    public void OnEnter(){
        myPara.animator.Play("EnemyWalk");
    }
}

public class CS_AttackState : CS_IState
{
    private CS_FSM FSM;
    private Para myPara;

    private AnimatorStateInfo info;
    public CS_AttackState(CS_FSM fsm){
        this.FSM=fsm;
        this.myPara=fsm.para;

    }
    public void OnExit(){

    }
    public void OnUpdate(){
        info = myPara.animator.GetCurrentAnimatorStateInfo(0);
        if(info.normalizedTime>=0.95f){
            FSM.TransitionState(StateType.Chase);
        }

    }
    public void OnEnter(){
        myPara.animator.Play("EnemyAttack");

    }
}

public class CS_ReactState : CS_IState
{
    private CS_FSM FSM;
    private Para myPara;
    private AnimatorStateInfo info;
    public CS_ReactState(CS_FSM fsm){
        this.FSM=fsm;
        this.myPara=fsm.para;

    }
    public void OnExit(){

    }
    public void OnUpdate(){
        info = myPara.animator.GetCurrentAnimatorStateInfo(0);
        if(info.normalizedTime>=0.95f){
            FSM.TransitionState(StateType.Chase);
        }

    }
    public void OnEnter(){
        myPara.animator.Play("EnemyReact");

    }
}

public class CS_HitState : CS_IState
{
    private CS_FSM FSM;
    private Para myPara;

    private bool isRight;

    private AnimatorStateInfo info;
    public CS_HitState(CS_FSM fsm){
        this.FSM=fsm;
        this.myPara=fsm.para;

    }
    public void OnExit(){
        myPara.isHit=false;
        myPara.myRigidbody2D.velocity=Vector2.zero;
    }
    public void OnUpdate(){
        info = myPara.animator.GetCurrentAnimatorStateInfo(0);
        if(isRight){
            myPara.myRigidbody2D.velocity=new Vector2(-myPara.hitSpeed,myPara.myRigidbody2D.velocity.y);
        }else{
            myPara.myRigidbody2D.velocity=new Vector2(myPara.hitSpeed,myPara.myRigidbody2D.velocity.y);
        }

        if(info.normalizedTime>=0.98f){
            myPara.myRigidbody2D.velocity=Vector2.zero;
            FSM.TransitionState(StateType.Chase);
        }

    }
    public void OnEnter(){
        myPara.target=GameObject.FindGameObjectWithTag("Player").transform;
        FSM.FlipTo(myPara.target);
        myPara.animator.Play("EnemyHit",0,0f);
        isRight = myPara.target.position.x>=FSM.transform.position.x;  
        myPara.currentHitCount++;

    }
}

public class CS_DeathState : CS_IState
{
    private CS_FSM FSM;
    private Para myPara;
    private float timer;
    public CS_DeathState(CS_FSM fsm){
        this.FSM=fsm;
        this.myPara=fsm.para;

    }
    public void OnExit(){
        timer=0;
        myPara.canBeHit=true;

    }
    public void OnUpdate(){
        timer+=Time.deltaTime;
        if(timer>=myPara.deathTime){
            FSM.DestroySelf();
        }
        myPara.myRigidbody2D.velocity=new Vector2(0,myPara.myRigidbody2D.velocity.y);

    }
    public void OnEnter(){
        myPara.canBeHit=false;
        myPara.animator.Play("EnemyDead");
       

    }
}