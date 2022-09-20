using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Pathfinding2 : MonoBehaviour
{
    public Tilemap map;
    public Tilemap vis;
    public RuleTile emptyTile;
    public RuleTile targetTile;
    public RuleTile pathTile;

    public Vector3Int target;
    public Vector3Int pos;

    Vector3Int[] Neighbours(Vector3Int cell)
    {
        return new [] {
            cell + new Vector3Int(0, -1),
            cell + new Vector3Int(1, 0),
            cell + new Vector3Int(0, 1),
            cell + new Vector3Int(-1, 0)
        };
    }

    public void DrawTiles(List<Vector3Int> tiles, RuleTile tile)
    {
        foreach (var t in tiles)
        {
            vis.SetTile(t, tile);
        }
    }

    public void RefreshVisualisation()
    {
        for (int x = -10; x < 10; x++)
        {
            for (int y = -10; y < 10; y++) {
                vis.SetTile(new Vector3Int(x, y, 0), emptyTile);
            }
        }


        var toCheck = new List<Vector3Int>();
        toCheck.Add(pos);

        var checkedTiles = new List<Vector3Int>();
        var ns = Neighbours(pos);

        for (int i = 0; i < toCheck.Count; i++)
        {
            foreach (var t in ns)
            {
                if (checkedTiles.Contains(t))
                {
                    // do nothing
                }
                else if (t == target)
                {
                    Debug.Log("found!!");
                    return;
                }
                else if (map.GetSprite(t) && map.GetSprite(t).name == "spritesheet_retina_16")
                {
                    // do nothing
                }
                else
                {
                    toCheck.Add(t);
                }
            }

            DrawTiles(toCheck, pathTile);
        }

        vis.SetTile(target, targetTile);
        Debug.Log(target);
    }

    public void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            var worldpos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            target = map.WorldToCell(worldpos);
            pos = map.WorldToCell(transform.position);
            RefreshVisualisation();
        }
    }
}
