using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameSystem : MonoBehaviour
{

	//�@�X�^�[�g�{�^��������������s����
	public void StartGame()
	{
		SceneManager.LoadScene("skillSetScene");
	}

	//�@�΋ǊJ�n�{�^��������������s����
	public void MainGame()
	{
		SceneManager.LoadScene("mainScene");
	}

	//�@�^�C�g���ɖ߂�{�^��������������s����
	public void BackScene()
	{
		SceneManager.LoadScene("Title");
	}

	//�@�Q�[���I���{�^��������������s����
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
