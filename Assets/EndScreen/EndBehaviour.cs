using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class EndBehaviour : MonoBehaviour {

	public GameObject fade;
	public AudioClip theme;
	public VideoClip end1;
	public VideoClip end2;
	public VideoClip end3;
	public GameObject canv1;
	public GameObject canv2;
	public Sprite fungi;
	public Sprite fungiDef;
	public Sprite fungiExpl;
	public Sprite starY;
	public Sprite starN;

	private VideoPlayer vidpl;
	private AudioSource audsrc;
	private Text tStar;
	private Text tEnd;
	private Text tFact;
	private SpriteRenderer spStar;
	private SpriteRenderer spEnd;
	private bool cinPlayed;

	void Start () {
		vidpl = GetComponent<VideoPlayer> ();
		audsrc = GetComponent<AudioSource> ();
		tStar = canv2.transform.GetChild (0).GetComponent<Text> ();
		tEnd = canv2.transform.GetChild (1).GetComponent<Text> ();
		tFact = canv2.transform.GetChild (2).GetComponent<Text> ();
		spStar = canv2.transform.GetChild (4).GetComponent<SpriteRenderer> ();
		spEnd = canv2.transform.GetChild (5).GetComponent<SpriteRenderer> ();
		cinPlayed = false;
	}

	void Update () {
		if(!cinPlayed){
			if(Input.GetKeyDown (KeyCode.W)){
				vidpl.clip = end1;
				StartCoroutine ("ending",1);
			}else if(Input.GetKeyDown (KeyCode.G)){
				vidpl.clip = end3;
				StartCoroutine ("ending",3);
			}else if(Input.GetKeyDown (KeyCode.N)){
				vidpl.clip = end2;
				StartCoroutine ("ending",2);
			}
		}

		if (Input.GetKey (KeyCode.Escape)) {//Quitte le jeu
			Application.Quit ();
		}
	}

	IEnumerator ending(int nbEnd){
		SpriteRenderer sprend = fade.GetComponent<SpriteRenderer> ();
		cinPlayed = true;
		while(sprend.color.a < 0.98f){
			sprend.color = new Color (0,0,0,sprend.color.a+0.05f);
			yield return new WaitForSeconds (0.005f);
		}
		vidpl.Play ();
		while(vidpl.isPlaying){
			yield return new WaitForSeconds (0.1f);
		}
		vidpl.Stop ();
		Debug.Log ("yas");
		sprend.color = new Color (0, 0, 0, 0);
		canv1.SetActive (false);
		audsrc.clip = theme;
		audsrc.Play ();
		audsrc.loop = true;
		switch(nbEnd){
		case 1:
			tFact.text = "Fungoad est sain et sauf !";
			tEnd.text = "Vous avez sauvé Fungoad !";
			spEnd.sprite = fungi;
			break;
		case 2:
			tFact.text = "Fungoad est blessé mais vivant !";
			tEnd.text = "Fungoad est (presque) sain et sauf !";
			spEnd.sprite = fungiExpl;
			break;
		case 3:
			tFact.text = "Sympa la soupe de champi...";
			tEnd.text = "Les champis sont vos amis !";
			spEnd.sprite = fungiDef;
			break;
		}
		if(StarHolder.star){
			spStar.sprite = starY;
			tStar.text = "Vous avez trouvé l'étoile !";
		}else{
			spStar.sprite = starN;
			tStar.text = "Vous n'avez pas trouvé l'étoile !";
		}
	}
}
