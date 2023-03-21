using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    //////////////// VARIABLES ////////////////

    [Header("Movement")]
    public float moveSpeed; //////////////// GENERAL SPEED FOR PLAYER ////////////////////
    public float walkSpeed;
    public float sprintSpeed;
    public float crouchSpeed;

    public float groundDrag;
    public float playerHealth;

    public float jumpForce;
    public float jumpCoolDown;
    public float airMultiplier;
    public float gravity = -9.81f;
    bool readyToJump = true;

    [Header("Sliding")]
    public float slideTimer;
    public float maxSlideTimer;
    public float slideSpeed;

    bool firstSprintButton = false;
    float timeOfFirstButton;
    bool reset = false;
    bool isSliding = false;

    [Header("Crouching")]
    public bool isCrouching = false;
    public float crouchYScale;
    private float startYScale;

    [Header("HeadBob Parameter")]
    [SerializeField] private float walkBobSpeed = 14f;
    [SerializeField] private float walkBobAmount = 0.05f;
    [SerializeField] private float sprintBobSpeed = 18f;
    [SerializeField] private float sprintBobAmount = 0.11f;
    [SerializeField] private float crouchBobSpeed = 8f;
    [SerializeField] private float crouchBobAmount = 0.25f;
    private float DefaultYPOS = 1.585626f;
    float timer;

    ///////// KEYBINDS /////////

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.LeftControl;


    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask WhatIsGround;
    public bool isGrounded;

    [Header("Movement")]

    public Transform orientation; /////////////////// MOVE THE PLAYERS ORIENTATION ///////////////////
    public Transform Player;

    float playerrot; /// Player Rotation
    float horizontalInput; //////////////////// MOVEMENT OVERALL ////////////////
    float verticalInput; ////////////////////// HEIGHT OF JUMP //////////////////

    Vector3 moveDirection; ///////// DIRECTION THE PLAYER MOVES IN

    Rigidbody rb; /// PLAYER RIGID BODY
    public Camera playerCamera;

    public TextMeshProUGUI health;


    /////////////////// MOVEMENT STATE ///////////////////

    bool IsSprinting = false;
    public bool Headbob = true;

    public MovementState state;

    public enum MovementState
    {
        walking,
        sprinting,
        crouching,
        air,
        sliding
    }




    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //playerCamera = GetComponent<Camera>();
        rb.freezeRotation = true;
        startYScale = Player.transform.localScale.y;
    }

    // Update is called once per frame
    void Update()
    {
        //Ground Check
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, WhatIsGround);

        PlayerInput(); /////////// THIS IS TO CONSTANTLY CHECK ON THE PLAYER IF THEY PRESS DOWN A MOVEMENT KEY AND It is called every frame
        SpeedControl();
        StateHandler();
        Sliding();

        moveDirection.y = gravity * Time.deltaTime;

        ////// Handles Drag on the player /////
        if (isGrounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = 0f;
        }

        if(Headbob == true)
        {
            HandleHeadbob();
        }
    }

    private void FixedUpdate() //// THIS METHOD IS BEING USED FOR THE PHYSICS CALCULATIONS SINCE FIXEDUPDATE CAN RUN SEVERAL TIMES In one frame.
    {
        MovePlayer();
        if(isSliding == true)
        {
            SlideMovement();
        }
    }

    /////////////// PLAYER INPUT METHOD ///////////////////////
    void PlayerInput()
    {
        /////////////// MOVEMENT SYSTEM //////////////////
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        ////////////////// JUMP SYSTEM ///////////////////

        if (Input.GetKeyDown(jumpKey) && readyToJump == true && isGrounded == true) /// This checks if the player pressed space or what ever keybind they selected to jump
        {
            Debug.Log("Jumping");
            readyToJump = false; //Changes the bool status to false so the player can't spam 
            Jump(); // Calls the method

            Invoke(nameof(ResetJump), jumpCoolDown); // Calls the function Reset Jump and calls the variable JumpCoolDown.
        }

        if(Input.GetKeyDown(crouchKey) && isCrouching == false && isGrounded == true)
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            isCrouching = true;
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);

        }
        else if(Input.GetKeyDown(crouchKey) && isCrouching == true && isGrounded == true)
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
            isCrouching = false;
            state = MovementState.crouching;
            moveSpeed = walkSpeed;
        }
    }

    private void StateHandler()
    {
        // MODE - SPRINTING
        if (isGrounded && Input.GetKey(sprintKey) && isCrouching == false)
        {
            state = MovementState.sprinting;
            moveSpeed = sprintSpeed;
            IsSprinting = true;
            isSliding = false;
        }
        else if (isCrouching == true && isSliding == false)
        {
            state = MovementState.crouching;
            moveSpeed = crouchSpeed;
            isSliding = false;
        }
        else if(isSliding == true)
        {
            state = MovementState.sliding;
        }
        else if (isGrounded)
        {

            state = MovementState.walking;
            moveSpeed = walkSpeed;
            isSliding = false;

        }
        else if (!isGrounded)
        {
            state = MovementState.air;
            isSliding = false;
        }
        else
        {
            IsSprinting = false;
            isSliding = false;
        }

    }

    /////////// SLIDING MECHANIC /////////////
   
    private void Sliding()
    {

        
        if (Input.GetKeyDown(crouchKey) && firstSprintButton)
        {
            if (Time.time - timeOfFirstButton < 0.75f)
            {
                Debug.Log("DoubleClicked");
                isSliding = true;
            }
            else
            {
                Debug.Log("Too late");
                isSliding = false;
            }

            reset = true;
        }

        if (Input.GetKeyDown(sprintKey) && !firstSprintButton)
        {
            firstSprintButton = true;
            timeOfFirstButton = Time.time;
        }

        if (reset)
        {
            firstSprintButton = false;
            reset = false;
        }
        if(rb.velocity.magnitude == 0)
        {
            reset = true;
            isSliding = false;
            Debug.Log("Reset");
        }

    }

    private void SlideMovement()
    {
        Vector3 inputDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        rb.AddForce(inputDirection.normalized * slideSpeed, ForceMode.Force);

        slideTimer -= Time.deltaTime;

        if(slideTimer <= 0)
        {
            StopSliding();
        }
    }

    private void StopSliding()
    {
        isSliding = false;
        slideTimer = maxSlideTimer;
    }

    private void MovePlayer()
    {
        // CALCULATES THE MOVEMENT DIRECTION

        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        //WHEN ON THE GROUND
        if (isGrounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force); //// THIS ADDS FORCE TO THE RIGID BODY AND MAKES THE PLAYER MOVE IN THE DIRECTION PRESSED IN
        }

        else if (!isGrounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force); // IF THE PLAYER IS IN THE AIR AND NOT TOUCHING THE GROUND
        }
    }

    /////////////// Speed of player ////////////

    private void SpeedControl()
    {
        Vector3 flatvel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        if (flatvel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatvel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        ///////// RESET Y VELOCITY /////////////////
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }

    ///////// HEALTH SYSTEM //////////

    private void Health(int damage)
    {
        playerHealth -= damage;
        health.text = playerHealth + "/100";
        if (playerHealth <= 0)
        {
            SceneManager.LoadScene("DeathScreen");
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "EnemyBullets")
        {
            Health(15);
        }
    }

    /////////////// HEAD BOB ///////////////

    private void HandleHeadbob()
    {

        if (Mathf.Abs(moveDirection.x) > 0.1f || Mathf.Abs(moveDirection.z) > 0.1f && isGrounded)
        {
            timer += Time.deltaTime * (isCrouching ? crouchBobSpeed : IsSprinting ? sprintBobSpeed : walkBobSpeed);
            if (IsSprinting == true && state != MovementState.air && state != MovementState.sliding)
            {
                playerCamera.transform.localPosition = new Vector3(playerCamera.transform.localPosition.x, DefaultYPOS + Mathf.Sin(timer) * (sprintBobAmount), playerCamera.transform.localPosition.z);
            }
            if(state == MovementState.walking && state != MovementState.sliding)
            {
                playerCamera.transform.localPosition = new Vector3(playerCamera.transform.localPosition.x, DefaultYPOS + Mathf.Sin(timer) * (walkBobAmount), playerCamera.transform.localPosition.z);
            }
            if (state == MovementState.crouching && state != MovementState.sliding)
            {
                playerCamera.transform.localPosition = new Vector3(playerCamera.transform.localPosition.x, DefaultYPOS + Mathf.Sin(timer) * (crouchBobAmount), playerCamera.transform.localPosition.z);
            }
        }

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "GuardAttack")
        {
            other.transform.parent.GetComponent<GuardAI>().playerInAttackRange = true;
        }
        if (other.tag == "GuardSight")
        {
            other.transform.parent.GetComponent<GuardAI>().playerInSightRange = true;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "GuardAttack")
        {
            other.transform.parent.GetComponent<GuardAI>().playerInAttackRange = false;
        }
        if (other.tag == "GuardSight")
        {
            other.transform.parent.GetComponent<GuardAI>().playerInSightRange = false;
        }
    }


}
