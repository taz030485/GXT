using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public void PointAt(Vector3 position, Vector3 target)
    {   
        transform.position = position;
        transform.LookAt(target,Vector3.up);
    }
}
