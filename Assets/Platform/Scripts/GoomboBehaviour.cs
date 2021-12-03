using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoomboBehaviour : MonoBehaviour {

	public float speed;

	private Rigidbody2D rb2d;
	private Vector3 startVector;
	private bool moveToRight;

	void Start () {
		rb2d = GetComponent<Rigidbody2D> ();
		startVector = transform.position;
		moveToRight = false;
	}

	void FixedUpdate () {
		if (transform.position.x < startVector.x-1.5f || moveToRight) {//Bordure gauche
			Vector2 movement = new Vector2 (1, 0);
			rb2d.velocity = movement * speed;
			moveToRight = true;
		} else {
			Vector2 movement = new Vector2 (-1, 0);
			rb2d.velocity = movement * speed;
		}
		if(transform.position.x > startVector.x){
			moveToRight = false;
		}
	}
}
