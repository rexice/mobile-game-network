using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefreshData : MonoBehaviour
{
    public bool isRefreshing;
    public NetworkID id;
    public ControlCharacter player;

    void Init()
    {
        isRefreshing = false;
        id = GetComponent<NetworkID>();
        player = GetComponent<ControlCharacter>();
    }

    void Start()
    {
        Init();
    }

    [UnityEditor.Callbacks.DidReloadScripts]
    private static void OnScriptsReloaded()
    {
        foreach (var o in Object.FindObjectsOfType<RefreshData>())
        {
            o.Init();
        }
    }

    public void GotData(string jsonData)
    {
        var newpos = JsonUtility.FromJson<Vector3>(jsonData);
        player.SetPos(newpos);
    }

    IEnumerator Refresh()
    {
        isRefreshing = true;

        while (true)
        {
            yield return new WaitForSeconds(1f);

            //Debug.Log("asking position for " + id.name);

            HttpFetch.Get("http://127.0.0.1:8125/positions/" + id.name, this);
        }

        isRefreshing = false;

        yield break;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isRefreshing)
        {
            StartCoroutine(Refresh());
        }
    }
}
