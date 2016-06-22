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

//Class which attempts to allow the player to walk along surface meshes. Attaches to root player object - usually a Capsule with mesh renderer disabled. 
//The gameObject must have a rigidbody component.
public class genericWalker : MonoBehaviour
{
    public Material[] skyboxSelector;
    public float moveSpeed;
    void Start()
    {
        moveSpeed = PlayerPrefs.GetFloat("worldSpeed");
    }
    void Update()
    {
        checkForMovement();
        faceFinder(this.gameObject, 2);
    }

    /// <summary>
    /// Checks to see if the user is inputting a movement, calls the movement function. Called on every frame
    /// </summary>
    void checkForMovement()
    {
        float rotation = Input.GetAxis("Horizontal");
        float translation = Input.GetAxis("Vertical");
        moveObject(this.gameObject, rotation, translation);
    }

    /// <summary>
    /// Moves an object.
    /// </summary>
    /// <param name="attachedObject">The object to be moved</param>
    /// <param name="rotation">A float representing the objects movement on the horizontal axis</param>
    /// <param name="translation">A float representing the objects movement on the vertical axis</param>
    void moveObject(GameObject attachedObject, float rotation, float translation)
    {
        attachedObject.transform.Translate(0, 0, (translation* (moveSpeed/3)*Time.deltaTime));
        //transform.Rotate(transform.up * 50 * rotation, Space.Self);
        attachedObject.transform.RotateAround(attachedObject.transform.position, attachedObject.transform.up, 50f * rotation * Time.deltaTime);
    }
    /// <summary>
    /// A function that when called looks for the nearest surface to move to, and and changes the players angle if it finds one.
    /// </summary>
    /// <param name="attachedObject">The object to fire the ray from</param>
    /// <param name="distanceBelowPlayer">Represents distance below that the rays should originate from</param>
    /// <returns>A boolean, if a successful hit was detected, true is returned. False otherwise.</returns>

    bool faceFinder(GameObject attachedObject, float distanceBelowPlayer)
    {
        RaycastHit hit;
        //If a ray fired downwards hits the ground
        if (Physics.Raycast(transform.position, -transform.up, out hit, 2)) {
            //Downward ray hit object, so lock to that.
            Debug.DrawRay(transform.position, -transform.up * 10, Color.green);
            changeAngle(this.gameObject, hit.normal);
            if (hit.distance > 1.07f) {
                GetComponent<Rigidbody>().AddForce(-transform.up * 10);
            }
            else {
                GetComponent<Rigidbody>().velocity = Vector2.zero;
            }
            return (true);
        }
        else {
            Debug.DrawRay(transform.position, -transform.up * 10, Color.red);
            //Downward ray lost tracking, find edge
            if (Physics.Raycast(attachedObject.transform.position - attachedObject.transform.up * distanceBelowPlayer, -attachedObject.transform.forward, out hit, 2)
                     || Physics.Raycast(attachedObject.transform.position - attachedObject.transform.up * distanceBelowPlayer, attachedObject.transform.forward, out hit, 2)) {
                changeAngle(this.gameObject, hit.normal);
                return (true);
            }
            else {
                return (false);
            }
        }
    }
    /// <summary>
    /// Slerps between a vector of the players old rotation, and the rotation supplied.
    /// </summary>
    /// <param name="attachedObject">A GameObject that should be rotated</param>
    /// <param name="newRotation">Vector3 representing new rotation</param>
    void changeAngle(GameObject attachedObject, Vector3 newRotation)
    {
        Vector3 projection = attachedObject.transform.forward - (Vector3.Dot(attachedObject.transform.forward, newRotation) * newRotation);
        attachedObject.transform.rotation = Quaternion.Slerp(attachedObject.transform.rotation, Quaternion.LookRotation(projection, newRotation), Time.deltaTime * (moveSpeed/2f));
    }
}
