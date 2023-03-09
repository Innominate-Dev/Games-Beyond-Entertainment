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

    public float groundDrag;
    public float playerHealth;

    public float jumpForce;
    public float jumpCoolDown;
    public float airMultiplier;
    public float gravity = -9.81f;
    bool readyToJump = true;

    [Header("Crouching")]
    public bool crouching = false;
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
        air
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

        if(Input.GetKeyDown(crouchKey) && crouching == false && isGrounded == true)
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            crouching = true;
        }
        else if(Input.GetKeyDown(crouchKey) && crouching == true && isGrounded == true)
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
            crouching = false;
        }
    }

    private void StateHandler()
    {
        if(Input.GetKeyDown(crouchKey))
        {
            state = MovementState.crouching;
        }

        // MODE - SPRINTING
        if (isGrounded && Input.GetKey(sprintKey))
        {
            state = MovementState.sprinting;
            moveSpeed = sprintSpeed;
            IsSprinting = true;

        }
        else if (isGrounded)
        {
            state = MovementState.walking;
            moveSpeed = walkSpeed;

        }
        else if (!isGrounded)
        {
            state = MovementState.air;
            

        }
        else
        {
            IsSprinting = false;
        }

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

    private void HandleHeadbob()
    {

        if (Mathf.Abs(moveDirection.x) > 0.1f || Mathf.Abs(moveDirection.z) > 0.1f && isGrounded)
        {
            timer += Time.deltaTime * (IsSprinting ? sprintBobSpeed : walkBobSpeed);
            if (IsSprinting == true && state != MovementState.air)
            {
                playerCamera.transform.localPosition = new Vector3(playerCamera.transform.localPosition.x, DefaultYPOS + Mathf.Sin(timer) * (sprintBobAmount), playerCamera.transform.localPosition.z);
            }
            if(state == MovementState.walking)
            {
                playerCamera.transform.localPosition = new Vector3(playerCamera.transform.localPosition.x, DefaultYPOS + Mathf.Sin(timer) * (walkBobAmount), playerCamera.transform.localPosition.z);
            }
        }

    }


}
