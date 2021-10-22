using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_Boom : MonoBehaviour
{
    public Animator myAnimator;
    public Rigidbody2D myRigidbody2D;
    public float health;
    public float maxHealth;
    public float speed;
    public float maxSpeed;
    public float damage;
    public Transform target;
    public Transform point;
    public float area;
    public LayerMask targetLayerMash;
    public bool isDead;

    
  
    public void Reset(){
        gameObject.layer=LayerMask.NameToLayer("FlyEnemy");
        isDead=false;
        myRigidbody2D.gravityScale=0;
        health=maxHealth;
        speed=Random.Range(maxSpeed-1,maxSpeed);
        
        
    }

    void OnEnable(){
        Reset();
    }

    void Awake(){
        myRigidbody2D=GetComponent<Rigidbody2D>();
        myAnimator=GetComponent<Animator>();
       target=GameObject.FindGameObjectWithTag("Player").transform;
       Reset();
    }

    void start(){
         target=GameObject.FindGameObjectWithTag("Player").transform;
       Reset();
    }

    private void Update(){
        
       if(isDead)return;
       transform.position=Vector2.MoveTowards(transform.position,target.position,speed*Time.deltaTime);
       if(Physics2D.OverlapCircle(point.position,area,targetLayerMash)){
            myAnimator.Play("Boom");
       }
    }
 
    public void BoomSlow(){
        myAnimator.speed=0.2f;
    }

    public void BoomReset(){
          isDead=true;
          myAnimator.speed=1;
    }
    public void BeHit(float d){
        CS_UI.count+=(int)d;
        CS_ParitclePool.instance.GetFromPool(transform);
        health-=d;
        if(health<=0){
            isDead=true;
            Dead();
        }else{
            StartCoroutine(SpeedChange());
        }
        

    }

    IEnumerator SpeedChange(){
       speed=-speed;
       yield return new WaitForSecondsRealtime(0.2f);
       speed=-speed;
    }
    

    public void Dead(){

        isDead=true;
        myRigidbody2D.gravityScale=1;
        myAnimator.Play("Dead");
        gameObject.layer=LayerMask.NameToLayer("Enemy");
    }

    public void Destroy(){
        CS_BoomPool.instance.RetrunPool(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other){
        if(!isDead)return;
        if(other.CompareTag("Player")){
            other.GetComponent<CS_Player>().HitPlayer(this.transform,damage);
        }else if(other.GetComponent<CS_LittleSK>()){
            other.GetComponent<CS_LittleSK>().BeHit(damage);
        }else if(other.GetComponent<CS_FSM>()){
            other.GetComponent<CS_FSM>().BeHit(damage);

        }

        

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(point.position , area);
    }

}
