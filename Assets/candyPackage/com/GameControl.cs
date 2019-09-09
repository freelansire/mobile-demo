using UnityEngine;
using System.Collections; 
using System.Collections.Generic;

public class GameControl : MonoBehaviour {

	public GUISkin _theSkin;

	public  GameObject  dulce01;
	public  GameObject  dulce02;
	public  GameObject  dulce03;
	public  GameObject  dulce04;
	public  GameObject  dulce05;
	public  GameObject  dulce06;

	public  GameObject  dulce100;

	public Texture tex;

	public bool changingsweet = false;
	public int file1,col1,file2,col2;
	public string direction;

	public int continousplay = 0;
	public bool performPlay = false;

	public bool returning = false;

	public string typePlay;


		// Here we define the size of the candy matrix
	private const int numfiles = 9;
	private const int numCol = 8;

	private int Score = 0;

	private float ifPlayerPlayed =0;
	private bool showPossiblePlay = false;
	private GameObject destroySweetMoves1;
	private GameObject destroySweetMoves2;

	/*public int[,] sweetMatrix = {  {0, 0, 0, 0, 0, 0, 0, 0},
		{0, 0, 0, 0, 0, 0, 0, 0},
		{0, 0, 0, 0, 0, 0, 0, 0},
		{0, 0, 0, 0, 0, 0, 0, 0},
		{0, 0, 0, 0, 0, 0, 0, 0},
		{0, 0, 0, 0, 0, 0, 0, 0},
		{0, 0, 0, 0, 0, 0, 0, 0},
		{0, 0, 0, 0, 0, 0, 0, 0}};
*/

	public int[,] sweetMatrix = new int[numfiles, numCol];

	public GameObject[,] sweetMatrixGO = new GameObject[numfiles,numCol];

	public List<GameObject> EliminateCandy  = new List<GameObject>();
	public List<GameObject> ExplodeCandy  = new List<GameObject>();
	public int numCandyInAction;
	public int numTotalCandyMoves;
	// Use this for initialization
	void Start () {

		// We create a scenario with random candy making sure there are no plays.
		createGame();

	}


	void createGame(){

		GameObject   myCandy;
		int indexCandy;
		int i,j;

		numCandyInAction = numfiles*numCol;
		numTotalCandyMoves = numfiles*numCol;

		for (i=0;i<numfiles;i++){
			for (j=0;j<numCol;j++){
				do {
					sweetMatrix[i,j] = Random.Range(1,7);
					indexCandy = sweetMatrix[i,j];
				}while (ifPlayerNotPlayed(i,j,indexCandy));
				
				switch (indexCandy)
				{
				case 1:
					myCandy = Instantiate(dulce01, new Vector3(j, i*-1, 0),transform.rotation) as GameObject;
					myCandy.GetComponent<simpleCandy>().constructor(indexCandy,i,j);
					sweetMatrixGO[i,j] = myCandy;
					break;
				case 2:
					myCandy = Instantiate(dulce02, new Vector3(j, i*-1, 0),transform.rotation) as GameObject;
					myCandy.GetComponent<simpleCandy>().constructor(indexCandy,i,j);
					sweetMatrixGO[i,j] = myCandy;
					break;
				case 3:
					myCandy = Instantiate(dulce03, new Vector3(j, i*-1, 0),transform.rotation) as GameObject;
					myCandy.GetComponent<simpleCandy>().constructor(indexCandy,i,j);
					sweetMatrixGO[i,j] = myCandy;
					break;
				case 4:
					myCandy = Instantiate(dulce04, new Vector3(j, i*-1, 0),transform.rotation) as GameObject;
					myCandy.GetComponent<simpleCandy>().constructor(indexCandy,i,j);
					sweetMatrixGO[i,j] = myCandy;
					break;
				case 5:
					myCandy = Instantiate(dulce05, new Vector3(j, i*-1, 0),transform.rotation) as GameObject;
					myCandy.GetComponent<simpleCandy>().constructor(indexCandy,i,j);
					sweetMatrixGO[i,j] = myCandy;
					break;
				case 6:
					myCandy = Instantiate(dulce06, new Vector3(j, i*-1, 0),transform.rotation) as GameObject;
					myCandy.GetComponent<simpleCandy>().constructor(indexCandy,i,j);
					sweetMatrixGO[i,j] = myCandy;
					break;
				}
				
				
				
			}
		}

	}

