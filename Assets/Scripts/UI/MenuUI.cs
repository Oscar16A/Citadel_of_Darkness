using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
public class MenuUI : MonoBehaviour
{
	
	public TMP_Text mouseInput;
	public TMP_Text volumeInput;

	public Slider mouseSlider;
	public Slider volumeSlider;

	public Animator transition_anim;
	public float transitionTime = 1f;

	public GameObject Title_panel;
	public GameObject Menu_panel;
	public GameObject Popup_panel;
	public GameObject transitionUI_img;
	private bool title = true;
		
	public void Start()
	{
		if(StatsData.loadingFromPauseMenu)
		{
			Title_panel.SetActive(false);
			Menu_panel.SetActive(true);
			transitionUI_img.SetActive(false);
		}
		Cursor.lockState = CursorLockMode.None;
	}
	public void Update()
	{
		//print("update");
		if(Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter) && title)
		{
			StartCoroutine((Transition()));	
		}
	}
    public void Play()
	{
		print("Playing next level...");
		StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
	}
	IEnumerator Transition()
	{
		title = false;
		transition_anim.SetTrigger("TransitionLevel");
		Title_panel.SetActive(false);
		yield return new WaitForSeconds(transitionTime/2);
		transition_anim.SetTrigger("Start");
		Menu_panel.SetActive(true);
	}
		
	IEnumerator LoadLevel(int index)
	{
		transition_anim.SetTrigger("TransitionLevel");
		yield return new WaitForSeconds(transitionTime);
		SceneManager.LoadScene(index);
	}
	IEnumerator QuitGame()
	{
		transition_anim.SetTrigger("TransitionLevel");
		Popup_panel.SetActive(false);
		yield return new WaitForSeconds(transitionTime);
		Application.Quit();
	}
	public void Quit()
	{
		print("Quitting...");
		StartCoroutine(QuitGame());
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

		//AUDIO
		//Sets Master Volume level to input value
		AkSoundEngine.SetRTPCValue("Master_Volume", input);

	}
	public void Restore()
	{
		mouseInput.text = "3.2";
		volumeInput.text = "70";

		StatsData.mouseSensitivity = 3.2f * 10 + 70;
		StatsData.volumeLevel = 100;

		mouseSlider.value = 3.2f;
		volumeSlider.value = 70;

		//AUDIO
		//Sets Master Volume level to volumeSlider.value 
		AkSoundEngine.SetRTPCValue("Master_Volume", volumeSlider.value);

	}
}
