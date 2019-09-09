using UnityEngine;
using System.Collections;

public class CandyExplode : MonoBehaviour {

	public AudioClip sonidoExplo;

	void Awake(){
		gameObject.SetActive(false);
	}

	// Use this for initialization
	void Start () {


	}

	public void habilitar(Vector3 miPos){
		transform.position = miPos;
		gameObject.SetActive(true);
		AudioSource.PlayClipAtPoint(sonidoExplo, transform.position);
		//audio.PlayOneShot(sonidoExplo);
		Destroy (this,1);
		Destroy (gameObject,1);
	}

	// Update is called once per frame
	void Update () {
	
	}
}
