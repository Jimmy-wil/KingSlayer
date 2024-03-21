using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;


[CreateAssetMenu(menuName = "Scriptable Object/item")]
public class Itemscript :   ScriptableObject
{
    [Header("Only gameplay")]
    public TileBase tile;
   
    public ItemType type;
    public ActionType actionType;
    public Vector2Int range = new Vector2Int(5, 4);


    [Header("only UI")] public bool stackable = true;
    
    [Header("Both")]
    public Sprite image;

}

public enum ItemType
{
    Ingredients,
    Armor,
    MeleeWeapon,
    Consumable,
    RangedWeapon
}

public enum ActionType
{
    Attack,
    Consume,
    Craft,
}