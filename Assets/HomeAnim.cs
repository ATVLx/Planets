using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeAnim : MonoBehaviour {

	public TextMesh FaceText;
	public GameController _gc;
    SpriteRenderer sr;
    public Sprite spritehappy, spritemed, spritesad, Spritedead;
    public int status = 0;
    Color tmp = new Color();
    bool ondying = false;
    public Animator anim;

	// Use this for initialization
	void Start () 
	{
		_gc = GameObject.Find("GameController").GetComponent<GameController>();
        sr = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
       
        if(status==0){
            sr.sprite = spritehappy;
        }
        if (status == 1)
        {
            sr.sprite = spritemed;
        }
        if (status == 2)
        {
            sr.sprite = spritesad;
        }
        if (status == 3)
        {
            sr.sprite = Spritedead;
            sr.color = new Color(Random.Range(0.7f, 1), Random.Range(0.5f, 0.7f) , Random.Range(0.5f, 0.7f) );
            StartCoroutine(alphasetter());
        }


    }

    IEnumerator alphasetter(){
        if (ondying)
        {
            yield return null;
        }
        else
        {
            ondying = true;
            StartCoroutine(waitforboolof());
            tmp.a = 0;
            yield return new WaitForSecondsRealtime(0.2f);
            tmp = new Color(Random.Range(0.7f, 1), Random.Range(0.5f, 0.7f), Random.Range(0.5f, 0.7f));
            tmp.a = 255;
            sr.color = tmp;
            tmp.a = 0;
            yield return new WaitForSecondsRealtime(0.4f);
            tmp = new Color(Random.Range(0.7f, 1), Random.Range(0.5f, 0.7f), Random.Range(0.5f, 0.7f));
            tmp.a = 255;
            sr.color = tmp;
           
            yield return new WaitForSecondsRealtime(0.6f);
            tmp = new Color(Random.Range(0.7f, 1), Random.Range(0.5f, 0.7f), Random.Range(0.5f, 0.7f));
            tmp.a = 0;
            sr.color = tmp;
           
            yield return new WaitForSecondsRealtime(0.8f);
            tmp = new Color(Random.Range(0.7f, 1), Random.Range(0.5f, 0.7f), Random.Range(0.5f, 0.7f));
            tmp.a = 255;
            sr.color = tmp;

            yield return new WaitForSecondsRealtime(1.0f);
            tmp = new Color(Random.Range(0.7f, 1), Random.Range(0.5f, 0.7f), Random.Range(0.5f, 0.7f));
            tmp.a = 0;
            sr.color = tmp;

        }


    }

    IEnumerator waitforboolof()
    {
        yield return new WaitForSeconds(3);
        ondying = false;
        StopCoroutine(alphasetter());
        StopCoroutine(waitforboolof());
    }
        public void SetFace()
	{
		_gc.UpdateFace();
		_gc.isInvincible=false;
        //		
       
	}

    
	public void Blowup()
	{
		Instantiate(_gc.BigBoomPrefab, transform.position, Quaternion.identity);
		_gc.GameOver();
	}
    public int updateface(int up)
    {
        status = up;
        return up;
    }
    public void animateplanettxt(){
        anim.SetTrigger("vibrate");
    }
}
