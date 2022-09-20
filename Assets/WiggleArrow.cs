using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WiggleArrow : MonoBehaviour
{
    public GameObject art;

    // Update is called once per frame
    void Update()
    {
        var t = ((1 + Mathf.Sin(Time.time * 10f)) / 2f);
        var x = Mathf.Lerp(-0.05f, 0.22f, t);
        art.transform.localPosition = new Vector3(x,
                                                  art.transform.localPosition.y,
                                                  art.transform.localPosition.z);
    }
}
