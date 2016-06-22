/*
    Copyright (c) 2016 Kevin Norman.

    Permission is hereby granted, free of charge, to any person obtaining
    a copy of this software and associated documentation files (the "Software"),
    to deal in the Software without restriction, including without limitation
    the rights to use, copy, modify, merge, publish, distribute, sublicense,
    and/or sell copies of the Software, and to permit persons to whom the Software
    is furnished to do so, subject to the following conditions:

    The above copyright notice and this permission notice shall be included in
    all copies or substantial portions of the Software.

    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
    EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
    OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
    IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
    CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
    TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE
    OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

using UnityEngine;
using System;
using UnityEngine.SceneManagement;
/// <summary>
/// Interface that forces implementers to implement activate. Used to ensure that all UI elements implement the activate method, so we can call it.
/// </summary>
public interface IObjectAction
{
    void activate();
}
/// <summary>
/// Class that handles detecting where the user is looking and actioning UI elements
/// </summary>
public class cameraLookAtUI : MonoBehaviour
{
    //Game objects to watch for look selections, settable from Unity inspector.
    Boolean LookingAtObject = false;
    public float majorUIDelay; /*!< The delay before a UI element in minorUIElem activates */
    public float minorUIDelay; /*!< The delay before a UI element in minorUIElem activates */
    public Color selectedWorldColor; /*!<Unity Color to set selected world to*/
    public Color unselectedWorldColor; /*!<Unity Color to set unselected world to*/
    GameObject objectBeingLookedAt; /*!< Stores a reference to the GameObject the player is looking at*/
    public GameObject countdownTimerDisplay; /*!< stores a reference to the text object that will be updated with the visual timer display, settable from Unity inspector*/

    public float worldScale; /*!< Default world scale multiplier*/
    float acceleration = 1; /*!< The starting acceleration for scrolling through countup/countdown ui elements*/
    SecondsTimer uiTimer;
    void Start()
    {
        //Grab a reference to the countdownTimer object
        countdownTimerDisplay = GameObject.FindGameObjectWithTag("countdown");
        toggleObjectVisibility(countdownTimerDisplay, false);
        uiTimer = new SecondsTimer(3000);


    }

    void Update()
    {
        RaycastHit hit;
        //if a ray fired forward in relation to the camera hits something
        if (Physics.Raycast(transform.position, transform.forward, out hit, 20)) {
            Debug.DrawRay(transform.position, transform.forward * 5, Color.green);
            objectBeingLookedAt = hit.collider.gameObject;
            //If we've just started looking at a major UI element
            if (objectBeingLookedAt.tag.Equals("Major") && !LookingAtObject) {
                print("major UI");
                if(majorUIDelay==0) {
                    uiTimer.setDelay(3000);
                } else {
                    uiTimer.setDelay(majorUIDelay);
                }
                uiTimer.resetTimer();
                selectObject(objectBeingLookedAt);
                LookingAtObject = true;
            }
            //If we've just started looking at a minor UI element
            else if (objectBeingLookedAt.tag.Equals("Minor") && !LookingAtObject) {
                print("minor UI");
                if (minorUIDelay == 0) {
                    uiTimer.setDelay(1000);
                }
                else {
                    uiTimer.setDelay(minorUIDelay);
                }
                uiTimer.resetTimer();
                selectObject(objectBeingLookedAt);
                LookingAtObject = true;
            } 
            //If we're looking at a UI element and the timer finishes
            if (uiTimer.finished() && LookingAtObject) {
                activateObject(objectBeingLookedAt);
                if(objectBeingLookedAt.tag == "Major")
                {
                    //save preferences - An example of how to save data between scenes. Feel free to delete this entire if statement if this functionality is not required.
                    savePreferences.saveData(GameObject.FindGameObjectWithTag("MoveSpeed"));
                }
            //If we're looking at a UI element
            } else if(LookingAtObject) {
                placeTimer(countdownTimerDisplay, hit, 0.005F, uiTimer);
            }
        }//if it doesn't hit anything
        else {
            //if we were looking at something previously
            if(LookingAtObject) {
                //user stopped looking at something interesting
                toggleObjectVisibility(countdownTimerDisplay, false);
                GameObject[] majorUI = GameObject.FindGameObjectsWithTag("Major");
                GameObject[] minorUI = GameObject.FindGameObjectsWithTag("Minor");
                unselectAll(majorUI);
                unselectAll(minorUI);
                acceleration = 1;
                LookingAtObject = false;
            }

        }
    }
    /// <summary>
    /// Method which accepts the timer object and the RaycastHit object, and scale, and sets position to the hit point, and sets scale and text as desired.
    /// </summary>
    /// <param name="timer">GameObject which stores the text mesh that will be moved</param>
    /// <param name="hit">The position to place the timer (ie, where the ray hit) </param>
    /// <param name="scale">The size of the timer</param>
    /// <param name="uiTimer">The timer whose value should display</param>
    /// 
    void placeTimer(GameObject timer, RaycastHit hit, float scale, SecondsTimer uiTimer)
    {
        float text = Mathf.RoundToInt(uiTimer.timeSinceStarted() / 1000);
        toggleObjectVisibility(timer, true);
        timer.transform.position = hit.collider.transform.position;
        timer.transform.rotation = Quaternion.LookRotation(timer.transform.position - transform.position);
        timer.transform.localScale = new Vector3(scale * text, scale * text, scale * text);
        if (timer.GetComponent<TextMesh>().text != text.ToString()) {
            timer.GetComponent<TextMesh>().text = text.ToString();
        }
    }
    /// <summary>
    /// Method that when given a game object, attempts to locate a script on it that implements IObjectAction, and calls its activate method
    /// </summary>
    /// <param name="uiElement">The UI element to activate</param>
    void activateObject(GameObject uiElement)
    {
        try {
            print("Activating object " + uiElement.name);
            IObjectAction objectScript = uiElement.GetComponent(typeof(IObjectAction)) as IObjectAction;
            objectScript.activate();
        } catch(NullReferenceException e) {
            Debug.LogWarning(uiElement.name + " must have a script that implements IObjectAction");
        }
    }

    /// <summary>
    /// Toggles an objects visibility by enabling or disabling its' renderer
    /// </summary>
    /// <param name="objToHide">GameObject to effect</param>
    /// <param name="show">True to show, false to hide</param>
    void toggleObjectVisibility(GameObject objToHide, Boolean show)
    {
        if(show) {
            objToHide.GetComponent<Renderer>().enabled = true;
        } else {
            objToHide.GetComponent<Renderer>().enabled = false;
        }
        
    }



/// <summary>
/// Unselects a list of game objects by setting the world colour back to the unselected colour.
/// </summary>
/// <param name="gameObjectList">List of game objects</param>
    void unselectAll(GameObject[] gameObjectList)
    {
        foreach (GameObject e in gameObjectList) {
            e.GetComponent<Renderer>().material.SetColor("_Color", unselectedWorldColor);
            print("unselecting: " + e.name);
        }
    }
/// <summary>
/// Highlights a given gameobject with the selected world colour
/// </summary>
/// <param name="gameObjectSelected">The object to highlight</param>
    void selectObject(GameObject gameObjectSelected)
    {
        gameObjectSelected.GetComponent<Renderer>().material.SetColor("_Color", selectedWorldColor);
    }
}
