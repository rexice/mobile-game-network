using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileUtil : MonoBehaviour
{
    public static bool InsideBounds(Vector3Int cell, int minX, int maxX, int minY, int maxY)
    {
        return cell.x <= maxX
            && cell.x >= minX
            && cell.y <= maxY
            && cell.y >= minY;
    }
}
