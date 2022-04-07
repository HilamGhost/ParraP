using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInPosition : MonoBehaviour
{
    public CutsceneManager gm;
    public Sprite changeSprite;
    public GameObject ps;

    void StartDestroy()
    {
        Destroy(ps,CutsceneManager.audioSource.clip.length);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.tag == "Player") 
        {
            collision.GetComponent<PlayerController>().isMoving= false;
            collision.GetComponent<PlayerController>().isGrounded = true;
            StartCoroutine(gm.FinishCutscene());
            ps.SetActive(true);
            GetComponent<SpriteRenderer>().sprite = changeSprite;
            StartDestroy();
        }
    }
}
