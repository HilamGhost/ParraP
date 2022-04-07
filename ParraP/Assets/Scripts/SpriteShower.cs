using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteShower : MonoBehaviour
{
    GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.gameManager;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager != null) 
        {
            if (gameManager.splitCount == 0)
            {
                GetComponent<SpriteRenderer>().enabled = transform.parent.GetComponentInParent<PropController>().isSecronByID[GetComponentInParent<PropID>().ID];
            }
            if (gameManager.splitCount == 1)
            {
                GetComponent<SpriteRenderer>().enabled = transform.parent.GetComponentInParent<PropController>().isSecronByID[GetComponentInParent<PropID>().ID] && gameManager.isSecronByID[GetComponentInParent<PropID>().ID];
            }
        }
        
    }
}
