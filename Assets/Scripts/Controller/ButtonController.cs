using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    Image image;
    Color targetColor;
    Color pointerHover = new Color(70, 70, 70, 0.6f);
    Color pointerExit = new Color(70, 70, 70, 0);
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        targetColor = pointerExit;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        image.color = Color.Lerp(image.color, targetColor, 0.3f);
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        targetColor = pointerHover;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        targetColor = pointerExit;
    }
}
