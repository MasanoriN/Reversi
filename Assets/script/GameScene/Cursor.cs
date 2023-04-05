using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor : MonoBehaviour
{　　//カーソルの動きを管理
    GameObject Game;
    Game G_script;
    int z , x ; 

    // Start is called before the first frame update
    void Start()
    {
        Game = GameObject.Find("Game"); //Gameオブジェクトにアクセス
        G_script = Game.GetComponent<Game>();  //Gameオブジェクトのスクリプトにアクセス
        //カーソルのあるマス座標を（z,x）で表す
        //最初は左上（0,0）
        z = 0;
        x = 0;
    }
    

    public void UButtonDown()
	{
        if (z != 0)  //上端（z=0）だと動かない
        {
            GetComponent<AudioSource>().Play();
            transform.Translate(0, 0, 10);
            z--;
        }
    }

	public void DButtonDown()
	{
        if (z != 7)  //下端（z=7）だと動かない
        {
            GetComponent<AudioSource>().Play();
            transform.Translate(0, 0, - 10);
            z++;
        }
    }

    public void RButtonDown()
	{
        if (x != 7)//右端（x=7）のときは動かない
        {
            GetComponent<AudioSource>().Play();
            transform.Translate(10, 0, 0);
            x++;  //xを更新
        }
    }

    public void LButtonDown()
	{
        if (x != 0)  //左端（x=0）だと動かない
        {
            GetComponent<AudioSource>().Play();
            transform.Translate(-10, 0, 0);
            x--;
        }
    }

    public void EButtonDown()
	{
        if (G_script.Board[z, x] == 0)
        {
            G_script.Playerturn(z, x);  //Blackturnにカーソルの現在のマス座標を渡す
        }
    }
}
