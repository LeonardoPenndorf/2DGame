using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimForPlayer : MonoBehaviour
{
    // public vairables
    public float direction = 0; // direction of the enemy that shot the projectile

    // [SerializeField] variables
    [SerializeField] float speed, 
                           xBounds, 
                           yBounds, 
                           innacuracy;

    // private variables
    private Transform AimPoint;
    private Rigidbody2D ProjectileRB;
    float xVelocity, yVelocity;
    private Vector3 velocityVector;

    // Start is called before the first frame update
    void Start()
    {
        AimPoint = GameObject.FindGameObjectWithTag("Player").transform.Find("AimPoint");
        ProjectileRB = GetComponent<Rigidbody2D>();

        SetVelocity();
        SetRotation();
    }

    private void SetVelocity()
    {
        velocityVector = AimPoint.position - transform.position;

        xVelocity = EnsureMinMagnitude(velocityVector.x);
        yVelocity = Mathf.Clamp(velocityVector.y, -yBounds, yBounds);

        if ((direction < 0 && xVelocity < 0) || (direction >= 0 && xVelocity > 0)) // ensure the prjectile flies in the same direction the enemy is facing
        {
            xVelocity = -xVelocity;
        }

        if (innacuracy > 0) yVelocity += Random.Range(-innacuracy, innacuracy);

        ProjectileRB.velocity = new Vector2(xVelocity, yVelocity).normalized * speed;
    }

    private void SetRotation()
    {
        float rotation = Mathf.Atan2(-yVelocity, -xVelocity) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rotation);
    }

    private float EnsureMinMagnitude(float xVelocity)
    {
        if (Mathf.Abs(xVelocity) < xBounds)
        {
            return xVelocity < 0 ? -xBounds : xBounds;
        }

        return xVelocity;
    }
}
