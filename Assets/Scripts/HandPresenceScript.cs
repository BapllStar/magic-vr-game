using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class HandPresenceScript : MonoBehaviour
{
    public bool showController = false;
    public InputDeviceCharacteristics controllerCharacteristics;
    public List<GameObject> controllerPrefabs;
    public GameObject handModelPrefab;

    private InputDevice targetDevice;
    private GameObject spawnedController;
    private GameObject spawnedHandModel;
    private Animator handAnimator;

    //send out
    public float publicGripValue;
    public bool publicGripBool;
    public float publicTriggerValue;
    public bool publicTriggerBool;


    void Start()
    {
        StartCoroutine(GetMyDevices(2.0f));
    }

   IEnumerator GetMyDevices(float delayTime)
	{
        yield return new WaitForSeconds(delayTime);
        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(controllerCharacteristics, devices);
        Debug.Log("Detected " + devices.Count + " devices.");

        foreach (var item in devices)
        {
            Debug.Log(item.name + " | " + item.characteristics);
        }

       if (devices.Count > 0)
		{
            targetDevice = devices[0];
            GameObject prefab = controllerPrefabs.Find(controller => controller.name == targetDevice.name);
			if (prefab)
			{
                Instantiate(prefab, transform);
			}
			else
			{
                Debug.LogError("Did not find corresponding controller model");
                spawnedController = Instantiate(controllerPrefabs[0], transform);
			}

            spawnedHandModel = Instantiate(handModelPrefab, transform);
            handAnimator = spawnedHandModel.GetComponent<Animator>();
        }
            
            
    }

    void UpdateHandAnimation()
	{
        if (targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue))
		{
            handAnimator.SetFloat("Trigger", triggerValue);
		}
		else
		{
            handAnimator.SetFloat("Trigger", 0);
		}

        if (targetDevice.TryGetFeatureValue(CommonUsages.grip, out float gripValue))
        {
            handAnimator.SetFloat("Grip", gripValue);
            publicGripValue = gripValue;
        }
        else
        {
            handAnimator.SetFloat("Grip", 0);
        }
    }

    void Update()
    {
        if (targetDevice.TryGetFeatureValue(CommonUsages.grip, out float gripValue))
        {
            publicGripValue = gripValue;
        }
        if (targetDevice.TryGetFeatureValue(CommonUsages.gripButton, out bool gripButtonBool))
        {
            publicGripBool = gripButtonBool;
        }
        if (targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue))
        {
            publicTriggerValue = triggerValue;
        }
        if (targetDevice.TryGetFeatureValue(CommonUsages.triggerButton, out bool triggerButtonBool))
        {
            publicTriggerBool = triggerButtonBool;
        }


        if (showController)
		{
            spawnedHandModel.SetActive(false);
            spawnedController.SetActive(true);
		}
		else
		{
            spawnedHandModel.SetActive(true);
            spawnedController.SetActive(false);
            UpdateHandAnimation();
		}
    }
}
