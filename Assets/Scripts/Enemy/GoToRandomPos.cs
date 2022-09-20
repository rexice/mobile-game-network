using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToRandomPos : State
{
    public void Tick(Enemy e)
    {
        do
        {
            var diff = new Vector3Int(Random.Range(-2, 2),
                                      Random.Range(-2, 2),
                                      0);

            e.targetCell = e.map.WorldToCell(e.transform.position) + diff;
        } while (!TileUtil.InsideBounds(e.targetCell, e.minX, e.maxX, e.minY, e.maxY));

        e.Move();
    }

    public State Transition(Enemy e)
    {
        var curCell = e.map.WorldToCell(e.transform.position);
        var playerCell = e.map.WorldToCell(e.player.transform.position);
        var distance = Vector3Int.Distance(playerCell, curCell);

        if (distance <= e.aquisitionRange)
        {
            return new ReactToPlayer();
        }

        return null;
    }
}
