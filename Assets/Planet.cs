using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour {

	public Color _color;
	public TrailRenderer _tr;
	public MeshRenderer _mr;
	public Transform _t;
	public LayerMask BodiesLayer;
	public GameController _gc;
	public Rigidbody2D _rb;
//	private bool destroyed = false;
	public ParticleSystem _ps;
	public LineRenderer _lr;
	public Transform _home;
	public int orbitFlags;
	public bool toLeft;
	public TextMesh ping;
	public Transform check;
	public Transform pingContainer,checkContainer;
	public SpriteRenderer checkSprite;
	public GameObject HitFx;

	// Use this for initialization
	void Start () {
		_home = GameObject.Find("Home").GetComponent<Transform>();
		_rb.simulated = false;
		_gc = GameObject.Find("GameController").GetComponent<GameController>();
//		float randomsize = Random.Range(0.1f,1f); 
//		_t.localScale = new Vector3(randomsize, randomsize, randomsize);
//		_tr.startWidth = randomsize;
		_color = new Color(Random.Range(0.5f,1f), Random.Range(0.5f,1f), Random.Range(0.5f,1f), 1f);
		_mr.material.color = _color;
		_tr.startColor = new Color(_color.r,_color.g,_color.b,1f);
		_tr.endColor = new Color(_color.r,_color.g,_color.b,0f);
		var main = _ps.main;
		main.startColor = _color;
//		_ps.main.startColor = _color;
		StartCoroutine(StartPlanet());
		if (_t.position.x < _home.position.x)
		{
			toLeft = true;
		} else
		{
			toLeft = false;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
       
		if (!_gc.clearedThisMove)
		{
			if (toLeft && (_t.position.x > _home.position.x))
			{
				toLeft = false;
				orbitFlags += 1;
				UpdateOrbit();
			} else
				if (!toLeft && (_t.position.x < _home.position.x))
				{
					toLeft = true;
					orbitFlags += 1;
					UpdateOrbit();
				}
		}
	}

	public void RemovePlanet()
	{
		_ps.Play();
		_lr.enabled = false;
		_gc.RemovePlanet(_rb);
		_rb.simulated = false;
		_mr.enabled = false;
	}

	void OnCollisionEnter2D (Collision2D collision)
	{
        if(collision.collider.tag == "Player"){
            RemovePlanet();
        }
		if (!_gc.isInvincible && collision.collider.gameObject.layer == 9)
		{
//			if (collision.collider.transform.localScale.x > _t.localScale.x)
//			{
			RemovePlanet();
			_gc.FaceHit();
            FindObjectOfType<HomeAnim>().animateplanettxt();
//				destroyed = true;
//				Destroy(gameObject);
//			}
		} else 
			if (collision.collider.gameObject.layer == 8)
			{
//				if (_mr.isVisible)
				_gc._as.PlayOneShot(_gc.HitSound);
				if (_gc.slowArea.OverlapPoint(collision.transform.position))
				{
					ContactPoint2D contact = collision.contacts[0];
					Vector3 pos = contact.point;
					Instantiate(HitFx, pos, Quaternion.identity);
					_gc.Slowdown();
				}
			}
	}
//
	void OnBecameInvisible ()
	{
//		
	}

	void OnBecameVisible ()
	{
		
	}

	void UpdateOrbit ()
    {
      
//		ping.text = orbitFlags.ToString();
		if (orbitFlags == 3)
		{
			if (!_gc.orbitBodies.Contains(_rb))
			{
				_gc._as.PlayOneShot(_gc.OrbitSound);
				_gc.orbitBodies.Add(_rb);
			}
//			checkContainer.gameObject.SetActive(true);
			checkSprite.enabled = true;
			_gc.UpdateOrbitUI();
		}
	}

	public void ClearOrbitFlags ()
	{
			orbitFlags = 0;
	//		checkContainer.gameObject.SetActive(false);
			checkSprite.enabled = false;
			UpdateOrbit();
	}

	public void MakeStatic ()
	{
		float randomsize = Random.Range(3f,4f); 
		_t.localScale = new Vector3(randomsize, randomsize, randomsize);
		_rb.simulated = false;
		_tr.startWidth = randomsize;
		_lr.startWidth = randomsize;
		_lr.endWidth = randomsize;
	}

	public void MakeFlying ()
	{
		float randomsize = Random.Range(0.4f,1f); 
		_t.localScale = new Vector3(randomsize, randomsize, randomsize);
		_tr.startWidth = randomsize;
		_lr.startWidth = randomsize;
		_lr.endWidth = randomsize;
		checkContainer.localScale = Vector3.one * (1.5f-randomsize) * 0.7f;
//		check.transform.lossyScale = Vector3.one;
		Func.SetGlobalScale(check,Vector3.one);
	}

	public IEnumerator StartPlanet() {
		yield return new WaitForSeconds(2f); // waits 3 seconds
		_rb.simulated = true;
	}

}
