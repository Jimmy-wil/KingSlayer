using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class HealthBar : NetworkBehaviour
{
    [SerializeField]
    private GameObject HealthBarGameObject;
    private Health healthSystem;


    private void Start()
    {
        healthSystem = GetComponentInParent<Health>();
       
        Setup(healthSystem);

    }

    private void Setup(Health healthSystem){
        this.healthSystem = healthSystem;
        healthSystem.OnHealthChanged += HealthSystemOnHealthChanged;
    }

    private void HealthSystemOnHealthChanged(object sender, System.EventArgs e){
         transform.Find("Bar").localScale = new Vector3(healthSystem.GetHealthPercent(), 1);
    }

}
