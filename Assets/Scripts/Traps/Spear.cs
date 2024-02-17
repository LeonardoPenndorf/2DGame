using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear : MonoBehaviour
{
    // private variables
    private BoxCollider2D spearCollider;

    // Start is called before the first frame update
    void Start()
    {
        spearCollider = transform.Find("HurtBox").GetComponent<BoxCollider2D>();
    }

    private void EnableHurtbox() { spearCollider.enabled = true; }

    private void disableHurtbox() { spearCollider.enabled = false; }
}
