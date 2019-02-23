﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerSimple : MonoBehaviour, DynamicLightSource
{
    [Header("Parameters")]
    public float velocity = 5;
    public float turnSpeed = 10;

    [Header("Child Controllers")]
    public DynamicLightController playerLightController;

    int _lanternUses;
    private int lanternUses
    {
        get
        {
            return _lanternUses;
        }
        set
        {
            _lanternUses = value;

            if (playerLightController && playerLightController.IsSetup())
                OnLightSourceValueChange(_lanternUses);
        }
    }
    private int maxLanternUses = 6;
    private Vector2 input;
    private float angle;
    private Quaternion targetRotation;
    private Transform levelCamera;

    public bool inWater = false; // TODO: Dont make public, bring WaterCollision.cs into this script
    public float jumpForce = 1.0f;
    public float gravityModifier = 1.0f;
    public LayerMask ground;

    private float lockMovementTime = 0.0f;
    private Rigidbody rb;
    private CapsuleCollider capsuleCollider;
    private Vector3 lastGroundedPosition;

    private CharacterController characterController;
    private Animator animator;

    public void Setup(Vector3 startingPosition, int startingLanternUses, int maxLanternUses)
    {
        this.transform.position = startingPosition;
        this.lanternUses = startingLanternUses;
        this.maxLanternUses = Mathf.Max(startingLanternUses, maxLanternUses);

        if (playerLightController)
        {
            playerLightController.Setup(this);
        }
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        levelCamera = Camera.main.transform;

        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }


    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -25)
        {
            transform.position = new Vector3(0, 0, 0);
        }
    }



    #region LanternFunctions

    /// <summary>
    /// Notifies light controller of new value
    /// </summary>
    public void OnLightSourceValueChange(int newValue)
    {
        playerLightController.CalculateLightTargets(newValue);
    }

    /// <summary>
    /// Returns the number of lantern uses the player currently has
    /// </summary>
    /// <returns></returns>
    public int GetLightSourceValue()
    {
        return lanternUses;
    }

    /// <summary>
    /// Returns the number of lantern uses the player currently has
    /// </summary>
    /// <returns></returns>
    public int GetCurrentLanternUsesLeft()
    {
        return lanternUses;
    }

    /// <summary>
    /// Uses the lantern and returns true if possible, otherwise, false;
    /// </summary>
    public bool RequestLanternUse()
    {
        if (lanternUses == 0)
        {
            return false;
        }

        lanternUses--;
        return true;
    }

    /// <summary>
    /// Adds to the lantern uses and returns true if possible, otherwise, false;
    /// </summary>
    public bool RequestLanternAddition()
    {
        if (lanternUses == maxLanternUses)
        {
            return false;
        }

        lanternUses++;
        return true;
    }

    #endregion



    private void GetInput()
    {
        input.x = Input.GetAxisRaw(StringConstants.Input.HorizontalMovement);
        input.y = Input.GetAxisRaw(StringConstants.Input.VerticalMovement);
    }

    private void CalculateDirection()
    {
        angle = Mathf.Atan2(input.x, input.y);
        angle = Mathf.Rad2Deg * angle;
        angle += levelCamera.eulerAngles.y;
    }

    private void Rotate()
    {
        targetRotation = Quaternion.Euler(0, angle, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
    }

    private void Move()
    {
        transform.position += transform.forward * velocity * Time.deltaTime;
    }

    void FixedUpdate()
    {
        // Modify gravity when falling
        if (rb.useGravity)
        {
            // Increase gravity on when falling, how most other platformers do it.
            if (rb.velocity.y < -0.01f)
            {
                rb.AddForce(Physics.gravity * rb.mass * gravityModifier);
            }
            else
            {
                rb.AddForce(Physics.gravity * rb.mass);
            }
        }

        if (lockMovementTime > 0.0f)
        {
            lockMovementTime -= Time.deltaTime;
            return;
        }

        // TODO: Player can just repeatedly jump out of water to bypass neagtive effect.
        //       Later on, add some functionality so that the player can get out of water
        if (Grounded() && Input.GetButton(StringConstants.Input.JumpButton))
        {
            //Debug.Log("Pressed A");
            rb.AddForce(0, jumpForce, 0, ForceMode.Impulse);
        }

        if (Input.GetButton(StringConstants.Input.ActivateButton))
        {

            //Debug.Log("Pressed X"); 

            /*
            AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
            float clipLength = 0;
            foreach (AnimationClip clip in clips)
            {
                if (clip.name.Equals("WK_heavy_infantry_08_attack_B"))
                {
                    clipLength = clip.length;
                }
            }

            animator.SetInteger("AnimationState", 2);

            do
            {
                clipLength -= Time.deltaTime;
            } while (clipLength > 0);
            */

            //animator.SetTrigger("Attack");
            //Debug.Log(animator.GetInteger("State"));
            //StartCoroutine("AttackCoroutine");

            animator.SetInteger("AnimationState", 2);
            return;
        }

        GetInput();

        if (Mathf.Abs(input.x) == 0 && Mathf.Abs(input.y) == 0)
        {
            animator.SetInteger("AnimationState", 0);
            return;
        }

        animator.SetInteger("AnimationState", 1);
        CalculateDirection();
        Rotate();
        Move();

        Debug.Log("AnimationState = " + animator.GetInteger("AnimationState"));
    }

    private bool Grounded()
    {
        bool grounded = Physics.Raycast(transform.position, -Vector3.up, 1.0f, ground);
        if (grounded)
        {
            lastGroundedPosition = this.transform.position;
        }
        return grounded;
    }

    private void OnCollisionEnter(Collision collision)
    {
        rb.angularVelocity = new Vector3(0, 0, 0);
    }

    private void OnCollisionStay(Collision collision)
    {
        rb.angularVelocity = new Vector3(0, 0, 0);
    }

    public float GetVelocity()
    {
        return velocity;
    }

    public void SetVelocity(float newVelocity)
    {
        velocity = newVelocity;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Crystal"))
        {
            lanternUses += (int)other.gameObject.GetComponent<CrystalScript>().GetWarmth();
            //warmth += ;
            other.gameObject.SetActive(false);
        }
    }

    public void ScaleLightSource(float percent)
    {
        playerLightController.Scale(percent);
    }

    public void GoToLastGroundedPosition()
    {
        this.transform.position = lastGroundedPosition;
        lockMovementTime = 1.0f;
    }

    
    private IEnumerator AttackCoroutine()
    {
        animator.SetInteger("AnimationState", 2);

        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length + 
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
    }
    
}
