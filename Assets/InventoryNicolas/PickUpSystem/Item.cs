using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] public ItemSO InventoryItem;

    [SerializeField]  public int Quantity = 1;

    [SerializeField] private AudioSource audioSource;

    [SerializeField] private float duration = 0.3f;

    private void Start()
    {
        GetComponent<SpriteRenderer>().sprite = InventoryItem.ItemImage;
    }

    public void DestroyItem()
    {
        GetComponent<Collider2D>().enabled = false;
        StartCoroutine(AnimateItemPickup());
    }

    [ServerRpc(RequireOwnership=false)]
    private void DestroyItemServerRpc()
    {
        Destroy(gameObject);

    }

    private IEnumerator AnimateItemPickup()
    {
        audioSource.Play();
        Vector3 startScale = transform.localScale;
        Vector3 endScale = Vector3.zero;
        float currentTime = 0;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            transform.localScale = Vector3.Lerp(startScale, endScale, currentTime / duration);
            yield return null;
        }

        DestroyItemServerRpc();

    }
}
