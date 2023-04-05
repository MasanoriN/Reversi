using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartUI : MonoBehaviour
{
    public static int Player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BlackButtonDown()
	{
        GetComponent<AudioSource>().Play();
        Player = 1;
        StartCoroutine("LoadGameScene");
    }

    public void WhiteButtonDown()
	{
        GetComponent<AudioSource>().Play();
        Player = -1;
        StartCoroutine("LoadGameScene");
    }
    IEnumerator LoadGameScene()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("GameScene");
    }
}
