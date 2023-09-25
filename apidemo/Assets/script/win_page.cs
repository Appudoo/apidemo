using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class win_page : MonoBehaviour
{
    public static win_page instance;
    public GameObject prefabe, p;
    string[] Wish = { "AMAZING!", "SENSATIONAL!", "WONDERFULL!", "WEL DONE!", "UNBELIEVABLE!", "SPLENDID!", "EXCELLENT!" };
    public Text wish_text;
    GameObject[] btn = new GameObject[14];
    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        instance = this;
        int randomNum = Random.Range(0, Wish.Length);
        wish_text.text = Wish[randomNum];

    }
    public void gen(string str)
    {
        char[] k = str.ToCharArray();
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
