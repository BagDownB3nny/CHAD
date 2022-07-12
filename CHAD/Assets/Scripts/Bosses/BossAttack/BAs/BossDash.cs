using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDash : BossAttack
{

    [SerializeField]
    private float dashInterval;
    [SerializeField]
    private int numberOfDashes;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float windUpTime;
    [SerializeField]
    private float windUpSpeed;

    private float timeToNextDash;
    private bool isDashing = false;

    private float timeToDash;
    private bool isWindingUp = false;

    public Vector3 direction;
    public Vector3 targetPosition;

    public float distanceToDash;

    private void Update()
    {
        if (NetworkManager.gameType == GameType.Server) {
            if (timeToNextDash < 0)
            {
                if (!isDashing && !isWindingUp)
                {
                    BossManager.instance.bossAttacker.FindTarget();
                    StartCoroutine(WindUp());
                }
            } else
            {
                timeToNextDash -= Time.deltaTime;
            }
        }
    }

    public override void SetAttack()
    {
        timeToNextDash = 0;
    }

    public override void SetMovement()
    {
        return;
    }

    public IEnumerator WindUp()
    {
        targetPosition = BossManager.instance.GetComponent<BossStatsManager>().target.transform.position;
        direction = targetPosition - BossManager.instance.transform.position;
        isWindingUp = true;
        timeToDash = windUpTime;
        while (timeToDash > 0)
        {
            timeToDash -= Time.deltaTime;
            BossManager.instance.bossMover.Move(-direction.normalized * windUpSpeed * Time.deltaTime);
            yield return null;
        }
        isWindingUp = false;
        StartCoroutine(Dash());
    }

    public IEnumerator Dash()
    {
        isDashing = true;
        distanceToDash = Vector3.Distance(BossManager.instance.transform.position, targetPosition);
        while (distanceToDash > 0)
        {
            ServerSend.Broadcast("Distance to dash: " + distanceToDash);
            BossManager.instance.bossMover.Move(direction.normalized * speed * Time.deltaTime);
            distanceToDash -= speed * Time.deltaTime;
            yield return null;
        }
        isDashing = false;
        numberOfDashes -= 1;
        if (numberOfDashes == 0)
        {
            BossManager.instance.bossAttacker.EndAttack("primary");
            Destroy(gameObject);
        }
        else
        {
            timeToNextDash = dashInterval;
        }
    }
}
