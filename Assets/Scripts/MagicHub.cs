using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicHub : MonoBehaviour
{
    public GameObject handPresence;
    private HandPresenceScript handPresenceScript;
    private float gripValue;
    private bool gripBool;
    private float triggerValue;
    private bool triggerBool;
    public GameObject trailObject;
    public GameObject magicOptionPrefab;
    public List<GameObject> summonedOptions = new List<GameObject> { };
    public GameObject cameraOffset;
    public float optionDistance = 0.2f;
    public float angleOffset = 35.0f;
    public bool isInverted = false;
    public List<int> magicNumbers = new List<int> { };
    //private LineRenderer lineRenderer;

    void Start()
    {
        handPresenceScript = handPresence.GetComponent<HandPresenceScript>();
        //lineRenderer = GetComponent<LineRenderer>();
    }

    void Update()
    {
        gripValue = handPresenceScript.publicGripValue;
        gripBool = handPresenceScript.publicGripBool;
        triggerValue = handPresenceScript.publicTriggerValue;
        triggerBool = handPresenceScript.publicTriggerBool;

        if (triggerBool)
		{
            transform.localPosition = Vector3.zero;
            magicNumbers.Clear();
            trailObject.GetComponent<TrailRenderer>().emitting = triggerBool;
            if (summonedOptions.Count == 0)
			{
                SummonOptions(5);
            }
		}
		else
		{
            trailObject.GetComponent<TrailRenderer>().emitting = triggerBool;
            if (summonedOptions.Count != 0)
			{
				foreach (var item in summonedOptions)
				{
                    Destroy(item);
                    summonedOptions.Remove(item);
				}
            }
        }

		foreach (var item in summonedOptions)
		{
			if (item.GetComponent<TriggerOption>().isTouched)
			{
                transform.localPosition = item.transform.localPosition;
                magicNumbers.Add(item.GetComponent<TriggerOption>().index);
                foreach (var itemTwo in summonedOptions)
				{
                    Destroy(item);
                    summonedOptions.Remove(item);
				}
            }
			
		}
        

        /*lineRenderer.positionCount = summonedOptions.Count * 2;
		for (int i = 0; i < summonedOptions.Count*2; i += 2)
		{
            lineRenderer.SetPosition(i, summonedOptions[(i+1)/2].transform.position);
            lineRenderer.SetPosition(i+1, transform.position);
        }*/



    }

    private void SummonOptions(int count)
	{
        float angle = 2 * Mathf.PI / count;
        float angleRadianOffset = angleOffset * Mathf.PI / 180;
        int inverter;
        if (isInverted) inverter = -1;
        else inverter = 1;
        
        if (count > 1)
		{
            for (int i = 0; i < count; i++)
            {
                Vector3 offset = new Vector3(inverter * optionDistance * Mathf.Cos(i * angle + angleRadianOffset), inverter * optionDistance * Mathf.Sin(i * angle + angleRadianOffset), 0);
                GameObject instance = Instantiate(magicOptionPrefab, transform);
                instance.transform.localPosition += offset;
                instance.transform.SetParent(cameraOffset.transform);
                instance.GetComponent<TriggerOption>().index = i;
                summonedOptions.Add(instance);
            }
        }   
		else
		{
            GameObject instance = Instantiate(magicOptionPrefab, transform);
            instance.transform.SetParent(cameraOffset.transform);
        }
    }

}