	// function that starts a new candy matrix
	void destroyAllSweets(){
		int i;
		int j;
		for (i=0;i<numfiles;i++){
			for (j=0;j<numCol;j++){
				sweetMatrixGO[i,j].GetComponent<simpleCandy>().destruirCDulce();
			}
		}

	}
	// Update is called once per frame
	void Update () {

		if (!showPossiblePlay){
			if (ifPlayerPlayed<4) {
				ifPlayerPlayed = ifPlayerPlayed + 1*Time.deltaTime;
			} else {
				checkIfPlayerWasteTime();
				showPossiblePlay = true;
			}
		}
				// We check the candy list to see if they should be destroyed
		if (ExplodeCandy.Count>0 && numCandyInAction == numTotalCandyMoves){
			candyExplodeVertical();
		} 

		// Start the candy swap for another
		if (this.changingsweet){
			simpleCandy claseDulce1 = sweetMatrixGO[file1,col1].GetComponent<simpleCandy>();
			simpleCandy claseDulce2 = sweetMatrixGO[file2,col2].GetComponent<simpleCandy>();

			switch (this.direction)
			{
			case "arriba":
				if (claseDulce1.transform.position.y<this.file2*-1){
					claseDulce1.transform.Translate(0,Time.deltaTime*5,0);
					claseDulce2.transform.Translate(0,-Time.deltaTime*5,0);
				} else{
					this.cambiarPosicionTotal();
					this.changingsweet = false;
				}
				break;
			case "abajo":
				if (claseDulce1.transform.position.y>this.file2*-1){
					claseDulce1.transform.Translate(0,-Time.deltaTime*5,0);
					claseDulce2.transform.Translate(0,Time.deltaTime*5,0);
				} else{
					this.cambiarPosicionTotal();
					this.changingsweet = false;
				}
				break;
			case "izquierda":
				if (claseDulce1.transform.position.x>this.col2){
					claseDulce1.transform.Translate(-Time.deltaTime*5,0,0);
					claseDulce2.transform.Translate(Time.deltaTime*5,0,0);
				} else{
					this.cambiarPosicionTotal();
					this.changingsweet = false;
				}
				break;
			case "derecha":
				if (claseDulce1.transform.position.x<this.col2){
					claseDulce1.transform.Translate(Time.deltaTime*5,0,0);
					claseDulce2.transform.Translate(-Time.deltaTime*5,0,0);
				} else{
					this.cambiarPosicionTotal();
					this.changingsweet = false;
				}
				break;
			}
		}


	}
		
	// Function if there are plays available

