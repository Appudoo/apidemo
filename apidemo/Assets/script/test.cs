using SimpleJSON;
//using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings;
using UnityEngine.SceneManagement;
using JetBrains.Annotations;
using UnityEngine.Networking;
using System.Net;
using Unity.Burst.CompilerServices;

public class test : MonoBehaviour
{
    public static test inst;
    //public win_page win_Page;
    public GameObject prefab, home, puzzle, play, puzzle_img, mix_p, Ans_word, win_page;
    public Transform parent;
    public GameObject[] btn = new GameObject[12];
    public Transform parent2, mixbtn_p, Ans_word_p;
    public GameObject[] btn2 = new GameObject[12];
    public GameObject prefab2;
    public int total_level, L_no;
    JSONArray jSON_getpuzzle, jSON;
    GameObject[] MixBtn = new GameObject[14];
    GameObject[] fBtn = new GameObject[14];
    int cnt = 0;
    char[] word1 = new char[14];
    char[] wrong_ans = new char[14];
    bool isended, isplayed;
    public Sprite green;


    // Start is called before the firt frame update
    void Start()
    {

        inst = this;
        StartCoroutine(getdata());
        for (int i = 0; i < 26; i++)
        {
            alphabetArray[i] = ((char)('a' + i)).ToString();
        }

        //StartCoroutine(getpuzzle());
    }

    // Update is called once per frame
    void Update()
    {

    }
    //public test (win_page win_Page)
    //{
    //    this.win_Page = win_Page;

