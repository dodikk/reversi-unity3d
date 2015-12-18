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
		this._mutableBoardModel = new MatrixBoard();
		this._turnCalculator = new TurnCalculator();
		this._boardModel = this._mutableBoardModel;

//		UnityEngine.Debug.developerConsoleVisible = true;
//		UnityEngine.Debug.LogError("----Start-----");
//		UnityEngine.Debug.LogError ("Turn label : " + this._turnLabel.ToString());


		this._root = GameObject.Find("root"); 
//		UnityEngine.Debug.LogError ("Root : " + root.ToString ());

		populateBallsList();
		populateCellsList ();
		populateCellsMatrix ();
		setSampleTurnText ();
		// doSampleTurn ();

		highlightAvailableTurnsSample();
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

		GameObject cellC5 = this._cellsMatrix[newItemPosition.Row, newItemPosition.Column];
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

	private void highlightAvailableTurnsSample()
	{
		var turns = this._turnCalculator.GetValidTurnsForBoard(this._boardModel);
		foreach (IReversiTurn singleTurn in turns)
		{
			ICellCoordinates turnCell = singleTurn.Position;
			GameObject cellCube = this._cellsMatrix[turnCell.Row, turnCell.Column];
			cellCube.GetComponent<Renderer>().material = this._highlightedCellMaterial;
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
			for (int column = 0; column != BOARD_SIZE; ++column) 
			{
				int flatIndex = row * BOARD_SIZE + column;
				var cell = this._cellsList[flatIndex];
				string cellName = cell.name;

				var cellPosition = BoardCoordinatesConverter.CellNameToCoordinates(cellName);
				this._cellsMatrix [cellPosition.Row, cellPosition.Column] = cell;
			}
	}

	private void setSampleTurnText()
	{
		var cellPosition = BoardCoordinatesConverter.CellNameToCoordinates("C2");

		GameObject C2CellFromMatrix = this._cellsMatrix[cellPosition.Row, cellPosition.Column];
		this._turnLabel.text = C2CellFromMatrix.name;

		//		UnityEngine.Debug.LogError ("A1Cell matrix name : " + A1CellFromMatrix.name);
		//		UnityEngine.Debug.LogError ("Turn text : " + this._turnLabel.text);
	}

	private void populateBallsList()
	{
		this._ballsMatrix = new GameObject[8, 8];
		{
			var d4Pos = BoardCoordinatesConverter.CellNameToCoordinates("D4");
			this._ballsMatrix[d4Pos.Row, d4Pos.Column] = this._ballD4;
			this._mutableBoardModel.TryConsumeCellByBlackPlayer(d4Pos);

			var d5Pos = BoardCoordinatesConverter.CellNameToCoordinates("D5");
			this._ballsMatrix[d5Pos.Row, d5Pos.Column] = this._ballD5;
			this._mutableBoardModel.TryConsumeCellByWhitePlayer(d5Pos);

			var e4Pos = BoardCoordinatesConverter.CellNameToCoordinates("E4");
			this._ballsMatrix[e4Pos.Row, e4Pos.Column] = this._ballE4;
			this._mutableBoardModel.TryConsumeCellByWhitePlayer(e4Pos);

			var e5Pos = BoardCoordinatesConverter.CellNameToCoordinates("E5");
			this._ballsMatrix[e5Pos.Row, e5Pos.Column] = this._ballE5;
			this._mutableBoardModel.TryConsumeCellByBlackPlayer(e5Pos);

			this._mutableBoardModel.IsTurnOfBlackPlayer = true;
		}
	}
	#endregion

	#region GUI elements
	public TextMesh _turnLabel; 

	private GameObject _root;
	private GameObject[] _cellsList;
	private GameObject[,] _cellsMatrix;
	private GameObject[,] _ballsMatrix;

	public Material _whiteItemMaterial;
	public Material _blackItemMaterial;
	public Material _highlightedCellMaterial;
	public Material _blackCellMaterial;
	public Material _whiteCellMaterial;

	public GameObject _ballD5;
	public GameObject _ballE5;
	public GameObject _ballD4;
	public GameObject _ballE4;
	#endregion

	#region Model

	private IBoardState 	_boardModel	      ;
	private ITurnCalculator _turnCalculator	  ;
	private MatrixBoard 	_mutableBoardModel;

	#endregion


	private static int BOARD_SIZE = 8;
}
