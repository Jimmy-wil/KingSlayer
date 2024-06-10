using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public interface WeaponInterface
{
    public bool IsAttacking { get; set;}
    public Vector2 Pointerposition { get; set; }

    public void Attack() { }

}
