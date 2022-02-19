using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClothMesh : MonoBehaviour
{
	public List<ClothNode> clothMatrix = new List<ClothNode>();
	public Camera mainCam;
	public int nodeCount;
	public float nodeDistance, gravityModifier;
	float ropeLength;
	private int L;
	public GameObject clothNodePrefab;
	Vector2 origin, target;
	public float averageFault, faultTolerence;
	public bool meshGenerated;

	public void Init()
	{
		meshGenerated = false;
		mainCam = Camera.main;
		averageFault = 100;
		ropeLength = nodeCount * nodeDistance;
		L = nodeCount - 1;
		meshGenerated = true;
	}
	private void FixedUpdate()
	{
		if (meshGenerated)
		{
			InverseKinematics();
		}
	}
	public void SetPosition(int i, Vector2 v)
	{
		if (i == 1)
			origin = v;
		else
			target = v;
	}
	public void AddNewNode(Vector2 pos, ClothMaker cm, bool isLocked)
	{
		clothNodePrefab = cm.nodePrefab;
		ClothNode a = Instantiate(clothNodePrefab).GetComponent<ClothNode>();
		clothMatrix.Add(a);
		a.m = this;
		a.isLocked = isLocked;
		a.transform.position = pos;
		nodeCount++;
	}
	private void InverseKinematics()
	{
		for (int i = 0; i < nodeCount; i++)
		{
			if(!clothMatrix[i].isLocked)
				clothMatrix[i].transform.position = (Vector2)clothMatrix[i].transform.position + (Vector2.down * 9.81f*gravityModifier);
		}
		averageFault = (Vector2.Distance(clothMatrix[0].transform.position, target) + Vector2.Distance(clothMatrix[clothMatrix.Count - 1].transform.position, origin)) / 2f;
		if (averageFault <= faultTolerence)
		{
			return;
		}
		else
		{
			clothMatrix[0].transform.position = target;
			clothMatrix[0].SetTailPoint(clothMatrix[1].transform.position, nodeDistance);
			for (int i = 1; i < nodeCount - 1; i++)
			{
				if(!clothMatrix[i].isLocked)
					clothMatrix[i].transform.position = clothMatrix[i - 1].tailPoint;
				clothMatrix[i].SetTailPoint(clothMatrix[i + 1].transform.position, nodeDistance);
			}
			clothMatrix[L].transform.position = origin;
			clothMatrix[L].SetTailPoint(clothMatrix[L - 1].transform.position, nodeDistance);
			for (int i = L - 1; i > 0; i--)
			{
				if(!clothMatrix[i].isLocked)
					clothMatrix[i].transform.position = clothMatrix[i + 1].tailPoint;
				clothMatrix[i].SetTailPoint(clothMatrix[i - 1].transform.position, nodeDistance);
			}
		}	
	}
}
