using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGen : MonoBehaviour
{
	public List<MeshNode> meshNodes = new List<MeshNode>();
	public GameObject meshNodePrefab, lineRendererPrefab;
	public int nodeCount;
	public float velocityMax, bounds;
	public Gradient falloffGrad;
	public int maxConnectionDistance;

	private void Start()
	{
		for(int i = 0; i< nodeCount; i++)
		{
			meshNodes.Add(Instantiate(meshNodePrefab).GetComponent<MeshNode>());
			meshNodes[i].m = this;
			meshNodes[i].UID = i;
			meshNodes[i].transform.position = new Vector3(Random.Range(-bounds/2f, bounds/2f), Random.Range(-bounds/2f, bounds/2f), 0);
			meshNodes[i].velocity = new Vector2(((Random.value - .5f) * velocityMax * 2f), ((Random.value - .5f) * velocityMax * 2f));
		}
	}
	private void FixedUpdate()
	{
		foreach(MeshNode node in meshNodes)
		{
			foreach(MeshNode other in meshNodes)
			{
				if (node.UID > other.UID)
				{
					float distance = Vector2.Distance(node.transform.position, other.transform.position);
					if (!node.connectedNodes.Contains(other) && distance <= maxConnectionDistance)
					{
						//add
						node.connectedNodes.Add(other);
						node.lines.Add(Instantiate(lineRendererPrefab, node.transform).GetComponent<LineRenderer>());
					}
				}
			}
			node.UpdateNode();
		}
	}
}
