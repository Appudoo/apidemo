using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class test : MonoBehaviour
{
    public GameObject prefab,rawImg,home,puzzle;
    public Transform parent;
    public GameObject[] btn = new GameObject[12];
    public Transform parent2;
    public GameObject[] btn2 = new GameObject[12];
    public GameObject prefab2, rawImg2;

    // Start is called before the firt frame update
    void Start()
    {
        StartCoroutine(getdata());
        //StartCoroutine(getpuzzle());
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
            
            btn[i].transform.GetChild(1).GetComponent<Text>().text = jSONArray[i]["cat_name"] ;
            string str = jSONArray[i]["_id"];
            Debug.Log("test :"+str);
            btn[i].GetComponent<Button>().onClick.AddListener(() => StartCoroutine(getpuzzle(str)));
            WWW webImg = new WWW("http://localhost:3000/images/" + jSONArray[i]["Image"]);
            yield return webImg;
            Texture2D texture = webImg.texture;
            btn[i].transform.GetChild(0).GetComponent<RawImage>().texture = texture;
        }

    }

    void onclick(string str)
    {

        //StartCoroutine(getpuzzle(str));
        int a = 12;
        string[] randomWords = GenerateRandomWords(a);
        Debug.Log(randomWords);
        

    }
    static string[] GenerateRandomWords(int count)
    {
        string[] randomWords = new string[count];
        System.Random random = new System.Random();

        for (int i = 0; i < count; i++)
        {
            randomWords[i] = Guid.NewGuid().ToString().Substring(0, 5); // Generate random 5-character words
        }

        return randomWords;
    }
    IEnumerator getpuzzle(string str)
    {


        home.SetActive(false);
        puzzle.SetActive(true);
        WWW pzl_data = new WWW("http://localhost:3000/puzzleBycat/" + str);
        yield return pzl_data;
        JSONArray jSON = (JSONArray)JSON.Parse(pzl_data.text);
        Debug.Log(pzl_data.text);
        for (int i = 0; i < jSON.Count; i++)
        {
            btn2[i] = Instantiate(prefab2, parent2);

            btn2[i].transform.GetChild(1).GetComponent<Text>().text = jSON[i]["puzzle_name"].Value;
            //string str = jSON[i]["_id"];
            WWW webImg = new WWW("http://localhost:3000/images/" + jSON[i]["Image"]);
            yield return webImg;
            Texture2D texture = webImg.texture;
            btn2[i].transform.GetChild(0).GetComponent<RawImage>().texture = texture;
            //btn[i].GetComponent<Button>().onClick.AddListener(() => (str));
        }
    }



}
