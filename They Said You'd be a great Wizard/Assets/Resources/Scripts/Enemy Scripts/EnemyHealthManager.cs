using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthManager : MonoBehaviour
{
    // Start is called before the first frame update
    public float maxHealth;
    public float currentHealth;

    void Start()
    {
    currentHealth = maxHealth;    
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void damageEnemy(float damage){
        currentHealth -=damage;
    }
}
