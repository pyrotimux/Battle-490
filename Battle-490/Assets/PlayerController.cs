using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    private Vector3 targetPosition;
    private Vector3 directionVector;
    private Camera mainCamera;

    public float walkMultiplier = 1f;
    public bool defaultIsWalk = false;
    public float smooth = 0.0005F;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        // see if user pressed the mouse down
        if (Input.GetMouseButtonDown(0))
        {
            // We need to actually hit an object
            RaycastHit hit;
            if (!Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out hit, 100))
                return;
            // We need to hit something (with a collider on it)
            if (!hit.transform)
                return;

            // Get input vector from kayboard or analog stick and make it length 1 at most
            targetPosition = hit.point;
            directionVector = hit.point - transform.position;
            directionVector.y = 0;
            if (directionVector.magnitude > 1)
                directionVector = directionVector.normalized;
        }

        if (walkMultiplier != 1)
        {
            if ((Input.GetKey("left shift") || Input.GetKey("right shift") || Input.GetButton("Sneak")) != defaultIsWalk)
            {
                directionVector *= walkMultiplier;
            }
        }

        // Apply direction
        Vector3 diff = targetPosition - transform.position;
        //motor.desiredFacingDirection = diff.normalized;
        //motor.desiredMovementDirection = Vector3.forward;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, smooth);
        if (diff.magnitude < .1f)
        {
            transform.position = targetPosition;
            //motor.desiredMovementDirection = Vector3.zero;
        }
    }


}
