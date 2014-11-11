using System;
using UnityEngine;

namespace AssemblyCSharp
{
	public enum InputButton
	{
		INPUT_BUTTON_A,
		INPUT_BUTTON_B,
		INPUT_BUTTON_X,
		INPUT_BUTTON_Y
	}
	
	public class InputHelper
	{
		public static Texture2D GetButtonTexture(InputButton _button)
		{
			string texName = "Textures/Xbox360_Button_";
			switch(_button)
			{
			case InputButton.INPUT_BUTTON_A:
				texName += "A";
				break;
				
			case InputButton.INPUT_BUTTON_B:
				texName += "B";
				break;
				
			case InputButton.INPUT_BUTTON_X:
				texName += "X";
				break;
				
			case InputButton.INPUT_BUTTON_Y:
				texName += "Y";
				break;
				
			default:
				Debug.LogError("Unknown button");
				return null;
			}
			
			return (Texture2D)Resources.Load(texName);
		}
		
		public static string GetButtonName(InputButton _button)
		{
			switch(_button)
			{
			case InputButton.INPUT_BUTTON_A:
				return "A";
				
			case InputButton.INPUT_BUTTON_B:
				return "B";
				
			case InputButton.INPUT_BUTTON_X:
				return "X";
				
			case InputButton.INPUT_BUTTON_Y:
				return "Y";
				
			default:
				Debug.LogError("Unknown button");
				return string.Empty;
			}
		}
	}
}

