using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClothNode : MonoBehaviour
{
	public ClothMesh m;
	public Vector2 tailPoint;
	public LineRenderer lr;
	public bool isLocked;
	public void SetTailPoint(Vector2 parentPosition, float spacing)
	{
		tailPoint = Vector2.Lerp(transform.position, parentPosition, spacing / Vector2.Distance(transform.position, parentPosition));
		lr.SetPosition(0, transform.position);
		lr.SetPosition(1, tailPoint);
	}
}
