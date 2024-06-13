using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CraftableItemSO : ItemSO
{
    public CraftableItemType type;

    public List<CraftableItemSO> recipes;

    public List<ItemSO> craftedItem;



}
