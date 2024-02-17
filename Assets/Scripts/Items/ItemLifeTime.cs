using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Collections.AllocatorManager;

public class ItemLifeTime : MonoBehaviour
{
    // [SerializeField] variables
    [SerializeField] float timer, // timer needs to pas before the blinking starts
                           blinkDuration, // determines how long the items blinks
                           opacity, 
                           interval;

    // private variables
    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(LifeTime());
    }

    private IEnumerator LifeTime() // explode after timer runs out
    {
        yield return new WaitForSeconds(timer);

        Color initialColor = spriteRenderer.color;

        for (int i = 0; i < blinkDuration; i++)
        {
            spriteRenderer.color = new Color(initialColor.r, initialColor.g, initialColor.b, opacity);

            yield return new WaitForSeconds(interval); 
            
            spriteRenderer.color = new Color(initialColor.r, initialColor.g, initialColor.b, 1);

            yield return new WaitForSeconds(interval);
        }

        Destroy(gameObject);
    }
}