	void checkIfPlayerWasteTime(){

		int i;
		int j;
		int indexCandy;
		bool foundOne = false;

		for (i=0;i<numfiles;i++){
			for (j=0;j<numCol;j++){

				if (i-1>=0) {
					indexCandy = sweetMatrix[i,j];
					sweetMatrix[i,j] = sweetMatrix[i-1,j];
					sweetMatrix[i-1,j] = indexCandy;
					if (ifPlayerNotPlayed(i-1,j,indexCandy)){
						sweetMatrixGO[i-1,j].GetComponent<simpleCandy>().incrementarScale();
						sweetMatrixGO[i,j].GetComponent<simpleCandy>().incrementarScale();
						destroySweetMoves1 = sweetMatrixGO[i-1,j];
						destroySweetMoves2 = sweetMatrixGO[i,j];
						foundOne = true;
					}
					indexCandy = sweetMatrix[i,j];
					sweetMatrix[i,j] = sweetMatrix[i-1,j];
					sweetMatrix[i-1,j] = indexCandy;
					if (foundOne) break;
				}
				if (i+1<numfiles) {
					indexCandy = sweetMatrix[i,j];
					sweetMatrix[i,j] = sweetMatrix[i+1,j];
					sweetMatrix[i+1,j] = indexCandy;
					if (ifPlayerNotPlayed(i+1,j,indexCandy)){
						sweetMatrixGO[i+1,j].GetComponent<simpleCandy>().incrementarScale();
						sweetMatrixGO[i,j].GetComponent<simpleCandy>().incrementarScale();
						destroySweetMoves1 = sweetMatrixGO[i+1,j];
						destroySweetMoves2 = sweetMatrixGO[i,j];
						foundOne = true;
					}
					indexCandy = sweetMatrix[i,j];
					sweetMatrix[i,j] = sweetMatrix[i+1,j];
					sweetMatrix[i+1,j] = indexCandy;
					if (foundOne) break;
				}
				if (j-1>=0) {
					indexCandy = sweetMatrix[i,j];
					sweetMatrix[i,j] = sweetMatrix[i,j-1];
					sweetMatrix[i,j-1] = indexCandy;
					if (ifPlayerNotPlayed(i,j-1,indexCandy)){
						sweetMatrixGO[i,j-1].GetComponent<simpleCandy>().incrementarScale();
						sweetMatrixGO[i,j].GetComponent<simpleCandy>().incrementarScale();
						destroySweetMoves1 = sweetMatrixGO[i,j-1];
						destroySweetMoves2 = sweetMatrixGO[i,j];
						foundOne = true;
					}
					indexCandy = sweetMatrix[i,j];
					sweetMatrix[i,j] = sweetMatrix[i,j-1];
					sweetMatrix[i,j-1] = indexCandy;
					if (foundOne) break;
				}
				if (j+1<numCol) {
					indexCandy = sweetMatrix[i,j];
					sweetMatrix[i,j] = sweetMatrix[i,j+1];
					sweetMatrix[i,j+1] = indexCandy;
					if (ifPlayerNotPlayed(i,j+1,indexCandy)){
						sweetMatrixGO[i,j+1].GetComponent<simpleCandy>().incrementarScale();
						sweetMatrixGO[i,j].GetComponent<simpleCandy>().incrementarScale();
						destroySweetMoves1 = sweetMatrixGO[i,j+1];
						destroySweetMoves2 = sweetMatrixGO[i,j];
						foundOne = true;
					}
					indexCandy = sweetMatrix[i,j];
					sweetMatrix[i,j] = sweetMatrix[i,j+1];
					sweetMatrix[i,j+1] = indexCandy;
					if (foundOne) break;
				}
			
			}
			if (foundOne) break;
		}
		if (!foundOne){
			restartGame();
		}
	}

	// restart the game

	void restartGame(){

		destroyAllSweets();
		createGame();
		checkIfPlayerWasteTime();
		//turnOffPossiblePlay();
	}

	public void cambiarPosicionTotal(){

		// this function is called when the candy swap is complete
		GameObject dulceRespaldo;
		sweetMatrixGO[this.file1,this.col1].GetComponent<simpleCandy>().nuevaPocicion(this.file2,this.col2);
		sweetMatrixGO[this.file2,this.col2].GetComponent<simpleCandy>().nuevaPocicion(this.file1,this.col1);
		sweetMatrix[this.file1,this.col1] = sweetMatrixGO[this.file2,this.col2].GetComponent<simpleCandy>().tip;
		sweetMatrix[this.file2,this.col2] = sweetMatrixGO[this.file1,this.col1].GetComponent<simpleCandy>().tip;
		dulceRespaldo = sweetMatrixGO[this.file2,this.col2];
		sweetMatrixGO[this.file2,this.col2] = sweetMatrixGO[this.file1,this.col1];
		sweetMatrixGO[this.file1,this.col1] = dulceRespaldo;
		//this.seeYou(this.file2,this.col2);

		if (this.performPlay){

			if (sweetMatrixGO[this.file1,this.col1].GetComponent<simpleCandy>().tip == 100)
				sweetMatrixGO[this.file1,this.col1].GetComponent<simpleCandy>().idDestroySweet = sweetMatrixGO[this.file2,this.col2].GetComponent<simpleCandy>().tip;
			if (sweetMatrixGO[this.file2,this.col2].GetComponent<simpleCandy>().tip == 100)
				sweetMatrixGO[this.file2,this.col2].GetComponent<simpleCandy>().idDestroySweet = sweetMatrixGO[this.file1,this.col1].GetComponent<simpleCandy>().tip;

			this.returning = false;

		}

		if (sweetMatrixGO[this.file1,this.col1].GetComponent<simpleCandy>().tipSweetDestroy !="" &&sweetMatrixGO[this.file2,this.col2].GetComponent<simpleCandy>().tipSweetDestroy !=""){
			EliminateCandy = new List<GameObject>();
			EliminateCandy.Add(sweetMatrixGO[this.file1,this.col1]);
			EliminateCandy.Add(sweetMatrixGO[this.file2,this.col2]);
			eliminarDulces();

			if (this.performPlay){
				this.continousplay++;
			}

			repoblarMatriz();
			
			if (this.continousplay==0 && this.performPlay){
				this.returning = true;
				swapDulces(this.file1,this.col1,this.file2,this.col2,this.direction);
				this.performPlay = false;
			}

		} else {

			ExplodeCandy.Add(sweetMatrixGO[this.file2,this.col2]);
			ExplodeCandy.Add(sweetMatrixGO[this.file1,this.col1]);
		}
	}


