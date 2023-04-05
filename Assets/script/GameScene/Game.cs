using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    //�Q�[���Ǘ�
    // Start is called before the first frame update
    GameObject Counter;
    GameUI U_script;
    GameObject EnemyPlayer;
    EnemyPlayer E_script;
    GameObject Pointer;
    public Disk DiskPrefab;
    public int[,] Board = new int[8,8];       //�Ֆʂ̏󋵔c���@board[z(�c),x�i���j]�@-1:���C0:�Ȃ�,1:��
    Disk[,] disks = new Disk[8,8];     //��������disk�̊Ǘ�
    int turn;       //���݂̃^�[���i-1:���C1:���j
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
        for (int z = 3; z <= 4; z++)   //�����̂S��z�u
        {
            for (int x = 3; x <= 4; x++)
            {
                if (z == x) Board[z, x] = -1;
                else Board[z, x] = 1;
            }
        }
        for (int z = 0; z < 8; z++)  //Disk�𐶐��C�z�u
        {
            for (int x = 0; x < 8; x++)
            {
                var Disk = Instantiate(DiskPrefab);
                if (Board[z, x] == 0)
                {
                    Disk.transform.position = new Vector3(10 * x, 5.25f, -5 - 10 * z);
                    Disk.SetActive(false,turn);  //�ł���Ă��Ȃ��f�B�X�N�͉B��
                }
                else if (Board[z, x] == -1) Disk.transform.position = new Vector3(10 * x, 5.25f, -5 - 10 * z);
                else if (Board[z, x] == 1)
                {
                    //���Ԃ����Ƃ��Ƀ{�[�h�ɖ�����Ȃ��悤y���W��0.5��
                    Disk.transform.position = new Vector3(10 * x, 5.5f, -5 - 10 * z);   
                    Disk.transform.rotation = Quaternion.Euler(180, 0, 0);     //���ɗ��Ԃ�
                }
                disks[z, x] = Disk;    //��������Disk��disks�Ɋi�[
            }
        }

    }

	private void Update()
	{
        if (turn == 0 && turnflag == false)  //�^�[��0�̂Ƃ��^�[���̓r���łȂ���ΏI��������
        {
            Invoke("GameEnd",2.5f);
            turnflag = true;
        }
        else if (turn == -1*StartUI.Player && turnflag == false)  //enemy�̃^�[�����������łȂ��ꍇ
		{
            turnflag = true;  //�������ł��邱�Ƃ������t���O��True��
            //Whiteturn���Ăяo���D
            //�v���C���[���u������ɏ������Ԃ�������
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
    void EndCheck() //�Q�[���I���m�F
    {
        int black = Count_mark(1,Board);  //���f�B�X�N�̐�
        int white = Count_mark(-1,Board); //���f�B�X�N�̐�
        var hands = Find_hand(Board, turn); //���݂̃^�[���̑łĂ��
        //�ǂ�����łĂȂ��Ƃ��C�܂��͑S�}�X���܂����Ƃ�
        if (hands.Count == 0 || black + white == 64)
        {
            turn = 0;  //�^�[����0�ɂ���i�J�[�\�������������C���̃^�[�����n�܂�Ȃ��j
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
