using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameUI : MonoBehaviour
{
    //盤面の枚数に対する割合でバーを作成
    //現在のそれぞれの色の枚数を表示
    //ゲーム終了時には終了メッセージを表示
    GameObject Game;
    Game G_script;
    GameObject CounterBar;
    GameObject BlackCount;
    GameObject WhiteCount;
    GameObject PassText;
    public bool PassFlag;
    int t;
    Slider counter;    //Slider


    // Start is called before the first frame update
    void Start()
    {
        Game = GameObject.Find("Game");
        G_script = Game.GetComponent<Game>();
        CounterBar = GameObject.Find("CounterBar");  //画面上部のバー
        BlackCount = GameObject.Find("BlackCount");  //バー左の黒ディスクカウンター
        WhiteCount = GameObject.Find("WhiteCount");  //バー右の白ディスクカウンター
        PassText = GameObject.Find("PassText");      //パスされたときに表示されるテキスト
        PassText.SetActive(false);
        PassFlag = false;
        t = 0;
        counter = CounterBar.GetComponent<Slider>();
        float max = 100f;   //スライドバーの上限は100％
        float current = 50f;   //現在の値：最初は50％（初期配置の2：2）
        counter.maxValue = max;
        counter.value = current;
    }

    void Update()
    {
        float black = G_script.Count_mark(1, G_script.Board);  //黒ディスクの数
        float white = G_script.Count_mark(-1, G_script.Board);   //白ディスクの数
        float board = black + white;                           //盤面にあるディスクの数

        //それぞれの枚数を文字列に変換
        string black_txt = black.ToString();
        string white_txt = white.ToString();


        //10枚未満の場合0をつける（例）1 -> 01
        if (black < 10) black_txt = "0" + black_txt;
        if (white < 10) white_txt = "0" + white_txt;
        counter.value = (black / board) * 100;   //スライドバーの値を計算（ボードにおける黒の占める割合）
        //カウンターのテキストを更新
        BlackCount.GetComponent<TextMeshProUGUI>().text = black_txt;
        WhiteCount.GetComponent<TextMeshProUGUI>().text = white_txt;
        if (PassFlag)
        {
            PassText.SetActive(true);
            if (t == 150)
            {
                PassText.SetActive(false);
                PassFlag = false;
                t = 0;
            }
            else t++;
        }

    }
}

