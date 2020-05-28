using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Key_UI : MonoBehaviour
{
	public GameObject keyUI;
	
	void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Player")
		{
			keyUI.SetActive(true);
			Destroy(gameObject);
		}
	}
}
