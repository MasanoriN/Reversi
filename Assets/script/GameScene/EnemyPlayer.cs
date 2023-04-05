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
        //�Ֆʂ̕]��
        //�p���ł������C�p�Ɍq�����Ă��܂��}�X�͕]�����
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
        var best = Search(brd, -1*StartUI.Player, D);  //�x�X�g�Ȏ��T��(�Ԃ�l�F�{�[�h�C�őP��)
        int[] hand = best.Item2;  //�őP��
        if (hand != null)
        {
            G_script.Setboard(hand[0], hand[1]);  //�T���������ł�
            string s = "(" + hand[0] + "," + hand[1] + ")";
            //Debug.Log(s);  //�ł�������o�́i�m�F�p�j
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
        if (depth == 0 || hands.Count == 0) return (brd, null);  //D���܂œǂݐ؂����ꍇ�C�ł肪�Ȃ��ꍇ�C�Ֆʂ݂̂�Ԃ�
        foreach (var hand in hands)
        {
            z = hand.Item1;
            x = hand.Item2;
            dir = hand.Item3;
            if ((z == 0 || z == 7) && (x == 0 || x == 7))
            {//�p������ꍇ�͂Ƃ�
                best_hand[0] = z;
                best_hand[1] = x;
                brd_best = G_script.SetSimulation(brd, z, x, turn, dir);   //�őP�̔ՖʂɊp���Ƃ����Ֆʂ�ݒ�
                break;  //foreach�𔲂���i�V���Ȏ�̒T������߂�j
            }
            brd_nxt = G_script.SetSimulation(brd, z, x, turn, dir);  //�����ɑł����ꍇ�̔Ֆʁi�����F���݂̔ՖʁC���W�C���Ԃ��F�C�����j
            var s = Search(brd_nxt, -1 * turn, depth - 1);  //���̎���T���i�ċA�F�[�����P���炷�j
            brd_nxt_nxt = s.Item1; //�T�������Ֆʂ��擾
            //���̎��ɂ����Ă��ꂼ��̐F����_�����擾���C�����Ƃ�i�������荂���_�����Ƃ��Ă�����̂��悢�_���Ƃ���j
            //�]���֐��̈����i�ՖʁC�^�[���C�]������F�j
            score_diff = Calc_score(brd_nxt_nxt, turn, turn, dir.Sum()) - Calc_score(brd_nxt_nxt, turn, -1*turn, dir.Sum());
            if (score_diff > score_max)
            {//�X�R�A���X�V��������W�C�ՖʁC�ō��X�R�A���X�V����
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
        int score = 0;  //�X�R�A
        float w1 = 0;  //�Ֆʕ]���̏d��
        float w2 = 0;  //�萔�]���̏d��
        int disk_board = G_script.Count_mark(1, brd) + G_script.Count_mark(-1, brd);  //�{�[�h��ɂ���f�B�X�N����
        var corners = new List<(int, int)>();     //�p�ɂ�����Ă���f�B�X�N���W���i�[����
        bool c_flag = false;    //�ӂ̊p�ׂ̃}�X�ɕ]�����Ă��鑤�̃f�B�X�N���ł���Ă����true
        switch (disk_board)
        {
            case { } n when (n <= 15):
                w1 = 3;  //���ՂقǔՖʂɐݒ肳��Ă���X�R�A��傫�����f
                w2 = 1;  //�łĂ��̐��ɂ�����d��
                if(turn == color)   score -= 3 * a;
                break;

            case { } n when (n <= 45):
                w1 = 2;
                w2 = 4;
                if (turn == color)  score -= 2*a;
                break;

            case { } n when (n > 45):
                //�I�Ղ͖����Ǝ萔��Ֆʕ]���l��蔽�f������
                w1 = 0;
                w2 = 10;
                score += 2 * G_script.Count_mark(color, brd);
                break;
        }

        //�Ֆʕ]���l�ɂ��X�R�A���Z�C�p�Ƀf�B�X�N�̒u����Ă��邩�̊m�F
        for (int z = 0; z < 8; z++)
        {
            for (int x = 0; x < 8; x++)
            {
                if (brd[z, x] == color && disk_board <= 45) score += (int)w1 * brd_score1[z, x];  //������Ă���f�B�X�N�̐F�������F��������X�R�A�����Z
                else if (brd[z, x] == color && disk_board > 45) score += (int)w1 * brd_score2[z, x];
                if ((z == 0 || z == 7) && (x == 0 || x == 7) && brd[z, x] == color)  //�p�Ƀf�B�X�N��������Ă����烊�X�g�ɒǉ�
                {
                    corners.Add((z, x));
                }
                else if ((z == 1) && (x == 0 || z == 7) && brd[z, x] == color) c_flag = true;
                else if ((x == 1) && (z == 0 || z == 7) && brd[z, x] == color) c_flag = true;
            }
        }

        //���Ԃ����Ƃ̂ł��Ȃ��m��΂ɂ��X�R�A���Z
        if (corners.Count > 0)
		{
            foreach (var corner in corners)
			{  
                int z = corner.Item1;
                int x = corner.Item2;
                while (z >= 0)
				{//������Ɍ���
                    if (brd[z, x] == color) score += 30;  
                    if (brd[z, x] != color) break;  //�f�B�X�N�̘A�����r�؂ꂽ��break
                    z--;
				}

                z = corner.Item1;
                while (z < 8)
				{//����������
                    if (brd[z, x] == color) score += 30;
                    if (brd[z, x] != color) break;
                    z++;
				}

                z = corner.Item1;
                while (x >= 0)
				{//������������
                    if (brd[z, x] == color) score += 30;
                    if (brd[z, x] != color) break;
                    x--;
				}

                x = corner.Item2;
                while (x < 8)
				{//�E����������
                    if (brd[z, x] == color) score += 30;
                    if (brd[z, x] != color) break;
                    x++;
				}
            }
		}
        
        else if(c_flag)
		{
            //�ǌ^�Ƃ����R�ɂ��X�R�A���Z
            //�R�F�ӂɂ����Ċp�ȊO�ɂ��ׂăf�B�X�N���u����Ă���`
            //�܂�����̃f�B�X�N���Ȃ��C�����̃f�B�X�N���A�����Ă���ꍇ�����_
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

        //�萔�ɂ��X�R�A���Z
        //�^�[���ƕ]�����鑤�̐F�������ꍇ�C�łĂ�萔���X�R�A�ɉ��Z
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
