using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorScript : MonoBehaviour
{
    public GameObject mirrorCam;
	public Vector2 rotation;
	private void Start()
	{
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
	}
	private void FixedUpdate()
	{
		transform.Translate((Vector3.forward * .2f * Input.GetAxis("Vertical") + (Vector3.right * .2f * Input.GetAxis("Horizontal"))));
		rotation.x = Mathf.Clamp(rotation.x + (-Input.GetAxis("Mouse Y")*4f), -90f, 90f);
		rotation.y = rotation.y + (Input.GetAxis("Mouse X")*4f);
		transform.GetChild(0).localEulerAngles = rotation;

		mirrorCam.transform.position = new Vector3(transform.position.x+15, transform.position.y, transform.position.z);
		mirrorCam.transform.LookAt(transform.GetChild(0));
	}
}
