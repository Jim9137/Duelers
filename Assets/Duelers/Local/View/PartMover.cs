using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartMover : MonoBehaviour
{

    private static bool canMove = true;

    [SerializeField]
    private float damping = 5.0f; //Slows our lerp between object and mouse.

    [SerializeField]
    private float viewDistance = 1.5f; //The maximum distance object can go away from it's original position.

    private Vector3 originalPos; //Original position of this object.
    private Vector3 center; //Center between object and mouse.
    private Vector3 cursorPosition; //Hold variable, keeps the position for the cursor as reference for the next frame.

    public static void SetMovement(bool value)
    {

        canMove = value;

    }

    void Start()
    {

        originalPos = transform.position;

    }

    void Update()
    {

        if (!canMove)
        {

            return;

        }

        var mousePos = Input.mousePosition; //Get the position of our mouse on screen in pixels.

        cursorPosition = Camera.main.ScreenToWorldPoint(mousePos); //Now convert that mouse screen position into world position.

        Vector3 offset = cursorPosition - originalPos;

        cursorPosition = originalPos + Vector3.ClampMagnitude(offset, viewDistance);

        center = new Vector3((originalPos.x + cursorPosition.x) / 2, (originalPos.y + cursorPosition.y) / 2, originalPos.z); //Calculate the center between current mouse position

        transform.position = Vector3.Lerp(transform.position, center, Time.deltaTime * damping); //Apply the object movement.

    }
}
