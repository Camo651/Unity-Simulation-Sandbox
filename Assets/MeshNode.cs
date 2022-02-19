using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshNode : MonoBehaviour
{
	public int UID;
	public MeshGen m;
	public List<LineRenderer> lines = new List<LineRenderer>();
	public List<MeshNode> connectedNodes = new List<MeshNode>();
	public Vector2 velocity;

	public void FixedUpdate()
	{
		if (transform.position.x > m.bounds*1.6f || transform.position.x < -m.bounds*1.6f)
			velocity.x *= -1;
		if (transform.position.y > m.bounds*.9f || transform.position.y < -m.bounds*.9f)
			velocity.y *= -1;

		transform.Translate(velocity);
	}
	public void UpdateNode()
	{
		//update all nodes
		for (int i = 0; i < connectedNodes.Count; i++)
		{
			float distance = Vector2.Distance(transform.position, connectedNodes[i].transform.position);
			if (distance > m.maxConnectionDistance)
			{
				GameObject lr = lines[i].gameObject;
				lines.RemoveAt(i);
				connectedNodes.RemoveAt(i);
				Destroy(lr);
			}
			else
			{
				lines[i].startColor = m.falloffGrad.Evaluate(distance / m.maxConnectionDistance);
				lines[i].endColor = m.falloffGrad.Evaluate(distance / m.maxConnectionDistance);

				lines[i].SetPosition(0, transform.position);
				lines[i].SetPosition(1, connectedNodes[i].transform.position);
			}
		}
	}
}
