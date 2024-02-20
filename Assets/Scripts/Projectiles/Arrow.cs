using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    // [SerializeField] variables
    [SerializeField] int damage;
    [SerializeField] float timer, fadeDuration;

    // private variables
    private Collider2D ArrowCollider;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        ArrowCollider = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerHealth>().TakeDamage(damage, transform);
            Destroy(gameObject);
        }
        else if (ArrowCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))) // when arrows collide with the wall they get stuck and fade away after a short delay
        {
            rb.velocity = Vector3.zero;
            ArrowCollider.enabled = false;

            StartCoroutine(FadeAway());
        }
    }

    private IEnumerator FadeAway()
    {
        yield return new WaitForSeconds(timer);

        float currentTime = 0;

        Color initialColor = spriteRenderer.color;

        while (currentTime < fadeDuration)
        {
            // Calculate the proportion of the fade based on the elapsed time
            float alpha = Mathf.Lerp(1f, 0f, currentTime / fadeDuration);

            spriteRenderer.color = new Color(initialColor.r, initialColor.g, initialColor.b, alpha);

            yield return null;
            currentTime += Time.deltaTime;
        }

        Destroy(gameObject);
    }
}
