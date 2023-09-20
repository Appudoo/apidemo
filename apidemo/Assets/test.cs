using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class test : MonoBehaviour
{
    public GameObject prefab;
    public Transform parent;
    public GameObject[] btn = new GameObject[12];

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(getdata());
        StartCoroutine(getpuzzle());
    }

    // Update is called once per frame
    void Update()
    {

    }
    IEnumerator getdata()
    {
        WWW web = new WWW("http://localhost:3000/all");
        
        yield return web;
      
        JSONArray jSONArray = (JSONArray)JSON.Parse(web.text);

        for (int i = 0; i < jSONArray.Count; i++)
        {
            btn[i] = Instantiate(prefab, parent);
            
            btn[i].transform.GetChild(0).GetComponent<Text>().text = jSONArray[i]["cat_name"] ;
            string str = jSONArray[i]["_id"];
            btn[i].GetComponent<Button>().onClick.AddListener(() => onclick(str));
        }

    }

    void onclick(string str)
    {
        Debug.Log(str);
        StartCoroutine(getpuzzle(str));
       
    }

    IEnumerator getpuzzle(string str = "650a8a6df3821b0d15a68555")
    {
        Debug.Log(str);
        WWW pzl_data = new WWW("localhost:3000/puzzleBycat/"+ str);
        yield return pzl_data;
        JSONArray jSON = (JSONArray)JSON.Parse(pzl_data.text);
        Debug.Log(pzl_data.text);
        Debug.Log(jSON[0]["word"]);
      
    }
    
}
