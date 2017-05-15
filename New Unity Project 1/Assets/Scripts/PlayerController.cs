using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    [SerializeField]
    private float playerSpeed = 10;

    [SerializeField]
    private float climbSpeed;

    private bool facingRight;

    private bool attackInput;

    private Rigidbody2D myrigidbody;

    private Animator animator;
   
    #region ground&jump
    [SerializeField]
    private Transform[] groundPoints;

    [SerializeField]
    private float groundRadius;
    [SerializeField]
    private LayerMask whatIsGround;

    private bool isGrounded;

    private bool jumpInput;
    [SerializeField]
    private float jumpForce;

    bool jumpAttackNotFinish;
    #endregion
   
    #region StateInit

    enum State {Attack,Climb,Dead,Glide,Idle,Jump,Jump_Attack,Jump_Throw,Run,Slide,Throw}

    private State playerState;

    int attackHash = Animator.StringToHash("Base Layer.Attack");

    int climbHash = Animator.StringToHash("Base Layer.Climb");
   

    int deadHash = Animator.StringToHash("Base Layer.Dead");
    int glideHash = Animator.StringToHash("Base Layer.Glide");
    int idleHash = Animator.StringToHash("Base Layer.Idle");
    int jumpHash = Animator.StringToHash("Base Layer.Jump");
    int jump_AttackHash = Animator.StringToHash("Base Layer.JumpAttack");
    int jump_ThrowHash = Animator.StringToHash("Base Layer.JumpThrow");
    int runHash = Animator.StringToHash("Base Layer.Run");
    int slideHash = Animator.StringToHash("Base Layer.Slide");
    int throwHash = Animator.StringToHash("Base Layer.Throw");

    #endregion

    //ladder
    private bool climbUpInput;

    private bool climbDownInput;

    private bool ladderTrigger;

    private float ladderTriggerXposition;

    public void LadderTriggerSetting(bool trigger)
    {
        ladderTrigger = trigger;
    }
    public void LadderTriggerXpositionSetting(float x)
    {
        ladderTriggerXposition = x;
    }

    void Start () {
        myrigidbody = GetComponent<Rigidbody2D>();
        facingRight = true;

        animator = GetComponent<Animator>();
	}
	
    void Update()
    {
        receiveInput();
       // Debug.Log(animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
            //.IsName("Base Layer.Climb"));
    }

	void FixedUpdate () {
        PlayerStateSetting();

        #region Climb gravity
        if (playerState == State.Climb)
        {
            myrigidbody.gravityScale = 0;
        }
        else
        {
            myrigidbody.gravityScale = 1;
        }
        #endregion

        isGrounded = IsGrounded();
        //is in Air
        if (isGrounded)
        {
            animator.SetBool("jump", false);
            animator.SetBool("jumpAttack", false);
        }
        //anim val setting
       
        animator.SetBool("attack", false);
        
        //animator.SetBool("climb", false);

        bool controllable;

        controllable = StateControllable();

        float horizontal = Input.GetAxis("Horizontal");

        float veltical = Input.GetAxis("Vertical");

        HandleMovement(horizontal,controllable);

        Flip(horizontal,controllable);

        Attacks();

        Climb(veltical);
       
        ResetValues();

    }
    // MAY cause problem  NEED UPGRADE
    private void Climb(float veltical)
    {
       // Debug.Log("isGrounded"+ isGrounded+"veltical"+veltical);
        if (isGrounded && ladderTrigger && climbUpInput&&playerState==State.Idle)
        {
            myrigidbody.position = new Vector3(ladderTriggerXposition, myrigidbody.position.y);

            animator.SetBool("climb", true);

            //isGrounded = false;
            
            myrigidbody.velocity = new Vector2(myrigidbody.velocity.x, veltical * climbSpeed);
        }
        if (climbUpInput && !climbDownInput)
        {
        if (!isGrounded && playerState == State.Climb )
        {
            //anim speed setting
            animator.speed = 1;
                //float animTime = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        //        Debug.Log("up");
            myrigidbody.velocity = new Vector2(myrigidbody.velocity.x, veltical * climbSpeed);
            return;

        }
        }
        if (!climbUpInput && climbDownInput)
        {
           // Debug.Log("down"+ "isGrounded" + isGrounded+"state:"+ playerState.ToString());
            if (!isGrounded&& playerState == State.Climb )
        {
            //anim speed setting
            animator.speed = 1;
                //float animTime = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
                //if (animTime > 0.9 && animTime < 1.0)
                //{
                //    //may not work
                //    animator.Play(climbHash);
                //}
               // Debug.Log("down");
                myrigidbody.velocity = new Vector2(myrigidbody.velocity.x, veltical * climbSpeed);
            return;
        }
       }
        if (!isGrounded && playerState == State.Climb && !climbUpInput && !climbDownInput)
        {
            animator.speed = 0;
            myrigidbody.velocity = new Vector2(myrigidbody.velocity.x, 0);
         
            return;
        }
        if(isGrounded&& playerState == State.Climb)
        {
            animator.SetBool("climb", false);
        }

        //if(isGrounded&& playerState == State.Climb)
        //{
        //    animator.SetBool("climb", false);
        //}





        //     transform.position.Set(ladderTriggerXposition,transform.position.y, transform.position.z);

    }
    private void PlayerStateSetting()
    {
        int animState = animator.GetCurrentAnimatorStateInfo(0).fullPathHash;
      
        if(animState == attackHash)
        {
            playerState = State.Attack;
        }
        else if (animState == climbHash)
        {
            playerState = State.Climb;
        }
        
        else if (animState == deadHash)
        {
            playerState = State.Dead;
        }
        else if (animState == glideHash)
        {
            playerState = State.Glide;
        }
        else if (animState == idleHash)
        {
            playerState = State.Idle;
        }
        else if (animState == jumpHash)
        {
            playerState = State.Jump;
        }
        else if (animState == jump_AttackHash)
        {
            playerState = State.Jump_Attack;
        }
        else if (animState == jump_ThrowHash)
        {
            playerState = State.Jump_Throw;
        }
        else if (animState == runHash)
        {
            playerState = State.Run;
        }
        else if (animState == slideHash)
        {
            playerState = State.Slide;
        }
        else if (animState == throwHash)
        {
            playerState = State.Throw;
        }


    }

    
    private bool StateControllable()
    {
        bool controllable = false;
        if (playerState==State.Idle||playerState==State.Run)
        {
            controllable = true;
        }else
        {
            controllable = false;
        }
        return controllable;
    }

    private void HandleMovement(float horizontal,bool controllable)
    {
        //not in climb
        if (!controllable)
        {
            if (Mathf.Sign(myrigidbody.velocity.x)!=Mathf.Sign(horizontal))
            {
                //if user want slow
            myrigidbody.velocity = new Vector2(myrigidbody.velocity.x*0.97f, myrigidbody.velocity.y);
            }
            
            if(myrigidbody.velocity.x == 0&&(playerState==State.Jump|| playerState == State.Jump_Attack|| playerState == State.Jump_Throw))
            {
                myrigidbody.velocity = new Vector2(horizontal * playerSpeed, myrigidbody.velocity.y);
            }
            return;
        }
        
            myrigidbody.velocity = new Vector2(horizontal* playerSpeed, myrigidbody.velocity.y);

        animator.SetFloat("speed", Mathf.Abs(horizontal));
        
        //jump
        if (isGrounded && jumpInput)
        {
            isGrounded = false;
            myrigidbody.AddForce(new Vector2(0, jumpForce));
            animator.SetBool("jump", true);
        }

    }
    private void Attacks()
    {
        if (attackInput&&StateControllable())
        {
            animator.SetBool("attack",true);
            //need controllable false
           // float velY = myrigidbody.velocity.y;

            myrigidbody.velocity = new Vector2(0, myrigidbody.velocity.y);
        }else if (attackInput && playerState==State.Jump)
        {
            animator.SetBool("jumpAttack", true);
        }

        
    }

    private void receiveInput()
    {

        if (Input.GetKey(KeyCode.J)) 
        {
            attackInput = true;
        }
        if (Input.GetKey(KeyCode.K))
        {
            jumpInput = true;
        }
        if (Input.GetKey(KeyCode.W))
        {
            climbUpInput = true;
        }
        if (Input.GetKey(KeyCode.S))
        {
            climbDownInput = true;
        }

    }

    private void Flip(float horizontal,bool controllable)
    {
        if (horizontal > 0 & !facingRight || horizontal < 0 && facingRight)
        {
            if (controllable&&isGrounded)
            {
                facingRight = !facingRight;

                Vector3 theScale = transform.localScale;

                theScale.x *= -1;

                transform.localScale = theScale;
            }
        }
    }
    private void ResetValues()
    {
        attackInput = false;
        jumpInput = false;
        climbUpInput = false;
        climbDownInput = false;
    }

    private bool IsGrounded()
    {
        if(myrigidbody.velocity.y <= 0)
        {
            foreach (Transform point in groundPoints)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(point.position, groundRadius, whatIsGround);

                for(int i = 0; i < colliders.Length; i++)
                {
                    if (colliders[i].gameObject != gameObject)
                    {
                        return true;
                    }
                }

            }
        }
        return false;
    }

}
