using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera cam;
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
        cam.m_Lens.OrthographicSize = Mathf.Lerp(cam.m_Lens.OrthographicSize, zoomSize, zoomSpeed);

    }

    void ZoomOut()
    {
        cam.m_Lens.OrthographicSize = Mathf.Lerp(cam.m_Lens.OrthographicSize, 10, zoomSpeed);
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
