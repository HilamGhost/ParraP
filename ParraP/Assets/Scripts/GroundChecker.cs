using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    public bool isGrounded;
    [SerializeField] LayerMask GroundLayerMask;
    [SerializeField] LayerMask PlatformLayerMask;
    [SerializeField] LayerMask PropLayerMask;

    private void OnTriggerStay2D(Collider2D collision)
    {
        isGrounded = collision != null && (((1 << collision.gameObject.layer) & GroundLayerMask) != 0) || collision != null && (((1 << collision.gameObject.layer) & PlatformLayerMask) != 0 || collision != null && (((1 << collision.gameObject.layer) & PropLayerMask) != 0));
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        isGrounded = false;
    }
}
