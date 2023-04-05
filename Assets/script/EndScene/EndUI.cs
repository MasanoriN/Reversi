using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class EndUI : MonoBehaviour
{
	GameObject BlackText;
	GameObject WhiteText;
	GameObject ResultText;
	[SerializeField] AudioSource clickSource;
	[SerializeField] AudioSource winSource;
	[SerializeField] AudioSource loseSource;
	[SerializeField] AudioSource drawSource;
	[SerializeField] AudioClip click;
	[SerializeField] AudioClip win;
	[SerializeField] AudioClip lose;
	[SerializeField] AudioClip draw;

	// Start is called before the first frame update
	private void Start()
	{
		ResultText = GameObject.Find("ResultText");
		BlackText = GameObject.Find("BlackText");
		WhiteText = GameObject.Find("WhiteText");
		BlackText.GetComponent<TextMeshProUGUI>().text = "Å~" + Game.EndBlack;
		WhiteText.GetComponent<TextMeshProUGUI>().text = "Å~" + Game.EndWhite;
		if (Game.EndBlack == Game.EndWhite)
		{
			ResultText.GetComponent<TextMeshProUGUI>().text = "DRAW";
			ResultText.GetComponent<TextMeshProUGUI>().color = new Color(0.05f, 0.4f, 0.1f, 1.0f);
			drawSource.PlayOneShot(draw);
		}
		else if (StartUI.Player == 1)
		{
			if (Game.EndBlack > Game.EndWhite)
			{
				ResultText.GetComponent<TextMeshProUGUI>().text = "YOU WIN";
				ResultText.GetComponent<TextMeshProUGUI>().color = new Color(1.0f, 0f, 0f, 1.0f);
				winSource.PlayOneShot(win);
			}
			else if (Game.EndBlack < Game.EndWhite)
			{
				ResultText.GetComponent<TextMeshProUGUI>().text = "YOU LOSE";
				ResultText.GetComponent<TextMeshProUGUI>().color = new Color(0f, 0f, 1.0f, 1.0f);
				loseSource.PlayOneShot(lose);
			}
		}
		else
		{
			if (Game.EndBlack > Game.EndWhite)
			{
				ResultText.GetComponent<TextMeshProUGUI>().text = "YOU LOSE";
				ResultText.GetComponent<TextMeshProUGUI>().color = new Color(0f, 0f, 1.0f, 1.0f);
				loseSource.PlayOneShot(lose);
			}
			else if (Game.EndBlack < Game.EndWhite)
			{
				ResultText.GetComponent<TextMeshProUGUI>().text = "YOU WIN";
				ResultText.GetComponent<TextMeshProUGUI>().color = new Color(1.0f, 0f, 0f, 1.0f);
				winSource.PlayOneShot(win);
			}
		}
	}

	public void PlayButtonDown()
	{
		clickSource.PlayOneShot(click);
		StartCoroutine("LoadStartScene");
	}

	IEnumerator LoadStartScene()
	{
		yield return new WaitForSeconds(0.5f);
		SceneManager.LoadScene("StartScene");
	}
}
