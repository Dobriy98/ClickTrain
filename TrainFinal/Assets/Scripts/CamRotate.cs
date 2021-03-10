using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamRotate : MonoBehaviour
{
    [SerializeField] private float speed = 10f;

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved)
            {
                transform.Rotate(touch.deltaPosition.y * speed, -touch.deltaPosition.x * speed, 0, Space.World);
            }
        }
    }
}
