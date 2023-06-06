using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerCustomisationSettings", menuName = "ScriptableObjects", order = 1)]
public class PlayerCustomisationSettings : ScriptableObject
{
	[SerializeField] private int isMale = 1;
	[SerializeField] private Color[] colorScheme = new Color[6];


	public int GetIsMale() => isMale;
	public void SetColor(int _colorID, Color _pickedColor, bool _isMale)
	{
		colorScheme[_colorID] = _pickedColor;
	}

	public Color[] GetColorScheme()
	{
		int textureCount = 0;
		for(int i = 0; i < colorScheme.Length; i++)
		{
			if(colorScheme[i].a == 0)
			{
				textureCount++;
			}

			if(textureCount == colorScheme.Length)
				return null;
		}
		return colorScheme;
	}
	
	public void ResetColorScheme()
	{
		for(int i = 0; i < colorScheme.Length; i++)
		{
			colorScheme[i] = Color.gray;
		}
	}
	
	public bool CheckColorDefault()
	{
		foreach (Color color in colorScheme)
		{
			if (color != Color.gray)
				return false;
		}

		return true;
	}
}
