using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameSystem : MonoBehaviour
{

	//　スタートボタンを押したら実行する
	public void StartGame()
	{
		SceneManager.LoadScene("skillSetScene");
	}

	//　対局開始ボタンを押したら実行する
	public void MainGame()
	{
		SceneManager.LoadScene("mainScene");
	}

	//　タイトルに戻るボタンを押したら実行する
	public void BackScene()
	{
		SceneManager.LoadScene("Title");
	}

	//　ゲーム終了ボタンを押したら実行する
	public void EndGame()
	{
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_WEBPLAYER
				Application.OpenURL("http://www.yahoo.co.jp/");
#else
				Application.Quit();
#endif
	}
}
