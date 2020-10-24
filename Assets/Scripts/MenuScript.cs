using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MenuScript : MonoBehaviour
{
    public Transform menuPanel;

    Event keyEvent;

    TMPro.TMP_Text buttonText;

    KeyCode newKey;

    public KeyMapping KeyMapping;

    bool waitingForKey;

 

 

    void Start ()

    {
        KeyMapping = GameObject.Find("KeyMapping").GetComponent<KeyMapping>();
        //Assign menuPanel to the Panel object in our Canvas

        //Make sure it's not active when the game starts

        //menuPanel = transform.FindChild("Panel");

        waitingForKey = false;

 

        /*iterate through each child of the panel and check

         * the names of each one. Each if statement will

         * set each button's text component to display

         * the name of the key that is associated

         * with each command. Example: the ForwardKey

         * button will display "W" in the middle of it

         */

        for(int i = 0; i < menuPanel.childCount; i++)

        {

            if(menuPanel.GetChild(i).name == "ShootButton")

                menuPanel.GetChild(i).GetComponentInChildren<TMPro.TMP_Text>().text = KeyMapping.KM.shoot.ToString();

            else if(menuPanel.GetChild(i).name == "BoostButton")

                menuPanel.GetChild(i).GetComponentInChildren<TMPro.TMP_Text>().text = KeyMapping.KM.boost.ToString();

            else if(menuPanel.GetChild(i).name == "LeftButton")

                menuPanel.GetChild(i).GetComponentInChildren<TMPro.TMP_Text>().text = KeyMapping.KM.left.ToString();

            else if(menuPanel.GetChild(i).name == "RightButton")

                menuPanel.GetChild(i).GetComponentInChildren<TMPro.TMP_Text>().text = KeyMapping.KM.right.ToString();

            else if(menuPanel.GetChild(i).name == "JumpButton")

                menuPanel.GetChild(i).GetComponentInChildren<TMPro.TMP_Text>().text = KeyMapping.KM.jump.ToString();

        }

    }

 

 

    void Update ()

    {

        /*//Escape key will open or close the panel

        if(Input.GetKeyDown(KeyCode.Escape) && !menuPanel.gameObject.activeSelf)

            menuPanel.gameObject.SetActive(true);

        else if(Input.GetKeyDown(KeyCode.Escape) && menuPanel.gameObject.activeSelf)

            menuPanel.gameObject.SetActive(false);*/

    }

 

    void OnGUI()

    {

        /*keyEvent dictates what key our user presses

         * bt using Event.current to detect the current

         * event

         */

        keyEvent = Event.current;

 

        //Executes if a button gets pressed and

        //the user presses a key

        if(keyEvent.isKey && waitingForKey)

        {

            newKey = keyEvent.keyCode; //Assigns newKey to the key user presses

            waitingForKey = false;

        }

    }

 

    /*Buttons cannot call on Coroutines via OnClick().

     * Instead, we have it call StartAssignment, which will

     * call a coroutine in this script instead, only if we

     * are not already waiting for a key to be pressed.

     */

    public void StartAssignment(string keyName)

    {

        if(!waitingForKey)

            StartCoroutine(AssignKey(keyName));

    }

 

    //Assigns buttonText to the text component of

    //the button that was pressed

    public void SendText(TMPro.TMP_Text text)

    {

        buttonText = text;

    }

 

    //Used for controlling the flow of our below Coroutine

    IEnumerator WaitForKey()

    {

        while(!keyEvent.isKey)

            yield return null;

    }

 

    /*AssignKey takes a keyName as a parameter. The

     * keyName is checked in a switch statement. Each

     * case assigns the command that keyName represents

     * to the new key that the user presses, which is grabbed

     * in the OnGUI() function, above.

     */

    public IEnumerator AssignKey(string keyName)

    {

        waitingForKey = true;

 

        yield return WaitForKey(); //Executes endlessly until user presses a key

 

        switch(keyName)

        {

        case "shoot":

            KeyMapping.KM.shoot = newKey; //Set shoot to new keycode

            buttonText.text = KeyMapping.KM.shoot.ToString(); //Set button text to new key

            PlayerPrefs.SetString("shootButton", KeyMapping.KM.shoot.ToString()); //save new key to PlayerPrefs

            break;

        case "boost":

            KeyMapping.KM.boost = newKey; //set boost to new keycode

            buttonText.text = KeyMapping.KM.boost.ToString(); //set button text to new key

            PlayerPrefs.SetString("boostButton", KeyMapping.KM.boost.ToString()); //save new key to PlayerPrefs

            break;

        case "left":

            KeyMapping.KM.left = newKey; //set left to new keycode

            buttonText.text = KeyMapping.KM.left.ToString(); //set button text to new key

            PlayerPrefs.SetString("leftButton", KeyMapping.KM.left.ToString()); //save new key to playerprefs

            break;

        case "right":

            KeyMapping.KM.right = newKey; //set right to new keycode

            buttonText.text = KeyMapping.KM.right.ToString(); //set button text to new key

            PlayerPrefs.SetString("rightButton", KeyMapping.KM.right.ToString()); //save new key to playerprefs

            break;

        case "jump":

            KeyMapping.KM.jump = newKey; //set jump to new keycode

            buttonText.text = KeyMapping.KM.jump.ToString(); //set button text to new key

            PlayerPrefs.SetString("jumpButton", KeyMapping.KM.jump.ToString()); //save new key to playerprefs

            break;

        }

 

        yield return null;

    }


}
