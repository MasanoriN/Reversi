using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    //ゲーム管理
    // Start is called before the first frame update
    GameObject Counter;
    GameUI U_script;
    GameObject EnemyPlayer;
    EnemyPlayer E_script;
    GameObject Pointer;
    public Disk DiskPrefab;
    public int[,] Board = new int[8,8];       //盤面の状況把握　board[z(縦),x（横）]　-1:白，0:なし,1:黒
    Disk[,] disks = new Disk[8,8];     //生成したdiskの管理
    int turn;       //現在のターン（-1:白，1:黒）
    bool turnflag = false;
    public static int EndBlack;
    public static int EndWhite;

    void Start()

    {
        Counter = GameObject.Find("CounterBar");
        U_script = Counter.GetComponent<GameUI>();
        EnemyPlayer = GameObject.Find("EnemyPlayer");
        E_script = EnemyPlayer.GetComponent<EnemyPlayer>();
        Pointer = GameObject.Find("Pointer");
        Pointer.SetActive(false);
        turn = 1;
        for (int z = 3; z <= 4; z++)   //中央の４つを配置
        {
            for (int x = 3; x <= 4; x++)
            {
                if (z == x) Board[z, x] = -1;
                else Board[z, x] = 1;
            }
        }
        for (int z = 0; z < 8; z++)  //Diskを生成，配置
        {
            for (int x = 0; x < 8; x++)
            {
                var Disk = Instantiate(DiskPrefab);
                if (Board[z, x] == 0)
                {
                    Disk.transform.position = new Vector3(10 * x, 5.25f, -5 - 10 * z);
                    Disk.SetActive(false,turn);  //打たれていないディスクは隠す
                }
                else if (Board[z, x] == -1) Disk.transform.position = new Vector3(10 * x, 5.25f, -5 - 10 * z);
                else if (Board[z, x] == 1)
                {
                    //裏返したときにボードに埋もれないようy座標を0.5に
                    Disk.transform.position = new Vector3(10 * x, 5.5f, -5 - 10 * z);   
                    Disk.transform.rotation = Quaternion.Euler(180, 0, 0);     //黒に裏返す
                }
                disks[z, x] = Disk;    //生成したDiskをdisksに格納
            }
        }

    }

	private void Update()
	{
        if (turn == 0 && turnflag == false)  //ターン0のときターンの途中でなければ終了処理へ
        {
            Invoke("GameEnd",2.5f);
            turnflag = true;
        }
        else if (turn == -1*StartUI.Player && turnflag == false)  //enemyのターンかつ処理中でない場合
		{
            turnflag = true;  //処理中であることを示すフラグをTrueに
            //Whiteturnを呼び出す．
            //プレイヤーが置いた後に少し時間をあける
            Debug.Log("aa");
            Invoke("Enemyturn",2.0f);
		}
    }
	
    void GameEnd() 
    {
        EndBlack = Count_mark(1, Board);
        EndWhite = Count_mark(-1, Board);
        SceneManager.LoadScene("EndScene");
    }
    void EndCheck() //ゲーム終了確認
    {
        int black = Count_mark(1,Board);  //黒ディスクの数
        int white = Count_mark(-1,Board); //白ディスクの数
        var hands = Find_hand(Board, turn); //現在のターンの打てる手
        //どちらも打てないとき，または全マス埋まったとき
        if (hands.Count == 0 || black + white == 64)
        {
            turn = 0;  //ターンを0にする（カーソルも動かせず，白のターンも始まらない）
        }
    }

    public void Playerturn(int z, int x)
	{
        
        if (StartUI.Player == 1)
		{
            Setboard(z, x);
            string s = "turn:" + turn;
            Debug.Log(s);
        }
        else
		{
            Setboard(z, x);
            string s = "turn:" + turn;
            Debug.Log(s);
        }
	}
    
    public void Enemyturn()
	{
        int[,] board_copy = new int[8, 8];
        for (int z = 0; z < 8; z++)
        {
            for (int x = 0; x < 8; x++)
            {
                if (Board[z, x] == 1) board_copy[z, x] = 1;
                else if (Board[z, x] == -1) board_copy[z, x] = -1;
            }
        }
        E_script.SetDisk(board_copy);
        turnflag = false;
	}


	public void Setboard(int z, int x)
	{
        int[] dir = disks[z, x].Checkhand(z, x, turn, Board);
        if (dir.Sum() > 0)
        {
            GetComponent<AudioSource>().Play();
            Board[z, x] = turn;
            disks[z, x].SetActive(true, turn);
            Vector3 dpos = disks[z, x].transform.position;
            if (turn != StartUI.Player)
			{
                Pointer.SetActive(true);
                Pointer.transform.position = new Vector3(dpos.x, 5.75f, dpos.z);
            }
            for (int i = 0; i < 8; i++)
            {
                int cur_z = z;
                int cur_x = x;
                if (dir[i] == 0) continue;
                while (dir[i] > 0)
                {
                    if (i == 0)
                    {
                        cur_z--;
                        dir[i]--;
                        Board[cur_z, cur_x] = turn;
                        disks[cur_z, cur_x].Rotate(turn);
                    }
                    else if (i == 1)
                    {
                        cur_z--;
                        cur_x++;
                        dir[i]--;
                        Board[cur_z, cur_x] = turn;
                        disks[cur_z, cur_x].Rotate(turn);
                    }
                    else if (i == 2)
                    {
                        cur_x++;
                        dir[i]--;
                        Board[cur_z, cur_x] = turn;
                        disks[cur_z, cur_x].Rotate(turn);
                    }
                    else if (i == 3)
                    {
                        cur_z++;
                        cur_x++;
                        dir[i]--;
                        Board[cur_z, cur_x] = turn;
                        disks[cur_z, cur_x].Rotate(turn);
                    }
                    else if (i == 4)
                    {
                        cur_z++;
                        dir[i]--;
                        Board[cur_z, cur_x] = turn;
						disks[cur_z, cur_x].Rotate(turn);
                    }
                    else if (i == 5)
                    {
                        cur_z++;
                        cur_x--;
                        dir[i]--;
                        Board[cur_z, cur_x] = turn;
						disks[cur_z, cur_x].Rotate(turn);
                    }
                    else if (i == 6)
                    {
                        cur_x--;
                        dir[i]--;
                        Board[cur_z, cur_x] = turn;
                        disks[cur_z, cur_x].Rotate(turn);
                    }
                    else if (i == 7)
                    {
                        cur_z--;
                        cur_x--;
                        dir[i]--;
                        Board[cur_z, cur_x] = turn;
						disks[cur_z, cur_x].Rotate(turn);
                    }

                }
            }
            var hands = Find_hand(Board,-1*turn);
            if (hands.Count > 0) turn *= -1;
			else
			{
                EndCheck();
                if (turn != 0)
				{
                    U_script.PassFlag = true;
                    Debug.Log("pass");
                }
			}
        }
	}

    public int[,] SetSimulation(int[,] brd, int z, int x,int turn, int[] dir)
    {
        brd[z, x] = turn;
        for (int i = 0; i < 8; i++)
        {
            int cur_z = z;
            int cur_x = x;
            if (dir[i] == 0) continue;
            while (dir[i] > 0)
            {
                if (i == 0)
                {
                    cur_z--;
                    dir[i]--;
                    brd[cur_z, cur_x] = turn;
                }
                else if (i == 1)
                {
                    cur_z--;
                    cur_x++;
                    dir[i]--;
                    brd[cur_z, cur_x] = turn;
                }
                else if (i == 2)
                {
                    cur_x++;
                    dir[i]--;
                    brd[cur_z, cur_x] = turn;
                }
                else if (i == 3)
                {
                    cur_z++;
                    cur_x++;
                    dir[i]--;
                    brd[cur_z, cur_x] = turn;
                }
                else if (i == 4)
                {
                    cur_z++;
                    dir[i]--;
                    brd[cur_z, cur_x] = turn;
                }
                else if (i == 5)
                {
                    cur_z++;
                    cur_x--;
                    dir[i]--;
                    brd[cur_z, cur_x] = turn;
                }
                else if (i == 6)
                {
                    cur_x--;
                    dir[i]--;
                    brd[cur_z, cur_x] = turn;
                }
                else if (i == 7)
                {
                    cur_z--;
                    cur_x--;
                    dir[i]--;
                    brd[cur_z, cur_x] = turn;
                }

            }
        }
        return brd;
    }
    
    public int Count_mark(int color, int[,] brd)
    {
        int count = 0;
        for (int z = 0; z < 8; z++)
        {
            for (int x = 0; x < 8; x++)
            {
                if (brd[z, x] == color) count++;
            }
        }
		return count;
    }
    public List<(int, int, int[])> Find_hand(int[,] brd, int tur) 
    {
        var hands = new List<(int, int, int[])>();
        for (int z = 0; z < 8; z++)
        {
            for (int x = 0; x < 8; x++)
            {
                if (brd[z, x] == 0)
                {
                    int[] dir = disks[z, x].Checkhand(z, x, tur, brd);
                    if (dir.Sum() > 0)
                    {
                        hands.Add((z, x, dir));
                    }
                }
            }
        }
        return hands;
    }
}
