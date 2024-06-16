using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomItemScript : MonoBehaviour
{
    public List<Item> items;

    public Item GetItem()
    {
        return items[Random.Range(0, items.Count-1)];
    }
}
