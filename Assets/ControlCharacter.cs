using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ControlCharacter : MonoBehaviour, HasPosition
{
    public Tilemap map;

    public bool isMoving;
    public Vector3Int targetCell;
    public Vector3Int currentCell;
    public NetworkID id;
    public Vector3 nextPos;
    public Vector3 lastPos;

    void Init()
    {
        isMoving = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }


    [UnityEditor.Callbacks.DidReloadScripts]
    private static void OnScriptsReloaded()
    {
        foreach (var o in Object.FindObjectsOfType<ControlCharacter>())
        {
            o.Init();
        }
    }

    Vector3Int RelativeCell(Vector2 direction)
    {
        return map.WorldToCell(transform.position + (Vector3)direction);
    }

    public void SetPos(Vector3 pos)
    {
        transform.position = map.WorldToCell(pos);
    }

    struct MovementData
    {
        public Vector3 direction;
        public Vector3 current_pos;
    }

    IEnumerator WaitToMove()
    {

        isMoving = true;

        while (map.WorldToCell(transform.position) != targetCell)
        {
            yield return new WaitForSeconds(0.3f);

            var curpos = map.WorldToCell(transform.position);

            if (lastPos == curpos && nextPos != targetCell)
            {
                curpos = new Vector3Int(Mathf.FloorToInt(nextPos.x), Mathf.FloorToInt(nextPos.y), Mathf.FloorToInt(nextPos.z));
            }

            var diff = targetCell - curpos;
            
            Vector3 direction;
            if (Mathf.Abs(diff.x) > Mathf.Abs(diff.y))
            {
                //nextcell = RelativeCell((new Vector2(diff.x, 0)).normalized);
                direction = new Vector3(diff.x, 0, 0).normalized;
            }
            else
            {
                //nextcell = RelativeCell((new Vector2(0, diff.y)).normalized);
                direction = new Vector3(0, diff.y, 0).normalized;
            }

            var movement = new MovementData();
            movement.direction = direction;
            movement.current_pos = new Vector3(curpos.x, curpos.y, curpos.z);
            nextPos = movement.current_pos + movement.direction;

            var json = JsonUtility.ToJson(movement);
            Debug.Log(json);
            HttpFetch.Post("http://127.0.0.1:8125/set-position/" + id.name, json);

            lastPos = movement.current_pos;
        }

        isMoving = false;

        yield break;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var worldpos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            targetCell = map.WorldToCell(worldpos);

            if (!isMoving)
            {
                StartCoroutine(WaitToMove());
            }
        }
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
    }
}
