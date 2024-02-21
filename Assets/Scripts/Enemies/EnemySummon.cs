using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySummon : MonoBehaviour
{
    // [SerializeField] variables
    [SerializeField] float summonXDistance,
                           summonYDistance,
                           maxCooldown,
                           yOffset;
    [SerializeField] GameObject SummonPrefab;
    [SerializeField] string[] animationsArray;

    // private variables
    private Transform AimPoint;
    private Animator EnemyAnimator;
    private EnemyAggro enemyAggro;
    private EnemyMovement enemyMovement;
    private AnimationChecker animationsChecker; // class containing functions to check which animtions are running
    private TogglePauseGame togglePauseGame;
    private float cooldown, xDistance, yDistance;

    // Start is called before the first frame update
    void Start()
    {
        EnemyAnimator = GetComponent<Animator>();
        enemyAggro = GetComponent<EnemyAggro>();
        animationsChecker = GetComponent<AnimationChecker>();
        enemyMovement = GetComponent<EnemyMovement>();
        togglePauseGame = GameObject.FindWithTag("UI").GetComponent<TogglePauseGame>();

        AimPoint = GameObject.FindGameObjectWithTag("Player").transform.Find("AimPoint");
    }

    // Update is called once per frame
    void Update()
    {
        if (togglePauseGame.GetGameIsPaused()) return;

        cooldown -= Time.deltaTime;

        if (cooldown <= 0 && enemyAggro.GetIsAggroed())
        {
            CheckRange();
        }
    }

    private void CheckRange() // check if player is within summon range
    {
        xDistance = Mathf.Abs(AimPoint.position.x - transform.position.x);
        yDistance = Mathf.Abs(AimPoint.position.y - transform.position.y);

        if ((xDistance <= summonXDistance) &&  (yDistance <= summonYDistance) && !animationsChecker.CheckAnimations(animationsArray) && enemyMovement.GetCanAttack())
        {
            EnemyAnimator.SetTrigger("IsSummoning");
            cooldown = maxCooldown;
        }
    }

    private void Summon() // called during the summon animation
    {
        GameObject NewSummon = Instantiate(SummonPrefab) as GameObject;

        NewSummon.transform.position = new Vector3(AimPoint.position.x, transform.position.y + yOffset, 0);
    }
}
