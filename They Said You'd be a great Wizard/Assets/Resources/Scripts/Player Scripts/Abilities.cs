using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Abilities : MonoBehaviour
{
    public PlayerManager playerManager;
    public HealthManager healthManager;
    public Rigidbody2D rigid;
    public float dashCoolDownStart;
    public float currentDashCoolDown;
    public float dashSpeed;
    public float dashTime;
    public float startDashTime;
    float ButtonCooler = 0.3f; // Half a second before reset
    int ButtonCount = 0;
    public int direction;
    public bool dashUp;
    public bool dashLeft;
    public bool dashRight;
    public bool dashing;
    public bool canDash;
    public bool playedDash = false;

    public GameObject dashEffect;
    public GameObject dashEffectFlip;
    public Transform dashFlipSpawn;
    //  public GameObject trail;
    public int dashCost = 40;
    // Start is called before the first frame update
    void Start()
    {
        playerManager = GetComponent<PlayerManager>();
        rigid = playerManager.playerMovement.r2d;
        healthManager = GetComponent<HealthManager>();
    }

    // Update is called once per frame
    void Update()
    {
        dash();

        if (playerManager.playerMovement.isGrounded)
        {
            canDash = true;
        }
        if (healthManager.currentMana > dashCost)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {

                if (ButtonCooler > 0 && ButtonCount == 1/*Number of Taps you want Minus One*/)
                {
                    if (canDash)
                    {
                        dashUp = true;
                        dashing = true;
                        healthManager.useMana(dashCost);

                    }
                }
                else
                {
                    ButtonCooler = 0.3f;
                    ButtonCount += 1;
                }
            }
            if (Input.GetKeyDown(KeyCode.A))
            {

                if (ButtonCooler > 0 && ButtonCount == 1/*Number of Taps you want Minus One*/)
                {
                    dashLeft = true;
                    dashing = true;
                    healthManager.useMana(dashCost);

                }
                else
                {
                    ButtonCooler = 0.3f;
                    ButtonCount += 1;
                }
            }
            if (Input.GetKeyDown(KeyCode.D))
            {

                if (ButtonCooler > 0 && ButtonCount == 1/*Number of Taps you want Minus One*/)
                {
                    dashRight = true;
                    dashing = true;
                    healthManager.useMana(dashCost);
                }
                else
                {
                    ButtonCooler = 0.3f;
                    ButtonCount += 1;
                }
            }
            if (ButtonCooler > 0)
            {

                ButtonCooler -= 1 * Time.deltaTime;

            }
            else
            {
                ButtonCount = 0;
            }
        }
    }
    public Transform dashEffectSpawnLocation;
    public void dash()
    {
        if (direction == 0)
        {
            if (dashLeft)
            {

                Instantiate(dashEffectFlip, dashFlipSpawn.position, Quaternion.identity);
                // trail.SetActive(true);
                direction = 1;
                dashLeft = false;

                // BarMan.setCanHurt(false);
            }
            else if (dashRight)
            {

                Instantiate(dashEffect, dashEffectSpawnLocation.position, Quaternion.identity);
                //  trail.SetActive(true);
                direction = 2;

                dashRight = false;
                //BarMan.setCanHurt(false);
            }
            else if (dashUp)
            {
                //Instantiate(dashEffect, transform.position, Quaternion.identity);
                // trail.SetActive(true);
                direction = 3;

                dashUp = false;
                //BarMan.setCanHurt(false);
            }
            /*else if (Input.GetKeyDown(KeyCode.DownArrow) && BarMan.currentStamina >= dashCost)
            {
                Instantiate(dashEffect, transform.position, Quaternion.identity);
                trail.SetActive(true);
                direction = 4;
                PlayerAnimator.SetBool("isDashing", true);
                BarMan.useStamina(dashCost);
                dashPlay();
            }*/
        }
        else
        {
            if (dashTime <= 0)
            {
                direction = 0;
                dashTime = startDashTime;
                rigid.velocity = Vector2.zero;
                //trail.SetActive(false);
                dashing = false;
                playedDash = false;
                //  Invoke(/*playerManager.changeAnimationState("Walking")*/"changeAnimationState(Walking)",1);
                // PlayerAnimator.SetBool("isDashing", false);
                //BarMan.setCanHurt(true);
            }
            else
            {
                dashTime -= Time.deltaTime;
                if (direction == 1)
                {

                    rigid.velocity = Vector2.left * dashSpeed;
                    if (!playedDash)
                    {
                        playerManager.soundManager.playSound("Dash");
                        playedDash = true;
                    }

                }
                else if (direction == 2)
                {

                    rigid.velocity = Vector2.right * dashSpeed;
                    if (!playedDash)
                    {
                        playerManager.soundManager.playSound("Dash");
                        playedDash = true;
                    }

                }
                else if (direction == 3)
                {
                    rigid.velocity = Vector2.up * dashSpeed;
                    if (!playedDash)
                    {
                        playerManager.soundManager.playSound("Dash");
                        playedDash = true;
                    }
                    if (!playerManager.playerMovement.isGrounded)
                    {
                        canDash = false;
                    }
                }
                else if (direction == 4)
                {
                    rigid.velocity = Vector2.down * dashSpeed;
                    if (!playedDash)
                    {
                        playerManager.soundManager.playSound("Dash");
                        playedDash = true;
                    }
                }
            }
        }
    }
}
