using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class PauseMenu : MonoBehaviour
{
	public static bool isGamePause = false;
	public GameObject Pause_Panel;

	MenuUI menuScript;

	public TMP_Text mouseInput;
	public TMP_Text volumeInput;

	public Slider mouseSlider;
	public Slider volumeSlider;

	public Animator transition_anim;
	public float transitionTime = 1f;

	//public GameObject Title_panel;
	//public GameObject Menu_panel;
	public GameObject Popup_panel;

	void Start()
	{
		menuScript = GetComponent<MenuUI>();
	}

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			if (isGamePause)
			{
				Resume();
			}
			else
			{
				Pause();
			}
		}
		
	}
	public void Resume()
	{
		Pause_Panel.SetActive(false);
		Time.timeScale = 1f;
		isGamePause = false;
	}
	void Pause()
	{
		Pause_Panel.SetActive(true);
		Time.timeScale = 0f;
		isGamePause = true;
	}
	public void Quit()
	{
		print("Quitting");
		Application.Quit();
	}
	public void Restore()
	{
		mouseInput.text = "3.2";
		volumeInput.text = "70";

		StatsData.mouseSensitivity = 3.2f * 10 + 70;
		StatsData.volumeLevel = 100;

		mouseSlider.value = 3.2f;
		volumeSlider.value = 70;

	}
	public void MouseSensitivity(float input)
	{
		mouseInput.text = input.ToString("F1");
		StatsData.mouseSensitivity = input * 10 + 70;

		// N*10 + 70
	}
	public void VolumeLevel(float input)
	{
		volumeInput.text = input.ToString();
		StatsData.volumeLevel = input;

	}
	public void LoadMenu()
	{
		SceneManager.LoadScene(0);
	}
	
}
