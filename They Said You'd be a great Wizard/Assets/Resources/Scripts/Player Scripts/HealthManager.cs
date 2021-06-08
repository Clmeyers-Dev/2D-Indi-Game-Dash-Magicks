using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    public float MaxHealth;
    public float currentHealth;
    public float MaxMana;
    public float currentMana;
    public float ManaRegenSpeed;
    public float regenTimerMax;
    public float currentRegenTimer;
    public bool IFrame;
    public float IframeLength;
    public float currentIFrameTime;

    // Start is called before the first frame update
    void Start()
    {
        currentMana = MaxMana;
    }

    // Update is called once per frame
    void Update()
    {
        if(currentRegenTimer>0){
            currentRegenTimer -= Time.deltaTime;
        }else{
        regenerateMana();
        }
    }
    public void useMana(float cost){
        currentMana -= cost;
        currentRegenTimer = regenTimerMax;
    }
    public void regenerateMana(){
        if(currentMana<MaxMana){
            currentMana+=ManaRegenSpeed;
        }
    }
    public void updateMaxStats(){
        
    }
}
