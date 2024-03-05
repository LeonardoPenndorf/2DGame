using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSummon : MonoBehaviour
{
    // [SerializeField] variables
    [SerializeField] GameObject SummonPrefab;
    [SerializeField] float maxWaitTime, // after teleporting wait a bit before summoning
                           maxCooldown, // cooldown between summons
                           yOffset; // y offset from the aim point

    // private variables
    private BossTeleport bossTeleport;
    private Transform AimPoint;
    private TogglePauseGame togglePauseGame;
    private bool isPresent;
    private float waitTime,
                  cooldown;

    // Start is called before the first frame update
    void Start()
    {
        bossTeleport = GetComponent<BossTeleport>();
        AimPoint = GameObject.FindGameObjectWithTag("Player").transform.Find("AimPoint");
        togglePauseGame = GameObject.FindWithTag("UI").GetComponent<TogglePauseGame>();
    }

    // Update is called once per frame
    void Update()
    {
        if (togglePauseGame.GetGameIsPaused()) return;

        cooldown -= Time.deltaTime;

        isPresent = bossTeleport.GetIsPresent();

        if (isPresent) waitTime = maxWaitTime;

        if (cooldown <= 0 && !isPresent)
        {
            waitTime -= Time.deltaTime;
            Summon();
        }
    }

    private void Summon()
    {
        if (waitTime > 0) return;

        GameObject NewSummon = Instantiate(SummonPrefab) as GameObject;

        NewSummon.transform.position = new Vector3(AimPoint.position.x, AimPoint.position.y + yOffset, 0);

        cooldown = maxCooldown;
    }
}
