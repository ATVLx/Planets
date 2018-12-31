using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Virtualjoystick1 : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    private Image BGimg;
    private RawImage Thumbimg;
    private Vector3 inputVector;



    private void Start()
    {
        BGimg = GetComponent<Image>();
        Thumbimg = transform.GetChild(0).GetComponent<RawImage>();
    }

    public virtual void OnDrag(PointerEventData PED)
    {
        Vector2 Pos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(BGimg.rectTransform, PED.position, PED.pressEventCamera, out Pos)) 
        {
            //Debug.Log("clicked on joystick");
            Pos.x = (Pos.x/BGimg.rectTransform.sizeDelta.x);
            Pos.y = (Pos.y/BGimg.rectTransform.sizeDelta.y);
            inputVector = new Vector3(Pos.x * 2f , 0, Pos.y * 2f);
            inputVector = (inputVector.magnitude > 1.0f) ? inputVector.normalized : inputVector;
            Debug.Log(inputVector);
            Thumbimg.rectTransform.anchoredPosition = new Vector3(inputVector.x * (BGimg.rectTransform.sizeDelta.x/2), inputVector.z *(BGimg.rectTransform.sizeDelta.y/2));
        }
    }





    //======================================================================================



    public virtual void OnPointerDown(PointerEventData PED)
    {
        OnDrag(PED);
    }
    public virtual void OnPointerUp(PointerEventData PED)
    {
        inputVector = Vector3.zero;
        //PED.useDragThreshold = true;
        Thumbimg.rectTransform.anchoredPosition = Vector3.zero;


    }
    public float Horizontal()
    {
        if (inputVector.x != 0) {
            Debug.Log(inputVector.x);
            return inputVector.x;
       
    }
        else return Input.GetAxis("Horizontal");

    }
    public float Vertical()
    {
        if (inputVector.z!= 0)
            return inputVector.z;
        else return Input.GetAxis("Vertical");
    }

}
