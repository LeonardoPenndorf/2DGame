using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FireWaveNavigator : MonoBehaviour
{
    private GameObject fireWave;
    private Collider2D navCollider;
    private int groundLayerMask, platformsLayerMask;

    // Start is called before the first frame update
    void Start()
    {
        fireWave = transform.parent.gameObject;
        navCollider = GetComponent<Collider2D>();
        groundLayerMask = LayerMask.GetMask("Ground");
        platformsLayerMask = LayerMask.GetMask("Platforms");
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        CheckGround();
    }

    private void CheckGround()
    {
        if (!navCollider.IsTouchingLayers(groundLayerMask) && !navCollider.IsTouchingLayers(platformsLayerMask))
        {
            Destroy(fireWave);
        }
    }
}
