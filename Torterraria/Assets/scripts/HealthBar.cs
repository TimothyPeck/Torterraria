using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarScript : MonoBehaviour
{
    public Image healthBarImage; 
    public Health health; 
    
    public void UpdateHealthBar()
    {
        healthBarImage.fillAmount = Mathf.Clamp(health.health / health.baseHealth, 0, 1f);
    }
}
