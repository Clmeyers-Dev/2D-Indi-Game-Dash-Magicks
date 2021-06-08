using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootStepPlayer : MonoBehaviour
{
   // Start is called before the first frame update
    public  AudioSource audioSource;
    public  AudioClip currentSound;
    void Start()
    {
     
       
       
    }//Assets/Resources/Audio/Player Sounds/Dash/Dash.mp3

    // Update is called once per frame
    void Update()
    {
        
    }
    public void playSound(string sound){
        AudioClip soundToPlay = Resources.Load<AudioClip>("Audio/Player Sounds/" +sound);
        currentSound = soundToPlay;
        audioSource.PlayOneShot(soundToPlay);
    }
}
