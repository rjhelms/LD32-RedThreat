﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameController : MonoBehaviour
{

	private static GameController _instance;

	public static GameController Instance {
		get {
			if (_instance == null) {
				_instance = GameObject.FindObjectOfType<GameController> ();
				DontDestroyOnLoad (_instance.gameObject);
			}
			return _instance;
		}
	}

	public bool Running;
	public int Score;
	public int HitPoints;
	public int Lives;
	public int[] Ammo = {100, -1, -1};
	public Text[] AmmoText;
	public Text ScoreText;
	public Image WeaponSelectorImage;
	public Image HealthBarImage;
	public GameObject CivilianPrefab;
	public GameObject CommiePrefab;
	public GameObject CapitalistPrefab;
	public GameObject BureaucratPrefab;

	public Vector3[] WeaponSelectorPositions;

	public Transform CommieContainer;
	public Transform CivilianContainer;
	public Transform CapitalistContainer;
	public Transform ProjectileContainer;
	public Transform BureaucratContainer;
	public Transform PlayerTransform;

	public AudioClip[] PlayerWeaponSounds;

	public AudioClip PowerUpSound;
	public AudioClip PlayerHitSound;
	public AudioClip CommieSound;
	public AudioClip CivilianSound;
	public AudioClip EnragedSound;

	public AudioSource SFXSource;

	void Awake ()
	{
		if (_instance == null) {
			_instance = this;
			DontDestroyOnLoad (this);
		} else {
			if (this != _instance)
				Destroy (this.gameObject);
		}
	}

	void Start ()
	{
		Initialize ();
	}

	void OnLevelWasLoaded (int level)
	{
		Initialize ();
	}

	void Update ()
	{
		if (Running) {
			if (HitPoints > 32) {
				HitPoints = 32;
			}
			if (HitPoints <= 0) {
				Debug.Log ("You died");
				Pause ();
			}

			for (int i = 0; i < AmmoText.Length; i++) {
				if (Ammo [i] > 99)
					Ammo [i] = 99;

				if (Ammo [i] < 0)
					Ammo [i] = 0;

				AmmoText [i].text = (Ammo [i]).ToString ();
			}
		
			ScoreText.text = Score.ToString ();
			HealthBarImage.rectTransform.sizeDelta = new Vector2 (HitPoints * 2, 8);
		}
	}

	void Initialize ()
	{
		AmmoText = new Text[3];
		AmmoText [0] = GameObject.Find ("LeafletValue").GetComponent<Text> ();
		AmmoText [1] = GameObject.Find ("MoneyValue").GetComponent<Text> ();
		AmmoText [2] = GameObject.Find ("MegaphoneValue").GetComponent<Text> ();
		ScoreText = GameObject.Find ("ScoreValue").GetComponent<Text> ();
		WeaponSelectorImage = GameObject.Find ("WeaponSelector").GetComponent<Image> ();
		HealthBarImage = GameObject.Find ("HealthBar").GetComponent<Image> ();

		WeaponSelectorPositions = new Vector3[3];
		WeaponSelectorPositions [0] = WeaponSelectorImage.rectTransform.localPosition;
		WeaponSelectorPositions [1] = WeaponSelectorPositions [0] + new Vector3 (36, 0, 0);
		WeaponSelectorPositions [2] = WeaponSelectorPositions [1] + new Vector3 (36, 0, 0);

		CommieContainer = GameObject.Find ("Commies").transform;
		CivilianContainer = GameObject.Find ("Civilians").transform;
		CapitalistContainer = GameObject.Find ("Capitalists").transform;
		ProjectileContainer = GameObject.Find ("Projectiles").transform;
		BureaucratContainer = GameObject.Find ("Bureaucrats").transform;
		PlayerTransform = GameObject.Find ("Player(Clone)").transform;

		SFXSource = GameObject.Find ("SoundFX").GetComponent<AudioSource> ();
	}

	public void Pause ()
	{
		Running = false;
		Time.timeScale = 0;
	}
	public void Resume ()
	{
		Running = true;
		Time.timeScale = 1;
	}

	public void BureaucratHit ()
	{
		SFXSource.PlayOneShot (PlayerHitSound);
		Score -= 100;
		Ammo [0] -= 10;
		Ammo [1] -= 10;
		Ammo [2] -= 10;
		HitPoints -= 5;
	}
}