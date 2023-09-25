using SimpleJSON;
//using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings;
using UnityEngine.SceneManagement;


public class test : MonoBehaviour
{
    public static test inst;
    //public win_page win_Page;
    public GameObject prefab, home, puzzle, play, puzzle_img, mix_p, Ans_word,win_page;
    public Transform parent;
    public GameObject[] btn = new GameObject[12];
    public Transform parent2, mixbtn_p, Ans_word_p;
    public GameObject[] btn2 = new GameObject[12];
    public GameObject prefab2;
    public int level_no,L_no=1;

    GameObject[] MixBtn = new GameObject[14];
    GameObject[] fBtn = new GameObject[14];
    int cnt = 0;
    char[] word1 = new char[14];
    char[] wrong_ans = new char[14];
    bool isended;
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
            WWW webImg = new WWW("http://localhost:3000/images/" + jSONArray[i]["Image"]);
            yield return webImg;
            Texture2D texture = webImg.texture;
            btn[i].transform.GetChild(0).GetComponent<RawImage>().texture = texture;
        }

    }

    IEnumerator getpuzzle(string str)
    {

        
        home.SetActive(false);
        puzzle.SetActive(true);
        WWW pzl_data = new WWW("http://localhost:3000/puzzleBycat/" + str);
        yield return pzl_data;
        JSONArray jSON = (JSONArray)JSON.Parse(pzl_data.text);
        level_no = jSON.Count;
        
        Debug.Log(pzl_data.text);
        for (int i = 0; i < jSON.Count; i++)
        {
            btn2[i] = Instantiate(prefab2, parent2);

            btn2[i].transform.GetChild(0).GetComponent<Text>().text = (i+1).ToString();
            string pzl_id = jSON[i]["_id"];
            WWW webImg = new WWW("http://localhost:3000/images/" + jSON[i]["Image"]);
            yield return webImg;
            Texture2D texture = webImg.texture;
            btn2[i].GetComponent<Button>().interactable = false;
            //btn2[i].transform.GetChild(0).GetComponent<RawImage>().texture = texture;
            btn2[i].GetComponent<Button>().onClick.AddListener(() => StartCoroutine(get_Single_puzzle(pzl_id)));
        }
        level();
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
        word_logic(jSON);
    }
    void level()
    {
        Debug.Log(L_no);
        for (int i = 0; i < L_no; i++)
        {
            btn2[i].GetComponent<Button>().interactable = true;
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

    void onclick_mix_btn(string mix, int pos, JSONArray jSON)
    {
        if(isended)
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
                
               win(jSON[0]["word"]);
                break;
            }

        }
    }


    void onclick_return(string s, int pos, int k, JSONArray json)
    {

        MixBtn[pos].transform.GetChild(0).GetComponent<Text>().text = s;
        MixBtn[pos].GetComponent<Button>().interactable = true;
        fBtn[k].transform.GetChild(0).GetComponent<Text>().text = "";
        cnt--;
         win(json[0]["word"]);

    }
    void win(string Final_ans)
    {
        
        Debug.Log(cnt+"   "+Final_ans.Length);
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
            if(s == Final_ans)
            {
                isended = true;
                //level.instance.gen(s);
                PlayerPrefs.SetString("ans", Final_ans);
                
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
        L_no++;
        //win_page.SetActive(false);
        //puzzle.SetActive(true);
        SceneManager.LoadScene(Application.loadedLevel);
    }

}
