using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Damageable : MonoBehaviour {

    public float standardHealth = 100f;
    public float currentHealth = 100f;
       
    public virtual void ReceiveDamage(float damageTaken)
    {
        currentHealth -= damageTaken;       
    }


}
