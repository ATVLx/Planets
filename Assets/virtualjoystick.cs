using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class virtualjoystick : MonoBehaviour {

    private Vector2 startTouch, DeltaSwipe;
    bool isDrag = false;
	// Use this for initialization
	void Start () {
		
	}
    private void Reset()
    {

    }

    // Update is called once per frame
    void Update () {
        if(Input.touches.Length>0){
            if(Input.touches[0].phase==TouchPhase.Began){
                startTouch = Input.touches[0].position;
                isDrag = true;
            }
            else if(Input.touches[0].phase == TouchPhase.Ended|| Input.touches[0].phase == TouchPhase.Canceled){
                isDrag = false;
                Reset();

            }
        }
        DeltaSwipe = Vector2.zero;
        if(isDrag){
            if(Input.touches.Length>0){
                DeltaSwipe = Input.touches[0].position - startTouch;
            }
        }
		
	}
}
