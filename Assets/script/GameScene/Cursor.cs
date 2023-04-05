using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor : MonoBehaviour
{�@�@//�J�[�\���̓������Ǘ�
    GameObject Game;
    Game G_script;
    int z , x ; 

    // Start is called before the first frame update
    void Start()
    {
        Game = GameObject.Find("Game"); //Game�I�u�W�F�N�g�ɃA�N�Z�X
        G_script = Game.GetComponent<Game>();  //Game�I�u�W�F�N�g�̃X�N���v�g�ɃA�N�Z�X
        //�J�[�\���̂���}�X���W���iz,x�j�ŕ\��
        //�ŏ��͍���i0,0�j
        z = 0;
        x = 0;
    }
    

    public void UButtonDown()
	{
        if (z != 0)  //��[�iz=0�j���Ɠ����Ȃ�
        {
            GetComponent<AudioSource>().Play();
            transform.Translate(0, 0, 10);
            z--;
        }
    }

	public void DButtonDown()
	{
        if (z != 7)  //���[�iz=7�j���Ɠ����Ȃ�
        {
            GetComponent<AudioSource>().Play();
            transform.Translate(0, 0, - 10);
            z++;
        }
    }

    public void RButtonDown()
	{
        if (x != 7)//�E�[�ix=7�j�̂Ƃ��͓����Ȃ�
        {
            GetComponent<AudioSource>().Play();
            transform.Translate(10, 0, 0);
            x++;  //x���X�V
        }
    }

    public void LButtonDown()
	{
        if (x != 0)  //���[�ix=0�j���Ɠ����Ȃ�
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
            G_script.Playerturn(z, x);  //Blackturn�ɃJ�[�\���̌��݂̃}�X���W��n��
        }
    }
}
