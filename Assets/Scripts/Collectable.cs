using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
	public GameObject effect;

    public void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag("Player"))
		{
			Pickup();
		}
	}
	
	void Pickup()
	{
		Instantiate(effect, transform.position, transform.rotation);

		StatsData.collectables++;

		Destroy(gameObject);
	}
}
