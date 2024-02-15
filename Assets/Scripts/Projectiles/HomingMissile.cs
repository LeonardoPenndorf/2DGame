using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEditor;
using UnityEngine;

public class HomingMissile : MonoBehaviour
{
    // [SerializeField] variables
    [SerializeField] float speed, rotateSpeed;

    // private variables
    private Transform aimPoint;
    private Rigidbody2D rb;
    private Vector2 direction;
    private float rotateAmount; 

    // Start is called before the first frame update
    void Start()
    {
        aimPoint = GameObject.FindWithTag("Player").transform.Find("AimPoint");
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Rotate();

        rb.velocity = transform.right * speed;
    }

    private void Rotate()
    {
        direction = (Vector2)aimPoint.position - rb.position;

        direction.Normalize();

        rotateAmount = Vector3.Cross(direction, transform.right).z;

        rb.angularVelocity = -rotateAmount * rotateSpeed;
    }
}
