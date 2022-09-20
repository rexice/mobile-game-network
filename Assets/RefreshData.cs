using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefreshData : MonoBehaviour
{
    public bool isRefreshing;
    public NetworkID id;
    public HasPosition pos;

    void Init()
    {
        // TODO: disable when building for real
        Application.runInBackground = true;

        isRefreshing = false;
        id = GetComponent<NetworkID>();
        pos = GetComponent<HasPosition>();
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
        pos.SetPos(newpos);
    }

    IEnumerator Refresh()
    {
        isRefreshing = true;

        while (true)
        {
            yield return new WaitForSeconds(0.1f);

            Debug.Log("asking position for " + id.name);

            HttpFetch.Get("http://127.0.0.1:8125/position/" + id.name, this);
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
