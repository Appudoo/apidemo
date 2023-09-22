using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class test : MonoBehaviour
{
    public GameObject prefab, home, puzzle,play,puzzle_img;
    public Transform parent;
    public GameObject[] btn = new GameObject[12];
    public Transform parent2;
    public GameObject[] btn2 = new GameObject[12];
    public GameObject prefab2;

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

            btn[i].transform.GetChild(1).GetComponent<Text>().text = jSONArray[i]["cat_name"];
            string str = jSONArray[i]["_id"];
            Debug.Log("test :" + str);
            btn[i].GetComponent<Button>().onClick.AddListener(() => StartCoroutine(getpuzzle(str)));
            WWW webImg = new WWW("http://localhost:3000/images/" + jSONArray[i]["Image"]);
            yield return webImg;
            Texture2D texture = webImg.texture;
            btn[i].transform.GetChild(0).GetComponent<RawImage>().texture = texture;
        }

    }

    void onclick(string str)
    {
        StartCoroutine(getpuzzle(str));

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
            string pzl_id = jSON[i]["_id"];
            WWW webImg = new WWW("http://localhost:3000/images/" + jSON[i]["Image"]);
            yield return webImg;
            Texture2D texture = webImg.texture;
            btn2[i].transform.GetChild(0).GetComponent<RawImage>().texture = texture;
            btn2[i].GetComponent<Button>().onClick.AddListener(() => StartCoroutine(get_Single_puzzle(pzl_id)));
        }
    }

     IEnumerator get_Single_puzzle(string str)
    {
        Debug.Log(str);
        home.SetActive(false);
        puzzle.SetActive(false);
        play.SetActive(true);
        WWW GetSingle_puzzle = new WWW("http://localhost:3000/puzzle/" + str);
        yield return GetSingle_puzzle;
        JSONArray jSON = (JSONArray)JSON.Parse(GetSingle_puzzle.text);
        Debug.Log(jSON[0]);
        WWW get_img = new WWW("http://localhost:3000/images/" + jSON[0]["Image"]);
        
        yield return get_img;
        Texture2D pzl_img = get_img.texture;
        puzzle_img.GetComponent<RawImage>().texture = pzl_img;
    }



}