    //}
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
            btn[i].GetComponent<Button>().onClick.AddListener(() => level_manager(str));
            WWW webImg = new WWW("http://localhost:3000/images/" + jSONArray[i]["Image"]);
            yield return webImg;
            Texture2D texture = webImg.texture;
            btn[i].transform.GetChild(0).GetComponent<RawImage>().texture = texture;
        }

    }

    IEnumerator getpuzzle(string str)
    {

        Debug.Log(L_no);
        home.SetActive(false);
        puzzle.SetActive(true);
        WWW pzl_data = new WWW("http://localhost:3000/puzzleBycat/" + str);
        yield return pzl_data;
        jSON_getpuzzle = (JSONArray)JSON.Parse(pzl_data.text);
        total_level = jSON_getpuzzle.Count;

        Debug.Log(pzl_data.text);
        for (int i = 0; i < jSON_getpuzzle.Count; i++)
        {
            btn2[i] = Instantiate(prefab2, parent2);

            btn2[i].transform.GetChild(0).GetComponent<Text>().text = (i + 1).ToString();
            string pzl_id = jSON_getpuzzle[i]["_id"];
            WWW webImg = new WWW("http://localhost:3000/images/" + jSON_getpuzzle[i]["Image"]);
            yield return webImg;
            Texture2D texture = webImg.texture;
            btn2[i].GetComponent<Button>().interactable = false;
            int temp = i + 1;
            //btn2[i].transform.GetChild(0).GetComponent<RawImage>().texture = texture;
            btn2[i].GetComponent<Button>().onClick.AddListener(() => StartCoroutine(get_Single_puzzle(pzl_id, temp)));
        }
        level();
    }

    IEnumerator example(string str)
    {
        WWWForm data = new WWWForm();
        data.AddField("status", 3);
        UnityWebRequest web = UnityWebRequest.Post("http://localhost:3000/update/" + str, data);
        yield return web.SendWebRequest();
        //Debug.Log(" unity web Req =  "+web.downloadHandler.text);
        //JSONArray json = (JSONArray)JSON.Parse(web.downloadHandler.text);
        if (web.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("success");
        }
    }

    IEnumerator get_Single_puzzle(string str, int pos)
    {
        if (pos < L_no)
        {
            isplayed = true;
        }
        else
        {
            isplayed = false;
        }
        Debug.Log(str);
        home.SetActive(false);
        puzzle.SetActive(false);
        play.SetActive(true);
        WWW GetSingle_puzzle = new WWW("http://localhost:3000/puzzle/" + str);
        yield return GetSingle_puzzle;
        jSON = (JSONArray)JSON.Parse(GetSingle_puzzle.text);
        Debug.Log(jSON[0]);
        WWW get_img = new WWW("http://localhost:3000/images/" + jSON[0]["Image"]);
        yield return get_img;
        Texture2D pzl_img = get_img.texture;
        puzzle_img.GetComponent<RawImage>().texture = pzl_img;
        word_logic(jSON);
    }
    void level()
    {
        Debug.Log(L_no);
        for (int i = 0; i < L_no; i++)
        {
            btn2[i].GetComponent<Button>().interactable = true;

            if (i < L_no - 1)
            {
                btn2[i].GetComponent<Image>().sprite = green;
            }


        }
    }
    string[] alphabetArray = new string[26];
    void word_logic(JSONArray jSON)
    {
        string word = (jSON[0]["word"]);
        word1 = word.ToCharArray();
        for (int i = 0; i < word1.Length; i++)
        {
            fBtn[i] = Instantiate(Ans_word, Ans_word_p);
        }
        int missing_word = 14 - word.Length;

        List<string> mixword = new List<string>();
        for (int i = 0; i < word1.Length; i++)
        {
            mixword.Add(word1[i].ToString());
        }
        for (int i = 0; i < missing_word; i++)
        {
            int Random_number = UnityEngine.Random.Range(0, alphabetArray.Length);
            mixword.Add(alphabetArray[Random_number]);
        }
        for (int i = 0; i < mixword.Count; i++)
        {
            // logic of value swaping
            int Random_num = UnityEngine.Random.Range(0, mixword.Count);
            string temp = mixword[Random_num];
            mixword[Random_num] = mixword[i];
            mixword[i] = temp;
        }
        string fAns = string.Join("", mixword);
        wrong_ans = fAns.ToCharArray();
        for (int i = 0; i < wrong_ans.Length; i++)
        {
            MixBtn[i] = Instantiate(mix_p, mixbtn_p);
            MixBtn[i].transform.GetChild(0).GetComponent<Text>().text = wrong_ans[i].ToString();
            string ch = wrong_ans[i].ToString();
            int temp = i;
            MixBtn[i].GetComponent<Button>().onClick.AddListener(() => onclick_mix_btn(ch, temp, jSON));
            Debug.Log(word1.Length);

        }
    }
    void level_manager(string str)
    {
        if (!PlayerPrefs.HasKey("max_" + str))
        {
            PlayerPrefs.SetInt("max_" + str, 1);
        }
        else
        {
            L_no = PlayerPrefs.GetInt("max_" + str);
        }
    }
    void onclick_mix_btn(string mix, int pos, JSONArray jSON)
    {
        if (cnt < 0)
        {
            return;
        }
        if (isended)
        {
            Debug.Log("return");
            return;
        }

        for (int k = 0; k < fBtn.Length; k++)
        {

            if (fBtn[k].GetComponentInChildren<Text>().text == "")
            {
                fBtn[k].transform.GetChild(0).GetComponent<Text>().text = mix;
                cnt++;
                MixBtn[pos].transform.GetChild(0).GetComponent<Text>().text = "";
                MixBtn[pos].GetComponent<Button>().interactable = false;
                //string s = jSON[k]["word"];
                //if (cnt == s.Length)
                //{
                //    Debug.Log("full");
                //}
                
                fBtn[k].GetComponent<Button>().onClick.AddListener(() => onclick_return(mix, pos, k, jSON));

                win(jSON);
                break;
            }

        }
    }


    void onclick_return(string s, int pos, int k, JSONArray json)
    {
        if (cnt == 0)
        {
            return;
        }

        MixBtn[pos].transform.GetChild(0).GetComponent<Text>().text = s;
        MixBtn[pos].GetComponent<Button>().interactable = true;
        fBtn[k].transform.GetChild(0).GetComponent<Text>().text = "";
        cnt--;
        win(json);

    }
    void win(JSONArray json)
    {
        string Final_ans = json[0]["word"];
        Debug.Log(cnt + "   " + Final_ans.Length);
        char[] str;
        if (cnt == Final_ans.Length)
        {
            str = new char[cnt];
            for (int i = 0; i < cnt; i++)
            {
                str[i] = char.Parse(fBtn[i].transform.GetChild(0).GetComponent<Text>().text);

            }

            string s = new string(str);
            Debug.Log(s);
            if (s.ToLower() == Final_ans.ToLower())
            {
                isended = true;
                //level.instance.gen(s);
                PlayerPrefs.SetString("ans", Final_ans);
                //StartCoroutine(example(json[0]["_id"]));
                for (int i = 0; i < L_no - 1; i++)
                {
                    btn2[L_no - 1].transform.GetChild(1).GetComponent<Image>().enabled = true;
                }
                if (!isplayed)
                {
                    L_no++;
                    PlayerPrefs.SetInt("max_" + json[0]["cat_id"], L_no);
                }

                win_page.SetActive(true);
                play.SetActive(false);
                for (int i = 0; i < cnt; i++)
                {
                    Debug.Log(fBtn[i].gameObject.name);
                    fBtn[i].GetComponent<Image>().sprite = green;
                    fBtn[i].GetComponent<Button>().interactable = false;
                }

            }
        }

    }
    public void onclickOnword()
    {

        //win_page.SetActive(false);
        //puzzle.SetActive(true);
        SceneManager.LoadScene(Application.loadedLevel);
    }
    public void onclickHint()
    {
        string str = jSON[0]["word"];
        int random_index;
        do
        {
            random_index = UnityEngine.Random.Range(0, str.Length);
            Debug.Log("random_index " + random_index);
        } while (fBtn[random_index].transform.GetChild(0).GetComponent<Text>().text != "");
        char[] k = str.ToCharArray();
        cnt++;
        Debug.Log("cnt " + cnt);

        fBtn[random_index].transform.GetChild(0).GetComponent<Text>().text = str[random_index].ToString();
        for (int i = 0; i < 14; i++)
        {
            if (MixBtn[i].transform.GetChild(0).GetComponent<Text>().text == str[random_index].ToString())
            {
                MixBtn[i].transform.GetChild(0).GetComponent<Text>().text = "";
                MixBtn[i].GetComponent<Button>().interactable = false;
                break;
            }
        }
        win(jSON);
    }
}
