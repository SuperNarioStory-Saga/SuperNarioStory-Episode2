using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerBehaviour : MonoBehaviour {

	public float speed;
	public Camera mainCamera;
	public AudioClip clipDeath;
	public AudioClip clipHit;
	public AudioClip clipJump;
	public AudioClip clipLanding;
	public AudioClip clipSweep;
	public AudioClip clipMoonwalk;
	public AudioClip musicStar;
	public GameObject star;

	private Rigidbody2D rb2d;
	private Animator animator;
	private SpriteRenderer spr;
	private TrailRenderer trend;
	private AudioSource audiosrc;
	private AudioSource audioAux;
	private AudioSource music;
	private bool isQTETriggered;

	void Start () {
		rb2d = GetComponent<Rigidbody2D> ();
		animator = GetComponent<Animator> ();
		spr = GetComponent<SpriteRenderer> ();
		trend = GetComponent<TrailRenderer> ();
		audiosrc = GetComponents<AudioSource> ()[0];
		audioAux = GetComponents<AudioSource> () [1];
		music = GetComponents<AudioSource> ()[2];
		isQTETriggered = false;
		StarHolder.star = false;
	}

	void FixedUpdate () {
		//Controle par le joueur
		if (!isQTETriggered) {
			float moveHorizontal = Input.GetAxis ("Horizontal");
			Vector2 movement = new Vector2 (moveHorizontal, 0);
			rb2d.velocity = movement * speed;
		}
	}

	void Update(){
		//Animation
		if(!isQTETriggered){
			if (Input.GetKey (KeyCode.RightArrow)) {
				animator.SetBool ("isWalking", true);
				spr.flipX = false;
			} else if (Input.GetKey (KeyCode.LeftArrow)) {
				animator.SetBool ("isWalking", true);
				spr.flipX = true;
			} else {
				animator.SetBool ("isWalking", false);
			}
		}

		if (Input.GetKey (KeyCode.Escape)) {//Quitte le jeu
			Application.Quit ();
		}

		//Death
		if (this.transform.position.y < -6) {
			Debug.Log ("OOB");
			audiosrc.PlayOneShot (clipDeath);
			transform.position = new Vector3 (-39.13f,-2.09f,0);
			rb2d.gravityScale = 30;
			if(transform.rotation != Quaternion.identity){
				transform.rotation = Quaternion.identity;
			}
		}
	}

	void OnTriggerEnter2D (Collider2D other){
		if((other.gameObject.CompareTag("Goombo") || other.gameObject.CompareTag("Pirahnanas")) && !isQTETriggered){ //Mort par ennemy
			Debug.Log ("Ennemy");
			audiosrc.PlayOneShot (clipDeath);
			rb2d.gravityScale = 30;
			transform.position = new Vector3 (-39.13f,-2.09f,0);
		} else if ((other.gameObject.CompareTag("Goombo") || other.gameObject.CompareTag("Pirahnanas")) && isQTETriggered) {//Mort des ennemy
			Destroy (other.gameObject);
			audiosrc.PlayOneShot (clipHit);
		}
	}

	void OnTriggerStay2D (Collider2D other){
		if(other.gameObject.CompareTag ("QTE") && !isQTETriggered){ //Affichage du QTE
			other.gameObject.GetComponent<SpriteRenderer> ().color = new Vector4 (1,1,1,0.9f);
			other.gameObject.transform.GetChild(0).GetComponent<TextMesh>().color = new Vector4(1,0,0,1);
			if(Input.GetKeyDown (other.gameObject.transform.GetChild (0).GetComponent<TextMesh> ().text.ToLower ())){
				Regex reg = new Regex (@"QTE \((\d+)\)");
				int idQTE = int.Parse (reg.Match (other.gameObject.name).Groups[1].Value);
				isQTETriggered = true;
				StartCoroutine ("QTEExec",idQTE);
			}
		} else if(other.gameObject.CompareTag ("EndLevel")){
			SceneManager.LoadScene ("EndScreen");
		}
	}

	void OnTriggerExit2D (Collider2D other){
		if(other.gameObject.CompareTag ("QTE")){ //Fin affichage du QTE
			other.gameObject.GetComponent<SpriteRenderer> ().color = new Vector4 (1,1,1,0);
			other.gameObject.transform.GetChild(0).GetComponent<TextMesh>().color = new Vector4(1,0,0,0);
		}
	}

	IEnumerator QTEExec(int idQTE){
		int idStep = 0;
		int yolo = 0;
		spr.flipX = false;
		rb2d.gravityScale = 0;
		rb2d.velocity = Vector2.zero;
		audiosrc.PlayOneShot (clipJump);
		while(isQTETriggered){
			switch (idQTE) {
			case 0:
				switch (idStep) {
				case 0:
					transform.position = Vector3.MoveTowards (transform.position, new Vector3 (-32.91f, -0.92f, 0), 9 * Time.deltaTime);
					if (Vector3.Distance (transform.position, new Vector3 (-32.91f, -0.92f, 0)) < 0.1f) {
						idStep++;
					}
					break;
				case 1:
					transform.position = Vector3.MoveTowards (transform.position, new Vector3 (-31.3f, -1.35f, 0), 9 * Time.deltaTime);
					if (Vector3.Distance (transform.position, new Vector3 (-31.3f, -1.35f, 0)) < 0.1f) {
						idStep++;
					}
					break;
				case 2:
					transform.position = new Vector3 (-31.3f, -1.35f, 0);
					isQTETriggered = false;
					rb2d.gravityScale = 30;
					break;
				}
				yield return new WaitForSeconds (0.01f);
				break;
			case 1:
				switch (idStep) {
				case 0:
					transform.position = Vector3.MoveTowards (transform.position, new Vector3 (-25.58f, -1, 0), 9 * Time.deltaTime);
					if (Vector3.Distance (transform.position, new Vector3 (-25.58f, -1, 0)) < 0.1f) {
						idStep++;
					}
					break;
				case 1:
					transform.position = Vector3.MoveTowards (transform.position, new Vector3 (-24.21f, -2.21f, 0), 9 * Time.deltaTime);
					if (Vector3.Distance (transform.position, new Vector3 (-24.21f, -2.21f, 0)) < 0.1f) {
						idStep++;
					}
					break;
				case 2:
					transform.position = new Vector3 (-24.21f, -2.21f, 0);
					isQTETriggered = false;
					rb2d.gravityScale = 30;
					break;
				}
				yield return new WaitForSeconds (0.01f);
				break;
			case 2:
				switch (idStep) {
				case 0:
					transform.position = Vector3.MoveTowards (transform.position, new Vector3 (-19.53f, -1.98f, 0), 9 * Time.deltaTime);
					if (Vector3.Distance (transform.position, new Vector3 (-19.53f, -1.98f, 0)) < 0.1f) {
						idStep++;
					}
					break;
				case 1:
					audiosrc.PlayOneShot (clipLanding);
					idStep++;
					break;
				case 2:
					transform.position = Vector3.MoveTowards (transform.position, new Vector3 (-19.97f, -0.42f, 0), 9 * Time.deltaTime);
					if (Vector3.Distance (transform.position, new Vector3 (-19.97f, -0.42f, 0)) < 0.1f) {
						idStep++;
					}
					break;
				case 3:
					transform.position = Vector3.MoveTowards (transform.position, new Vector3 (-18.98f, -0.79f, 0), 9 * Time.deltaTime);
					if (Vector3.Distance (transform.position, new Vector3 (-18.98f, -0.79f, 0)) < 0.1f) {
						idStep++;
					}
					break;
				case 4:
					transform.position = new Vector3 (-18.98f, -0.79f, 0);
					isQTETriggered = false;
					rb2d.gravityScale = 30;
					break;
				}
				yield return new WaitForSeconds (0.01f);
				break;
			case 3:
				switch (idStep) {
				case 0:
					transform.position = Vector3.MoveTowards (transform.position, new Vector3 (-18.29f, 0.56f, 0), 9 * Time.deltaTime);
					if (Vector3.Distance (transform.position, new Vector3 (-18.29f, 0.56f, 0)) < 0.1f) {
						idStep++;
					}
					break;
				case 1:
					audiosrc.PlayOneShot (clipLanding);
					idStep++;
					break;
				case 2:
					transform.Rotate (Vector3.forward * 360 * Time.deltaTime);
					if (Mathf.Abs (transform.rotation.eulerAngles.z - 40) < 5) {
						idStep++;
					}
					break;
				case 3:
					transform.position = Vector3.MoveTowards (transform.position, new Vector3 (-19.02f, 1.82f, 0), 9 * Time.deltaTime);
					if (Vector3.Distance (transform.position, new Vector3 (-19.02f, 1.82f, 0)) < 0.1f) {
						idStep++;
					}
					break;
				case 4:
					transform.position = Vector3.MoveTowards (transform.position, new Vector3 (-17.64f, 1.36f, 0), 9 * Time.deltaTime);
					if (Vector3.Distance (transform.position, new Vector3 (-17.64f, 1.36f, 0)) < 0.1f) {
						idStep++;
					}
					break;
				case 5:
					transform.position = new Vector3 (-17.64f, 1.36f, 0);
					transform.rotation = Quaternion.identity;
					isQTETriggered = false;
					rb2d.gravityScale = 30;
					break;
				}
				yield return new WaitForSeconds (0.01f);
				break;
			case 4:
				switch (idStep) {
				case 0:
					audiosrc.PlayOneShot (clipSweep);
					idStep++;
					break;
				case 1:
					transform.position = Vector3.MoveTowards (transform.position, new Vector3 (-14.36f, 1.71f, 0), 9 * Time.deltaTime);
					transform.Rotate (Vector3.forward * 3600 * Time.deltaTime);
					if (Vector3.Distance (transform.position, new Vector3 (-14.36f, 1.71f, 0)) < 0.1f) {
						idStep++;
					}
					break;
				case 2:
					transform.position = new Vector3 (-14.36f, 1.71f, 0);
					transform.rotation = Quaternion.identity;
					rb2d.gravityScale = 11;
					isQTETriggered = false;
					break;
				}
				yield return new WaitForSeconds (0.01f);
				break;
			case 5:
				switch (idStep) {
				case 0:
					transform.rotation = Quaternion.Euler (0, 0, 45);
					idStep++;
					break;
				case 1:
					transform.position = Vector3.MoveTowards (transform.position, new Vector3 (-17.66f, 1.3f, 0), 12 * Time.deltaTime);
					if (Vector3.Distance (transform.position, new Vector3 (-17.66f, 1.3f, 0)) < 0.1f) {
						idStep++;
					}
					break;
				case 2:
					transform.Rotate (Vector3.back * 600 * Time.deltaTime);
					if (transform.rotation.eulerAngles.z < 5 || transform.rotation.eulerAngles.z > 100) {
						transform.rotation = Quaternion.identity;
						audiosrc.PlayOneShot (clipJump);
						idStep++;
					}
					break;
				case 3:
					transform.position = Vector3.MoveTowards (transform.position, new Vector3 (-17.34f, 4.01f, 0), 12 * Time.deltaTime);
					if (Vector3.Distance (transform.position, new Vector3 (-17.34f, 4.01f, 0)) < 0.1f) {
						idStep++;
					}
					break;
				case 4:
					transform.position = Vector3.MoveTowards (transform.position, new Vector3 (-16.03f, 3.53f, 0), 12 * Time.deltaTime);
					if (Vector3.Distance (transform.position, new Vector3 (-16.03f, 3.53f, 0)) < 0.1f) {
						idStep++;
					}
					break;
				case 5:
					transform.position = new Vector3 (-16.03f, 3.53f, 0);
					isQTETriggered = false;
					rb2d.gravityScale = 30;
					break;
				}
				yield return new WaitForSeconds (0.01f);
				break;
			case 6:
				switch (idStep) {
				case 0:
					transform.position = Vector3.MoveTowards (transform.position, new Vector3 (-12.14f, -3.2f, 0), 9 * Time.deltaTime);
					if (Vector3.Distance (transform.position, new Vector3 (-12.14f, -3.2f, 0)) < 0.1f) {
						idStep++;
					}
					break;
				case 1:
					transform.position = Vector3.MoveTowards (transform.position, new Vector3 (-10.29f, -3.6f, 0), 9 * Time.deltaTime);
					if (Vector3.Distance (transform.position, new Vector3 (-10.29f, -3.6f, 0)) < 0.1f) {
						idStep++;
					}
					break;
				case 2:
					audiosrc.PlayOneShot (clipJump);
					idStep++;
					break;
				case 3:
					transform.position = Vector3.MoveTowards (transform.position, new Vector3 (-10.29f, -1.1f, 0), 9 * Time.deltaTime);
					if (Vector3.Distance (transform.position, new Vector3 (-10.29f, -1.1f, 0)) < 0.1f) {
						idStep++;
					}
					break;
				case 4:
					transform.position = Vector3.MoveTowards (transform.position, new Vector3 (-8.73f, -2.22f, 0), 9 * Time.deltaTime);
					if (Vector3.Distance (transform.position, new Vector3 (-8.73f, -2.22f, 0)) < 0.1f) {
						idStep++;
					}
					break;
				case 5:
					audiosrc.PlayOneShot (clipJump);
					idStep++;
					break;
				case 6:
					transform.position = Vector3.MoveTowards (transform.position, new Vector3 (-8.73f, 0f, 0), 9 * Time.deltaTime);
					if (Vector3.Distance (transform.position, new Vector3 (-8.73f, 0f, 0)) < 0.1f) {
						idStep++;
					}
					break;
				case 7:
					transform.position = Vector3.MoveTowards (transform.position, new Vector3 (-7.12f, -0.55f, 0), 9 * Time.deltaTime);
					if (Vector3.Distance (transform.position, new Vector3 (-7.12f, -0.55f, 0)) < 0.1f) {
						idStep++;
					}
					break;
				case 8:
					transform.position = new Vector3 (-7.12f, -0.55f, 0);
					isQTETriggered = false;
					rb2d.gravityScale = 30;
					break;
				}
				yield return new WaitForSeconds (0.01f);
				break;
			case 7:
				switch (idStep) {
				case 0:
					transform.rotation = Quaternion.Euler (0, 0, -33);
					idStep++;
					break;
				case 1:
					transform.position = Vector3.MoveTowards (transform.position, new Vector3 (-9.56f, 4.02f, 0), 12 * Time.deltaTime);
					if (Vector3.Distance (transform.position, new Vector3 (-9.56f, 4.02f, 0)) < 0.1f) {
						audiosrc.PlayOneShot (clipSweep);
						idStep++;
					}
					break;
				case 2:
					transform.Rotate (Vector3.forward * 3000 * Time.deltaTime);
					yolo++;
					if (yolo > 30) {
						idStep++;
					}
					break;
				case 3:
					transform.rotation = Quaternion.Euler (0, 0, -152);
					idStep++;
					break;
				case 4:
					transform.position = Vector3.MoveTowards (transform.position, new Vector3 (-7.24f, -0.57f, 0), 13 * Time.deltaTime);
					if (Vector3.Distance (transform.position, new Vector3 (-7.24f, -0.57f, 0)) < 0.1f) {
						idStep++;
					}
					break;
				case 5:
					audiosrc.PlayOneShot (clipLanding);
					transform.rotation = Quaternion.identity;
					transform.position = new Vector3 (-7.24f, -0.57f, 0);
					isQTETriggered = false;
					rb2d.gravityScale = 30;
					break;
				}
				yield return new WaitForSeconds (0.01f);
				break;
			case 8:
				switch (idStep) {
				case 0:
					transform.position = Vector3.MoveTowards (transform.position, new Vector3 (-14.29f, 1, 0), 15 * Time.deltaTime);
					if (Vector3.Distance (transform.position, new Vector3 (-14.29f, 1, 0)) < 0.1f) {
						idStep++;
					}
					break;
				case 1:
					transform.position = new Vector3 (-14.29f, 1, 0);
					isQTETriggered = false;
					rb2d.gravityScale = 11;
					break;
				}
				yield return new WaitForSeconds (0.01f);
				break;
			case 9:
				switch (idStep) {
				case 0:
					transform.position = Vector3.MoveTowards (transform.position, new Vector3 (-4.91f, -2.57f, 0), 9 * Time.deltaTime);
					if (Vector3.Distance (transform.position, new Vector3 (-4.91f, -2.57f, 0)) < 0.1f) {
						idStep++;
					}
					break;
				case 1:
					transform.position = Vector3.MoveTowards (transform.position, new Vector3 (-4.25f, -2.99f, 0), 9 * Time.deltaTime);
					if (Vector3.Distance (transform.position, new Vector3 (-4.25f, -2.99f, 0)) < 0.1f) {
						idStep++;
					}
					break;
				case 2:
					transform.position = new Vector3 (-4.25f, -2.99f, 0);
					isQTETriggered = false;
					rb2d.gravityScale = 30;
					break;
				}
				yield return new WaitForSeconds (0.01f);
				break;
			case 10:
				switch (idStep) {
				case 0:
					transform.rotation = Quaternion.Euler (0, 0, -67);
					transform.position = new Vector3 (-3.6f, -3.3f, 0);
					idStep++;
					audiosrc.mute = true;
					music.mute = true;
					audioAux.PlayOneShot (clipMoonwalk);
					break;
				case 1:
					transform.position = Vector3.MoveTowards (transform.position, new Vector3 (1.77f, -3f, 0), 20 * Time.deltaTime);
					if (Vector3.Distance (transform.position, new Vector3 (1.77f, -3f, 0)) < 0.1f) {
						idStep++;
					}
					break;
				case 2:
					transform.position = Vector3.MoveTowards (transform.position, new Vector3 (1.75f, -3.85f, 0), 9 * Time.deltaTime);
					if (Vector3.Distance (transform.position, new Vector3 (1.75f, -3.85f, 0)) < 0.1f) {
						transform.rotation = Quaternion.identity;
						animator.SetBool ("isWalking", true);
						idStep++;
					}
					break;
				case 3:
					transform.position = Vector3.MoveTowards (transform.position, new Vector3 (-3f, -3.82f, 0), 3 * Time.deltaTime);
					if (Vector3.Distance (transform.position, new Vector3 (-3f, -3.82f, 0)) < 0.1f) {
						audiosrc.mute = false;
						music.mute = false;
						idStep++;
					}
					break;
				case 4:
					transform.position = new Vector3 (-3f, -3.82f, 0);
					audioAux.Stop ();
					animator.SetBool ("isWalking", false);
					isQTETriggered = false;
					rb2d.gravityScale = 30;
					break;
				}
				yield return new WaitForSeconds (0.01f);
				break;
			case 11:
				switch (idStep) {
				case 0:
					spr.flipX = true;
					idStep++;
					break;
				case 1:
					transform.position = Vector3.MoveTowards (transform.position, new Vector3 (-3.31f, -2.23f, 0), 9 * Time.deltaTime);
					if (Vector3.Distance (transform.position, new Vector3 (-3.31f, -2.23f, 0)) < 0.1f) {
						idStep++;
					}
					break;
				case 2:
					transform.position = Vector3.MoveTowards (transform.position, new Vector3 (-4.28f, -3f, 0), 9 * Time.deltaTime);
					if (Vector3.Distance (transform.position, new Vector3 (-4.28f, -3f, 0)) < 0.1f) {
						audiosrc.PlayOneShot (clipJump);
						idStep++;
					}
					break;
				case 3:
					transform.position = Vector3.MoveTowards (transform.position, new Vector3 (-5.42f, -0.15f, 0), 9 * Time.deltaTime);
					if (Vector3.Distance (transform.position, new Vector3 (-5.42f, -0.15f, 0)) < 0.1f) {
						idStep++;
					}
					break;
				case 4:
					transform.position = Vector3.MoveTowards (transform.position, new Vector3 (-7f, -0.61f, 0), 9 * Time.deltaTime);
					if (Vector3.Distance (transform.position, new Vector3 (-7f, -0.61f, 0)) < 0.1f) {
						spr.flipX = false;
						idStep++;
					}
					break;
				case 5:
					transform.Rotate (Vector3.back * 360 * Time.deltaTime);
					if (transform.rotation.eulerAngles.z < 330) {
						audiosrc.PlayOneShot (clipJump);
						idStep++;
					}
					break;
				case 6:
					transform.position = Vector3.MoveTowards (transform.position, new Vector3 (-0.98f, 4.11f, 0), 12 * Time.deltaTime);
					if (Vector3.Distance (transform.position, new Vector3 (-0.98f, 4.11f, 0)) < 0.1f) {
						idStep++;
					}
					break;
				case 7:
					transform.position = new Vector3 (-0.98f, 4.11f, 0);
					transform.rotation = Quaternion.identity;
					isQTETriggered = false;
					rb2d.gravityScale = 11;
					break;
				}
				yield return new WaitForSeconds (0.01f);
				break;
			case 12:
				switch (idStep) {
				case 0:
					transform.position = Vector3.MoveTowards (transform.position, new Vector3 (1.02f, -2.21f, 0), 9 * Time.deltaTime);
					if (Vector3.Distance (transform.position, new Vector3 (1.02f, -2.21f, 0)) < 0.1f) {
						idStep++;
					}
					break;
				case 1:
					transform.position = Vector3.MoveTowards (transform.position, new Vector3 (0f, -1.5f, 0), 9 * Time.deltaTime);
					if (Vector3.Distance (transform.position, new Vector3 (0f, -1.5f, 0)) < 0.1f) {
						idStep++;
					}
					break;
				case 2:
					transform.position = Vector3.MoveTowards (transform.position, new Vector3 (1.58f, -0.32f, 0), 9 * Time.deltaTime);
					if (Vector3.Distance (transform.position, new Vector3 (1.58f, -0.32f, 0)) < 0.1f) {
						idStep++;
					}
					break;
				case 3:
					transform.position = Vector3.MoveTowards (transform.position, new Vector3 (1.58f, -0.32f, 0), 9 * Time.deltaTime);
					if (Vector3.Distance (transform.position, new Vector3 (1.58f, -0.32f, 0)) < 0.1f) {
						idStep++;
					}
					break;
				case 4:
					transform.position = Vector3.MoveTowards (transform.position, new Vector3 (2.52f, -3.03f, 0), 9 * Time.deltaTime);
					if (Vector3.Distance (transform.position, new Vector3 (2.52f, -3.03f, 0)) < 0.1f) {
						idStep++;
					}
					break;
				case 5:
					transform.position = new Vector3 (2.52f, -3.03f, 0);
					isQTETriggered = false;
					rb2d.gravityScale = 30;
					break;
				}
				yield return new WaitForSeconds (0.01f);
				break;
			case 13:
				switch(idStep){
				case 0:
					audiosrc.PlayOneShot (clipSweep);
					idStep++;
					break;
				case 1:
					transform.position = Vector3.MoveTowards (transform.position, new Vector3 (-1.04f, -0.63f, 0), 6 * Time.deltaTime);
					transform.Rotate (Vector3.forward * 3000 * Time.deltaTime);
					if (Vector3.Distance (transform.position, new Vector3 (-1.04f, -0.63f, 0)) < 0.1f) {
						audiosrc.PlayOneShot (clipJump);
						transform.rotation = Quaternion.identity;
						idStep++;
					}
					break;
				case 2:
					transform.position = Vector3.MoveTowards (transform.position, new Vector3 (1.23f, -0.13f, 0), 6 * Time.deltaTime);
					if (Vector3.Distance (transform.position, new Vector3 (1.23f, -0.13f, 0)) < 0.1f) {
						idStep++;
					}
					break;
				case 3:
					transform.position = Vector3.MoveTowards (transform.position, new Vector3 (2.52f, -2.97f, 0), 6 * Time.deltaTime);
					if (Vector3.Distance (transform.position, new Vector3 (2.52f, -2.97f, 0)) < 0.1f) {
						idStep++;
					}
					break;
				case 4:
					transform.position = new Vector3 (2.52f, -2.97f, 0);
					isQTETriggered = false;
					rb2d.gravityScale = 30;
					break;
				}
				yield return new WaitForSeconds (0.01f);
				break;
			case 14:
				switch (idStep){
				case 0:
					transform.position = Vector3.MoveTowards (transform.position, new Vector3 (4.81f, -2.13f, 0), 9 * Time.deltaTime);
					if (Vector3.Distance (transform.position, new Vector3 (4.81f, -2.13f, 0)) < 0.1f) {
						audiosrc.PlayOneShot (clipSweep);
						idStep++;
					}
					break;
				case 1:
					transform.position = Vector3.MoveTowards (transform.position, new Vector3 (5.84f, 3.49f, 0), 12 * Time.deltaTime);
					transform.Rotate (Vector3.forward * 3000 * Time.deltaTime);
					if (Vector3.Distance (transform.position, new Vector3 (5.84f, 3.49f, 0)) < 0.1f) {
						audiosrc.PlayOneShot (clipSweep);
						idStep++;
					}
					break;
				case 2:
					transform.position = Vector3.MoveTowards (transform.position, new Vector3 (6.43f, -2.99f, 0), 12 * Time.deltaTime);
					transform.Rotate (Vector3.back * 3000 * Time.deltaTime);
					if (Vector3.Distance (transform.position, new Vector3 (6.43f, -2.99f, 0)) < 0.1f) {
						idStep++;
					}
					break;
				case 3:
					transform.position = new Vector3 (6.43f, -2.99f, 0);
					transform.rotation = Quaternion.identity;
					audiosrc.PlayOneShot (clipLanding);
					isQTETriggered = false;
					rb2d.gravityScale = 30;
					break;
				}
				yield return new WaitForSeconds (0.01f);
				break;
			case 15:
				switch (idStep) {
				case 0:
					transform.position = Vector3.MoveTowards (transform.position, new Vector3 (8.43f, 1.73f, 0), 12 * Time.deltaTime);
					if (Vector3.Distance (transform.position, new Vector3 (8.43f, 1.73f, 0)) < 0.1f) {
						idStep++;
					}
					break;
				case 1:
					transform.Rotate (Vector3.back * 700 * Time.deltaTime);
					if (transform.rotation.eulerAngles.z < 250) {
						transform.rotation = Quaternion.Euler (0,0,-109);
						idStep++;
					}
					break;
				case 2:
					transform.position = Vector3.MoveTowards (transform.position, new Vector3 (12.3f, 0f, 0), 15 * Time.deltaTime);
					if (Vector3.Distance (transform.position, new Vector3 (12.3f, 0f, 0)) < 0.1f) {
						audiosrc.PlayOneShot (clipLanding);
						idStep++;
					}
					break;
				case 3:
					transform.position = Vector3.MoveTowards (transform.position, new Vector3 (12.57f, -0.69f, 0), 9 * Time.deltaTime);
					transform.Rotate (Vector3.back * 3000 * Time.deltaTime);
					if (Vector3.Distance (transform.position, new Vector3 (12.57f, -0.69f, 0)) < 0.1f) {
						idStep++;
					}
					break;
				case 4:
					transform.position = new Vector3 (12.57f, -0.69f, 0);
					transform.rotation = Quaternion.identity;
					isQTETriggered = false;
					rb2d.gravityScale = 30;
					break;
				}
				yield return new WaitForSeconds (0.01f);
				break;
			case 16:
				switch (idStep) {
				case 0:
					transform.position = Vector3.MoveTowards (transform.position, new Vector3 (12.56f, 3.23f, 0), 9 * Time.deltaTime);
					if (Vector3.Distance (transform.position, new Vector3 (12.56f, 3.23f, 0)) < 0.1f) {
						idStep++;
					}
					break;
				case 1:
					transform.position = Vector3.MoveTowards (transform.position, new Vector3 (15.42f, 2f, 0), 9 * Time.deltaTime);
					if (Vector3.Distance (transform.position, new Vector3 (15.42f, 2f, 0)) < 0.1f) {
						idStep++;
					}
					break;
				case 2:
					transform.position = Vector3.MoveTowards (transform.position, new Vector3 (17f, -3.71f, 0), 9 * Time.deltaTime);
					if (Vector3.Distance (transform.position, new Vector3 (17f, -3.71f, 0)) < 0.1f) {
						idStep++;
					}
					break;
				case 3:
					transform.position = new Vector3 (17f, -3.71f, 0);
					isQTETriggered = false;
					rb2d.gravityScale = 30;
					break;
				}
				yield return new WaitForSeconds (0.01f);
				break;
			case 17:
				switch (idStep) {
				case 0:
					transform.position = Vector3.MoveTowards (transform.position, new Vector3 (11.95f, -1.37f, 0), 9 * Time.deltaTime);
					if (Vector3.Distance (transform.position, new Vector3 (11.95f, -1.37f, 0)) < 0.1f) {
						audiosrc.PlayOneShot (clipJump);
						idStep++;
					}
					break;
				case 1:
					transform.position = Vector3.MoveTowards (transform.position, new Vector3 (11.49f, 1.31f, 0), 9 * Time.deltaTime);
					if (Vector3.Distance (transform.position, new Vector3 (11.49f, 1.31f, 0)) < 0.1f) {
						idStep++;
						music.volume = 0;
						audioAux.PlayOneShot (musicStar);
					}
					break;
				case 2:
					transform.position = Vector3.MoveTowards (transform.position, new Vector3 (14.23f, 0.82f, 0), 9 * Time.deltaTime);
					if (Vector3.Distance (transform.position, new Vector3 (14.23f, 0.82f, 0)) < 0.1f) {
						trend.time = 1;
						Destroy (star);
						StarHolder.star = true;
						idStep++;
					}
					break;
				case 3:
					transform.position = Vector3.MoveTowards (transform.position, new Vector3 (14.29f, 3f, 0), 12 * Time.deltaTime);
					if (Vector3.Distance (transform.position, new Vector3 (14.29f, 3f, 0)) < 0.1f) {
						idStep++;
					}
					break;
				case 4:
					transform.Rotate (Vector3.forward * 3000 * Time.deltaTime);
					yolo++;
					if (yolo > 20) {
						transform.rotation = Quaternion.Euler (0, 0, -275);
						spr.flipX = true;
						idStep++;
					}
					break;
				case 5:
					transform.position = Vector3.MoveTowards (transform.position, new Vector3 (-4.72f, 1.2f, 0), 10 * Time.deltaTime);
					if (Vector3.Distance (transform.position, new Vector3 (-4.72f, 1.2f, 0)) < 0.1f) {
						transform.rotation = Quaternion.Euler (0, 0, -148);
						spr.flipX = false;
						idStep++;
					}
					break;
				case 6:
					transform.position = Vector3.MoveTowards (transform.position, new Vector3 (-2.43f, -3.32f, 0), 10 * Time.deltaTime);
					if (Vector3.Distance (transform.position, new Vector3 (-2.43f, -3.32f, 0)) < 0.1f) {
						transform.rotation = Quaternion.Euler (0, 0, -80);
						idStep++;
					}
					break;
				case 7:
					transform.position = Vector3.MoveTowards (transform.position, new Vector3 (22.56f, -1.38f, 0), 10 * Time.deltaTime);
					if (Vector3.Distance (transform.position, new Vector3 (22.56f, -1.38f, 0)) < 0.1f) {
						transform.rotation = Quaternion.Euler (0, 0, -228);
						spr.flipX = true;
						idStep++;
					}
					break;
				case 8:
					transform.position = Vector3.MoveTowards (transform.position, new Vector3 (20.22f, -3.74f, 0), 10 * Time.deltaTime);
					if (Vector3.Distance (transform.position, new Vector3 (20.22f, -3.74f, 0)) < 0.1f) {
						spr.flipX = false;
						audiosrc.PlayOneShot (clipLanding);
						transform.rotation = Quaternion.Euler (0, 0, -77);
						idStep++;
					}
					break;
				case 9:
					transform.position = Vector3.MoveTowards (transform.position, new Vector3 (25.65f, -3.5f, 0), 10 * Time.deltaTime);
					if (Vector3.Distance (transform.position, new Vector3 (25.65f, -3.5f, 0)) < 0.1f) {
						idStep++;
					}
					break;
				case 10:
					transform.position = Vector3.MoveTowards (transform.position, new Vector3 (37.04f, 0.84f, 0), 10 * Time.deltaTime);
					if (Vector3.Distance (transform.position, new Vector3 (37.04f, 0.84f, 0)) < 0.1f) {
						idStep++;
					}
					break;
				case 11:
					transform.position = Vector3.MoveTowards (transform.position, new Vector3 (38.76f, -2.21f, 0), 10 * Time.deltaTime);
					transform.Rotate (Vector3.forward * 3000 * Time.deltaTime);
					if (Vector3.Distance (transform.position, new Vector3 (37.04f, -2.21f, 0)) < 0.1f) {
						idStep++;
					}
					break;
				case 12:
					transform.position = new Vector3 (37.04f, -2.21f, 0);
					isQTETriggered = false;
					rb2d.gravityScale = 30;
					break;
				}
				yield return new WaitForSeconds (0.01f);
				break;
			case 18:
				switch (idStep) {
				case 0:
					transform.rotation = Quaternion.Euler (0, 0, -40);
					idStep++;
					break;
				case 1:
					transform.position = Vector3.MoveTowards (transform.position, new Vector3 (25.54f, 3.08f, 0), 12 * Time.deltaTime);
					if (Vector3.Distance (transform.position, new Vector3 (25.54f, 3.08f, 0)) < 0.1f) {
						audiosrc.PlayOneShot (clipSweep);
						idStep++;
					}
					break;
				case 2:
					transform.position = Vector3.MoveTowards (transform.position, new Vector3 (20.82f, 0.95f, 0), 12 * Time.deltaTime);
					transform.Rotate (Vector3.forward * 3000 * Time.deltaTime);
					if (Vector3.Distance (transform.position, new Vector3 (20.82f, 0.95f, 0)) < 0.1f) {
						audiosrc.PlayOneShot (clipSweep);
						idStep++;
					}
					break;
				case 3:
					transform.position = Vector3.MoveTowards (transform.position, new Vector3 (26.66f, -0.49f, 0), 12 * Time.deltaTime);
					transform.Rotate (Vector3.forward * 3000 * Time.deltaTime);
					if (Vector3.Distance (transform.position, new Vector3 (26.66f, -0.49f, 0)) < 0.1f) {
						audiosrc.PlayOneShot (clipSweep);
						idStep++;
					}
					break;
				case 4:
					transform.position = Vector3.MoveTowards (transform.position, new Vector3 (25.83f, -3.64f, 0), 12 * Time.deltaTime);
					transform.Rotate (Vector3.forward * 3000 * Time.deltaTime);
					if (Vector3.Distance (transform.position, new Vector3 (25.83f, -3.64f, 0)) < 0.1f) {
						idStep++;
					}
					break;
				case 5:
					transform.position = new Vector3 (25.83f, -3.64f, 0);
					transform.rotation = Quaternion.identity;
					audiosrc.PlayOneShot (clipLanding);
					isQTETriggered = false;
					rb2d.gravityScale = 30;
					break;
				}
				yield return new WaitForSeconds (0.01f);
				break;
			case 19:
				switch (idStep) {
				case 0:
					transform.position = Vector3.MoveTowards (transform.position, new Vector3 (26.37f, -2.64f, 0), 9 * Time.deltaTime);
					if (Vector3.Distance (transform.position, new Vector3 (26.37f, -2.64f, 0)) < 0.1f) {
						idStep++;
					}
					break;
				case 1:
					transform.position = Vector3.MoveTowards (transform.position, new Vector3 (26.97f, -3.01f, 0), 9 * Time.deltaTime);
					if (Vector3.Distance (transform.position, new Vector3 (26.97f, -3.01f, 0)) < 0.1f) {
						audiosrc.PlayOneShot (clipJump);
						idStep++;
					}
					break;
				case 2:
					transform.position = Vector3.MoveTowards (transform.position, new Vector3 (28.36f, 0.36f, 0), 9 * Time.deltaTime);
					if (Vector3.Distance (transform.position, new Vector3 (28.36f, 0.36f, 0)) < 0.1f) {
						idStep++;
					}
					break;
				case 3:
					transform.position = Vector3.MoveTowards (transform.position, new Vector3 (30.13f, -0.34f, 0), 9 * Time.deltaTime);
					if (Vector3.Distance (transform.position, new Vector3 (30.13f, -0.34f, 0)) < 0.1f) {
						idStep++;
					}
					break;
				case 4:
					transform.position = new Vector3 (30.13f, -0.34f, 0);
					isQTETriggered = false;
					rb2d.gravityScale = 11;
					break;
				}
				yield return new WaitForSeconds (0.01f);
				break;
			case 20:
				switch (idStep) {
				case 0:
					transform.position = Vector3.MoveTowards (transform.position, new Vector3 (30.26f, -2.57f, 0), 9 * Time.deltaTime);
					if (Vector3.Distance (transform.position, new Vector3 (30.26f, -2.57f, 0)) < 0.1f) {
						idStep++;
					}
					break;
				case 1:
					transform.position = Vector3.MoveTowards (transform.position, new Vector3 (29.99f, 0.79f, 0), 9 * Time.deltaTime);
					if (Vector3.Distance (transform.position, new Vector3 (29.99f, 0.79f, 0)) < 0.1f) {
						idStep++;
					}
					break;
				case 2:
					transform.position = Vector3.MoveTowards (transform.position, new Vector3 (32f, 1.61f, 0), 9 * Time.deltaTime);
					if (Vector3.Distance (transform.position, new Vector3 (32f, 1.61f, 0)) < 0.1f) {
						idStep++;
					}
					break;
				case 3:
					transform.position = Vector3.MoveTowards (transform.position, new Vector3 (32.63f, -3f, 0), 6 * Time.deltaTime);
					if (Vector3.Distance (transform.position, new Vector3 (32.63f, -3f, 0)) < 0.1f) {
						audiosrc.PlayOneShot (clipJump);
						idStep++;
					}
					break;
				case 4:
					transform.position = Vector3.MoveTowards (transform.position, new Vector3 (32f, -3.14f, 0), 6 * Time.deltaTime);
					if (Vector3.Distance (transform.position, new Vector3 (32f, -3.14f, 0)) < 0.1f) {
						idStep++;
					}
					break;
				case 5:
					transform.position = new Vector3 (32f, -3.14f, 0);
					isQTETriggered = false;
					rb2d.gravityScale = 9;
					break;
				}
				yield return new WaitForSeconds (0.01f);
				break;
			case 21:
				switch(idStep){
				case 0:
					transform.position = Vector3.MoveTowards (transform.position, new Vector3 (33.07f, -2.29f, 0), 6 * Time.deltaTime);
					if (Vector3.Distance (transform.position, new Vector3 (33.07f, -2.29f, 0)) < 0.1f) {
						idStep++;
					}
					break;
				case 1:
					transform.position = new Vector3 (33.07f, -2.29f, 0);
					isQTETriggered = false;
					rb2d.gravityScale = 9;
					break;
				}
				yield return new WaitForSeconds (0.01f);
				break;
			case 22:
				switch(idStep){
				case 0:
					transform.position = new Vector3 (33, -2.78f, 0);
					idStep++;
					break;
				case 1:
					transform.position = Vector3.MoveTowards (transform.position, new Vector3 (32.42f, 1.65f, 0), 7 * Time.deltaTime);
					if (Vector3.Distance (transform.position, new Vector3 (32.42f, 1.65f, 0)) < 0.1f) {
						audiosrc.PlayOneShot (clipSweep);
						idStep++;
					}
					break;
				case 2:
					transform.position = Vector3.MoveTowards (transform.position, new Vector3 (37.24f, 3.71f, 0), 7 * Time.deltaTime);
					transform.Rotate (Vector3.forward * 3000 * Time.deltaTime);
					if (Vector3.Distance (transform.position, new Vector3 (37.24f, 3.71f, 0)) < 0.1f) {
						idStep++;
					}
					break;
				case 3:
					transform.position = new Vector3 (37.24f, 3.71f, 0);
					transform.rotation = Quaternion.identity;
					isQTETriggered = false;
					rb2d.gravityScale = 30;
					break;
				}
				yield return new WaitForSeconds (0.01f);
				break;
			case 23:
				switch (idStep) {
				case 0:
					transform.position = Vector3.MoveTowards (transform.position, new Vector3 (37.67f, -1.45f, 0), 9 * Time.deltaTime);
					if (Vector3.Distance (transform.position, new Vector3 (37.67f, -1.45f, 0)) < 0.1f) {
						idStep++;
					}
					break;
				case 1:
					transform.position = Vector3.MoveTowards (transform.position, new Vector3 (38.87f, -2.12f, 0), 9 * Time.deltaTime);
					if (Vector3.Distance (transform.position, new Vector3 (38.87f, -2.12f, 0)) < 0.1f) {
						idStep++;
					}
					break;
				case 2:
					transform.position = new Vector3 (38.87f, -2.12f, 0);
					isQTETriggered = false;
					break;
				}
				yield return new WaitForSeconds (0.01f);
				break;
			}
		}
	}
}
