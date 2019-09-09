using UnityEngine;
using System.Collections;

public class simpleCandy : MonoBehaviour {

	public GameObject marcaPosibleJugada;
	public GameObject myBrandPossibleplay;

	public GameObject  miExplo;

	public GameObject miExploPre;

	public int tip;
	public int file;
	public int fileFuture;
	public int cols;
	public bool imSelected = false;
	public bool touchFloor = true;
	public float speedFall = 0;

	public int idDestroySweet = 1;

	public string tipSweetDestroy;
	// Use this for initialization

	void Awake(){
		myBrandPossibleplay = Instantiate(marcaPosibleJugada,transform.position,transform.rotation) as GameObject;
		myBrandPossibleplay.gameObject.SetActive(false);
		miExploPre = Instantiate(miExplo,transform.position,transform.rotation) as GameObject;
		if (this.transform.Find("superDulce")!=null){
			this.transform.Find("superDulce").gameObject.GetComponent<Renderer>().enabled = false;
		}
		if (this.transform.Find("bomba")!=null){
			this.transform.Find("bomba").gameObject.GetComponent<Renderer>().enabled = false;
		}
	}

	void Start () {

	}

	public void constructor(int tip,int file,int cols){
		this.tip = tip;
		this.file = file;
		this.cols = cols;
	}

	public void nuevaPocicion(int file,int cols){
		this.file = file;
		this.cols = cols;
		transform.position = new Vector3(this.cols,this.file*-1,0);
	}

	public void incrementarScale(){
		myBrandPossibleplay.gameObject.SetActive(true);
		myBrandPossibleplay.transform.position = transform.position;
		transform.localScale = new Vector3(1.2f,1.2f,1);
	}

	public void decrementarScale(){
		myBrandPossibleplay.gameObject.SetActive(false);
		transform.localScale = new Vector3(1,1,1);
	}
	// Update is called once per frame
	void Update () {
		if (!touchFloor){
			caer();
		}
	}

	void OnMouseDown() {
		GameObject mycamera = GameObject.Find("mycamera");
		if (!mycamera.GetComponent<GameControl>().changingsweet){
			imSelected = true;
		}
	}

	void OnMouseUp(){
		imSelected = false;
	}

	void OnMouseExit() {

		if (imSelected){
			GameObject mycamera = GameObject.Find("mycamera");
			imSelected = false;
			if (mycamera.GetComponent<GameControl>().numCandyInAction == mycamera.GetComponent<GameControl>().numTotalCandyMoves && !mycamera.GetComponent<GameControl>().changingsweet){
				float hitdist = 8.0f;
				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
				Vector2 posMouse = ray.GetPoint(hitdist);

				Vector2 posDulce = transform.position;
			
				float distX = Mathf.Abs((posMouse.x)-posDulce.x);
				float distY = Mathf.Abs(posMouse.y-posDulce.y);
				if (distX>distY){
					if ((posMouse.x)<posDulce.x){
						mycamera.GetComponent<GameControl>().performPlay = true;
						mycamera.GetComponent<GameControl>().continousplay=0;
						mycamera.GetComponent<GameControl>().swapDulces(file,cols,file,cols-1,"izquierda");

					} else {
						mycamera.GetComponent<GameControl>().performPlay = true;
						mycamera.GetComponent<GameControl>().continousplay=0;
						mycamera.GetComponent<GameControl>().swapDulces(file,cols,file,cols+1,"derecha");
					
					}
				} else {
					if (posMouse.y<posDulce.y){
						mycamera.GetComponent<GameControl>().performPlay = true;
						mycamera.GetComponent<GameControl>().continousplay=0;
						mycamera.GetComponent<GameControl>().swapDulces(file,cols,file+1,cols,"abajo");
					
					} else {
						mycamera.GetComponent<GameControl>().performPlay = true;
						mycamera.GetComponent<GameControl>().continousplay=0;
						mycamera.GetComponent<GameControl>().swapDulces(file,cols,file-1,cols,"arriba");
					}
				}
			}

		}

	}

	public void dejarCaerDulce(int file,int cols){

		fileFuture = file;
		this.cols = cols;
		touchFloor = false;
		GameObject mycamera = GameObject.Find("mycamera");
		mycamera.GetComponent<GameControl>().numCandyInAction--;


	}