	// This function is responsible for verifying if the random candy marks a play
	// returns true or false depending on the verification
	public bool ifPlayerNotPlayed(int i,int j,int indice){

		//int contH = 0;
		//int contV = 0;
		int contH1 = 0;
		int contV1 = 0;
		int contH2 = 0;
		int contV2 = 0;
		bool seLograJugada  = false;



		if (i-1>=0) {
			if (this.sweetMatrix[i-1,j] == indice) contH1++;
		}
		if (i-2>=0 && contH1>0) {
			if (this.sweetMatrix[i-2,j] == indice) contH1++;
		}
		if (i+1<numfiles) {
			if (this.sweetMatrix[i+1,j] == indice) contH2++;
		}
		if (i+2<numfiles && contH2>0) {
			if (this.sweetMatrix[i+2,j] == indice) contH2++;
		}
		if (j-1>=0) {
			if (this.sweetMatrix[i,j-1] == indice) contV1++;
		}
		if (j-2>=0 && contV1>0 ) {
			if (this.sweetMatrix[i,j-2] == indice) contV1++;
		}
		if (j+1<numCol) {
			if (this.sweetMatrix[i,j+1] == indice) contV2++;
		}
		if (j+2<numCol && contV2>0) {
			if (this.sweetMatrix[i,j+2] == indice) contV2++;
		}
		
		if ((contH1+contH2) >= 2){
			seLograJugada = true;
		} 
		
		if ((contV1 + contV2) >= 2){
			seLograJugada = true;
		}

		return seLograJugada;

	}

	// Return the indicated scale to the candy that were considered possible moves
	public void turnOffPossiblePlay(){
		ifPlayerPlayed = 0;
		showPossiblePlay = false;
		if (destroySweetMoves1) destroySweetMoves1.GetComponent<simpleCandy>().decrementarScale();
		if (destroySweetMoves2) destroySweetMoves2.GetComponent<simpleCandy>().decrementarScale();
	}


	// function that changes the position of one candy for another

	public void swapDulces(int file1,int col1, int file2, int col2, string direction){

		this.file1 = file1;
		this.file2 = file2;
		this.col1 = col1;
		this.col2 = col2;
		this.direction = direction;

		turnOffPossiblePlay();

		switch (direction)
		{
		case "abajo":
			if (file2<numfiles) {
				this.changingsweet = true;
			}
			break;
		case "arriba":
			if (file2>=0) {
				this.changingsweet = true;
			}
			break;
		case "izquierda":
			if (col2>=0) {
				this.changingsweet = true;
			}
			break;
		case "derecha":
			if (col2<numCol) {
				this.changingsweet = true;
			}
			break;
		}


		
	}

	// function that sees if I can make a prize play.
	public bool seeYou(int file,int cols){
		bool foundAward = false;

		if (!foundAward) foundAward = searchPrimePrize(file,cols);
		if (!foundAward) foundAward = findSecondPrize(file,cols);
		if (!foundAward) foundAward = buscaTercerPremio(file,cols);
		if (!foundAward) foundAward = buscarCuartoPremio(file,cols);

		if (sweetMatrix[file,cols] == 100){
			if (sweetMatrixGO[file,cols].GetComponent<simpleCandy>().idDestroySweet != 0){
				if (!foundAward) foundAward = borrarDulcesPorPoder(file,cols);
			}
		}

		if (foundAward && this.performPlay){
			this.continousplay++;
		}
		return foundAward;

	}
	
