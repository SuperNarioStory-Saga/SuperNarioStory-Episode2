using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class StarBehaviour : MonoBehaviour {

	public AudioClip clipStart;
	public GameObject fade;
	public VideoClip cin;
	public GameObject vidplHold;

	private TrailRenderer trRend;
	private AudioSource audiosrc;
	private AudioSource music;
	private VideoPlayer vidpl;

	void Start () {
		trRend = GetComponent<TrailRenderer> ();
		trRend.sortingLayerName = "QTE";
		audiosrc = GetComponents<AudioSource> ()[0];
		music = GetComponents<AudioSource> () [1];
		vidpl = vidplHold.GetComponent<VideoPlayer> ();
		StartCoroutine("Boing");
	}

	IEnumerator Boing(){
		float i = 0;
		while (true) {
			if (this.transform.position.x > 11) {
				i = 0;
				trRend.Clear ();
				this.transform.position = new Vector3 (-11.5f, this.transform.position.y, this.transform.position.z);
			} else {
				this.transform.position = new Vector3 (this.transform.position.x + 0.1f, Mathf.Abs (Mathf.Sin (i)*1.2f)-2.25f, this.transform.position.z);
				i += 0.1f;
				yield return new WaitForSeconds (0.01f);
			}	
		}	
	}

	IEnumerator Launch(){
		music.volume = 0.3f;
		audiosrc.PlayOneShot (clipStart);
		SpriteRenderer sprfade = fade.GetComponent<SpriteRenderer> ();
		while(sprfade.color.a < 0.97f){
			if(music.volume > 0){
				music.volume -= 0.05f;
			}
			sprfade.color = new Vector4 (0, 0, 0, sprfade.color.a + 0.05f);
			yield return new WaitForSeconds (0.001f);
		}
		yield return new WaitForSeconds (0.2f);
		vidpl.Play ();
		while(vidpl.isPlaying){
			yield return new WaitForSeconds (0.2f);
		}
		SceneManager.LoadScene ("Platform");
	}

	void Update(){
		if(Input.GetKeyDown (KeyCode.Return)){
			StartCoroutine ("Launch");
		}

		if (Input.GetKey (KeyCode.Escape)) {//Quitte le jeu
			Application.Quit ();
		}
	}
}
