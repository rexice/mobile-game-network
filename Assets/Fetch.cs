using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Fetch : MonoBehaviour
{
    public bool fetch = false;

    IEnumerator Fetc(string uri)
    {
        using (UnityWebRequest req = UnityWebRequest.Get(uri))
        {
            yield return req.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            switch (req.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + req.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + req.error);
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log(pages[page] + ":\nReceived: " + req.downloadHandler.text);
                    break;
            }
        }
    }

    void Update()
    {
        if (fetch) {
            fetch = false;

            StartCoroutine(Fetc("http://127.0.0.1:8125/"));
        }
    }
}
