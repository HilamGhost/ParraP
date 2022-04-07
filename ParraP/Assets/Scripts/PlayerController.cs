using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float JumpSpeed;
    Rigidbody2D rb;
    [SerializeField] GroundChecker checker;
    public bool isGrounded;
    public  bool isMoving;
    ///
    float GroundRemember = 0f;
    float GroundRememberTimer = 0.2f;
    float JumpRemember = 0f;
    float JumpRememberTimer = 0.2f;
    ///VISUAL THINGS
    Animator playerAnimator;
    AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        checker = GetComponentInChildren<GroundChecker>();
        if(gameObject.GetComponentInParent<PositionFinder>() != null) gameObject.GetComponentInParent<PositionFinder>().AddPlayerController(gameObject.transform);
        playerAnimator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }
    void Update()
    {
        isGrounded = checker.isGrounded;
        #region Inputlar
         if (Input.GetKeyDown(KeyCode.Space)) JumpRemember = JumpRememberTimer;
         if (isGrounded) GroundRemember = GroundRememberTimer;
        #endregion
        #region Animator Vars
        playerAnimator.SetBool("isMoving", isMoving);
        playerAnimator.SetBool("isGrounded", isGrounded);
        #endregion
        #region Timers
        GroundRemember -= Time.deltaTime;
        JumpRemember -= Time.deltaTime;
        #endregion
    }


    void FixedUpdate()
    {
        Move();
        if (GroundRemember > 0 && JumpRemember > 0)
        {
            Jump();
        }
    }
    void Move() 
    {
        float h = Input.GetAxisRaw("Horizontal") * speed;
        rb.velocity = new Vector3 (h,rb.velocity.y,0);
        if (rb.velocity.x != 0) isMoving = true;
        else isMoving = false;


        ///FLIP
        if (h > 0) GetComponent<SpriteRenderer>().flipX = true;
        if (h < 0) GetComponent<SpriteRenderer>().flipX = false;
    }
   
    void Jump() 
    {       
            rb.AddForce(transform.up*JumpSpeed,ForceMode2D.Impulse);
            GroundRemember = 0;
            JumpRemember = 0;
    }
    public void PlaySound(AudioClip audioClip) 
    {
        audioSource.PlayOneShot(audioClip);
    }
    
       
    
    
}
