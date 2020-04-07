using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StopWatch : MonoBehaviour
{
	public Text timer;
	private float startTime;

	void Start()
	{
		startTime = Time.time;
	}
	void Update()
	{
		float t = Time.time - startTime;

		string minutes = ((int)t / 60).ToString("00");
		string seconds = (t % 60).ToString("00");

		timer.text = string.Format("{0}:{1}",minutes,seconds);
	
	}

}
