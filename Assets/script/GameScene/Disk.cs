using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using DG.Tweening;
using System.Linq;

//�������ꂽ�f�B�X�N���Ǘ�����
public class Disk : MonoBehaviour
{
    
	//dir[8]:���Ԃ����Ƃ̂ł������(��C�E�΂ߏ�C�E�C�E�΂߉��C���C���΂߉��C���C���΂ߏ�)
	public void SetActive(bool value, int turn)   //disk�̕\���E��\���؂�ւ�
    {
        if (value == false) gameObject.SetActive(value);  //value��false�̏ꍇ�C��\���ɂ���
        else if (value == true && turn == 1)  //value��true�ō��̃^�[���̎�
        {
            Rotate(turn);
            gameObject.SetActive(value);  //���̌�C�\��
        }
        else if (value == true && turn == -1)  //���̃^�[���̏ꍇ
        {
            gameObject.SetActive(value);  //���̂܂ܕ\��
        }
    }
    
	public void Rotate(int turn)�@�@//�f�B�X�N�̉�]
	{
        //�f�B�X�N�̍����������悤�Ƀ^�[���ɂ����y���W�𒲐�
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

    public int[] Checkhand(int z, int x, int turn, int[,] board)  //�f�B�X�N���u���邩�̃`�F�b�N�C�u����ꍇ�̗��Ԃ������
    {
        int[] dir = new int[8];
        int cur_z = z-1;  //���ڂ��Ă���}�X
        int cur_x = x;
        int count = 0;  //���Ԃ���f�B�X�N�̐�
        while (cur_z >= 0)   //�����������
        {
            if (board[cur_z, cur_x] == 0) break;    //�󔒂̃}�X������ꍇ�͗��Ԃ��Ȃ�����break, dir��0�̂܂�
            else if (board[cur_z, cur_x] == turn)     //�����F�̃f�B�X�N���m�F���ꂽ��dir�ɗ��Ԃ��閇�����i�[
            {
                dir[0] = count;   //�����ׂɓ����F���������ꍇ�ł��C�i�[�����count��0�Ȃ̂Ŗ��Ȃ�
                break;
            }
            //�ǂ�ɂ����Ă͂܂�Ȃ�(�ׂ��Ⴄ�F)�ꍇ�C����ɗׂ̃}�X�����Ă����D
            cur_z--;   //������ֈړ�
            count++;  //���Ԃ������𑝂₷
        }
        cur_z = z - 1;
        cur_x = x + 1;
        count = 0;
        while (cur_z >= 0 && cur_x < 8)  //�E�΂ߏ������
        {
            if (board[cur_z, cur_x] == 0) break;    //�󔒂̃}�X������ꍇ�͗��Ԃ��Ȃ�����break, dir��0�̂܂�
            else if (board[cur_z, cur_x] == turn)     //�����F�̃f�B�X�N���m�F���ꂽ��dir�ɗ��Ԃ��閇�����i�[
            {
                dir[1] = count;   //�����ׂɓ����F���������ꍇ�ł��C�i�[�����count��0�Ȃ̂Ŗ��Ȃ�
                break;
            }
            cur_z--;
            cur_x++;
            count++;
        }
        cur_z = z;
        cur_x = x + 1;
        count = 0;
        while (cur_x < 8)  //�E������
        {
            if (board[cur_z, cur_x] == 0) break;    //�󔒂̃}�X������ꍇ�͗��Ԃ��Ȃ�����break, dir��0�̂܂�
            else if (board[cur_z, cur_x] == turn)     //�����F�̃f�B�X�N���m�F���ꂽ��dir�ɗ��Ԃ��閇�����i�[
            {
                dir[2] = count;   //�����ׂɓ����F���������ꍇ�ł��C�i�[�����count��0�Ȃ̂Ŗ��Ȃ�
                break;
            }
            cur_x++;
            count++;
        }
        cur_z = z + 1;
        cur_x = x + 1;
        count = 0;
        while (cur_z < 8 && cur_x < 8)  //�E�΂߉�������
        {
            if (board[cur_z, cur_x] == 0) break;    //�󔒂̃}�X������ꍇ�͗��Ԃ��Ȃ�����break, dir��0�̂܂�
            else if (board[cur_z, cur_x] == turn)     //�����F�̃f�B�X�N���m�F���ꂽ��dir�ɗ��Ԃ��閇�����i�[
            {
                dir[3] = count;   //�����ׂɓ����F���������ꍇ�ł��C�i�[�����count��0�Ȃ̂Ŗ��Ȃ�
                break;
            }
            cur_z++;
            cur_x++;
            count++;
        }
        cur_z = z + 1;
        cur_x = x;
        count = 0;
        while (cur_z < 8)  //��������
        {
            if (board[cur_z, cur_x] == 0) break;    //�󔒂̃}�X������ꍇ�͗��Ԃ��Ȃ�����break, dir��0�̂܂�
            else if (board[cur_z, cur_x] == turn)     //�����F�̃f�B�X�N���m�F���ꂽ��dir�ɗ��Ԃ��閇�����i�[
            {
                dir[4] = count;   //�����ׂɓ����F���������ꍇ�ł��C�i�[�����count��0�Ȃ̂Ŗ��Ȃ�
                break;
            }
            cur_z++;
            count++;
        }
        cur_z = z + 1;
        cur_x = x - 1;
        count = 0;
        while (cur_z < 8 && cur_x >= 0)  //���΂߉�������
        {
            if (board[cur_z, cur_x] == 0) break;    //�󔒂̃}�X������ꍇ�͗��Ԃ��Ȃ�����break, dir��0�̂܂�
            else if (board[cur_z, cur_x] == turn)     //�����F�̃f�B�X�N���m�F���ꂽ��dir�ɗ��Ԃ��閇�����i�[
            {
                dir[5] = count;   //�����ׂɓ����F���������ꍇ�ł��C�i�[�����count��0�Ȃ̂Ŗ��Ȃ�
                break;
            }
            cur_z++;
            cur_x--;
            count++;
        }
        cur_z = z;
        cur_x = x - 1;
        count = 0;
        while (cur_x >= 0)  //��������
        {
            if (board[cur_z, cur_x] == 0) break;    //�󔒂̃}�X������ꍇ�͗��Ԃ��Ȃ�����break, dir��0�̂܂�
            else if (board[cur_z, cur_x] == turn)     //�����F�̃f�B�X�N���m�F���ꂽ��dir�ɗ��Ԃ��閇�����i�[
            {
                dir[6] = count;   //�����ׂɓ����F���������ꍇ�ł��C�i�[�����count��0�Ȃ̂Ŗ��Ȃ�
                break;
            }
            cur_x--;
            count++;
        }
        cur_z = z - 1;
        cur_x = x - 1;
        count = 0;
        while (cur_z >= 0 && cur_x >= 0)  //���΂ߏ������
        {
            if (board[cur_z, cur_x] == 0) break;    //�󔒂̃}�X������ꍇ�͗��Ԃ��Ȃ�����break, dir��0�̂܂�
            else if (board[cur_z, cur_x] == turn)     //�����F�̃f�B�X�N���m�F���ꂽ��dir�ɗ��Ԃ��閇�����i�[
            {
                dir[7] = count;   //�����ׂɓ����F���������ꍇ�ł��C�i�[�����count��0�Ȃ̂Ŗ��Ȃ�
                break;
            }
            cur_z--;
            cur_x--;
            count++;
        }
        return dir;
    }
}
