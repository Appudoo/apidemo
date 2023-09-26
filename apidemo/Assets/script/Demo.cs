using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class Demo : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(postDemo());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    IEnumerator postDemo()
    {
        WWWForm form = new WWWForm();
        form.AddField("status", 1);
        
        UnityWebRequest req =  UnityWebRequest.Post("http://localhost:3000/update/6512699dc2d1c09e3b0a2aa9",form);
        //req.Send();
        yield return req.SendWebRequest();
        if (req.result != UnityWebRequest.Result.Success)
        {
            print(req.error);
        }
        else
        {
            print("Finished ...");
        }
    }
}
