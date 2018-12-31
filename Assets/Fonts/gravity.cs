using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gravity : MonoBehaviour {
    public Rigidbody2D rb;
    public float Gravity = 0.5f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void FixedUpdate()
    {
        gravity[] attractors = FindObjectsOfType<gravity>(); 
        foreach(gravity gravity in attractors)
        {
            if(gravity !=this)
            
            attract(gravity);
        }
    }
    void attract(gravity objecttoattrack ){
        Rigidbody2D rbtoAttract = objecttoattrack.rb;
        Vector3 direction = rb.position - rbtoAttract.position;
        float distance = direction.magnitude;
        float forcemagnitude =Gravity* (rb.mass * rbtoAttract.mass) / Mathf.Pow(distance, 2);
        Vector3 force = direction.normalized * forcemagnitude;

        rbtoAttract.AddForce(force);


    }
}
