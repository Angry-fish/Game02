using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CS_Player : MonoBehaviour
{
    // Start is called before the first frame update

    private Rigidbody2D myRigid2D;
    private Collider2D myCollider2D;
    private Animator myAnimator;
    private SpriteRenderer mySpriteRender;

    private CS_PlayerBeHitUI myCS_PlayerBeHitUI;

    public GameObject myParticleSystem;

    public float speed;
    public Transform groundCheckPoint;
    public LayerMask groundLayer;
    public float checkFloat;
    public bool isGround;
    public float health;

    private float horizontalMove;
    private float face=1;
    private int flick;

    [Header("Jump")]
    public float jumpForce;
    public bool isJump;
    public int jumpMaxcount;
    private int jumpCount;
    private bool jumpPressed;

    [Header("BeHit")]
    public float hitTimeScale;
    public bool isHit;
    public float hitSpeed;
    public bool hitIsRight;
    public float beHitWaitTime;
    public float protectTime;

     private bool canBeHit;
    
    private float protectTimeLeft;

    private float beHitWaitTimeLeft;


    [Header("Dash")]
    public bool isDash;
    public float dashTime;

    public float dashCD;
    public float dashSpeed;

    private float dashTimeLeft;
    private float lastDash=-10;
   



    [Header("Attack")]

  
    public float lightSpeed;
    public float heavySpeed;
    public float interval=2f;//combo time
    public float lightShakeTime;
    public float ligthStrength;
    public float heavyShakeTime;
    public float heavyStrength;
    public float heavyDamage;
    public float lightDamage;
    private bool isCombo;
    private int comboStep;
    private float timer;
    private bool isAttack;
    private string attackType;



    void Start()
    {
        
        canBeHit=true;
        mySpriteRender=GetComponent<SpriteRenderer>();
        myRigid2D = GetComponent<Rigidbody2D>();
        myCollider2D = GetComponent<Collider2D>();
        myAnimator = GetComponent<Animator>();
        myCS_PlayerBeHitUI=GetComponent<CS_PlayerBeHitUI>();
        
    }

    // Update is called once per frame
    void Update()
    {
        horizontalMove=0;
        if(Input.GetKeyDown(KeyCode.P)){
           HitPlayer(this.transform,1f);
       }
        if(isHit)return;

        if(isDash)return;

        if(Input.GetKeyDown(KeyCode.L)){
            if(Time.time>(lastDash+dashCD)){
                ReadyToDash();
            }
        }     
        if(isAttack)return;

        horizontalMove = Input.GetAxisRaw("Horizontal");
        SetFace();
        Flip();
        Attack();
        if(Input.GetKeyDown(KeyCode.W)&&jumpCount>0){
           jumpPressed=true;
        }
        
    }

   void FixedUpdate(){
       GroundCheck();
       myAnimator.SetFloat("Horizontal",horizontalMove);
       myAnimator.SetFloat("Vertical",myRigid2D.velocity.y);
       myAnimator.SetBool("isGround",isGround);    
        if(isDash)Dash();
        if(!isDash && !isHit)Move();
        if(jumpPressed)Jump();
        
        
    }

    void Attack(){
        if(Input.GetKeyDown(KeyCode.J) && !isAttack){
            isAttack=true;
            comboStep++;
            if(comboStep>3)comboStep=1;      
            timer=interval;
            attackType="Light";
            myAnimator.SetTrigger("LightAttack");
            myAnimator.SetInteger("ComboStep",comboStep);
        }
        if(Input.GetKeyDown(KeyCode.K) && !isAttack){
            isAttack=true;
            comboStep++;
            if(comboStep>3)comboStep=1;      
            timer=interval;
            attackType="Heavy";
            myAnimator.SetTrigger("HeavyAttack");
            myAnimator.SetInteger("ComboStep",comboStep);
        }
        if(!isCombo)StartCoroutine(attackCombo());
    }
      public void AttackOver(){
        isAttack =false;
    }

    IEnumerator attackCombo(){
        isCombo = true;
        while(timer>=0){
            timer-=Time.deltaTime;
            yield return null;
        }
        ComboReset();

    }

    void ComboReset(){
        timer=0;
        comboStep=0;
        isCombo = false;
    }
    void SetFace(){
        if(horizontalMove !=0){
            face = horizontalMove;
        }
    }

    void Flip(){
          transform.localScale=new Vector3(-face,1,1);
    }
  

   void ReadyToDash(){
       isDash=true;
       dashTimeLeft=dashTime;
       lastDash=Time.time;

}

    void Dash(){         
            if(dashTimeLeft>0){
                dashTimeLeft-=Time.deltaTime;
                myRigid2D.velocity=new Vector2(dashSpeed*face,0);
                CS_shadowPool.instance.GetFromPool();
                
            }else{
                DashOver();
            }     

    }
    
    void DashOver(){
        isDash=false;
        myRigid2D.velocity=Vector2.zero;
    }

    void GroundCheck(){
        isGround = Physics2D.OverlapCircle(groundCheckPoint.position,checkFloat,groundLayer);
        if(isGround){
            jumpCount=jumpMaxcount;
            isJump=false;
        }
    }
    void Move(){
        if(isAttack){
            if(attackType=="Light"){
                myRigid2D.velocity=new Vector2(face*lightSpeed,myRigid2D.velocity.y);
            }
            if(attackType=="Heavy"){
                myRigid2D.velocity=new Vector2(face*heavySpeed,myRigid2D.velocity.y);
            }
        }else{
         myRigid2D.velocity=new Vector2(horizontalMove*speed,myRigid2D.velocity.y);       
        }
           
        
    }
    
    void Jump(){    
        if(isGround){
            isJump=true;
            myRigid2D.velocity=new Vector2(myRigid2D.velocity.x,jumpForce);
            jumpPressed=false;           
        }else if(jumpCount>0){
            isJump=true;
            myRigid2D.velocity=new Vector2(myRigid2D.velocity.x,jumpForce);
            jumpCount--;
            jumpPressed=false;
        }
         myAnimator.SetFloat("Vertical",myRigid2D.velocity.y);
         myAnimator.ResetTrigger("Jump");
          myAnimator.SetTrigger("Jump");
        
    }

    private void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("Enemy")){
            if(attackType=="Light"){
                if(other.GetComponent<CS_FSM>()!=null){
                    if(!other.transform.GetComponent<CS_FSM>().para.canBeHit)return;
                    CS_Camera.Instance.Shake(lightShakeTime,ligthStrength);
                    CS_Manager.Instance.HitAnima(myAnimator,other.transform.GetComponent<Animator>());
                    other.GetComponent<CS_FSM>().BeHit(lightDamage);
                }
                else if(other.GetComponent<CS_LittleSK>()){
                     if(!other.GetComponent<CS_LittleSK>().canBeHit)return;
                    CS_Camera.Instance.Shake(lightShakeTime,ligthStrength);
                    CS_Manager.Instance.HitAnima(myAnimator,other.transform.GetComponent<Animator>());
                     other.GetComponent<CS_LittleSK>().BeHit(lightDamage);
                        
                        
                }else if(other.GetComponent<CS_Boom>()){
                    CS_Camera.Instance.Shake(lightShakeTime,ligthStrength);
                    CS_Manager.Instance.HitAnima(myAnimator,other.transform.GetComponent<Animator>());
                     other.GetComponent<CS_Boom>().BeHit(lightDamage);

                }
                    
                
            }
            if(attackType=="Heavy"){    
                 if(other.GetComponent<CS_FSM>()!=null){
                     if(!other.transform.GetComponent<CS_FSM>().para.canBeHit)return;
                     CS_Camera.Instance.Shake(heavyShakeTime,heavyStrength);
                     CS_Manager.Instance.HitAnima(myAnimator,other.transform.GetComponent<Animator>());
                     other.GetComponent<CS_FSM>().BeHit(heavyDamage);
                 }else if(other.GetComponent<CS_LittleSK>()){
                     if(!other.GetComponent<CS_LittleSK>().canBeHit)return;
                     CS_Camera.Instance.Shake(heavyShakeTime,heavyStrength);
                     CS_Manager.Instance.HitAnima(myAnimator,other.transform.GetComponent<Animator>());
                     other.GetComponent<CS_LittleSK>().BeHit(heavyDamage);
                        
                        
                }else if(other.GetComponent<CS_Boom>()){
                    CS_Camera.Instance.Shake(heavyShakeTime,heavyStrength);
                    CS_Manager.Instance.HitAnima(myAnimator,other.transform.GetComponent<Animator>());
                     other.GetComponent<CS_Boom>().BeHit(heavyDamage);

                }
            }
        }

    }

    public void HitPlayer(Transform other, float damage){
        if(!canBeHit)return;
        
        CS_UI.count=0;
        Instantiate(myParticleSystem,transform.position,transform.rotation);
        HitProtect();
        ComboReset();
        DashOver();
        AttackOver();
        if(other.position.x<transform.position.x){
            hitIsRight=false;
        }else{
            hitIsRight=true;
        }
        myCS_PlayerBeHitUI.HitFlashScreen();
        myAnimator.SetTrigger("IsHit");
        StartCoroutine(beHitTime());
        isHit=true;    
        health-=damage;
        if(health<=0){
          health=0;
        }else{
          if(hitIsRight){
              myRigid2D.velocity=new Vector2(-1f*hitSpeed,myRigid2D.velocity.y+1f);

          }else{
              myRigid2D.velocity=new Vector2(hitSpeed,myRigid2D.velocity.y+1f);

          }
        }
        
       

    }

    public void HitOver(){

        isHit=false;
        myRigid2D.velocity=new Vector2(0,myRigid2D.velocity.y);
      
    }

    IEnumerator beHitTime(){
        beHitWaitTimeLeft=beHitWaitTime;
        Time.timeScale=hitTimeScale;
        while(beHitWaitTimeLeft>=0){
            beHitWaitTimeLeft-=Time.deltaTime;
            Time.timeScale = Time.timeScale+0.005f>1f ? 1:Time.timeScale+0.01f;
           yield return null;
        }
        
        Time.timeScale=1;

    }

    private void HitProtect(){
        protectTimeLeft=protectTime;
        if(canBeHit){
            StartCoroutine(hitProtect());
        }
    }

    IEnumerator hitProtect(){
        canBeHit=false;
        while(protectTimeLeft>=0){
            if(flick%5==0){
                mySpriteRender.color=new Color(1,1,1,0.1f);
            }else{
                mySpriteRender.color=new Color(1,1,1,0.8f);
            }
            protectTimeLeft-=Time.deltaTime;
            flick++;

            yield return null;
        }
        mySpriteRender.color=new Color(1,1,1,1);
        canBeHit=true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(groundCheckPoint.position , checkFloat);
    }
}
