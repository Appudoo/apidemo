using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class level : MonoBehaviour
{
    public static level instance;
    public GameObject prefabe, p;
    string[] Wish = { "AMAZING!", "SENSATIONAL!", "WONDERFULL!", "WELL DONE!", "UNBELIEVABLE!", "SPLENDID!", "EXCELLENT!" };
    public Text wish_text;
    GameObject[] btn = new GameObject[14];
    // Start is called before the first frame update

    void Start()
    {
        instance = this;
        int randomNum = Random.Range(0, Wish.Length);
        wish_text.text = Wish[randomNum];
    }
    private void OnEnable()
    {
        gen();

    }
    public void gen()
    {
        string str = PlayerPrefs.GetString("ans");
        Debug.Log("                                                   " + str);
        char[] k = str.ToCharArray();
        arpit:
        for (int i = 0; i < p.transform.childCount; i++)
        {
            DestroyImmediate(p.transform.GetChild(i).gameObject);
        }
        if(p.transform.childCount>0)
        {
            goto arpit;
        }
        for (int i = 0; i < k.Length; i++)
        {
            btn[i] = Instantiate(prefabe, p.transform);
            btn[i].transform.GetChild(0).GetComponent<Text>().text = k[i].ToString();
            btn[i].GetComponent<Button>().interactable = false;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
