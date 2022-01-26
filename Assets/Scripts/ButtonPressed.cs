using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonPressed : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private RectTransform pos;
    private Vector2 originalPos;
    private Vector2 downPos;
    
    private bool isDown = false;
    private Button thisButton;
    
    // Start is called before the first frame update
    void Awake()
    {
        pos = GetComponentInChildren<TMP_Text>().rectTransform;
        originalPos = pos.anchoredPosition;
        thisButton = gameObject.GetComponent<Button>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(thisButton.interactable)
            Down();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if(thisButton.interactable) 
            Up();
    }

    public void Disable()
    { 
        Down();
        gameObject.GetComponent<Button>().image.raycastTarget = false;
        gameObject.GetComponent<Button>().interactable = false;
    }


    public void Down()
    {
        if(!isDown)
            pos.anchoredPosition += new Vector2(0, -15f);
        isDown = true;
    }
 
    public void Up()
    {
        if(isDown)
            pos.anchoredPosition -= new Vector2(0, -15f);
        isDown = false;
    }

}
