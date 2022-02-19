using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodeHandle : MonoBehaviour
{
	public List<CodeBlock> codeSequence = new List<CodeBlock>();
	public List<object> RTV = new List<object>();//runtimevars
	public List<List<SpriteRenderer>> pixelMartix = new List<List<SpriteRenderer>>();

	public void ExecuteCode()
	{
		for(int i=0; i < codeSequence.Count; i++)
		{
			CodeBlock block = codeSequence[i];
			switch (block.blockType.ToLower())
			{
				case "lbl":i = (int)block.p1;break;
				case "new":RTV.Add(block.p1);break;
				case "var":RTV[(int)block.p2] = block.p1;break;
				case "gto":i = (int)block.p1;break;
				case "prt":pixelMartix[(int)block.p1][(int)block.p2].color = Color.HSVToRGB((float)block.p3, 1f, 1f);break;
			}
		}
	}
}
