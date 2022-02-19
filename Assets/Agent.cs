using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent
{
	public float velocity, angle;
	public Vector2 position;

	public Agent()
	{
		position = Vector2.zero;
	}
	public Vector2Int GetIntPos()
	{
		return new Vector2Int((int)position.x, (int)position.y);
	}
}
