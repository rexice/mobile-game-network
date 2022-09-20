using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ServerControlled : MonoBehaviour, HasPosition
{
    public Tilemap map;

    public void SetPos(Vector3 pos)
    {
        transform.position = map.WorldToCell(pos);
    }
}
