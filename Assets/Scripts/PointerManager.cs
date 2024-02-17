using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PointerManager : MonoBehaviour
{
    bool buttonDown = false;
    static Vector3 startPosition = Vector3.zero;
    Plane plane = new Plane(Vector3.up, 0);

    public static Vector3 StartPos
    {
        get 
        {
            return startPosition;
        }
    }

    public delegate void StartAction(Vector3 position, Vector2 screenPosition);
    public static event StartAction OnStart;

    public delegate void MoveAction(Vector3 position, Vector2 screenPosition);
    public static event MoveAction OnMove;

    public delegate void StopAction(Vector3 position, Vector2 screenPosition);
    public static event StopAction OnStop;

    public Camera mainCamera;

    public void OnClick(InputAction.CallbackContext context)
    {
        buttonDown = context.ReadValueAsButton();
        if (buttonDown)
        {
            // Button pressed / Touch started
            Vector2 screenPosition = Pointer.current.position.value;
            Ray ray = mainCamera.ScreenPointToRay(screenPosition);

            if (plane.Raycast(ray, out float distance))
            {
                startPosition = ray.GetPoint(distance);
            }

            if(OnStart != null)
            {
                OnStart(startPosition, screenPosition);
            }
            //Debug.Log("Start: "+startPosition);

        }else{

            // Button released / Touch ended
            Vector3 releasedPosition = Vector3.zero;
            Vector2 screenPosition = Pointer.current.position.value;
            Ray ray = mainCamera.ScreenPointToRay(screenPosition);
            
            if (plane.Raycast(ray, out float distance))
            {
                releasedPosition = ray.GetPoint(distance);
            }
            
            if(OnStop != null)
            {
                OnStop(releasedPosition, screenPosition);
            }
            //Debug.Log("End: "+releasedPosition);
        }
    }

    public void OnPoint(InputAction.CallbackContext context)
    {
        if (buttonDown)
        {
            Vector3 currentPosition = Vector3.zero;
            Vector2 screenPosition = context.ReadValue<Vector2>();
            Ray ray = mainCamera.ScreenPointToRay(screenPosition);
            
            if (plane.Raycast(ray, out float distance))
            {
                currentPosition = ray.GetPoint(distance);
            }

            if(OnMove != null)
            {
                OnMove(currentPosition, screenPosition);
            }
            //Debug.Log("Move: "+currentPosition);
        }
    }
}
