using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameTime : MonoBehaviour
{
    Text text;
    public float gameTime = 0;
    public int gameTimeInt;

    int m;
    int s;

    void Start()
    {
        text = GetComponent<Text>();
    }

    void Update()
    {
        gameTime += Time.deltaTime;
        gameTimeInt = (int)gameTime;

        m = gameTimeInt / 60;
        s = gameTimeInt % 60;
        text.text = m.ToString("00") + ":" + s.ToString("00");
    }

}
