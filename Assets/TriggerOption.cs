using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerOption : MonoBehaviour
{
	public bool isTouched = false;
	public int index;
    
	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Hand") isTouched = true;
	}
	private void OnTriggerExit(Collider other)
	{
		if (other.tag == "Hand") isTouched = false;
	}

}
