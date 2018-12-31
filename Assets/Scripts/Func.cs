using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Func : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public static bool isTrue ()
	{
		return true;
	}

	public static void MoveWithForceTo(Rigidbody targetRigid, Vector3 fromPoint, Vector3 toPoint, float ftoVel, float fmaxVel, float fmaxForce, float fgain)
	{
		Vector3 dist = toPoint - fromPoint;
		// calc a target vel proportional to distance (clamped to maxVel)
		Vector3 tgtVel = Vector3.ClampMagnitude(ftoVel * dist, fmaxVel);
		// calculate the velocity error
		Vector3 error = tgtVel - targetRigid.velocity;
		// calc a force proportional to the error (clamped to maxForce)
		Vector3 force = Vector3.ClampMagnitude(fgain * error, fmaxForce);
		targetRigid.AddForce(force);
	}

	public static void MoveWithForceTo2D(Rigidbody2D targetRigid, Vector2 fromPoint, Vector2 toPoint, float ftoVel, float fmaxVel, float fmaxForce, float fgain)
	{
		Vector2 dist = toPoint - fromPoint;
		// calc a target vel proportional to distance (clamped to maxVel)
		Vector2 tgtVel = Vector2.ClampMagnitude(ftoVel * dist, fmaxVel);
		// calculate the velocity error
		Vector2 error = tgtVel - targetRigid.velocity;
		// calc a force proportional to the error (clamped to maxForce)
		Vector2 force = Vector3.ClampMagnitude(fgain * error, fmaxForce);
		targetRigid.AddForce(force);
	}

	public static void TorqueWithForceTo(Rigidbody targetRigid, Vector3 fromAngle, Vector3 toAngle, float ftoVel, float fmaxVel, float fmaxForce, float fgain)
	{
		Vector3 diff = toAngle - fromAngle;
		Vector3 tgtTorque = Vector3.ClampMagnitude(ftoVel * diff, fmaxVel);
		Vector3 error = tgtTorque - targetRigid.angularVelocity;
		Vector3 force = Vector3.ClampMagnitude(fgain * error, fmaxForce);
		targetRigid.AddTorque(0f,0f,force.z);
	}
		
	public static void SetGlobalScale (Transform transform, Vector3 globalScale)
	{
		transform.localScale = Vector3.one;
		transform.localScale = new Vector3 (globalScale.x/transform.lossyScale.x, globalScale.y/transform.lossyScale.y, globalScale.z/transform.lossyScale.z);
	}
}
