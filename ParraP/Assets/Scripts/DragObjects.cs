using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragObjects : MonoBehaviour
{
    [SerializeField] LayerMask GroundLayerMask;
    public bool isGrabbed;
    public Vector3 firstPos;

    float deltax;
    float deltay;
    bool isCollide;
    BoxCollider2D box;

    [Header("CantDrag")]
    public bool cantDrag;
    public bool noGround;

    [Header("Visual Things")]
    public ParticleSystem particleProp;
    public Animator prop_animator;
    public SpriteRenderer propspriteRenderer;
    public AudioClip freezeSound;
    AudioSource audioSource;

    private void Start()
    {
        box = GetComponent<BoxCollider2D>();
       if(transform.parent != null) firstPos = transform.parent.localPosition;
        particleProp = GetComponentInChildren<ParticleSystem>();
        prop_animator = GetComponentInChildren<Animator>();
        propspriteRenderer = prop_animator.transform.GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
    }
    private void Update()
    {
        if (cantDrag)
        {
            propspriteRenderer.color = new Color(0, 255, 255, 255);
            

        }
        if (noGround || cantDrag) 
        {
            GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            GetComponent<Rigidbody2D>().isKinematic = true;
        }

    }
    private void OnMouseDown()
    {
        if (!cantDrag)
        {
            deltax = Camera.main.ScreenToWorldPoint(Input.mousePosition).x - gameObject.transform.position.x;
            deltay = Camera.main.ScreenToWorldPoint(Input.mousePosition).y - gameObject.transform.position.y;
            isGrabbed = true;
            Debug.Log(transform.name);
        }
    }
    private void OnMouseDrag()
    {
        if (isGrabbed)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (!isCollide)
            {
                gameObject.GetComponent<Rigidbody2D>().position = new Vector3(mousePosition.x - deltax, mousePosition.y - deltay, 0);
            }
            else
            {
                isGrabbed = false;
                gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, gameObject.GetComponent<Rigidbody2D>().velocity.y);
            }
            
        }


    }

    private void OnMouseUp()
    {
        isGrabbed = false;
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        isCollide = collision != null && (((1 << collision.gameObject.layer) & GroundLayerMask) != 0);
        Debug.Log(isCollide);
        if (collision.transform.tag == "Transport Zone")
        {
            Debug.Log("OUTSIDE");
            isGrabbed = false;
            gameObject.transform.localPosition = firstPos;
        }
    }

    private void OnBecameInvisible()
    {
        Debug.Log("OUTSIDE");
        isGrabbed = false;
        gameObject.transform.localPosition = firstPos;
    }
    public void ChangeCantDrag() 
    {
        audioSource.PlayOneShot(freezeSound);
        particleProp.Play();
        prop_animator.SetTrigger("Freeze");
        cantDrag = true;
       

    }
    public IEnumerator FixFreeze() 
    {
        cantDrag = false;
        yield return new WaitForSeconds(0.5f);
        cantDrag = true;
    }
}
