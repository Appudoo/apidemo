using SimpleJSON;
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
    }
}
