using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detection : MonoBehaviour
{
    [Header("Detections")]
    [SerializeField] private LayerMask layerGround;
    [Range(0,10)]
    [SerializeField] private float groundRadius = 0.2f;
    [SerializeField] private Transform groundPoint;

    public Collider2D ground;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HandleGround();
    }

    private void HandleGround()
    {
        ground = Physics2D.OverlapCircle(groundPoint.position, groundRadius, layerGround);
    }

    private void OnDrawGizmos()
    {
        if(groundPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundPoint.position, groundRadius);
        }
    }
}
