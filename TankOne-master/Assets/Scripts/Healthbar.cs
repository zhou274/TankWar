using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Healthbar : MonoBehaviour
{
    public Image foreground;
    private float maxHealth = 100f;

        

   public void Init()
    {
        foreground.fillAmount = 1;
    }
    public void Demage(float health)
    {        
        foreground.fillAmount = health / maxHealth;
    }


}
