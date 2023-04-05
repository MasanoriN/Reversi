using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameUI : MonoBehaviour
{
    //�Ֆʂ̖����ɑ΂��銄���Ńo�[���쐬
    //���݂̂��ꂼ��̐F�̖�����\��
    //�Q�[���I�����ɂ͏I�����b�Z�[�W��\��
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
        CounterBar = GameObject.Find("CounterBar");  //��ʏ㕔�̃o�[
        BlackCount = GameObject.Find("BlackCount");  //�o�[���̍��f�B�X�N�J�E���^�[
        WhiteCount = GameObject.Find("WhiteCount");  //�o�[�E�̔��f�B�X�N�J�E���^�[
        PassText = GameObject.Find("PassText");      //�p�X���ꂽ�Ƃ��ɕ\�������e�L�X�g
        PassText.SetActive(false);
        PassFlag = false;
        t = 0;
        counter = CounterBar.GetComponent<Slider>();
        float max = 100f;   //�X���C�h�o�[�̏����100��
        float current = 50f;   //���݂̒l�F�ŏ���50���i�����z�u��2�F2�j
        counter.maxValue = max;
        counter.value = current;
    }

    void Update()
    {
        float black = G_script.Count_mark(1, G_script.Board);  //���f�B�X�N�̐�
        float white = G_script.Count_mark(-1, G_script.Board);   //���f�B�X�N�̐�
        float board = black + white;                           //�Ֆʂɂ���f�B�X�N�̐�

        //���ꂼ��̖����𕶎���ɕϊ�
        string black_txt = black.ToString();
        string white_txt = white.ToString();


        //10�������̏ꍇ0������i��j1 -> 01
        if (black < 10) black_txt = "0" + black_txt;
        if (white < 10) white_txt = "0" + white_txt;
        counter.value = (black / board) * 100;   //�X���C�h�o�[�̒l���v�Z�i�{�[�h�ɂ����鍕�̐�߂銄���j
        //�J�E���^�[�̃e�L�X�g���X�V
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

