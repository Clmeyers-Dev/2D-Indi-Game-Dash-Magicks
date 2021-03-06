using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalControl : MonoBehaviour
{
    // Start is called before the first frame update   public static GlobalControl Instance;

    public static GlobalControl Instance;
    public float hp = 100;
   
    //TUTORIAL


    //Copy or our player, if we ever need it game-wide
    public GameObject Player;

    //Transition target is set by TransitionScript component, when interacted with. 
    //This enables us to spawn the player at custom location when next scene is loaded.
    //To use this, first go to your destination scene, and make an empty GameObject and position it
    //where you would like for player to spawn, and use Copy on transform component. 
    //Next, go to your source scene, make empty GameObject, and paste component values. This will position the 
    //game object at some arbitrary position. Assign this game object as transition target to the TransitionScript.
    //Your player will be moved to that location after next scene is loaded.
    public Transform TransitionTarget;



    //Pseudo-singleton concept from Unity dev tutorial video:
    void Awake()
    {
        Player = FindObjectOfType<PlayerManager>().gameObject;
        Application.targetFrameRate = 144;

        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        if (TransitionTarget == null)
            TransitionTarget = gameObject.transform;

    }
}
