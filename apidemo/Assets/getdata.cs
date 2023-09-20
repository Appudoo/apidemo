using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public class getcatdata : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(nameof(getdata));
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator getdata()
    {
        WWW web = new WWW("http://localhost:3000/all");
        yield return web;
        Debug.Log(web.text);
        JSONArray jSONArray = (JSONArray)web.text;
        Debug.Log(jSONArray.Count);
        
    }
}
