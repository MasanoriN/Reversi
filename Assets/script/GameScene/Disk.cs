using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using DG.Tweening;
using System.Linq;

//生成されたディスクを管理する
public class Disk : MonoBehaviour
{
    
	//dir[8]:裏返すことのできる方向(上，右斜め上，右，右斜め下，下，左斜め下，左，左斜め上)
	public void SetActive(bool value, int turn)   //diskの表示・非表示切り替え
    {
        if (value == false) gameObject.SetActive(value);  //valueがfalseの場合，非表示にする
        else if (value == true && turn == 1)  //valueがtrueで黒のターンの時
        {
            Rotate(turn);
            gameObject.SetActive(value);  //その後，表示
        }
        else if (value == true && turn == -1)  //白のターンの場合
        {
            gameObject.SetActive(value);  //そのまま表示
        }
    }
    
	public void Rotate(int turn)　　//ディスクの回転
	{
        //ディスクの高さが合うようにターンによってy座標を調整
        var pos = transform.position;
        if (turn == 1)
		{
            transform.DOJump(new Vector3(pos.x,5.5f,pos.z),4.0f,1,0.5f);
            transform.DORotate(new Vector3(0,0,180),0.5f);

        }
		else if (turn == -1)
		{
            transform.DOJump(new Vector3(pos.x, 5.25f, pos.z), 4.0f, 1, 0.5f);
            transform.DORotate(new Vector3(0, 0, 0), 0.5f);
        }
    }

    public int[] Checkhand(int z, int x, int turn, int[,] board)  //ディスクが置けるかのチェック，置ける場合の裏返せる方向
    {
        int[] dir = new int[8];
        int cur_z = z-1;  //注目しているマス
        int cur_x = x;
        int count = 0;  //裏返せるディスクの数
        while (cur_z >= 0)   //上方向を見る
        {
            if (board[cur_z, cur_x] == 0) break;    //空白のマスがある場合は裏返せないためbreak, dirは0のまま
            else if (board[cur_z, cur_x] == turn)     //同じ色のディスクが確認されたらdirに裏返せる枚数を格納
            {
                dir[0] = count;   //すぐ隣に同じ色があった場合でも，格納されるcountは0なので問題ない
                break;
            }
            //どれにも当てはまらない(隣が違う色)場合，さらに隣のマスを見ていく．
            cur_z--;   //上方向へ移動
            count++;  //裏返す枚数を増やす
        }
        cur_z = z - 1;
        cur_x = x + 1;
        count = 0;
        while (cur_z >= 0 && cur_x < 8)  //右斜め上を見る
        {
            if (board[cur_z, cur_x] == 0) break;    //空白のマスがある場合は裏返せないためbreak, dirは0のまま
            else if (board[cur_z, cur_x] == turn)     //同じ色のディスクが確認されたらdirに裏返せる枚数を格納
            {
                dir[1] = count;   //すぐ隣に同じ色があった場合でも，格納されるcountは0なので問題ない
                break;
            }
            cur_z--;
            cur_x++;
            count++;
        }
        cur_z = z;
        cur_x = x + 1;
        count = 0;
        while (cur_x < 8)  //右を見る
        {
            if (board[cur_z, cur_x] == 0) break;    //空白のマスがある場合は裏返せないためbreak, dirは0のまま
            else if (board[cur_z, cur_x] == turn)     //同じ色のディスクが確認されたらdirに裏返せる枚数を格納
            {
                dir[2] = count;   //すぐ隣に同じ色があった場合でも，格納されるcountは0なので問題ない
                break;
            }
            cur_x++;
            count++;
        }
        cur_z = z + 1;
        cur_x = x + 1;
        count = 0;
        while (cur_z < 8 && cur_x < 8)  //右斜め下を見る
        {
            if (board[cur_z, cur_x] == 0) break;    //空白のマスがある場合は裏返せないためbreak, dirは0のまま
            else if (board[cur_z, cur_x] == turn)     //同じ色のディスクが確認されたらdirに裏返せる枚数を格納
            {
                dir[3] = count;   //すぐ隣に同じ色があった場合でも，格納されるcountは0なので問題ない
                break;
            }
            cur_z++;
            cur_x++;
            count++;
        }
        cur_z = z + 1;
        cur_x = x;
        count = 0;
        while (cur_z < 8)  //下を見る
        {
            if (board[cur_z, cur_x] == 0) break;    //空白のマスがある場合は裏返せないためbreak, dirは0のまま
            else if (board[cur_z, cur_x] == turn)     //同じ色のディスクが確認されたらdirに裏返せる枚数を格納
            {
                dir[4] = count;   //すぐ隣に同じ色があった場合でも，格納されるcountは0なので問題ない
                break;
            }
            cur_z++;
            count++;
        }
        cur_z = z + 1;
        cur_x = x - 1;
        count = 0;
        while (cur_z < 8 && cur_x >= 0)  //左斜め下を見る
        {
            if (board[cur_z, cur_x] == 0) break;    //空白のマスがある場合は裏返せないためbreak, dirは0のまま
            else if (board[cur_z, cur_x] == turn)     //同じ色のディスクが確認されたらdirに裏返せる枚数を格納
            {
                dir[5] = count;   //すぐ隣に同じ色があった場合でも，格納されるcountは0なので問題ない
                break;
            }
            cur_z++;
            cur_x--;
            count++;
        }
        cur_z = z;
        cur_x = x - 1;
        count = 0;
        while (cur_x >= 0)  //左を見る
        {
            if (board[cur_z, cur_x] == 0) break;    //空白のマスがある場合は裏返せないためbreak, dirは0のまま
            else if (board[cur_z, cur_x] == turn)     //同じ色のディスクが確認されたらdirに裏返せる枚数を格納
            {
                dir[6] = count;   //すぐ隣に同じ色があった場合でも，格納されるcountは0なので問題ない
                break;
            }
            cur_x--;
            count++;
        }
        cur_z = z - 1;
        cur_x = x - 1;
        count = 0;
        while (cur_z >= 0 && cur_x >= 0)  //左斜め上を見る
        {
            if (board[cur_z, cur_x] == 0) break;    //空白のマスがある場合は裏返せないためbreak, dirは0のまま
            else if (board[cur_z, cur_x] == turn)     //同じ色のディスクが確認されたらdirに裏返せる枚数を格納
            {
                dir[7] = count;   //すぐ隣に同じ色があった場合でも，格納されるcountは0なので問題ない
                break;
            }
            cur_z--;
            cur_x--;
            count++;
        }
        return dir;
    }
}
