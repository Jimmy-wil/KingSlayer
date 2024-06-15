using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectDictionary : MonoBehaviour
{
    public Dictionary<int, GameObject> ItemDictionary;

    [SerializeField] private List<GameObject> list;

    private void Start()
    {
        ItemDictionary = new Dictionary<int, GameObject>();

        for (int i = 0; i < list.Count; i++)
        {
            ItemDictionary.Add(i, list[i]);
        }
            
        foreach (var item in ItemDictionary)
        {
            Debug.Log($"Key: {item.Key}; Value: {item.Value.name}");
        }
    }

}
