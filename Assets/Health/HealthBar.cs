using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    private Health healthSystem;


     private void Start()
    {
        healthSystem = GetComponentInParent<Health>();
        Setup(healthSystem);

    }

    private void Setup(Health healthSystem){
        this.healthSystem = healthSystem;
       // healthSystem.OnHealthChanged+=HealthSystem_OnHealthChanged;
    }

    private void HealthSystem_OnHealthChanged(object sender, System.EventArgs e){
        transform.Find("Bar").localScale = new Vector3(healthSystem.GetHealthPercent(),1);
    }
    
    public void Update()
    {
        Setup(healthSystem);
    }

}
