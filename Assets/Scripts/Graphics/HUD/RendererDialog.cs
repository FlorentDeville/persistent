using System;
using UnityEngine;

namespace AssemblyCSharp
{
	public class RendererDialog
	{
		static private GUIStyle m_nameTextStyle;
		
		static private GUIStyle m_textStyle;

        static int originalWidth;
        static int originalHeight;

        static Matrix4x4 s_guiMatrix;

        static Rect s_groupRect;
        static Rect s_avatarRect;
        static Rect s_nameRect;
        static Rect s_buttonRect;
        static Rect s_textRect;

		static RendererDialog ()
		{
			m_nameTextStyle = new GUIStyle();
			m_nameTextStyle.normal.textColor = Color.white;
			m_nameTextStyle.fontSize = 20;
			m_nameTextStyle.alignment = TextAnchor.MiddleCenter;
			
			m_textStyle = new GUIStyle();
			m_textStyle.normal.textColor = Color.white;
			m_textStyle.fontSize = 20;
			m_textStyle.alignment = TextAnchor.UpperLeft;

            originalWidth = Screen.width;
            originalHeight = Screen.height;

            //Group rectangle
            float backgroundBoxPercentWidth = 0.9f;
            float backgroundBoxPercentHeight = 0.2f;
            int groupRectLeft = (int)(originalWidth * (1 - backgroundBoxPercentWidth) * 0.5f);
            int groupRectWidth = originalWidth - groupRectLeft * 2;
            int groupRectBottom = 10;
            int groupRectHeight = (int)(originalHeight * backgroundBoxPercentHeight);
            int groupRectTop = originalHeight - groupRectBottom - groupRectHeight;

            s_groupRect = new Rect(groupRectLeft, groupRectTop, groupRectWidth, groupRectHeight);

            //Avatar rectangle
            const int SPACE = 5;
            float avatarPercentHeight = 0.8f;
            int avatarRectSize = (int)(groupRectHeight * avatarPercentHeight) - SPACE * 2;
            s_avatarRect = new Rect(SPACE, SPACE, avatarRectSize, avatarRectSize);

            //Name rectangle
            int nameRectTop = avatarRectSize + SPACE * 2;
            int nameRectHeight = groupRectHeight - nameRectTop - SPACE;
            s_nameRect = new Rect(SPACE, nameRectTop, avatarRectSize, nameRectHeight);

            //Button rectangle
            int buttonSize = 32;
            int buttonLeft = groupRectWidth - SPACE - buttonSize;
            int buttonTop = groupRectHeight - SPACE - buttonSize;
            s_buttonRect = new Rect(buttonLeft, buttonTop, buttonSize, buttonSize);

            //Text rectangle
            int textLabelLeft = SPACE + avatarRectSize + SPACE * 3;
            int textLabelWidth = groupRectWidth - textLabelLeft - SPACE;
            int textLabelHeight = groupRectHeight;
            int textLabelTop = SPACE;
            s_textRect = new Rect(textLabelLeft, textLabelTop, textLabelWidth, textLabelHeight);
		}
		
		public static void Render(Texture2D _texAvatar, string _name, string _text, InputButton _buttonNext)
		{
            UpdateGuiMatrix();

            GUI.BeginGroup(s_groupRect, "");

            Rect boxRect = new Rect(0, 0, s_groupRect.width, s_groupRect.height);
            GUI.Box(boxRect, "");
			
			//avatar
            GUI.DrawTexture(s_avatarRect, _texAvatar);
			
            //name
            GUI.Label(s_nameRect, _name, m_nameTextStyle);
			
            //text
            GUI.Label(s_textRect, _text, m_textStyle);

			//button
            Texture2D texButton = InputHelper.GetButtonTexture(_buttonNext);
            GUI.DrawTexture(s_buttonRect, texButton);
			
			GUI.EndGroup();

            RestoreGuiMatrix();
		}

        private static void UpdateGuiMatrix()
        {
            Vector3 scale;
            scale.x = (float)Screen.width / (float)originalWidth; // calculate hor scale
            scale.y = (float)Screen.height / (float)originalHeight; // calculate vert scale
            scale.z = 1;
            s_guiMatrix = GUI.matrix; // save current matrix

            // substitute matrix - only scale is altered from standard
            GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, scale);
        }

        private static void RestoreGuiMatrix()
        {
            GUI.matrix = s_guiMatrix; // restore matrix
        }
	}
}

