using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemyPlayer : MonoBehaviour
{
    GameObject Game;
    Game G_script;
    int[,] brd_score1;
    int[,] brd_score2;
    int D = 19;
    // Start is called before the first frame update
    void Start()
    {
        Game = GameObject.Find("Game");
        G_script = Game.GetComponent<Game>();
        //盤面の評価
        //角が最も高く，角に繋がってしまうマスは評価低め
        brd_score1 = new int[8,8]{{120, -20, 20, 10, 10, 20, -20, 120},
                                  {-20, -40, -5, -5, -5, -5, -40, -20},
                                  { 20,  -5, 15,  1,  1, 15,  -5,  20},
                                  { 10,  -5,  1,  1,  1,  1,  -5,  10},
                                  { 10,  -5,  1,  1,  1,  1,  -5,  10},
                                  { 20,  -5, 15,  1,  1, 15,  -5,  20},
                                  {-20, -40, -5, -5, -5, -5, -40, -20},
                                  {120, -20,  20,10, 10,  20, -20, 120}};

        brd_score2 = new int[8, 8]{{120, 10, 25,  20,  20, 25, 10, 120},
                                  {  10,  5, 10,  10,  10, 10,  5,  10},
                                  {  25, 10, 30,  15,  15, 30, 10,  25},
                                  {  20, 10, 15,  15,  15, 15, 10,  20},
                                  {  20, 10, 15,  15,  15, 15, 10,  20},
                                  {  25, 10, 30,  15,  15, 30, 10,  25},
                                  {  10,  5, 10,  10,  10, 10,  5,  10},
                                  { 120, 20, 25,  20,  20, 25, 10, 120}};


    }

    public void SetDisk(int[,] brd)
    {
        var best = Search(brd, -1*StartUI.Player, D);  //ベストな手を探索(返り値：ボード，最善手)
        int[] hand = best.Item2;  //最善手
        if (hand != null)
        {
            G_script.Setboard(hand[0], hand[1]);  //探索した手を打つ
            string s = "(" + hand[0] + "," + hand[1] + ")";
            //Debug.Log(s);  //打った手を出力（確認用）
        }
    }

    public (int[,], int[]) Search(int[,] brd, int turn, int depth)
    {
        List<(int, int, int[])> hands = G_script.Find_hand(brd, turn);
        int[] best_hand = new int[2];
        int[,] brd_best = new int[8, 8];
        int[,] brd_nxt;
        int[,] brd_nxt_nxt;
        int[] dir;
        int score_max = -10000;
        int score_diff;
        int z, x;
        if (depth == 0 || hands.Count == 0) return (brd, null);  //D手先まで読み切った場合，打つ手がない場合，盤面のみを返す
        foreach (var hand in hands)
        {
            z = hand.Item1;
            x = hand.Item2;
            dir = hand.Item3;
            if ((z == 0 || z == 7) && (x == 0 || x == 7))
            {//角が取れる場合はとる
                best_hand[0] = z;
                best_hand[1] = x;
                brd_best = G_script.SetSimulation(brd, z, x, turn, dir);   //最善の盤面に角をとった盤面を設定
                break;  //foreachを抜ける（新たな手の探索をやめる）
            }
            brd_nxt = G_script.SetSimulation(brd, z, x, turn, dir);  //そこに打った場合の盤面（引数：現在の盤面，座標，裏返す色，方向）
            var s = Search(brd_nxt, -1 * turn, depth - 1);  //次の次を探索（再帰：深さを１減らす）
            brd_nxt_nxt = s.Item1; //探索した盤面を取得
            //次の次においてそれぞれの色から点数を取得し，差をとる（相手よりより高い点数をとっているものをよい点数とする）
            //評価関数の引数（盤面，ターン，評価する色）
            score_diff = Calc_score(brd_nxt_nxt, turn, turn, dir.Sum()) - Calc_score(brd_nxt_nxt, turn, -1*turn, dir.Sum());
            if (score_diff > score_max)
            {//スコアを更新したら座標，盤面，最高スコアを更新する
                best_hand[0] = z;
                best_hand[1] = x;
                score_max = score_diff;
                brd_best = brd_nxt;
            }
            else if (score_diff == score_max)
			{
                if ((best_hand[0] == 1 || best_hand[0] == 6) && (best_hand[1] == 1 || best_hand[1] == 6))
				{
                    best_hand[0] = z;
                    best_hand[1] = x;
                    brd_best = brd_nxt;
				}
			}
        }
        return (brd_best, best_hand);       
    }
    int Calc_score(int[,] brd, int turn, int color, int a)
    {
        int score = 0;  //スコア
        float w1 = 0;  //盤面評価の重み
        float w2 = 0;  //手数評価の重み
        int disk_board = G_script.Count_mark(1, brd) + G_script.Count_mark(-1, brd);  //ボード上にあるディスク枚数
        var corners = new List<(int, int)>();     //角におかれているディスク座標を格納する
        bool c_flag = false;    //辺の角隣のマスに評価している側のディスクが打たれているとtrue
        switch (disk_board)
        {
            case { } n when (n <= 15):
                w1 = 3;  //序盤ほど盤面に設定されているスコアを大きく反映
                w2 = 1;  //打てる手の数にかける重み
                if(turn == color)   score -= 3 * a;
                break;

            case { } n when (n <= 45):
                w1 = 2;
                w2 = 4;
                if (turn == color)  score -= 2*a;
                break;

            case { } n when (n > 45):
                //終盤は枚数と手数を盤面評価値より反映させる
                w1 = 0;
                w2 = 10;
                score += 2 * G_script.Count_mark(color, brd);
                break;
        }

        //盤面評価値によるスコア加算，角にディスクの置かれているかの確認
        for (int z = 0; z < 8; z++)
        {
            for (int x = 0; x < 8; x++)
            {
                if (brd[z, x] == color && disk_board <= 45) score += (int)w1 * brd_score1[z, x];  //おかれているディスクの色が同じ色だったらスコアを加算
                else if (brd[z, x] == color && disk_board > 45) score += (int)w1 * brd_score2[z, x];
                if ((z == 0 || z == 7) && (x == 0 || x == 7) && brd[z, x] == color)  //角にディスクがおかれていたらリストに追加
                {
                    corners.Add((z, x));
                }
                else if ((z == 1) && (x == 0 || z == 7) && brd[z, x] == color) c_flag = true;
                else if ((x == 1) && (z == 0 || z == 7) && brd[z, x] == color) c_flag = true;
            }
        }

        //裏返すことのできない確定石によるスコア加算
        if (corners.Count > 0)
		{
            foreach (var corner in corners)
			{  
                int z = corner.Item1;
                int x = corner.Item2;
                while (z >= 0)
				{//上方向に見る
                    if (brd[z, x] == color) score += 30;  
                    if (brd[z, x] != color) break;  //ディスクの連続が途切れたらbreak
                    z--;
				}

                z = corner.Item1;
                while (z < 8)
				{//下方向見る
                    if (brd[z, x] == color) score += 30;
                    if (brd[z, x] != color) break;
                    z++;
				}

                z = corner.Item1;
                while (x >= 0)
				{//左方向を見る
                    if (brd[z, x] == color) score += 30;
                    if (brd[z, x] != color) break;
                    x--;
				}

                x = corner.Item2;
                while (x < 8)
				{//右方向を見る
                    if (brd[z, x] == color) score += 30;
                    if (brd[z, x] != color) break;
                    x++;
				}
            }
		}
        
        else if(c_flag)
		{
            //良型とされる山によるスコア加算
            //山：辺において角以外にすべてディスクが置かれている形
            //また相手のディスクがなく，自分のディスクが連続している場合も加点
            if (brd[1, 0] == color)
            {
                int z = 2;
                while (z < 7)
				{
					if (brd[z, 0] == -1 * color)
					{
                        score -= 40;
                        break;
					}
                    else if (brd[z, 0] == 0) break;
					if (z == 6) score += 60;
                    z++;
                    score += 10;
				}
			}

            if (brd[1, 7] == color)
            {
                int z = 2;
                while (z < 7)
                {
                    if (brd[z, 7] == -1 * color)
					{
                        score -= 40;
                        break;
					}
                    else if (brd[z, 7] == 0) break;
                    if (z == 6) score += 60;
                    z++;
                    score += 10;
                }
            }

            if (brd[7, 1] == color)
			{
                int x = 2;
                while (x < 7)
                {
                    if (brd[7, x] == -1 * color)
					{
                        score -= 40;
                        break;
					}
                    else if (brd[7, x] == 0) break;
                    if (x == 6) score += 60;
                    x++;
                    score += 10;
                }
            }

            if (brd[0, 1] == color)
            {
                int x = 2;
                while (x < 7)
                {
                    if (brd[0, x] == -1 * color)
					{
                        score -= 40;
                        break;
					}
                    else if (brd[0, x] == 0) break;
                    if (x == 6) score += 60;
                    x++;
                    score += 10;
                }
            }
        }

        //手数によるスコア加算
        //ターンと評価する側の色が同じ場合，打てる手数をスコアに加算
        if (turn == color)
		{
            var hands = G_script.Find_hand(brd, turn);
            score += (int)w2 * hands.Count;
            hands = G_script.Find_hand(brd, -1 * turn);
            if (hands.Count == 0) score += 50; 

		}
        return score;
    }
}
