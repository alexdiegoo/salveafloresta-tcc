using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    public GameObject bossCameraPosition; // Referência para o GameObject que define a posição da câmera durante a luta contra o chefe
    private CinemachineVirtualCamera virtualCamera; // Referência para o componente CinemachineVirtualCamera

    private void Start()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    public void ActivateBossCamera()
    {
        // Desativa o componente CinemachineVirtualCamera para parar de seguir o jogador
        virtualCamera.enabled = false;

        // Posiciona a câmera no local do chefe
        transform.position = bossCameraPosition.transform.position;
        transform.rotation = bossCameraPosition.transform.rotation;
    }

    public void DeactivateBossCamera()
    {
        // Reativa o componente CinemachineVirtualCamera para voltar a seguir o jogador
        virtualCamera.enabled = true;
    }
}
