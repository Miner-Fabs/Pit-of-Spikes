using System;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;

    private float walkMovement; // value has more magnitude depending on how long walk button is pressed, positive if right, negative if left
    private float walkVelocity; // velocity when walking

    public float walkSpeed; // maximum walking speed
    public float jump; // strength of jumps

    public float jetSpeed; // strength of upward component of jetpack
    public float jetTorque; // strength of turning force of jetpack

    public float jetFuelMaximum; // maximum amount of fuel for either side of jetpack
    public float jetFuelUsage; // rate at which fuel is used
    public float jetFuelL; // amount of fuel in left side of jetpack
    public float jetFuelR; // amount of fuel in right side of jetpack
    public Action OnFuelChangeL; // used to tell Game HUD when fuel level changes
    public Action OnFuelChangeR;

    public float jetLandSpeed; // the maximum speed the player can be moving downwards to land
    public float jetStunSpeed; // the minimum speed the player must be moving to get stunned

    public float jetStunTorque; // the turning force applied if the player is stunned
    public float jetStunTime; // the amount of time the player is stunned for
    float stunStartTime; // the time at which the player was last stunned

    public bool isFlying; // true if the player is currently flying
    private bool stunChecked; // true if the player's head boxcast is touching a wall
    public bool isStunned; // true if the player is currently stunned
    private bool stunAnimated; // true if current stun animations have already been applied

    public LayerMask solidLayers; // used in raycasts that check for all solid layers
    public LayerMask groundLayer; // used in raycasts that check for ground
    public LayerMask oneWayUpLayer; // used in raycasts that check for one-way tiles that face up, down, left, and right
    public LayerMask oneWayDownLayer;
    public LayerMask oneWayLeftLayer;
    public LayerMask oneWayRightLayer;

    public Vector2 footBoxSize; // size of foot boxcast that determines if player is grounded when walking
    public float footCastDistance; // offset distance of foot boxcast
    public Vector2 headBoxSize; // size of head boxcast that determines if player should be stunned while flying
    public float headCastDistance; // offset distance of head boxcast
    public float landCastDistance; // length of raycast that determines if the player is close enough to the ground to land while flying

    private Animator anim; // handles animation changes

    public PauseScript pauseScript; // handles pausing
    private bool disableInput; // used to disable input when level won
    public bool disableStun; // used to disable stun checks from bonking for whole of spire level

    public GameObject respawnPoint; // used to keep track of current respawn point for a given level
    public GameObject[] allCheckpoints; // used to reference checkpoints for continuing from menu
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        disableInput = false;

        if (PlayerPrefs.GetInt("continueTroll") == 0 && PlayerPrefs.GetInt("trollCheckpointID") != 0)
        {
            int spawnCheckpointID = PlayerPrefs.GetInt("trollCheckpointID") - 1;
            respawnPoint = allCheckpoints[spawnCheckpointID];
            transform.position = respawnPoint.transform.position;

            PlayerPrefs.SetInt("continueTroll", 1);
            PlayerPrefs.Save();
        }
        else if (PlayerPrefs.GetInt("continueSpire") == 0 && PlayerPrefs.GetInt("spireCheckpointID") != 0)
        {
            int spawnCheckpointID = PlayerPrefs.GetInt("spireCheckpointID") - 1;
            respawnPoint = allCheckpoints[spawnCheckpointID];
            transform.position = respawnPoint.transform.position;

            PlayerPrefs.SetInt("continueSpire", 1);
            PlayerPrefs.Save();
        }

        //QualitySettings.vSyncCount = 0; // uncommented when testing low framerates
        //Application.targetFrameRate = 24;
    }

    // Update is called once per frame
    void Update()
    {
        // check if player wants to pause before anything else
        if (Input.GetKey(KeyCode.Escape) && !disableInput)
        {
            pauseScript.PauseGame();
        }

        if (isStunned)
        {
            if (!stunAnimated)
            {
                stunAnimated = true;
                anim.SetBool("isFlying", false);
                anim.SetBool("leftBoosterOn", false);
                anim.SetBool("rightBoosterOn", false);
                anim.SetBool("isStunned", true);
            }
            else if (Time.time > stunStartTime + jetStunTime)
            {
                anim.SetBool("isStunned", false);
                anim.SetBool("faceplantLeft", false);
                anim.SetBool("faceplantRight", false);
                isStunned = false;
                rb.freezeRotation = true;
                rb.rotation = 0;
            }
            else if (isNearGround() && rb.freezeRotation == false)
            {
                rb.freezeRotation = true;
                rb.rotation = 0;
                SoundManager.instance.PlayBonkSound();
                if (rb.linearVelocityX < 0)
                {
                    anim.SetBool("faceplantLeft", true);
                }
                else
                {
                    anim.SetBool("faceplantRight", true);
                }
            }
        }
        else if (isFlying)
        {

            if (isNearGround() && rb.linearVelocityY > jetLandSpeed)
            //&& (!(rb.rotation > 135 | rb.rotation < -135) | (rb.rotation > 175 | rb.rotation < -175)) // rotation check that i did not have time to fully implement
            {
                SoundManager.instance.EndJetSound();

                anim.SetBool("isFlying", false);
                anim.SetBool("leftBoosterOn", false);
                anim.SetBool("rightBoosterOn", false);
                isFlying = false;

                rb.freezeRotation = true;
                rb.rotation = 0;
            }
            else
            {
                if (Input.GetKey(KeyCode.Q) && Input.GetKey(KeyCode.E) && jetFuelL > 0 && jetFuelR > 0 && !disableInput)
                {
                    rb.AddForce(transform.up * jetSpeed * Time.deltaTime);

                    jetFuelL -= jetFuelUsage * Time.deltaTime;
                    jetFuelR -= jetFuelUsage * Time.deltaTime;

                    OnFuelChangeL?.Invoke();
                    OnFuelChangeR?.Invoke();

                    SoundManager.instance.StartJetSound();

                    anim.SetBool("leftBoosterOn", true);
                    anim.SetBool("rightBoosterOn", true);
                }
                else if (Input.GetKey(KeyCode.Q) && jetFuelL > 0 && !disableInput)
                {
                    rb.AddForce(transform.up * (jetSpeed / 2) * Time.deltaTime);
                    rb.AddTorque(-jetTorque * Time.deltaTime, ForceMode2D.Impulse);

                    jetFuelL -= jetFuelUsage * Time.deltaTime;

                    OnFuelChangeL?.Invoke();

                    SoundManager.instance.StartJetSound();

                    anim.SetBool("leftBoosterOn", true);
                    anim.SetBool("rightBoosterOn", false);

                }
                else if (Input.GetKey(KeyCode.E) && jetFuelR > 0 && !disableInput)
                {
                    rb.AddForce(transform.up * (jetSpeed / 2) * Time.deltaTime);
                    rb.AddTorque(jetTorque * Time.deltaTime, ForceMode2D.Impulse);

                    jetFuelR -= jetFuelUsage * Time.deltaTime;

                    OnFuelChangeR?.Invoke();

                    SoundManager.instance.StartJetSound();

                    anim.SetBool("leftBoosterOn", false);
                    anim.SetBool("rightBoosterOn", true);
                }
                else
                {
                    SoundManager.instance.EndJetSound();

                    anim.SetBool("leftBoosterOn", false);
                    anim.SetBool("rightBoosterOn", false);
                }
            }
        }
        else
        {
            if (!disableInput)
            {
                walkMovement = Input.GetAxis("Horizontal");
            }
            walkVelocity = walkMovement * walkSpeed;

            if (walkMovement > 0.3)
            {
                anim.SetBool("isWalking", true);
                anim.SetBool("isFacingLeft", false);
                anim.SetBool("isFacingRight", true);
            }
            else if (walkMovement < -0.3)
            {
                anim.SetBool("isWalking", true);
                anim.SetBool("isFacingLeft", true);
                anim.SetBool("isFacingRight", false);
            }
            else if (walkMovement == 0)
            {
                anim.SetBool("isWalking", false);
                anim.SetBool("isFacingLeft", false);
                anim.SetBool("isFacingRight", false);
            }

            anim.SetBool("isJumping", !isGrounded());

            if (Input.GetButtonDown("Jump") && isGrounded() && !disableInput)
            {
                rb.AddForce(new Vector2(rb.linearVelocityX, jump * 10));
                SoundManager.instance.PlayJumpSound();
            }

            if (((Input.GetKeyDown(KeyCode.Q) && jetFuelL > 0) | (Input.GetKeyDown(KeyCode.E) && jetFuelR > 0)) && !isGrounded() && !disableInput)
            {
                anim.SetBool("isFlying", true);
                isFlying = true;
                rb.freezeRotation = false;

                anim.SetBool("isWalking", false);
                anim.SetBool("isFacingLeft", false);
                anim.SetBool("isFacingRight", false);
                anim.SetBool("isJumping", false);
            }
        }
    }

    // FixedUpdate is called once per physics frame at a more consistent rate than Update
    void FixedUpdate()
    {
        if (!isStunned)
        {
            if (isFlying)
            {
                if (!headNearGround())
                {
                    stunChecked = false;
                }

                if (!stunChecked && headNearGround() && !disableStun)
                {
                    stunChecked = true;
                    if (rb.linearVelocity.sqrMagnitude >= jetStunSpeed)
                    {
                        // only invert x velocity if ceiling or floor hit
                        RaycastHit2D thisCast = Physics2D.BoxCast(transform.position, headBoxSize, 0, -transform.up, headCastDistance, solidLayers);
                        if (thisCast.normal != Vector2.up && thisCast.normal != Vector2.down)
                        {
                            rb.linearVelocityX = -(rb.linearVelocityX / 2);
                        }
                        else
                        {
                            rb.linearVelocityY = -(rb.linearVelocityY/ 2);
                        }

                        if (rb.rotation > 0)
                        {
                            rb.AddTorque(-jetStunTorque, ForceMode2D.Impulse);
                        }
                        else
                        {
                            rb.AddTorque(jetStunTorque, ForceMode2D.Impulse);
                        }
                        stunStartTime = Time.time;

                        SoundManager.instance.PlayBonkSound();
                        SoundManager.instance.EndJetSound();

                        isStunned = true;
                        stunAnimated = false;
                        isFlying = false;
                    }
                }
            }
            else
            {
                rb.linearVelocityX = walkVelocity;
            }
        }
    }

    public bool isGrounded()
    {
        if (Physics2D.BoxCast(transform.position, footBoxSize, 0, -transform.up, footCastDistance, groundLayer) |
            (Physics2D.BoxCast(transform.position, footBoxSize, 0, -transform.up, footCastDistance, oneWayUpLayer) && rb.linearVelocityY == 0))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    
    public bool headNearGround()
    {

        if (Physics2D.BoxCast(transform.position, headBoxSize, 0, -transform.up, headCastDistance, groundLayer) |
            (Physics2D.BoxCast(transform.position, headBoxSize, 0, -transform.up, headCastDistance, oneWayUpLayer) && rb.linearVelocityY < 0) |
            (Physics2D.BoxCast(transform.position, headBoxSize, 0, -transform.up, headCastDistance, oneWayDownLayer) && rb.linearVelocityY > 0) |
            (Physics2D.BoxCast(transform.position, headBoxSize, 0, -transform.up, headCastDistance, oneWayLeftLayer) && rb.linearVelocityX > 0) |
            (Physics2D.BoxCast(transform.position, headBoxSize, 0, -transform.up, headCastDistance, oneWayRightLayer) && rb.linearVelocityX < 0))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool isNearGround()
    {

        if (Physics2D.Raycast(transform.position, -Vector3.up, landCastDistance, groundLayer) |
            (Physics2D.Raycast(transform.position, -Vector3.up, landCastDistance, oneWayUpLayer) && rb.linearVelocityY < 0))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void KillPlayer()
    {
        transform.position = respawnPoint.transform.position;

        jetFuelL = 0;
        jetFuelR = 0;

        OnFuelChangeL?.Invoke();
        OnFuelChangeR?.Invoke();

        rb.freezeRotation = false;
        rb.linearVelocityX = 0;
        rb.linearVelocityY = 7;
        rb.AddTorque(jetStunTorque, ForceMode2D.Impulse);

        stunStartTime = Time.time;

        SoundManager.instance.PlayBonkSound();
        SoundManager.instance.EndJetSound();

        isStunned = true;
        stunAnimated = false;
        isFlying = false;

    }
    public void OnDisableInput()
    {
        disableInput = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position - transform.up * footCastDistance, footBoxSize);
        Gizmos.DrawWireCube(transform.position - transform.up * headCastDistance, headBoxSize);
        Gizmos.DrawLine(transform.position, transform.position - Vector3.up * landCastDistance);
    }
}
