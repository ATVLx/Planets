using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

	[Header("tweaks")]
	public float targetVelocity;
	public float maxVelocity;
	public float maxForce;
	public float gain;
	public float mouseSensitivity;
	public float gravityMultiplier;
	public float distanceMax;
	public float maxSpeed;
    float x;
    float y;
    float posx;
    float posy;
    float dir = -1;
    public Camera camera;
   public float HomeSpeed = 1;
    Vector3 orignalPosition;
    Touch initouch;
  public   float speed = 10;

	[Header("pre-populated")]
	public Rigidbody2D ControlledObj;
	public Transform ControlTarget, Starfield;
	public CircleCollider2D controlArea,slowArea;
	public GameObject PlanetPrefab,StarPrefab,BigBoomPrefab;
	public Animator TitleAnimator,ScoreAnimator,FaceAnimator;

	[Header("UI")]
	public Text SpeedMultiplier;
	public Text OrbitUI;
	public Text TopUI;
	public TextMesh FaceText;
    public Virtualjoystick joystick;
    public Joystick joystick1;

	[Header("variables")]
	public float speedUpBy;
	public List<Rigidbody2D> bodies = new List<Rigidbody2D>();
	public List<Rigidbody2D> staticBodies = new List<Rigidbody2D>();
	public List<Rigidbody2D> orbitBodies = new List<Rigidbody2D>();
	public List<Planet> planets = new List<Planet>();
	public int planetCount;
	public int orbitCount;
	public bool clearedThisMove,isMoving,isInvincible,isDead;
	public Vector3 controlTargetPrevPos;
	public int topCount;
	public int life;

	[Header("Audio")]
	public AudioSource _as;
	public AudioClip AppearSound, ExplodeSound, HitSound, ScoreSound, CheckSound, OrbitSound;

     int facemode = 0;
    
	// Use this for initialization
	void Start () 
	{
        orignalPosition=ControlledObj.position;
		for (int x = 0; x < 100; x++)
		{
			GameObject NewStar = Instantiate(StarPrefab, new Vector3(Random.Range(-150f,150f), Random.Range(-100f,100f), Random.Range(350f,830f)), Quaternion.identity) as GameObject;
			NewStar.transform.SetParent(Starfield);
		}
		Init();
	}

	void Init ()
	{
		isDead = false;
		ControlledObj.transform.position = new Vector3(0f,0f,-0.3f);
		ControlTarget.position = new Vector3(0f,0f,0f);
		ControlledObj.gameObject.SetActive(true);
		life = 3;
		//Cursor.lockState = CursorLockMode.Locked;
//		bodies.Clear();
		orbitBodies.Clear();
		planets.Clear();

		topCount = 0;

		UpdateTopUI();
		UpdateFace();
		UpdateOrbitUI();
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.touchCount > 0){

            Touch touch = Input.GetTouch(0);
            Vector2 touchPos = camera.ScreenToWorldPoint(touch.position);


            switch (touch.phase)
            {
                case TouchPhase.Began:
                    if (GetComponent<Collider2D>() == Physics2D.OverlapPoint(touchPos))
                    {
                        x = touchPos.x - touch.position.x;
                        y = touchPos.y - touch.position.y;
                        posx = x * Time.deltaTime * HomeSpeed * dir;
                        posy = y * Time.deltaTime * HomeSpeed * dir;
                    }
                    break;

                case TouchPhase.Moved:
                    if(GetComponent<Collider2D>()==Physics2D.OverlapPoint(touchPos))
                    {
                       
                    } break;
            }            
        }
            /*  if (Input.touchCount > 0)
              {
                  Touch touch= Input.GetTouch(0);
                  Vector2 touchPos = camera.ScreenToWorldPoint(touch.position);
                  if (touch.phase == TouchPhase.Began)
                  {
                      initouch = touch;
                  }
                  else if (touch.phase == TouchPhase.Moved)
                  {
                      x = initouch.position.x - touch.position.x;
                      y = initouch.position.y - touch.position.y;
                      posx = x * Time.deltaTime * HomeSpeed * dir;
                      posy = y * Time.deltaTime * HomeSpeed * dir;
                      Debug.Log("posX: " + posx + " Posy: " + posy);
                      ClearOrbit();
                  }
                  else if (touch.phase == TouchPhase.Ended)
                  {
                      initouch = new Touch();
                      posx = 0;
                      posy = 0;
                  }

              }*/


            Starfield.Rotate(0f, Time.deltaTime,0f);
        FindObjectOfType<HomeAnim>().updateface(facemode);
        UpdateFace();
        if (!isDead && (life > 0))
		{
			if (Input.GetKeyUp(KeyCode.Space) )
			{
				NewPlanet();
			}

	//		if (Input.GetKeyUp(KeyCode.A))
	//		{
	//			NewStaticPlanet();
	//		}

			if (Input.GetMouseButtonDown(1))
			{
				speedUpBy = 0;
			}

			if (Input.GetMouseButton(1))
			{
				speedUpBy += Time.deltaTime/2f;
	//			if (speedUpBy > 1)
	//			{
	//				speedUpBy = 1f;
	//			}
				SetTime(1+speedUpBy);
			}

			if (Input.GetMouseButtonUp(1))
			{
				SetTime(1);
			}


			if (!clearedThisMove && ((Input.GetAxis("Mouse X") != 0) || (Input.GetAxis("Mouse Y") != 0)))
			{
	//			isMoving = true;
				ClearOrbit();
				clearedThisMove = true;
				UpdateOrbitUI();
			}

			if (clearedThisMove && ((Input.GetAxis("Mouse X") == 0) || (Input.GetAxis("Mouse Y") == 0)))
			{
	//			isMoving = false;
				clearedThisMove = false;
			}
			if (topCount >= planetCount)
			{
				NewPlanet();
			}
		} else
		{
			if ((isDead) && (!ControlledObj.gameObject.activeSelf) && (Input.GetMouseButtonUp(0)))
			{
				Init();
			}
		}
        //
        Vector3 newPoint = ControlTarget.position + new Vector3(posx * Time.deltaTime * speed, 0f, 0f);
        if (controlArea.OverlapPoint(newPoint))
        {
            //remove this if condition (VLX)


            ControlTarget.position += new Vector3(posx * Time.deltaTime * speed, 0f, 0f);


        }
        else
        {
            posx = 0;
        }
        newPoint = ControlTarget.position + new Vector3(0f, posy * Time.deltaTime * speed, 0f);
        if (controlArea.OverlapPoint(newPoint))
        {

            ControlTarget.position += new Vector3(0f, posy * Time.deltaTime * speed, 0);

        }
        else
        {
            posy = 0;
        }
        //		if (controlTargetPrevPos != ControlTarget.position)
        //		{
        //		}
        //		controlTargetPrevPos = controlTargetPrevPos;

    }

	void ClearOrbit()
	{
		orbitBodies.Clear();
		for (int z = 0; z < planets.Count; z++)
		{
			if (planets[z] != null)
			{
				planets[z].ClearOrbitFlags();
			}
		}
	}

	void FixedUpdate () 
	{
		if (Time.timeScale < 1)
		{
			SetTime(Time.timeScale += (Time.fixedDeltaTime/1.5f));
		}

		if (Time.timeScale > 1 && !Input.GetMouseButton(1))
		{
			SetTime(1);
		}
			
		

		Func.MoveWithForceTo2D(ControlledObj, ControlledObj.transform.position, ControlTarget.position, targetVelocity, maxVelocity, maxForce, gain);

		for (int i = 1; i < bodies.Count; i++)
		{
			if (bodies[i] != null)
			{
				float distanceMultiplier = distanceMax - (Vector2.Distance(bodies[0].position, bodies[i].position)/2);
				if (distanceMultiplier < 1)
				{
					distanceMultiplier = 1f;
				}
				bodies[i].AddForce(gravityMultiplier * Mathf.Abs((bodies[0].transform.localScale.x - bodies[i].transform.localScale.x)) * Time.fixedDeltaTime * distanceMultiplier * (bodies[0].position - bodies[i].position));
				bodies[i].velocity = Vector2.ClampMagnitude(bodies[i].velocity, maxSpeed);
//				for (int j = 0; j < bodies.Count; j++)
//				{
//					if (bodies[j] != null)
//					{
//						float distanceMultiplier = distanceMax - (Vector2.Distance(bodies[j].position, bodies[i].position)/2);
//						if (distanceMultiplier < 1)
//						{
//							distanceMultiplier = 1f;
//						}
//		//				Debug.Log(distanceMultiplier);
//						bodies[i].AddForce(gravityMultiplier * Mathf.Abs((bodies[j].transform.localScale.x - bodies[i].transform.localScale.x)) * Time.fixedDeltaTime * distanceMultiplier * (bodies[j].position - bodies[i].position));
//					}
//				}
			}
		}
	}

	void SetTime (float setTo)
	{
		if (setTo > 2f)
		{
			setTo = 2f;
		}
		Time.timeScale = setTo;
		Time.fixedDeltaTime = 0.02F * Time.timeScale;
		SpeedMultiplier.text = System.Math.Round(Time.timeScale,2).ToString() + "x";
	}

	void NewPlanet ()
	{
		_as.PlayOneShot(AppearSound);
		GameObject newPlanet = Instantiate(PlanetPrefab, Random.insideUnitCircle.normalized * 10f, Quaternion.identity) as GameObject;
		bodies.Add(newPlanet.GetComponent<Rigidbody2D>());
		Planet _planet = newPlanet.GetComponent<Planet>();
		planets.Add(_planet);
		_planet.MakeFlying();
		planetCount += 1;
		UpdateOrbitUI();
	}

	void NewStaticPlanet ()
	{
		GameObject newPlanet = Instantiate(PlanetPrefab, Random.insideUnitCircle.normalized * 10f, Quaternion.identity) as GameObject;
		staticBodies.Add(newPlanet.GetComponent<Rigidbody2D>());
		Planet _planet = newPlanet.GetComponent<Planet>();
		_planet.MakeStatic();
	}

	public void RemovePlanet (Rigidbody2D removeThis)
	{
		_as.PlayOneShot(ExplodeSound);
		bodies.Remove(removeThis);
//		bool foundEmpty = false;
//		for (int i = 0; i < bodies.Count-1; i++)
//		{
//			if (!foundEmpty)
//			{
//				if (bodies[i] == null)
//				{
//					foundEmpty = true;
//				}
//			}
//			if (foundEmpty)
//			{
//				bodies[i] = bodies[i+1];
//			}
//		}
//
		for(int i = bodies.Count - 1; i > -1; i--)
		{
			if (bodies[i] == null)
				bodies.RemoveAt(i);
		}

//		while (bodies.Remove(null));
//		bodies.RemoveAll(list_item => list_item == null);
//		bodies.RemoveAt(bodies.Count);
		planetCount -= 1;
		UpdateOrbitUI();
	}

	public void UpdateOrbitUI ()
	{
//		OrbitUI.text = "<size=20>ORBITS</size>" + orbitBodies.Count.ToString() + "/" + planetCount.ToString();
		if (planetCount < 0)
		{
			planetCount = 0;
		}
		OrbitUI.text = orbitBodies.Count.ToString() + "/" + planetCount.ToString();
		if (orbitBodies.Count > topCount)
		{
			if ((!isDead) && (life < 3))
			{
				_as.PlayOneShot(ScoreSound);
				life += 1;
				UpdateFace();
			}
			topCount = orbitBodies.Count;
			UpdateTopUI();
			ScoreAnimator.SetTrigger("Scored");
		}

		if ((orbitBodies.Count > 0) && (!TitleAnimator.GetBool("Started")))
		{
			TitleAnimator.SetBool("Started",true);
		}
	}

	public void Slowdown ()
	{
		SetTime(0.25f);
	}

	public void AnimatorOff ()
	{
		TitleAnimator.transform.gameObject.SetActive(false);
	}

	public void FaceHit ()
	{
		life -= 1;
		if (life > 0)
		{
			FaceText.text = ">_<";
			FaceAnimator.SetTrigger("Hurt");
			isInvincible = true;
		} else
		{
			isDead = true;
			FaceText.text = "+_+";
			FaceAnimator.SetTrigger("Blowup");
		}
	}

	public void UpdateTopUI ()
	{
		TopUI.text = "<size=20>TOP:</size>" + topCount.ToString();
	}

	public void UpdateFace ()
	{
		if (life >= 3)
		{
			FaceText.text = "^_^";
            facemode = 0;
		} else
			if (life == 2)
			{
				FaceText.text = "-_-";
            facemode = 1;
			} else
				if (life == 1)
				{
					FaceText.text = "x_x";
            facemode = 2;
				} else
					if (life == 0)
					{
            facemode = 3;
						FaceText.text = "x_x";
					}
						
	}

	public void GameOver ()
	{
		for (int i = 0; i < planets.Count; i++)
		{
			if (planets[i] != null)
			{
				planets[i].RemovePlanet();
			}
		}
		planets.Clear();
		ControlledObj.gameObject.SetActive(false);
	}
}
