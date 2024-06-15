using UnityEngine;

public class OpacityControl2D : MonoBehaviour
{
    // Opacity value for when the player is over the object
    public float overOpacity = 0.5f;

    private SpriteRenderer spriteRenderer;
    private bool playerIsOver = false;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        // Check if a SpriteRenderer component is attached
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer component is missing on the object.");
            enabled = false; // Disable the script to prevent further errors
        }
    }

    void Update()
    {
        // Check if the SpriteRenderer component is valid
        if (spriteRenderer != null)
        {
            // Check if the player is over the object
            if (playerIsOver)
            {
                // Set the opacity of the sprite to the specified value
                Color currentColor = spriteRenderer.color;
                currentColor.a = overOpacity;
                spriteRenderer.color = currentColor;
            }
            else
            {
                // Optionally, reset the opacity to full when player is not over
                Color currentColor = spriteRenderer.color;
                currentColor.a = 1f;
                spriteRenderer.color = currentColor;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the player has entered the collider of the object
        if (other.CompareTag("Player"))
        {
            playerIsOver = true;
            Debug.Log("Player entered the area.");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // Check if the player has exited the collider of the object
        if (other.CompareTag("Player"))
        {
            playerIsOver = false;
            Debug.Log("Player exited the area.");
        }
    }
}