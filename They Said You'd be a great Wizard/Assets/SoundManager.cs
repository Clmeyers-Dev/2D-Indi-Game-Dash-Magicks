using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static AudioSource audioSource;
    public  AudioClip DashSound;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        DashSound =Resources.Load<AudioClip>("Audio/Player Sounds/Dash");
       
    }//Assets/Resources/Audio/Player Sounds/Dash/Dash.mp3

    // Update is called once per frame
    void Update()
    {
        
    }
    public void playSound(string sound){
        AudioClip soundToPlay = Resources.Load<AudioClip>("Audio/Player Sounds/" +sound);
        audioSource.PlayOneShot(soundToPlay);
    }
}
