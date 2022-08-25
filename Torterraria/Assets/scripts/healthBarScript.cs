using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image healthBarImage; 
    public Player player; 
    
    public void UpdateHealthBar()
    {
        // Defines the colour based on the current health of the player.
        Color newColor = Color.green;
        if ((float)player.health < (float)player.baseHealth * 0.25f)
        {
            newColor = Color.red;
        }
        else if ((float)player.health < (float)player.baseHealth * 0.66f)
        {
            // Orangey colour.
            newColor = new Color(1f, 0.64f, 0f, 1f);
        }

        // Assigns new colour to the healthBar
        healthBarImage.color = newColor;

        // Assigns new fill value to the healthBar, the value is the percentage of health reamaining.
        healthBarImage.fillAmount = Mathf.Clamp((float)player.health / (float)player.baseHealth, 0, 1f);
    }
}
