using UnityEngine;

public class EnemyHunterController : MonoBehaviour
{
    [Header("Dependencies")]
    public GameObject shotPrefab; // Prefab do objeto do tiro
    public Transform shootingPoint; // Referência ao objeto vazio criado anteriormente
    public Transform player;
    public EnemyPatrol enemyPatrol;

    [Header("Shot Settings")]
    public float shotForce = 5f; // Força do tiro
    public float rangeShots = 2f; // Intervalo entre os tiros

    private float timeNextShot;

    private void Start()
    {
        timeNextShot = Time.time;
    }

    private void Update()
    {
        if(enemyPatrol.StopPatrol())
        {    
            if (Time.time > timeNextShot + rangeShots)
            {
                Shoot();
                timeNextShot = Time.time;
            }
        }
       
    }

    private void Shoot()
    {
        GameObject shot = Instantiate(shotPrefab, shootingPoint.position, shootingPoint.rotation);
        Rigidbody2D shotRb = shot.GetComponent<Rigidbody2D>();
        shotRb.velocity = shootingPoint.right * shotForce;
    }
}
