using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HealthBar1 : MonoBehaviour
{
    [SerializeField] 
    private UnityEngine.UI.Image _healthBarForegroundImage;

    [SerializeField]
    private Health health;


    public void UpdateHealthBar(Health health)
    {
        _healthBarForegroundImage.fillAmount=health.GetHealthPercent();
    }
     private void Update()
    {
       UpdateHealthBar(health);

    }
}
