using UnityEngine;

public class EnemyHunterController : MonoBehaviour
{
    [Header("Dependencies")]
    public GameObject shotPrefab; // Prefab do objeto do tiro
    public Transform shootingPoint; // Referência ao objeto vazio criado anteriormente
    public Transform player;
    public EnemyPatrol enemyPatrol;
    public AnimationController animationController;

    [Header("Audio")]
    [SerializeField] private EnemiesSoundController enemySound = null;

    [Header("Shot Settings")]
    public float shotForce = 5f; // Força do tiro
    public float rangeShots = 2f; // Intervalo entre os tiros

    private float timeNextShot;

    private bool firstFrame = false;

    private void Start()
    {
        timeNextShot = Time.time;

        firstFrame = true;
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

        if(firstFrame)
        {
            enemySound = GameObject.FindObjectOfType<EnemiesSoundController>();
            firstFrame = false;
        }
    }

    public void Shoot()
    {      
        enemySound.PlayShot();
        animationController.PlayAnimation("Shooting");
        GameObject shot = Instantiate(shotPrefab, shootingPoint.position, shootingPoint.rotation);
        Rigidbody2D shotRb = shot.GetComponent<Rigidbody2D>();
        shotRb.velocity = shootingPoint.right * shotForce;

    }

}