	// Function looking for a 5 candy combo
    public bool searchPrimePrize(int file,int cols){

		int contfile1 = 0;
		int contfile2 = 0;
		int contcol1 = 0;
		int contcol2 = 0;
		int contTotal = 0;
		int i,j;
		bool ifCoincedience = true;
		GameObject myTransformation;

		if (sweetMatrix[file,cols]!=0){

		EliminateCandy = new List<GameObject>();
		ifCoincedience = true;
		contTotal = 0;
		i = file-1;
		while (i>=0 && ifCoincedience){
			if (sweetMatrix[file,cols] == sweetMatrix[i,cols]){
				contfile1++;
				EliminateCandy.Add(sweetMatrixGO[i,cols]);
			}else {
				ifCoincedience = false;
			}
			i--;
		}

		ifCoincedience = true;
		i = file+1;
			while (i<numfiles && ifCoincedience && (contfile1+contfile2)<4){
			if (sweetMatrix[file,cols] == sweetMatrix[i,cols]){
				contfile2++;
				EliminateCandy.Add(sweetMatrixGO[i,cols]);
			}else {
				ifCoincedience = false;
			}
			i++;
		}

		contTotal = contfile1 + contfile2;  
		
		if (contTotal >= 4){
			sweetMatrixGO[file,cols].GetComponent<simpleCandy>().destruirCDulce();
			myTransformation = Instantiate(dulce100, new Vector3(cols, file*-1, 0),transform.rotation) as GameObject;
			myTransformation.GetComponent<simpleCandy>().constructor(100,file,cols);
			sweetMatrixGO[file,cols] = myTransformation;
			sweetMatrix[file,cols] = 100;
			eliminarDulces();
			Score+=40;
			return true;
		}

            //now we look in the columns !

		EliminateCandy = new List<GameObject>();
		ifCoincedience = true;
		contTotal = 0;
		j = cols-1;
		while (j>=0 && ifCoincedience){
			if (sweetMatrix[file,cols] == sweetMatrix[file,j]){
				contcol1++;
				EliminateCandy.Add(sweetMatrixGO[file,j]);
			}else {
				ifCoincedience = false;
			}
			j--;
		}
		
		ifCoincedience = true;
		j = cols+1;
			while (j<numCol && ifCoincedience && (contcol1+contcol2)<4){
			if (sweetMatrix[file,cols] == sweetMatrix[file,j]){
				contcol2++;
				EliminateCandy.Add(sweetMatrixGO[file,j]);
			}else {
				ifCoincedience = false;
			}
			j++;
		}
		
		contTotal = contcol1 + contcol2;  
		if (contTotal >= 4){
			sweetMatrixGO[file,cols].GetComponent<simpleCandy>().destruirCDulce();
			myTransformation = Instantiate(dulce100, new Vector3(cols, file*-1, 0),transform.rotation) as GameObject;
			myTransformation.GetComponent<simpleCandy>().constructor(100,file,cols);
			sweetMatrixGO[file,cols] = myTransformation;
			sweetMatrix[file,cols] = 100;
			eliminarDulces();
			Score+=40;
			return true;
		}
		}
		return false;
	}

