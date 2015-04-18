﻿using UnityEngine;
using System.Collections;

public class Civilian : Actor
{
	public GameObject CommiePrefab;
	public bool Hit = false;

	// Use this for initialization
	void Start ()
	{
		animator = this.GetComponent<Animator> ();
		RigidBody = this.GetComponent<Rigidbody2D> ();
		MyDirection = (Direction)Random.Range (0, 4);
	}
	
	// Update is called once per frame
	void Update ()
	{
		Vector2 moveVector = new Vector2 ();

		switch (MyDirection) {
		case Direction.NORTH:
			moveVector += new Vector2 (0, 1);
			break;
		case Direction.EAST:
			moveVector += new Vector2 (1, 0);
			break;
		case Direction.SOUTH:
			moveVector += new Vector2 (0, -1);
			break;
		case Direction.WEST:
			moveVector += new Vector2 (-1, 0);
			break;
		}

		moveVector = moveVector.normalized * MoveSpeed;
	
		UpdateAnimation (moveVector.magnitude * AnimationSpeedFactor);
	
		RigidBody.velocity = moveVector;
	}

	void OnCollisionEnter2D (Collision2D coll)
	{
		if (!Hit) {
			ResolveNPCCollision (coll);
		}
	}

	public void BecomeCommie ()
	{
		GameObject newCommieObject = (GameObject)Instantiate (CommiePrefab, transform.position, Quaternion.identity);
		Commie newCommie = newCommieObject.GetComponent<Commie> ();
		newCommie.MyDirection = this.MyDirection;
		newCommie.StartVelocity = this.RigidBody.velocity;
		Destroy (this.gameObject);
	}
}