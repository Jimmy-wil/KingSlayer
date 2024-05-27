using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Playcontrol : MonoBehaviour
{
    // Start is called before the first frame update
   public NetworkVariable<bool> isPhantom = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    private Renderer playerRenderer;
    private Collider playerCollider;

    private void Start()
    {
        playerRenderer = GetComponent<Renderer>();
        playerCollider = GetComponent<Collider>();

        // Subscribe to changes in the isPhantom variable
        isPhantom.OnValueChanged += OnPhantomStatusChanged;

        // Apply the current phantom status at start
        OnPhantomStatusChanged(false, isPhantom.Value);
    }

    private void OnDestroy()
    {
        // Unsubscribe from changes in the isPhantom variable
        isPhantom.OnValueChanged -= OnPhantomStatusChanged;
    }

    // Method to handle changes in the isPhantom variable
    private void OnPhantomStatusChanged(bool oldValue, bool newValue)
    {
        if (newValue)
        {
            ActivatePhantomMode();
        }
        else
        {
            DeactivatePhantomMode();
        }
    }

    // Call this method to activate phantom mode
    public void ActivatePhantomMode()
    {
        // Change appearance to indicate phantom mode (e.g., make the player semi-transparent)
        Color color = playerRenderer.material.color;
        color.a = 0.5f; // Adjust alpha to make semi-transparent
        playerRenderer.material.color = color;

        // Disable the player's ability to interact with the world
        playerCollider.enabled = false;
        // You can add more logic here to disable other interactions (e.g., shooting, picking up items)
    }

    // Call this method to deactivate phantom mode if needed
    public void DeactivatePhantomMode()
    {
        // Restore appearance to normal
        Color color = playerRenderer.material.color;
        color.a = 1f; // Reset alpha to fully opaque
        playerRenderer.material.color = color;

        // Enable the player's ability to interact with the world
        playerCollider.enabled = true;
        // Restore other interactions as needed
    }
}
