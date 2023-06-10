using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CucaController : MonoBehaviour
{
    public CameraController cameraController; // Referência para o script CameraController
    public GameObject player;
    public float activationDistance = 5f; // Definir uma distância de ativação da câmera do chefe

    void Start()
    {
        
    }

    void Update()
    {
         // Verificar a distância entre o jogador e o chefe
        float distance = Vector2.Distance(transform.position, player.transform.position);

        if (distance <= activationDistance)
        {
            cameraController.ActivateBossCamera();
        }
    }
}
