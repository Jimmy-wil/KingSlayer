using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustSpriteOpacity : MonoBehaviour
{
    public float opacity = 0.5f; // Modifier cette valeur pour changer l'opacité
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        // Récupérer le composant SpriteRenderer
        spriteRenderer = GetComponent<SpriteRenderer>();

        // S'assurer que le SpriteRenderer existe
        if (spriteRenderer != null)
        {
            // Modifier l'opacité du SpriteRenderer
            Color color = spriteRenderer.color;
            color.a = opacity;
            spriteRenderer.color = color;
        }
        else
        {
            Debug.LogWarning("SpriteRenderer component not found!");
        }
    }
}