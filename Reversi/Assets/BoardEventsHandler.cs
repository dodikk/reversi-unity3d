using UnityEngine;
using UnityEngine.Rendering;
using System.Collections;

using Unity.Linq;
using System.Linq;
using ReversiKit;

public class BoardEventsHandler : MonoBehaviour {


	#region MonoBehaviour override
	// Use this for initialization
	void Start () 
	{
//		UnityEngine.Debug.developerConsoleVisible = true;
//		UnityEngine.Debug.LogError("----Start-----");
//		UnityEngine.Debug.LogError ("Turn label : " + this._turnLabel.ToString());


		this._root = GameObject.Find("root"); 
//		UnityEngine.Debug.LogError ("Root : " + root.ToString ());

		populateBallsList();
		populateCellsList ();
		populateCellsMatrix ();
		setSampleTurnText ();
		doSampleTurn ();
	}
	
	// Update is called once per frame
	void Update () 
	{
	}
	#endregion


	#region Cells Initialization
	private void doSampleTurn()
	{
		ICellCoordinates newItemPosition = BoardCoordinatesConverter.CellNameToCoordinates("C5");
		//UnityEngine.Debug.LogError("C5 coordinates : " + newItemPosition.Row.ToString() + ", " + newItemPosition.Column.ToString());

		GameObject cellC5 = this._cellsMatrix [newItemPosition.Row, newItemPosition.Column];
		var cellPosition = cellC5.transform.position;

		var sphere = GameObject.CreatePrimitive (PrimitiveType.Sphere);
		{
			sphere.transform.position = new Vector3(cellPosition.x + 0.1f, cellPosition.y, cellPosition.z - 1);
			sphere.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);

			var renderer = sphere.GetComponent<Renderer>();
			renderer.shadowCastingMode = ShadowCastingMode.Off;
			renderer.receiveShadows = false;
			renderer.material = this._blackItemMaterial;
		}
		this._ballsMatrix[newItemPosition.Row, newItemPosition.Column] = sphere;

		// change material
		ICellCoordinates d5Position = BoardCoordinatesConverter.CellNameToCoordinates("D5");
//		GameObject cellD5 = this._cellsMatrix[d5Position.Row, d5Position.Column];
		GameObject ballD5 = this._ballsMatrix[d5Position.Row, d5Position.Column];
		{
			var d5Renderer = ballD5.GetComponent<Renderer>();
			d5Renderer.material = this._blackItemMaterial;
		}
	}



	private void populateCellsList()
	{
		this._cellsList = this._root.Descendants().Where(x => x.tag == "FieldCell").OrderBy(x => x.name).ToArray();
		//		UnityEngine.Debug.LogError ("Cells count : " + this._cellsList.Length.ToString ());
	}

	private void populateCellsMatrix()
	{
		this._cellsMatrix = new GameObject[BOARD_SIZE,BOARD_SIZE];

		for (int row = 0; row != BOARD_SIZE; ++row)
			for (int column = 0; column != 8; ++column) 
			{
				int flatIndex = row * BOARD_SIZE + column;
				this._cellsMatrix [row,column] = this._cellsList[flatIndex];
			}
	}

	private void setSampleTurnText()
	{
		GameObject C2CellFromMatrix = this._cellsMatrix [2, 1];
		this._turnLabel.text = C2CellFromMatrix.name;

		//		UnityEngine.Debug.LogError ("A1Cell matrix name : " + A1CellFromMatrix.name);
		//		UnityEngine.Debug.LogError ("Turn text : " + this._turnLabel.text);
	}

	private void populateBallsList()
	{
		this._ballsMatrix = new GameObject[8, 8];
		{
			this._ballsMatrix[3, 3] = this._ballD4;
			this._ballsMatrix[3, 4] = this._ballD5;

			this._ballsMatrix[4, 3] = this._ballE4;
			this._ballsMatrix[4, 4] = this._ballE5;
		}
	}
	#endregion

	public TextMesh _turnLabel; 

	private GameObject _root;
	private GameObject[] _cellsList;
	private GameObject[,] _cellsMatrix;
	private GameObject[,] _ballsMatrix;

	public Material _whiteItemMaterial;
	public Material _blackItemMaterial;

	public GameObject _ballD5;
	public GameObject _ballE5;
	public GameObject _ballD4;
	public GameObject _ballE4;

	private static int BOARD_SIZE = 8;
}