	// Function looking for a candy combo type "L" or "T"  
	public bool findSecondPrize(int file,int cols){
		int contfile1 = 0;
		int contfile2 = 0;
		int contcol1 = 0;
		int contcol2 = 0;
		int contTotal = 0;
		int i,j,w;
		int contRowAward = 0;
		List<GameObject> candyListCheckBomb = new List<GameObject>();
		List<GameObject> candyListEliminateCandy = new List<GameObject>();

		bool ifCoincedience = true;
		if (sweetMatrix[file,cols]!=0){
			EliminateCandy = new List<GameObject>();
			ifCoincedience = true;
			contTotal = 0;
			i = file-1;
			while (i>=0 && ifCoincedience){
				if (sweetMatrix[file,cols] == sweetMatrix[i,cols]){
					contfile1++;
					EliminateCandy.Add(sweetMatrixGO[i,cols]);
					candyListCheckBomb.Add(sweetMatrixGO[i,cols]);
				}else {
					ifCoincedience = false;
				}
				i--;
			}
			
			ifCoincedience = true;
			i = file+1;
			while (i<numfiles && ifCoincedience && (contfile1+contfile2)<2){
				if (sweetMatrix[file,cols] == sweetMatrix[i,cols]){
					contfile2++;
					EliminateCandy.Add(sweetMatrixGO[i,cols]);
					candyListCheckBomb.Add(sweetMatrixGO[i,cols]);
				}else {
					ifCoincedience = false;
				}
				i++;
			}
			
			contTotal = contfile1 + contfile2;  
		
			if (contTotal >= 2){

				candyListCheckBomb.Add(sweetMatrixGO[file,cols]);
				contRowAward++;
			}
			
			//Now we look in the columns
			int rowCheck;
			int colCheck;
		
			if (contRowAward>0) {
				for (w=0;w<candyListCheckBomb.Count;w++){
					candyListEliminateCandy = new List<GameObject>();
					contcol1 = 0;
					contcol2 = 0;
					ifCoincedience = true;
					contTotal = 0;
					rowCheck = candyListCheckBomb[w].GetComponent<simpleCandy>().file;
					colCheck = candyListCheckBomb[w].GetComponent<simpleCandy>().cols;
					j =colCheck -1;
					while (j>=0 && ifCoincedience){
						if (sweetMatrix[file,cols] == sweetMatrix[rowCheck,j]){
							contcol1++;
							candyListEliminateCandy.Add(sweetMatrixGO[rowCheck,j]);
						}else {
							ifCoincedience = false;
						}
						j--;
					}
					
					ifCoincedience = true;
					j = candyListCheckBomb[w].GetComponent<simpleCandy>().cols+1;
					while (j<numCol && ifCoincedience && (contcol1+contcol2)<2){
						if (sweetMatrix[file,cols] == sweetMatrix[rowCheck,j]){
							contcol2++;
							candyListEliminateCandy.Add(sweetMatrixGO[rowCheck,j]);
						}else {
							ifCoincedience = false;
						}
						j++;
					}
					
					contTotal = contcol1 + contcol2;  
					if (contTotal >= 2){
					
						contRowAward++;
						break;
					}

				}
			}
			if (contRowAward>=2){

				for (w=0;w<candyListEliminateCandy.Count;w++){
					EliminateCandy.Add(candyListEliminateCandy[w]);
				}


				typePlay = "bomba";
				if (sweetMatrixGO[file,cols].GetComponent<simpleCandy>().tipSweetDestroy!=""){
					sweetMatrixGO[file,cols].GetComponent<simpleCandy>().efectoSuperDulce();
				}else {
					sweetMatrixGO[file,cols].GetComponent<simpleCandy>().mostrarBombaDulce();
				}

				eliminarDulces();
				Score+=30;
				return true;
			}
		}
		return false;
	}

	// Function looking for a candy combo of 4   

	public bool buscaTercerPremio(int file,int cols){
		int contfile1 = 0;
		int contfile2 = 0;
		int contcol1 = 0;
		int contcol2 = 0;
		int contTotal = 0;
		int i,j;
		bool ifCoincedience = true;
		if (sweetMatrix[file,cols]!=0){
		EliminateCandy = new List<GameObject>();
		ifCoincedience = true;
		contTotal = 0;
		i = file-1;
		while (i>=0 && ifCoincedience){
			if (sweetMatrix[file,cols] == sweetMatrix[i,cols]){
				contfile1++;
				EliminateCandy.Add(sweetMatrixGO[i,cols]);
			}else {
				ifCoincedience = false;
			}
			i--;
		}
		
		ifCoincedience = true;
		i = file+1;
			while (i<numfiles && ifCoincedience && (contfile1+contfile2)<3){
			if (sweetMatrix[file,cols] == sweetMatrix[i,cols]){
				contfile2++;
				EliminateCandy.Add(sweetMatrixGO[i,cols]);
			}else {
				ifCoincedience = false;
			}
			i++;
		}
		
		contTotal = contfile1 + contfile2;  
		//Debug.Log(EliminateCandy.Count);
		if (contTotal >= 3){
			typePlay = "horizontal";

				if (sweetMatrixGO[file,cols].GetComponent<simpleCandy>().tipSweetDestroy!=""){
					sweetMatrixGO[file,cols].GetComponent<simpleCandy>().efectoSuperDulce();
				}else {
					sweetMatrixGO[file,cols].GetComponent<simpleCandy>().mostrarSuperDulce();
				}
			eliminarDulces();
			Score+=20;
			return true;
		}
		
		//time we look in the columns
		
		EliminateCandy = new List<GameObject>();
		ifCoincedience = true;
		contTotal = 0;
		j = cols-1;
		while (j>=0 && ifCoincedience){
			if (sweetMatrix[file,cols] == sweetMatrix[file,j]){
				contcol1++;
				EliminateCandy.Add(sweetMatrixGO[file,j]);
			}else {
				ifCoincedience = false;
			}
			j--;
		}
		
		ifCoincedience = true;
		j = cols+1;
			while (j<numCol && ifCoincedience && (contcol1+contcol2)<3){
			if (sweetMatrix[file,cols] == sweetMatrix[file,j]){
				contcol2++;
				EliminateCandy.Add(sweetMatrixGO[file,j]);
			}else {
				ifCoincedience = false;
			}
			j++;
		}
		
		contTotal = contcol1 + contcol2;  
		if (contTotal >= 3){
			typePlay = "vertical";	
				if (sweetMatrixGO[file,cols].GetComponent<simpleCandy>().tipSweetDestroy!=""){
					sweetMatrixGO[file,cols].GetComponent<simpleCandy>().efectoSuperDulce();
				}else {
					sweetMatrixGO[file,cols].GetComponent<simpleCandy>().mostrarSuperDulce();
				}
					eliminarDulces();
				Score+=20;
			return true;
		}
		}
		return false;
	}