	void caer(){
		if (transform.position.y>fileFuture*-1){
			speedFall+=0.3f;
			if (speedFall>=0) transform.Translate(0,-Time.deltaTime*speedFall,0);
		} else{
			speedFall = 0f;
			touchFloor = true;
			nuevaPocicion(fileFuture,this.cols);
			GameObject mycamera = GameObject.Find("mycamera");
			mycamera.GetComponent<GameControl>().numCandyInAction++;
			mycamera.GetComponent<GameControl>().ExplodeCandy.Add(this.gameObject);

		}
	}

	public void mostrarSuperDulce(){

		GameObject mycamera = GameObject.Find("mycamera");
		if (this.transform.Find("superDulce")!=null){

			this.transform.Find("superDulce").gameObject.GetComponent<Renderer>().enabled = true;
			tipSweetDestroy = mycamera.GetComponent<GameControl>().typePlay;
			if (mycamera.GetComponent<GameControl>().typePlay == "horizontal"){
				this.transform.Find("superDulce").transform.rotation = new Quaternion(0.0f, 0.0f, 0.7f, 0.7f);
			} else {
				this.transform.Find("superDulce").transform.rotation = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);
			}
		}
	}

	public void mostrarBombaDulce(){
		GameObject mycamera = GameObject.Find("mycamera");
		if (this.transform.Find("bomba")!=null){
			this.transform.Find("bomba").gameObject.GetComponent<Renderer>().enabled = true;
			tipSweetDestroy = mycamera.GetComponent<GameControl>().typePlay;
		}
	}

	public void efectoSuperDulce(){
		GameObject mycamera = GameObject.Find("mycamera");

		if (this.transform.Find("superDulce")!=null){
			if (this.transform.Find("superDulce").gameObject.GetComponent<Renderer>().enabled){
				if (tipSweetDestroy == "horizontal"){
					mycamera.GetComponent<GameControl>().candyExplodeHorizontal(this.file,this.cols);
				}
				if (tipSweetDestroy == "vertical"){
					mycamera.GetComponent<GameControl>().dulcesExploVertical(this.file,this.cols);
				}
			}

		}
		if (this.transform.Find("bomba")!=null){
			if (this.transform.Find("bomba").gameObject.GetComponent<Renderer>().enabled){
			
				mycamera.GetComponent<GameControl>().candyExplodeBomb(this.file,this.cols);
			
			}
		}

		if (this.transform.Find("superDulce")!=null) this.transform.Find("superDulce").gameObject.GetComponent<Renderer>().enabled = false;
		if (this.transform.Find("bomba")!=null) this.transform.Find("bomba").gameObject.GetComponent<Renderer>().enabled = false;

		if (mycamera.GetComponent<GameControl>().typePlay == "bomba"){
			mostrarBombaDulce();
		} else {
			mostrarSuperDulce();
		}

	}

	public void destruirCDulce(){

		GameObject mycamera = GameObject.Find("mycamera");
		mycamera.GetComponent<GameControl>().sweetMatrix[this.file,this.cols] = 0;
		mycamera.GetComponent<GameControl>().sweetMatrixGO[this.file,this.cols] = null;
		if (miExploPre) miExploPre.GetComponent<CandyExplode>().habilitar(transform.position);

		if (this.transform.Find("superDulce")!=null){
			if (this.transform.Find("superDulce").gameObject.GetComponent<Renderer>().enabled){
				if (tipSweetDestroy == "horizontal"){
					mycamera.GetComponent<GameControl>().candyExplodeHorizontal(this.file,this.cols);
				}
				if (tipSweetDestroy == "vertical"){
					mycamera.GetComponent<GameControl>().dulcesExploVertical(this.file,this.cols);
				}
			}
		}
		if (this.transform.Find("bomba")!=null){
			if (this.transform.Find("bomba").gameObject.GetComponent<Renderer>().enabled){

					mycamera.GetComponent<GameControl>().candyExplodeBomb(this.file,this.cols);

			}
		}

		mycamera.GetComponent<GameControl>().turnOffPossiblePlay();

		Destroy (this);
		Destroy (gameObject);
		
	
	}
}
