using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ColorChartController : MonoBehaviour
{
    [SerializeField] private Texture2D colorChart;
    [SerializeField] private Image button;
    [SerializeField] private RectTransform cursor;
    [SerializeField] private Image cursorColor;

    [SerializeField] private Player playerCharacter;
    
    private int colorID = 0;
    
    public void SetColorID(int _colorID)
    {
        colorID = _colorID;
    }

    public int GetColorID() => colorID;

    public void SetButton(Image _button)
    {
        button = _button;
    }

    public void TriggerColorPick(BaseEventData _data)
    {
        PointerEventData pointer = _data as PointerEventData;

        if(pointer == null)
            return;
        
        cursor.position = pointer.position;
   
        Color colorPicked = colorChart.GetPixel(
            (int) ((cursor.localPosition.x + (GetComponent<RectTransform>().rect.width / 2)) * (colorChart.width / GetComponent<RectTransform>().rect.width)),
            (int) ((cursor.localPosition.y + (GetComponent<RectTransform>().rect.height / 2)) * (colorChart.height / GetComponent<RectTransform>().rect.height)));

        cursorColor.color = colorPicked;
        button.color = colorPicked;
        playerCharacter.SetColor(colorPicked);
    }
    
}
