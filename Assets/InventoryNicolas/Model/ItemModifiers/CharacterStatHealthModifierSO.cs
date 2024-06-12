using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
[CreateAssetMenu]
public class CharacterStatHealthModifierSO : CharacterStatModifierSO
{
    public override void AffectCharacter(GameObject character, float val)
    {
        Health health = character.GetComponent<Health>();
        if(health == null) return;
        NetworkObject networkObject = character.GetComponent<NetworkObject>();

        health.AddHpServerRpc((int)val);

            
    }
}
