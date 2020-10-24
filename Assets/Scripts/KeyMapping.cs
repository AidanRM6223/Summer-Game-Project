using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyMapping : MonoBehaviour
{
    public KeyMapping KM;

    public KeyCode jump {get; set;}
    public KeyCode left {get; set;}
    public KeyCode right {get; set;}
    public KeyCode shoot {get; set;}
    public KeyCode boost {get; set;}
    // Start is called before the first frame update
    void Awake()
    {
        //Singleton pattern
        if(KM == null)
        {
            DontDestroyOnLoad(gameObject);
            KM = this;
        }
        else if(KM != this)
        {
            Destroy(gameObject);
        }
        /*Assign each keycode when the game starts.
         * Loads data from PlayerPrefs so if a user quits the game,
         * their bindings are loaded next time. Default values
         * are assigned to each Keycode via the second parameter
         * of the GetString() function
         */
        jump = (KeyCode) System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("jumpKey", "UpArrow"));
        left = (KeyCode) System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("leftKey", "LeftArrow"));
        right = (KeyCode) System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("rightKey", "RightArrow"));
        shoot = (KeyCode) System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("shootKey", "Space"));
        boost = (KeyCode) System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("boostKey", "X"));
 
    }
 
    void Start ()
    {
 
    }
 
    void Update ()
    {
 
    }
}
