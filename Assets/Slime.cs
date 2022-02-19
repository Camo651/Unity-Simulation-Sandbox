using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour
{
	public GameObject tileSprite;
	public Vector2Int regionSize;
	public float pixelScale;
	public int agentCount;
	public float agentSpeed;
	public float senseOffset;
	public float steerStength;
	[Range(0,0.05f)]public float pheremoneDecay;

	public SpriteRenderer[][] colourMap;
	public float[][] baseMap;
	public Agent[] agents;

	private void Start()
	{
		//Initiation
		colourMap = new SpriteRenderer[regionSize.x][];
		baseMap = new float[regionSize.x][];
		for(int i=0;i<regionSize.x;i++)
		{
			colourMap[i] = new SpriteRenderer[regionSize.y];
			baseMap[i] = new float[regionSize.y];
			for(int y = 0; y < regionSize.y; y++)
			{
				colourMap[i][y] = Instantiate(tileSprite).GetComponent<SpriteRenderer>();
				colourMap[i][y].color = Color.black;
				colourMap[i][y].transform.position = new Vector2(i*pixelScale, y*pixelScale);
				colourMap[i][y].transform.localScale = Vector2.one * pixelScale;

				baseMap[i][y] = 0f;
			}
		}

		//Agents
		agents = new Agent[agentCount];
		for(int i = 0; i < agentCount; i++)
		{
			agents[i] = new Agent();
			agents[i].position = new Vector2(regionSize.x / 2, regionSize.y / 2);
			agents[i].angle = Random.value * 360f;
		}


		for (int x = -(regionSize.x / 2); x < regionSize.x-(regionSize.x/2); x++)
		{
			for (int y = -(regionSize.x / 2); y < regionSize.y-(regionSize.x / 2); y++)
			{
				float value = 1f;// ((x & y) | y) / (1f*regionSize.y);
				colourMap[x+(regionSize.x / 2)][y+(regionSize.x / 2)].color = Color.HSVToRGB((x^y)/255f, .8f, value);
			}
		}
		print(agents[0].GetType());
	}

	Vector2Int[] posOffset =
	{
		new Vector2Int(-1,1),
		new Vector2Int(0,1),
		new Vector2Int(1,1),
		new Vector2Int(1,0),
		new Vector2Int(0,0),
		new Vector2Int(-1,0),
		new Vector2Int(-1,-1),
		new Vector2Int(0,-1),
		new Vector2Int(1,-1)
	};
	List<int> deltasX = new List<int>();
	List<int> deltasY = new List<int>();
	List<float> deltasZ = new List<float>();
	public void FixedUpdate()
	{
		if (false)
		{
			//Agent handling
			for (int i = 0; i < agentCount; i++)
			{
				Agent a = agents[i];

				a.position = new Vector2(Mathf.Clamp(a.position.x + Mathf.Cos(toRadians(a.angle)) * agentSpeed, 1, regionSize.x - 1),
					Mathf.Clamp(a.position.y + Mathf.Sin(toRadians(a.angle)) * agentSpeed, 1, regionSize.y - 1));
				if (a.position.x > regionSize.x - 1.5f)
					a.position.x = 2f;
				if (a.position.x < 1.5f)
					a.position.x = regionSize.x - 2f;
				if (a.position.y > regionSize.y - 1.5f)
					a.position.y = 2f;
				if (a.position.y < 1.5f)
					a.position.y = regionSize.y - 2f;
				float left = Sense(a.position, a.angle, senseOffset);
				float right = Sense(a.position, a.angle, -senseOffset);
				float none = Sense(a.position, a.angle, 0f);

				if (left > right && left > none)
					a.angle += steerStength;
				else if (right > left && right > none)
					a.angle -= steerStength;
				else
					a.angle += (Random.value - .5f) / 3f;

				baseMap[a.GetIntPos().x][a.GetIntPos().y] = 1f;
			}

			//Diffusing the base map
			for (int x = 0; x < regionSize.x; x++)
			{
				for (int y = 0; y < regionSize.y; y++)
				{
					float sum = 0f;
					for (int i = 0; i < 9; i++)
					{
						sum += baseMap[Mathf.Clamp(x + posOffset[i].x, 0, regionSize.x - 1)][Mathf.Clamp(y + posOffset[i].y, 0, regionSize.y - 1)];
					}
					sum /= 9f;
					if (sum != baseMap[x][y])
					{
						deltasX.Add(x);
						deltasY.Add(y);
						deltasZ.Add(Mathf.Clamp(sum - pheremoneDecay, 0, 1f));
					}
				}
			}

			//Changing the base map after diffusion
			for (int i = 0; i < deltasX.Count; i++)
			{
				colourMap[deltasX[i]][deltasY[i]].color = new Color(deltasZ[i], deltasZ[i], deltasZ[i], 1f);
				baseMap[deltasX[i]][deltasY[i]] = deltasZ[i];
			}
			deltasX.Clear();
			deltasY.Clear();
			deltasZ.Clear();
		}
	}

	public float Sense(Vector2 pos, float a, float o)
	{
		int _x = (int)Mathf.Floor(pos.x + (Mathf.Cos(toRadians(a + o)) * agentSpeed * 2));
		int _y = (int)Mathf.Floor(pos.y + (Mathf.Sin(toRadians(a + o)) * agentSpeed * 2));
		return baseMap[Mathf.Clamp(_x,0,regionSize.x-1)][(int)Mathf.Clamp(_y,0,regionSize.y-1)];
	}
	public float toRadians(float angle)
	{
		return angle * (Mathf.PI / 180f);
	}
}
/*
 * SLIME MOLD SIMULATION
 * Each agent checks the tiles to the front, front right, and front left of it and looks at the values
 * It then rotates towards the brightest direction while always moving forwards
 * Bounces back when it hits the walls
 * Draws a bright pixel at its position
 * Each pixel around it slowly diffuses
 * 
 * 
 * Calc Agents
 * Copy map
 * Diffuse
 * Find changes pixels
 * Change sprites
 */