using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_LittleSK : MonoBehaviour
{
    public Transform me;

    public Animator myAnimator;

    public float health;
    
    public float maxHealth;

    public float walkspeed;

    public float maxWalkSpeed;
    public Transform target;

    private Vector2 targetPoint;

    public bool isAttack;

    public Transform attackCheckPoint;

    public float attackArea;

    public LayerMask targetLayer;

    public float damage;

    public bool canBeHit;


    



    void Awake()
    {
        me=this.transform;
        myAnimator=GetComponent<Animator>();
        Reset();
    }

    void OnEnable(){
        Reset();
    }

    public void Reset(){
        health = maxHealth;
        walkspeed=Random.Range(maxWalkSpeed-1,maxWalkSpeed);
        isAttack=false;
        canBeHit=true;
         myAnimator.Play("EnemyWalk");
    }

    // Update is called once per frame
    void Update()
    {
        if(health<=.1f )return;

        if(target==null)target = GameObject.FindGameObjectWithTag("Player").transform;

        targetPoint=new Vector2(target.position.x,me.position.y);
        if(Mathf.Abs(me.transform.position.y-target.position.y) < 3 &&  !isAttack ){
            FlipTo(target);
            me.transform.position=Vector2.MoveTowards(me.transform.position,targetPoint,walkspeed*Time.deltaTime);
           if(Physics2D.OverlapCircle(attackCheckPoint.position,attackArea,targetLayer)){     
                Attack();
           }
        }
    }

    public void Attack(){
       isAttack=true;
       myAnimator.speed=1.5f;
       myAnimator.Play("EnemyAttack",0,0f);
    }
    public void AttackOver(){
        myAnimator.speed=1;
        isAttack=false;
        myAnimator.Play("EnemyWalk");
    }

    private void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("Player")){
            target.GetComponent<CS_Player>().HitPlayer(me,damage);
        }
    }

    public void BeHit(float damage){
        AttackOver();
        CS_UI.count+=(int)damage;
        CS_ParitclePool.instance.GetFromPool(transform);
        health-=damage;
        if(health<=0)health=0;
        if(canBeHit)myAnimator.Play("EnemyDead");
        canBeHit=false;
    }

    public void Destroy(){
       
        CS_LSkPool.instance.RetrunPool(this.gameObject);
    }
    public void FlipTo(Transform target){
         if (target != null)
        {
            if (transform.position.x > target.position.x)
            {
                transform.localScale = new Vector3(-0.5f, 0.5f, 1);
            }
            else if (transform.position.x < target.position.x)
            {
                transform.localScale = new Vector3(0.5f, 0.5f, 1);
            }
        }
    }

     private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(attackCheckPoint.position , attackArea);
    }
}
