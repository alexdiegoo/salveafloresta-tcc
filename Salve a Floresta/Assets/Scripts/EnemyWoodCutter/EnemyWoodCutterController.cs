using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWoodCutterController : MonoBehaviour
{
    [Header("Dependencies")]
    public EnemyPatrol enemyPatrol;
    public AnimationController animationController;
    public EnemyFollow enemyFollow;

    void Update()
    {
        if(enemyPatrol.StopPatrol())
        {    
            enemyFollow.FollowPlayer();
            animationController.PlayAnimation("Run");
        }
    }
}
