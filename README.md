# World Explorer

This project provides you with two main scripts. The first is a player controller which was designed with the Oculus Rift DK2 in mind, but should work perfectly with more modern hardware. This player controller will attempt to keep the player locked to the surface of whatever three dimensional shape they are walking on, similar to Super Mario Galaxy. 
##Video demos
For some video demos of it in action, please see these three videos:

[Video demo showing a demo built as part of my Dissertation](https://www.youtube.com/watch?v=SDkdGDm5BXc)

[Video that was created to play in the background of a project fair, explaining how it works - also user testing with the Rift](https://www.youtube.com/watch?v=9096Hdaeh0M)

[Video showcasing the latest build of the software, as well as scene view](https://youtu.be/tIEHX5wSVyE)

##Assets
###A player controller that:
 - Allows for forwards, backwards, and rotation motion
 - Keeps the player locked to the surface of any given geometry, provided that it has a mesh collider (Think Super Mario Galaxy)
 - Is compatible with VR headsets - tested with Oculus Home and Rift DK2.

###A Virtual Reality User Interface controller script that:
 - Implements a gaze pointer type interface, where the user looks at three dimensional objects in order to select them
 - Follows Googles' guidelines on VR interfaces
 - Describes an interface (IObjectAction) that can be implemented in scripts attached to 3D objects either generated during development or pragmatically, to allow for totally new interfaces
 - Implements a non-blocking timer that is used to decide if the user has been looking at an element long enough
 - Displays said timer in three dimensional space to the user wearing the headset.

##Quickstart
To get started

 1. Clone this repository 
 2. Open the 'World Explorer' folder within the repo as a project with Unity. 
 3. Hit Play in Unity and use your standard method of control! Feel free to build on this scene. 

##Implementation details
###Creating a new scene

 1. Create a Capsule, and give it a Rigidbody, disabling Gravity. Add a Camera as a child of this Capsule and ensure its' position is set sensibly; (0,0,0) is usually a good bet.
 2. Attach the genericWalker script provided to this Capsule.
 3. If user interface is desired, Firstly attach CameraLookAtUI to the player camera.
 4. Tag the game object you wish to do something if the user looks at it with either "Major" or "Minor". This distinction allows you to set two separate activation delays for these two different classes of object.
 5. To define the action you want to take place when the game object is looked at, Create a new C# script called whatever you like, and implement IObjectAction, concretely defining activate() - where your action takes place. 
####Example
```
    Example which selects a different scene:

    using UnityEngine;
    using System.Collections;
    using UnityEngine.SceneManagement;

    public class goHome : MonoBehaviour,IObjectAction
    {
	// Use this for initialization
	void Start () {
    }
	
	// Update is called once per frame
	void Update () {
        
    }
    public void activate()
    {
        SceneManager.LoadScene(0);
    }
```
##Licence
```
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
```
##Contact me
Twitter is your best bet, Feel free to leave hate mail at @normankev141.


