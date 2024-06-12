using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ConsumableItemSO : ItemSO, IDestroyableItem, IItemAction
{
    [SerializeField] private List<ModifierData> modifiersData = new List<ModifierData>(); 
    
    public string ActionName => "Consume";
    public AudioClip actionSFX { get; private set; }
    public bool PerfomAction(GameObject character)
    {
        foreach (ModifierData data in modifiersData)
        {
            data.statModifier.AffectCharacter(character, data.value);
        }

        return true;
    }
}

public interface IDestroyableItem
{
    
}

public interface IItemAction
{
    public string ActionName { get; }
    public AudioClip actionSFX { get; }
    bool PerfomAction(GameObject character);

}

[Serializable]
public class ModifierData
{
    public CharacterStatModifierSO statModifier;
    public float value;
}