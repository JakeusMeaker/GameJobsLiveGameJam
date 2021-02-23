using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public int speed = 100;

    public float range;

    void Update()
    { 
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); 
        mousePos.z = 0;
        mousePos.x = -mousePos.x * range;
        mousePos.y = -mousePos.y * range + 1;
        transform.position = Vector3.Lerp(transform.position, mousePos, speed * Time.deltaTime);
    }
}
