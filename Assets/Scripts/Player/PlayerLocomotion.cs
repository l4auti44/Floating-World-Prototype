using FMOD.Studio;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using RotaryHeart.Lib.PhysicsExtension;
using UnityEditor.IMGUI.Controls;

public class PlayerLocomotion : MonoBehaviour
{
    PlayerManager playerManager;
    AnimatorManager animatorManager;
    InputManager inputManager;

    Vector3 moveDirection;
    Transform cameraObject;
    public Rigidbody playerRigibody;

    [Header("Falling")]
    public float inAirTimer;
    public float leapingVelocity;
    public float fallingVelocity;
    public float airMovementVelocity;
    public float rayCastHeightOffset = 0.2f;
    public LayerMask groundLayer;
    public float maxDistance = 1;


    [Header("Movement Flags")]
    public bool isSprinting;
    public bool isGrounded;
    public bool isJumping;
    public bool enableDebugThings;

    [Header("Movement Speeds")]
    public float walkingSpeed = 1.5f;
    public float runningSpeed = 5;
    public float sprintingSpeed = 7;
    public float rotationSpeed = 15;

    [Header("Jump Speeds")]
    public float jumpHeight = 3;
    public float gravityIntensity = -15;

    //AUDIO
    private EventInstance playerFootsteps;

    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
        animatorManager = GetComponent<AnimatorManager>();
        inputManager = GetComponent<InputManager>();
        playerRigibody = GetComponent<Rigidbody>();
        cameraObject = Camera.main.transform;
    }

    private void Start()
    {
        playerFootsteps = AudioManager.instance.CreateEventInstance(FMODEvents.instance.Rfootsteps);
    }

    public void HandleAllMovement()
    {
        HandleFallingAndLanding();

        if (playerManager.isInteracting)
            return;

        HandleMovement();
        HandleRotation();
        
    }
    private void HandleMovement()
    {
        if (isJumping)
            return;

        moveDirection = cameraObject.forward * inputManager.verticalInput;
        moveDirection = moveDirection + cameraObject.right * inputManager.horizontalInput;
        moveDirection.Normalize();
        moveDirection.y = 0;

        if (isSprinting)
        {
            moveDirection *= sprintingSpeed;
        }
        else
        {
            if (inputManager.moveAmount >= 0.5f)
            {
                moveDirection *= runningSpeed;
            }
            else
            {
                moveDirection *= walkingSpeed;
            }
        }

        
        
        

        Vector3 movementVelocity = moveDirection;
        playerRigibody.velocity = movementVelocity;

    }


    private void HandleRotation()
    {
        if (isJumping)
            return;

        Vector3 targetDirection = Vector3.zero;

        targetDirection = cameraObject.forward * inputManager.verticalInput;
        targetDirection = targetDirection + cameraObject.right * inputManager.horizontalInput;
        targetDirection.Normalize();
        targetDirection.y = 0;

        if (targetDirection == Vector3.zero)
        {
            targetDirection = transform.forward;
        }

        quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        transform.rotation = playerRotation;
    }
        

    private void UpdateSound()
    {
        if(inputManager.moveAmount > 0 && isGrounded && !playerManager.isInteracting)
        {
            PLAYBACK_STATE playbackState;
            playerFootsteps.getPlaybackState(out playbackState);
            if (playbackState.Equals(PLAYBACK_STATE.STOPPED))
            {
                playerFootsteps.start();
            }
        }
        else
        {
            playerFootsteps.stop(STOP_MODE.ALLOWFADEOUT);
        }
    }

    private void FixedUpdate()
    {
        UpdateSound();
    }

    private void HandleFallingAndLanding()
    {
        RaycastHit hit;
        Vector3 rayCastOrigin = transform.position;
        rayCastOrigin.y = rayCastOrigin.y + rayCastHeightOffset;
        Vector3 targetPosition;
        targetPosition = transform.position;

        if (!isGrounded)
        {
            if (!playerManager.isInteracting && !isJumping)
            {
                animatorManager.PlayTargetAnimation("Falling", true);
            }
            animatorManager.animator.SetBool("isUsingRootMotion", false);
            inAirTimer += Time.deltaTime;
            //LEFT AND RIGHT MOVEMENT
            moveDirection = cameraObject.right * inputManager.horizontalInput;
            playerRigibody.AddForce(moveDirection * airMovementVelocity);

            //DOWN AND FOWARD LEAPING
            playerRigibody.AddForce(transform.forward * leapingVelocity);
            playerRigibody.AddForce(-Vector3.up * fallingVelocity * inAirTimer);

            //BACK AND FOWARD
            moveDirection = cameraObject.forward * inputManager.verticalInput;
            playerRigibody.AddForce(moveDirection * airMovementVelocity);
        }

        if (enableDebugThings)
        {
            RotaryHeart.Lib.PhysicsExtension.DebugExtensions.DebugWireSphere(rayCastOrigin, Color.red, 0.2f, 1, PreviewCondition.Both, true);
        }
        

        //detect floor
        if (UnityEngine.Physics.SphereCast(rayCastOrigin, 0.15f, -Vector3.up, out hit, maxDistance, groundLayer))
        {
            if (!isGrounded && playerManager.isInteracting)
            {

                if (inAirTimer >= 1f)
                {
                    animatorManager.PlayTargetAnimation("Hard Landing", true);

                }
                else
                {
                    animatorManager.PlayTargetAnimation("Land", true);

                }
                playerRigibody.velocity = Vector3.zero;

            }

            Vector3 rayCastHitPoint = hit.point;
            targetPosition.y = rayCastHitPoint.y;
            inAirTimer = 0;
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }

        if (isGrounded && !isJumping)
        {
            if (playerManager.isInteracting || inputManager.moveAmount > 0)
            {
                transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime / 0.1f);
            }
            else
            {
                transform.position = targetPosition;
            }
        }
    }

    public void HandleJumping()
    {
        if (isGrounded && !playerManager.isInteracting)
        {
            animatorManager.animator.SetBool("isJumping", true);
            animatorManager.PlayTargetAnimation("Jump", true);

            float jumpingVelocity = Mathf.Sqrt(-2 * gravityIntensity * jumpHeight);
            Vector3 playerVelocity = moveDirection;
            playerVelocity.y = jumpingVelocity;
            playerRigibody.velocity = playerVelocity;
        }
    }

    public void HandleDodge()
    {
        if (playerManager.isInteracting)
        {
            return;
        }

        animatorManager.PlayTargetAnimation("Dodge", true, true);
        //TOGLE INVULNERABLE BOOL FOR NO HP DAMAGE DURING ANIMATION
    }
}
