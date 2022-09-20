using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HttpFetch : MonoBehaviour
{
    public bool doFetch = false;
    public NetworkID id;

    private static HttpFetch _instance;

    public static HttpFetch instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<HttpFetch>();
            }
            return _instance;
        }
    }

    IEnumerator InnerGet(string uri, RefreshData refreshData)
    {
        using (UnityWebRequest req = UnityWebRequest.Get(uri))
        {
            yield return req.SendWebRequest();

            switch (req.result)
            {
                case UnityWebRequest.Result.Success:
                    refreshData.GotData(req.downloadHandler.text);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError("HTTP error: " + req.error);
                    break;
            }
        }
    }

    IEnumerator InnerPost(string uri, string data)
    {
        using (UnityWebRequest req = UnityWebRequest.Post(uri, data))
        {
            yield return req.SendWebRequest();

            switch (req.result)
            {
                case UnityWebRequest.Result.Success:
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError("HTTP error: " + req.error);
                    break;
            }
        }
    }

    public static void Post(string uri, string data)
    {
        instance.StartCoroutine(instance.InnerPost(uri, data));
    }

    public static void Get(string uri, RefreshData refreshData)
    {
        instance.StartCoroutine(instance.InnerGet(uri, refreshData));
    }

    // Update is called once per frame
    void Update()
    {
        if (doFetch)
        {
            // StartCoroutine(Fetch("http://127.0.0.1:8125/position/" + id.name));
            doFetch = false;
        }
    }
}
