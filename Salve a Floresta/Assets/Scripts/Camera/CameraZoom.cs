using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] Rigidbody2D playerRb;

    bool zoomIn;

    [Range(2, 10)]
    [SerializeField] float zoomSize;

    [Range(0.01f, 0.1f)]
    [SerializeField] float zoomSpeed;

    [Range(1, 3)]
    [SerializeField] float waitTime;

    float waitCounter;

    void ZoomIn()
    {
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, zoomSize, zoomSpeed);

    }

    void ZoomOut()
    {
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, 10, zoomSpeed);
    }

    private void LateUpdate()
    {
        if(Mathf.Abs(playerRb.velocity.magnitude) < 4)
        {
            waitCounter += Time.deltaTime;
            if(waitCounter > waitTime)
            {
                zoomIn = true;
            }
        }
        else{
            zoomIn = false;
            waitCounter = 0;
        }

        if(zoomIn)
        {
            ZoomIn();
        }
        else
        {
            ZoomOut();
        }
    }
}
