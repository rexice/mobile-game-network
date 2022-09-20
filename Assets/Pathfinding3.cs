using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Pathfinding3 : MonoBehaviour
{
    public Tilemap map;
    public Tilemap vis;

    public RuleTile goalTile;
    public RuleTile allCheckedTile;
    public RuleTile toCheckTile;
    public RuleTile neighbourTile;
    public RuleTile emptyTile;
    public RuleTile pathTile;

    public Vector3Int start;
    public Vector3Int goal;

    public List<Vector3Int> allChecked;
    public List<Vector3Int> toCheck;
    public List<Vector3Int> currentNeighbours;
    public Dictionary<Vector3Int, Vector3Int> previousTile;
    public List<Vector3Int> path;

    void RefreshVisualisation()
    {
        for (int x = -10; x < 10; x++)
        {
            for (int y = -10; y < 10; y++)
            {
                vis.SetTile(new Vector3Int(x, y), emptyTile);
            }
        }


        foreach (var t in toCheck)
        {
            vis.SetTile(t, toCheckTile);
        }

        foreach (var t in currentNeighbours)
        {
            vis.SetTile(t, neighbourTile);
        }

        foreach (var t in allChecked)
        {
            vis.SetTile(t, allCheckedTile);
        }

        foreach (var p in path)
        {
            vis.SetTile(p, pathTile);
        }

        vis.SetTile(goal, goalTile);
    }

    Vector3Int[] FindNeighbours(Vector3Int tile)
    {
        return new [] {
            tile + new Vector3Int(0, 1),
            tile + new Vector3Int(1, 0),
            tile + new Vector3Int(0, -1),
            tile + new Vector3Int(-1, 0)
        };
    }

    bool CollidesWithWall(Vector3Int tile)
    {
        return map.GetSprite(tile) && map.GetSprite(tile).name == "spritesheet_retina_16";
    }

    bool OnGoal(Vector3Int tile)
    {
        return tile == goal;
    }

    // Update is called once per frame
    void Update()
    {
        start = map.WorldToCell(transform.position);

        if (Input.GetMouseButtonDown(0))
        {
            previousTile = new Dictionary<Vector3Int, Vector3Int>();
            toCheck = new List<Vector3Int>();
            allChecked = new List<Vector3Int>();
            currentNeighbours = new List<Vector3Int>();
            path = new List<Vector3Int>();

            var worldpos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            goal = map.WorldToCell(worldpos);

            toCheck.Add(start);

            RefreshVisualisation();
        }

        if (Input.GetMouseButtonDown(1))
        {
            currentNeighbours = new List<Vector3Int>();

            foreach (var t in toCheck.ToArray())
            {
                var neighbours = FindNeighbours(t);
                currentNeighbours.AddRange(neighbours);

                foreach (var n in neighbours)
                {
                    if (allChecked.Contains(n))
                    {
                        Debug.Log("tile " + n + " has already been checked");
                    }
                    else if (CollidesWithWall(n))
                    {

                    }
                    else if (OnGoal(n))
                    {
                        var currTile = t;
                        path.Add(currTile);

                        var limit = 20;
                        while (previousTile.ContainsKey(currTile))
                        {
                            Debug.Log("adding to path: " + currTile);
                            currTile = previousTile[currTile];
                            path.Add(currTile);

                            limit--;
                            if (limit == 0) { break; }
                        }

                        RefreshVisualisation();
                        
                        return;
                    }
                    else
                    {
                        toCheck.Add(n);
                        if (!previousTile.ContainsKey(n))
                        {
                            previousTile.Add(n, t);
                        }
                        Debug.Log(t + " was before " + n);
                    }
                }

                toCheck.Remove(t);
                allChecked.Add(t);
            }

            RefreshVisualisation();
        }

        if (Input.GetMouseButtonDown(2))
        {
            currentNeighbours = new List<Vector3Int>();

            RefreshVisualisation();
        }
    }
}
