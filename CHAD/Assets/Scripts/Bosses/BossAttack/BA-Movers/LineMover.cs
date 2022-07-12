using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineMover : BossAttackMover
{
    public Vector2 start;
    public Vector2 end;
    public bool isTravellingFromStartToEnd = true;


    public override void Move()
    {
        if (isTravellingFromStartToEnd)
        {
            Vector2 dir = (end - start).normalized * speed * Time.deltaTime;
            transform.Translate(dir);
        } else
        {
            Vector2 dir = (start - end).normalized * speed * Time.deltaTime;
            transform.Translate(dir);
        }
        CheckForDirectionChange();
    }

    

    public void SetLine(Vector2 _start, Vector2 _end)
    {
        start = _start;
        end = _end;
    }

    public void CheckForDirectionChange()
    {
        if (isTravellingFromStartToEnd) {
            Vector2 direction = (end - start).normalized;
            if (direction != (end - (Vector2)transform.position).normalized)
            {
                isTravellingFromStartToEnd = false;
            }
        } else
        {
            Vector2 direction = (start - end).normalized;
            if (direction != (start - (Vector2)transform.position).normalized)
            {
                isTravellingFromStartToEnd = true;
            }
        }
    }
}