	// Function looking for a candy combo of 3 

	public bool buscarCuartoPremio(int file,int cols){
		int contfile1 = 0;
		int contfile2 = 0;
		int contcol1 = 0;
		int contcol2 = 0;
		int contTotal = 0;
		int i,j;
		bool ifCoincedience = true;
		if (sweetMatrix[file,cols]!=0){
		EliminateCandy = new List<GameObject>();
		ifCoincedience = true;
		contTotal = 0;
		i = file-1;
		while (i>=0 && ifCoincedience){
			if (sweetMatrix[file,cols] == sweetMatrix[i,cols]){
				contfile1++;
				EliminateCandy.Add(sweetMatrixGO[i,cols]);
			}else {
				ifCoincedience = false;
			}
			i--;
		}
		
		ifCoincedience = true;
		i = file+1;
			while (i<numfiles && ifCoincedience && (contfile1+contfile2)<2){
			if (sweetMatrix[file,cols] == sweetMatrix[i,cols]){
				contfile2++;
				EliminateCandy.Add(sweetMatrixGO[i,cols]);
			}else {
				ifCoincedience = false;
			}
			i++;
		}
		
		contTotal = contfile1 + contfile2;  
		
		if (contTotal >= 2){
			EliminateCandy.Add(sweetMatrixGO[file,cols]);
			eliminarDulces();
			Score+=10;
			return true;
		}
		
		//times we look in the column
		
		EliminateCandy = new List<GameObject>();
		ifCoincedience = true;
		contTotal = 0;
		j = cols-1;
		while (j>=0 && ifCoincedience){
			if (sweetMatrix[file,cols] == sweetMatrix[file,j]){
				contcol1++;
				EliminateCandy.Add(sweetMatrixGO[file,j]);
			}else {
				ifCoincedience = false;
			}
			j--;
		}
		
		ifCoincedience = true;
		j = cols+1;
			while (j<numCol && ifCoincedience && (contcol1+contcol2)<2){
			if (sweetMatrix[file,cols] == sweetMatrix[file,j]){
				contcol2++;
				EliminateCandy.Add(sweetMatrixGO[file,j]);
			}else {
				ifCoincedience = false;
			}
			j++;
		}
		
		contTotal = contcol1 + contcol2;  
		if (contTotal >= 2){
			EliminateCandy.Add(sweetMatrixGO[file,cols]);
			eliminarDulces();
			Score+=10;
			return true;
		}
		}
		return false;
	}

	// function that erases sweets with the most powerful SUPER candy

	public bool borrarDulcesPorPoder(int file,int cols){

		int i,j;
		if (sweetMatrix[file,cols]!=0){
			
			EliminateCandy = new List<GameObject>();
			for (i=0;i<numfiles;i++){
				for (j=0;j<numCol;j++){
					if (sweetMatrix[i,j]==sweetMatrixGO[file,cols].GetComponent<simpleCandy>().idDestroySweet){
						EliminateCandy.Add(sweetMatrixGO[i,j]);
					}
				}
			}
			EliminateCandy.Add(sweetMatrixGO[file,cols]);
			eliminarDulces();
			Score+=100;
			return true;
	
		}
		return false;
	}

	// Eliminate all the sweet products of certain moves

	public void eliminarDulces(){
		int x;
		for (x=0;x<EliminateCandy.Count;x++){
			EliminateCandy[x].GetComponent<simpleCandy>().destruirCDulce();
		}
		EliminateCandy = new List<GameObject>();

	}

	// function that fills the blank space product of a play

