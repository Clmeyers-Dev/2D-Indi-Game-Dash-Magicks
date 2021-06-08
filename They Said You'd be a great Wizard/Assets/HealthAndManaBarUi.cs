using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealthAndManaBarUi : MonoBehaviour
{
    // Start is called before the first frame update
    public Slider ManaBar;
    public PlayerManager playerManager;
    void Start()
    {
        playerManager = FindObjectOfType<PlayerManager>();
        ManaBar.maxValue = playerManager.healthManager.MaxMana;
    }

    // Update is called once per frame
    void Update()
    {
        ManaBar.value = playerManager.healthManager.currentMana;
    }
}
