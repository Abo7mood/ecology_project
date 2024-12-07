using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Constructer
   public static PlayerController instance;
    Animator anim;
    Transform cam;
    CharacterController Character;
    public GroundCheck groundcheck;

    #endregion
    #region float&int
    private float horizontal;
    private float vertical;
    public float sprint;
    public float _speed;
    public float _verticalVelocity;
    public float _gravity;
    public float jumpHeight;
    #endregion

    #region boolean
    private bool isSprint;
    #endregion
    #region Other
    private Vector3 move;
    private Vector3 Velocity;

    #endregion
    private void Awake()
    {
        instance = this;
        Character = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
        cam = Camera.main.transform;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMove();
        JumpVoid();
    }
    void PlayerMove()
    {
        move = new Vector3(horizontal, 0, vertical);
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");


        isSprint = Input.GetButton("Sprint");
        sprint = isSprint ? 3f : 1;
        anim.SetFloat("Speed", Mathf.Clamp(move.magnitude, 0, 0.5f) + (isSprint ? .5f : 0));

        if (move.magnitude > 0.1f)
        {
            float angle = Mathf.Atan2(move.x, move.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            transform.rotation = Quaternion.Euler(0, angle, 0);
        }

       
        move = cam.TransformDirection(move);


        move = new Vector3(move.x * _speed * sprint, _verticalVelocity,
      move.z * _speed * sprint);
        Character.Move(move * Time.deltaTime);
        Character.Move(Velocity * Time.deltaTime);

        if (move.magnitude > 1)
            move = move.normalized;
     
    }
    private void JumpVoid()
    {
        //check if the player on the ground or not
        if (groundcheck.isGrounded)
        {
            //jump
            if (Input.GetButtonDown("Jump"))
            {
                Velocity.y = jumpHeight;
                anim.SetBool("IsJumping", true);
            }

        }
        //fall
        else
        {
            Velocity.y -= _gravity * Time.deltaTime;
            anim.SetBool("IsJumping", false);
        }

    }
}
