using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {
	// 小鼠
	public GameObject targetObject;
	// 相机与小鼠的距离
	private float distanceToTarget;

	// Use this for initialization
	void Start () {
		distanceToTarget = transform.position.x - targetObject.transform.position.x;
	}
	
	// Update is called once per frame
	void Update () {
		float targetObjectX = targetObject.transform.position.x;

		Vector3 newCameraPosition = transform.position;
		newCameraPosition.x = targetObjectX + distanceToTarget;
		transform.position = newCameraPosition;
	}
}
