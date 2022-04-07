using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPlayerInside : MonoBehaviour
{
    public LayerMask player;
    public ParticleSystem mirrorParticleSystem;
    public Sprite[] mirrorSprites;
    private void Start()
    {
        if (mirrorParticleSystem == null) CheckParticleSystem();
        mirrorParticleSystem.gameObject.SetActive(false);
        GetComponent<SpriteRenderer>().sprite = mirrorSprites[0];
    }
    private void Update()
    {
        if (mirrorParticleSystem == null) CheckParticleSystem();
            if (GameManager.gameManager.playerIsInside && GameManager.gameManager.canConnect) 
            {
                  GetComponent<SpriteRenderer>().sprite = mirrorSprites[1];
                   mirrorParticleSystem.gameObject.SetActive(true);
            }
        else 
        {
            
            StartCoroutine(ClosePortal());
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
         GameManager.gameManager.playerIsInside = (((1 << collision.gameObject.layer) & player) != 0); 

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        GameManager.gameManager.playerIsInside = false;
    }
    void CheckParticleSystem() 
    {
        mirrorParticleSystem = gameObject.GetComponentInChildren<ParticleSystem>();
    }
    IEnumerator ClosePortal() 
    {
        yield return new WaitForSeconds(1);
        mirrorParticleSystem.gameObject.SetActive(false);
        GetComponent<SpriteRenderer>().sprite = mirrorSprites[0];
    }
}
