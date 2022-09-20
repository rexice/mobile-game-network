using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Pathfinding : MonoBehaviour
{
    public Vector3Int targetCell;
    public Vector3Int currentCell;
    public Tilemap map;
    public Tilemap vis;

    public RuleTile targetTile;
    public RuleTile pathFindTile;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    Vector3Int[] GetNeighbours(Vector3Int tile)
    {
        Vector3Int[] l = {
            tile + new Vector3Int(0, 1, 0),
            tile + new Vector3Int(1, 0, 0),
            tile + new Vector3Int(0, -1, 0),
            tile + new Vector3Int(-1, 0, 0)
        };

        return l;
    }

    bool InnerFindPath(Vector3Int from, Vector3Int to, List<Vector3Int> foundTiles, int depth)
    {
        var neighbours = GetNeighbours(from);

        depth--;

        if (depth <= 0)
        {
            return false;
        }

        foreach (var n in neighbours)
        {
            var collision = (map.GetSprite(n) && map.GetSprite(n).name == "spritesheet_retina_16");

            if (!collision && !foundTiles.Contains(n))
            {
                foundTiles.Add(n);

                if (n == to)
                {
                    Debug.Log("found target");
                    return true;
                }

                var res = new List<Vector3Int>();

                if (InnerFindPath(n, to, foundTiles, depth))
                {
                    return true;
                }

                vis.SetTile(n, pathFindTile);
            }
        }

        return false;
    }

    void FindPath(Vector3Int from, Vector3Int to)
    {
        var foundTiles = new List<Vector3Int>();

        InnerFindPath(from, to, foundTiles, 10);
    }

    // Update is called once per frame
    void Update()
    {
        currentCell = map.WorldToCell(transform.position);

        if (Input.GetMouseButtonDown(0))
        {
            vis.SetTile(targetCell, null);

            var worldpos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            targetCell = map.WorldToCell(worldpos);

            vis.SetTile(targetCell, targetTile);

            FindPath(currentCell, targetCell);
        }

    }
}
