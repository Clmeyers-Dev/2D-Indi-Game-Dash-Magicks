using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    // Start is called before the first frame update
    Abilities abilities;
      [Header("MeleeAttackStats")]
      public float swordDamage;
    public Transform attackPoint;
    public float attackRange = .5f;
    public LayerMask enemyLayers;
    public float attackRate =2f;
    float nextAttackTime = 0f;
    public Transform attackUpPoint;
    [Header("Attacking Melee")]
    public bool isAttackPressed;
    public bool isAttacking;
    public float attackDelay = 0.3f;
    public PlayerMovement playerMovement; 
    public Animator playerAnimator;
    public HealthManager healthManager;
    public SoundManager soundManager;
    public string currentState;
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerAnimator = GetComponentInChildren<Animator>();
        abilities = GetComponent<Abilities>();
        
        playerAnimator.Play("Idle");
        healthManager.currentHealth = GlobalControl.Instance.hp;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0)){
            isAttackPressed = true;
        }
        if(playerMovement.isWalking&&playerMovement.isGrounded&&!isAttacking&&!abilities.dashing){
           changeAnimationState("Walking");
        }
        if(playerMovement.isWalking ==false && playerMovement.isGrounded&&!isAttacking&&!abilities.dashing){
            changeAnimationState("Idle");
           
        }
        if(playerMovement.isGrounded == false&&playerMovement.isJumpingUp&&!abilities.dashing){
            changeAnimationState("JumpUp");
        }
        if(playerMovement.isGrounded == false&&playerMovement.isFalling&&!abilities.dashing){
            changeAnimationState("JumpDown");
            
        }
        if(abilities.dashing){
            changeAnimationState("Dash");
        }
    
        
    }
     public    bool switchSound = false;
    void FixedUpdate(){
        if(isAttackPressed)
        {
            isAttackPressed = false;

            if(!isAttacking)
            {
           
                isAttacking = true;
                if(playerMovement.isGrounded&&!playerMovement.isWalking){
                    changeAnimationState("PlayerMeleeAttack");
                    if(!switchSound){
                    soundManager.playSound("Swing");
                    switchSound = !switchSound;
                    }else{
                        soundManager.playSound("Swing2");
                         switchSound = !switchSound;
                    }
                    Attack();
                }
                if(!playerMovement.isGrounded/*&&!playerMovement.isWalking*/){
                    changeAnimationState("PlayerAirAttack");
                    Attack();
                     if(!switchSound){
                    soundManager.playSound("Swing");
                    switchSound = !switchSound;
                    }else{
                        soundManager.playSound("Swing2");
                         switchSound = !switchSound;
                    }
                   // playerMovement.r2d.constraints = RigidbodyConstraints2D.FreezePositionY|RigidbodyConstraints2D.FreezePositionX;
                }
                if(playerMovement.isGrounded&&playerMovement.isWalking){
                    changeAnimationState("PlayerWalkAttack");
                    Attack();
                     if(!switchSound){
                    soundManager.playSound("Swing");
                    switchSound = !switchSound;
                    }else{
                        soundManager.playSound("Swing2");
                         switchSound = !switchSound;
                    }
                }
                //attackDelay = playerAnimator.GetCurrentAnimatorStateInfo(0).length;
                Invoke("attackComplete",attackDelay);


            }
        }
    }
  
    void attackComplete(){
        isAttacking = false;
       // fall();
    }
       public void Attack(){
       Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position,attackRange,enemyLayers);
        foreach(Collider2D enemy in hitEnemies){
            Debug.Log(enemy.name);
            EnemyHealthManager  eh = enemy.GetComponent<EnemyHealthManager>();
            eh.damageEnemy(swordDamage);
        }
       }
    void fall(){
      //  playerMovement.r2d.constraints = RigidbodyConstraints2D.None| RigidbodyConstraints2D.FreezeRotation;
        
       // playerMovement.r2d.constraints = RigidbodyConstraints2D.FreezeRotation;
    }
     public void savePlayer()
    {
        GlobalControl.Instance.hp = healthManager.currentHealth;
        //GlobalControl.Instance.canCastFireBall = statManager.canFireBall;
        //GlobalControl.Instance.canDash = statManager.canDash;
    }
   public void changeAnimationState(string newState){
        
        //Stop an animation from interrupting itself
        if(currentState == newState)return;

       //play the target animation
        playerAnimator.Play(newState);

        //Ressaign the Current State
        currentState = newState;
    }
    void OnDrawGizmosSelected(){
    if(attackPoint == null)
    return;
    Gizmos.DrawWireSphere(attackPoint.position,attackRange);
    if(attackUpPoint == null)
    return;
    Gizmos.DrawWireSphere(attackUpPoint.position,attackRange);

}
}
