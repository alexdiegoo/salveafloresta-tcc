using UnityEngine;

public class EnemyHunterController : MonoBehaviour
{
    [SerializeField] GameObject tiroPrefab; // Prefab do objeto do tiro
    [SerializeField] Transform shootingPoint; // Referência ao objeto vazio criado anteriormente
    [SerializeField] float shotForce = 5f; // Força do tiro
    [SerializeField] float rangeShots = 2f; // Intervalo entre os tiros

    private float tempoUltimoTiro;

    private void Start()
    {
        tempoUltimoTiro = Time.time;
    }

    private void Update()
    {
        if (Time.time > tempoUltimoTiro + rangeShots)
        {
            Shoot();
            tempoUltimoTiro = Time.time;
        }
    }

    private void Shoot()
    {
        GameObject shot = Instantiate(tiroPrefab, shootingPoint.position, shootingPoint.rotation);
        Rigidbody2D shotRb = shot.GetComponent<Rigidbody2D>();
        shotRb.velocity = shootingPoint.right * shotForce;
    }
}
