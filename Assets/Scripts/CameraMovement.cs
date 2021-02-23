using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public int speed = 100;

    void Update()
    { 
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); 
        mousePos.z = 0;
        mousePos.x = -mousePos.x * 0.03f;
        mousePos.y = -mousePos.y * 0.03f + 1;
        transform.position = Vector3.Lerp(transform.position, mousePos, speed * Time.deltaTime);
    }
}
