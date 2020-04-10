using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
public class MenuUI : MonoBehaviour
{
	//MouseMovement mMove;
	public TMP_Text mouseInput;
	public TMP_Text volumeInput;

	public void Awake()
	{
		//mMove = GetComponent<MouseMovement>();
	}
    public void Play()
	{
		print("Playing next level...");
	
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}
	public void Quit()
	{
		print("Quitting...");

		Application.Quit();
	}
	public void MouseSensitivity(float input)
	{    
		mouseInput.text = input.ToString("F1");
		StatsData.mouseSensitivity = input * 10 + 70;
		print("statsData.mouse=" + StatsData.mouseSensitivity);

		//print(input);
		// N*10 + 70
	}
	public void VolumeLevel(float input)
	{
		volumeInput.text = input.ToString();
		StatsData.volumeLevel = input;
		print("statsData.volume=" + StatsData.volumeLevel);
	}
}