	public void repoblarMatriz(){
		int i;
		int j;
		int w;
		int indexCandy;
		GameObject newSweet = null;
		int contVacio = 0;
		for (j=0;j<numCol;j++){
			contVacio = 0;
			for (i=numfiles-1;i>=0;i--){
				if (sweetMatrix[i,j]==0){
					contVacio++;

				} else {
					if (contVacio>0){
						sweetMatrixGO[i+contVacio,j] = sweetMatrixGO[i,j];
						sweetMatrixGO[i,j] = null;
						sweetMatrix[i+contVacio,j] = sweetMatrix[i,j];
						sweetMatrix[i,j] = 0;
					
						sweetMatrixGO[i+contVacio,j].GetComponent<simpleCandy>().dejarCaerDulce(i+contVacio,j);
					}
				}

			}
			for (w=0;w<contVacio;w++){

				indexCandy = Random.Range(1,7);
				switch (indexCandy)
				{
				case 1:
					newSweet = Instantiate(dulce01, new Vector3(j, (w+1), 0),transform.rotation) as GameObject;
					break;
				case 2:
					newSweet = Instantiate(dulce02, new Vector3(j, (w+1), 0),transform.rotation) as GameObject;
					break;
				case 3:
					newSweet = Instantiate(dulce03, new Vector3(j, (w+1), 0),transform.rotation) as GameObject;
					break;
				case 4:
					newSweet = Instantiate(dulce04, new Vector3(j, (w+1), 0),transform.rotation) as GameObject;
					break;
				case 5:
					newSweet = Instantiate(dulce05, new Vector3(j, (w+1), 0),transform.rotation) as GameObject;
					break;
				case 6:
					newSweet = Instantiate(dulce06, new Vector3(j, (w+1), 0),transform.rotation) as GameObject;
					break;
				}
				if (newSweet!=null){
					newSweet.GetComponent<simpleCandy>().constructor(indexCandy,1,1);
					newSweet.GetComponent<simpleCandy>().dejarCaerDulce(contVacio-w-1,j);
					sweetMatrixGO[contVacio-w-1,j] = newSweet;
					sweetMatrix[contVacio-w-1,j] = indexCandy;
				}

			
			}
		}
	}

	// function you are looking for if a play is valid

	public void candyExplodeVertical(){
		int x;
		for (x=0;x<ExplodeCandy.Count;x++){
			if (ExplodeCandy[x]!=null){
				seeYou(ExplodeCandy[x].GetComponent<simpleCandy>().file,ExplodeCandy[x].GetComponent<simpleCandy>().cols);
			}
		}
		ExplodeCandy = new List<GameObject>();
		repoblarMatriz();

		if (this.continousplay==0 && this.performPlay){
			this.returning = true;
			swapDulces(this.file1,this.col1,this.file2,this.col2,this.direction);
			this.performPlay = false;
		}


	}

	// combo Horizontal

	public void candyExplodeHorizontal(int file,int cols){
		int j;
		for (j=0;j<numCol;j++){
			if (sweetMatrix[file,j]!=0 && (j!=cols)){
				if (!EliminateCandy.Contains(sweetMatrixGO[file,j])){
					EliminateCandy.Add(sweetMatrixGO[file,j]);
				}
			}
		}
		Score+=40;
	}


	// combo Vertical

	public void dulcesExploVertical(int file,int cols){
		int i;
		for (i=0;i<numfiles;i++){
			if (sweetMatrix[i,cols]!=0 && (i!=file)){
				if (!EliminateCandy.Contains(sweetMatrixGO[i,cols])){
					EliminateCandy.Add(sweetMatrixGO[i,cols]);
				}
			}
		}
		Score+=40;
	}

	// combo Bomb

	public void candyExplodeBomb(int file,int cols){

		int i;
		int j;
		int iInicial = file-1;
		int jInicial = cols-1;
		int ifinal = file+1;
		int jfinal = cols+1;

		if (iInicial<0) iInicial = 0; 
		if (jInicial<0) jInicial = 0;
		if (ifinal>numfiles-1) ifinal = numfiles-1; 
		if (jfinal>numCol-1) jfinal = numCol-1; 

		for (i=iInicial;i<=ifinal;i++){
			for (j=jInicial;j<=jfinal;j++){
				if (sweetMatrix[i,j]!=0){
					if (!EliminateCandy.Contains(sweetMatrixGO[i,j])){
						EliminateCandy.Add(sweetMatrixGO[i,j]);
					}
				}
			}
		}
		Score+=50;
	}

	void OnGUI() {
		GUI.skin = _theSkin;
		GUI.Label( new Rect (10,10, 300, 100), "Score = " + Score);
	}
	
}
