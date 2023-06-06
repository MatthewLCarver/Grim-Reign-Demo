using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
	public CinemachineVirtualCamera cinemachineBrain;
	public float speed = 5f;
	//public float distance = 10f;
	public float minLookDistance = 0f;
	public float maxLookDistance = 10f;
	
	void Start()
	{
		//currentDistance = distance;
	}

	/// <summary>
	/// Will assist with obstructions of the camera and will move the camera forward to avoid camera clipping through
	/// obstacles
	/// </summary>
	void Update()
	{
		cinemachineBrain.transform.position = Vector3.MoveTowards(
			cinemachineBrain.transform.position, 
			cinemachineBrain.Follow.position, 
			Time.deltaTime * speed);
	}
}