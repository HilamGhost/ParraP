using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLoseChecker : MonoBehaviour
{
    public BossManager boss;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player") 
        {
            boss.Death();
            collision.transform.gameObject.SetActive(false);
        }
    }
}
