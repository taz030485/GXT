using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PointerManager : MonoBehaviour
{
    bool buttonDown = false;
    Vector2 startPosition = Vector2.zero;
    Vector2 currentPosition = Vector2.zero;

    public delegate void StartAction(Vector2 position);
    public static event StartAction OnStart;

    public delegate void MoveAction(Vector2 position);
    public static event MoveAction OnMove;

    public delegate void StopAction(Vector2 position);
    public static event StopAction OnStop;

    void Start()
    {
        
    }

    public void OnClick(InputAction.CallbackContext context)
    {
        buttonDown = context.ReadValueAsButton();
        if (buttonDown)
        {
            // Button pressed / Touch started
            startPosition = Pointer.current.position.value;

            if(OnStart != null)
            {
                OnStart(startPosition);
            }
            Debug.Log("Start: "+startPosition);
        }else{

            // Button released / Touch ended
            Vector2 releasedPosition = Pointer.current.position.value;
            
            if(OnStop != null)
            {
                OnStop(releasedPosition);
            }
            Debug.Log("End: "+releasedPosition);
        }
    }

    public void OnPoint(InputAction.CallbackContext context)
    {
        if (buttonDown)
        {
            currentPosition = context.ReadValue<Vector2>();
            if(OnMove != null)
            {
                OnMove(currentPosition);
            }
            Debug.Log("Move: "+currentPosition);
        }
    }
}
