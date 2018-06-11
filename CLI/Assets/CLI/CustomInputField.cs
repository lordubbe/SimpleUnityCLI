using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomInputField : InputField
{
    public Vector2 GetLocalCaretPosition()
    {
        if (isFocused)
        {
            TextGenerator gen = m_TextComponent.cachedTextGenerator;


            int pos = caretPosition;
            if (caretPosition > gen.characterCount - 1) pos = gen.characterCount - 1;
            if (caretPosition <= 0) pos = 1;

            try
            {
                UICharInfo charInfo = gen.characters[pos];
                float x = (charInfo.cursorPos.x + charInfo.charWidth) / m_TextComponent.pixelsPerUnit;
                float y = (charInfo.cursorPos.y) / m_TextComponent.pixelsPerUnit;
                return new Vector2(x, y);
            }
            catch
            {
                return Vector2.zero;
            }
        }
        else
            return Vector2.zero;
    }
}
