using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : Actor
{
	public GameObject[] Projectiles;
	public int CurrentWeapon = 0;
	public float FireRate;
	public Camera MyCamera;

	private float nextFire;

	// Use this for initialization
	void Start ()
	{
		BaseStart ();
		nextFire = Time.time;
		MyCamera = GameObject.FindObjectOfType<Camera> ();
	}

	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.X)) {
			if (MyController.Running) {
				MyController.Pause ();
			} else {
				MyController.Resume ();
			}
		}

		if (Input.GetKeyDown (KeyCode.R)) {
			Application.LoadLevel ("scene1");
		}
	}
	// Update is called once per frame
	void FixedUpdate ()
	{
		if (MyController.Running) {
			Moving = false;
			Vector2 moveVector = new Vector2 ();

			ProccesInput (ref moveVector);

			// move player
			if (!Moving) {
				moveVector *= 0;
			} else {
				moveVector = moveVector.normalized * MoveSpeed;
			}

			UpdateAnimation (moveVector.magnitude * AnimationSpeedFactor);

			RigidBody.velocity = moveVector;

			MyCamera.transform.position = new Vector3 (Mathf.Floor (transform.position.x), 
		                                           Mathf.Floor (transform.position.y) - 16, -10);
		} else {
			RigidBody.velocity = Vector2.zero;
		}
	}

	void ProccesInput (ref Vector2 moveVector)
	{
		if (Input.GetKey (KeyCode.W)) {
			MyDirection = Direction.NORTH;
			moveVector += new Vector2 (0, 1);
			Moving = true;
		}
		if (Input.GetKey (KeyCode.D)) {
			MyDirection = Direction.EAST;
			moveVector += new Vector2 (1, 0);
			Moving = true;
		}
		if (Input.GetKey (KeyCode.S)) {
			MyDirection = Direction.SOUTH;
			moveVector += new Vector2 (0, -1);
			Moving = true;
		}
		if (Input.GetKey (KeyCode.A)) {
			MyDirection = Direction.WEST;
			moveVector += new Vector2 (-1, 0);
			Moving = true;
		}

		if (Input.GetKey (KeyCode.Space)) {
			if (Time.time > nextFire && MyController.Ammo [CurrentWeapon] > 0) {
				FireProjectile (Projectiles [CurrentWeapon]);
				MyController.Ammo [CurrentWeapon]--;
				nextFire = Time.time + FireRate;
			}
		}



		if (Input.GetKey (KeyCode.Alpha1) && MyController.Ammo [0] > 0) {
			CurrentWeapon = 0;
			MyController.WeaponSelectorImage.rectTransform.localPosition = MyController.WeaponSelectorPositions [0];
		}

		if (Input.GetKey (KeyCode.Alpha2) && MyController.Ammo [1] > 0) {
			CurrentWeapon = 1;
			MyController.WeaponSelectorImage.rectTransform.localPosition = MyController.WeaponSelectorPositions [1];
		}

		if (Input.GetKey (KeyCode.Alpha3) && MyController.Ammo [2] > 0) {
			CurrentWeapon = 2;
			MyController.WeaponSelectorImage.rectTransform.localPosition = MyController.WeaponSelectorPositions [2];
		}
	}
}
