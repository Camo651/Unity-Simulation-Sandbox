using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClothMaker : MonoBehaviour
{
	public Camera mainCam;
	public GameObject cmPrefab;
	public ClothMesh currentMesh;
	public GameObject nodePrefab;
	public float nodeDistance, gravityModifier;

	public void Update()
	{
		Vector2 mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
		if (Input.GetMouseButton(0))
		{
			if(currentMesh == null)
			{
				currentMesh = Instantiate(cmPrefab).GetComponent<ClothMesh>();
				currentMesh.meshGenerated = false;
				currentMesh.gravityModifier = gravityModifier;
				currentMesh.nodeDistance = nodeDistance;
				currentMesh.SetPosition(0, mousePos);
				currentMesh.AddNewNode(mousePos, this, true);
			}
			else
			{
				if (Vector2.Distance(currentMesh.clothMatrix[currentMesh.clothMatrix.Count - 1].transform.position, mousePos) >= nodeDistance)
				{
					currentMesh.AddNewNode(mousePos, this, Input.GetKey(KeyCode.LeftShift));
					//currentMesh.Init();
				}
			}
		}
		else
		{
			if (currentMesh != null)
			{
				currentMesh.SetPosition(1, mousePos);
				currentMesh.Init();
				currentMesh = null;
			}
		}
	}
}
