using UnityEngine;
using UnityEngine.Rendering;
using System.Collections;

using Unity.Linq;
using System.Linq;
using ReversiKit;
using System.Collections.Generic;

public class BoardEventsHandler : MonoBehaviour 
{
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


		// Using lowercase methods in this class 
		// to distinguish own methods from built-in unity methods
		populateBallsList();
		populateCellsList ();
		populateCellsMatrix ();
        populateCellColours();
		getAvailableTurns();
	}
	
	// Update is called once per frame
	void Update() 
	{
		this.updateTurnLabel();
		this.highlightAvailableTurns();

		bool isMouseUpEvent = Input.GetMouseButtonUp(0);
		if (isMouseUpEvent)
		{
			this.handleMouseUpEvent();
		}
	}
	#endregion


	private void handleMouseUpEvent()
	{
		Vector3 mousePosition = Input.mousePosition;
		Ray ray = Camera.main.ScreenPointToRay(mousePosition);

		RaycastHit hitInfo;
		bool hit = Physics.Raycast(ray, out hitInfo);
		if (!hit)
		{
			return;
		}

		GameObject selectedCellOrBall = hitInfo.transform.gameObject;
		bool isCell = (CELL_TAG == selectedCellOrBall.tag);
		if (isCell)
		{
			this.handleTapOnCell(selectedCellOrBall);
		}
	}

	private void handleTapOnCell(GameObject cellCube)
	{
		// TODO : maybe compute matrix index by reference
		string cellName = cellCube.name;

		// TODO : extract query to ReversiKit
		IReversiTurn turn = this._validTurns.Where(t =>
		{
			string turnPositionName = BoardCoordinatesConverter.CoordinatesToCellName(t.Position);
			return cellName.Equals(turnPositionName);
		}).First();


		if (null != turn)
		{
			this.makeTurn(turn);
		}
	}

	private void makeTurn(IReversiTurn turn)
    {
		this.unhighlightAvailableTurns();
        this.drawChangesForTurn(turn);
        this._boardModel.ApplyTurn(turn);
        this.getAvailableTurns();
	}

    private void drawChangesForTurn(IReversiTurn turn)
    {
        Material activePlayerColour = 
            this._boardModel.IsTurnOfBlackPlayer ? 
            this._blackItemMaterial : 
            this._whiteItemMaterial ;


//        this.setColourForCell(activePlayerColour, turn.Position);

        this.createBallWithColourAtCell(activePlayerColour, turn.Position);
        foreach (ICellCoordinates flippedCell in turn.PositionsOfFlippedItems)
        {
            this.setColourForBallAtCell(activePlayerColour, flippedCell);
        }
    }

    private void createBallWithColourAtCell(Material activePlayerColour, ICellCoordinates cell)
    {
        var cellPosition = this._cellsMatrix[cell.Row, cell.Column].transform.position;

        var sphere = GameObject.CreatePrimitive (PrimitiveType.Sphere);
        {
            sphere.transform.position = new Vector3(cellPosition.x + 0.1f, cellPosition.y, cellPosition.z - 1);
            sphere.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
            sphere.tag = BALL_TAG;

            var renderer = sphere.GetComponent<Renderer>();
            renderer.shadowCastingMode = ShadowCastingMode.Off;
            renderer.receiveShadows = false;
            renderer.material = this._blackItemMaterial;
        }
    }

    private void setColourForBallAtCell(Material activePlayerColour, ICellCoordinates cell)
    {
        this._ballsMatrix[cell.Row, cell.Column].GetComponent<Renderer>().material = activePlayerColour;
    }

	private void updateTurnLabel()
	{
		this._turnLabel.text = 
			this._boardModel.IsTurnOfBlackPlayer ? 
			"Black Player Turn" :
			"White Player Turn" ;
	}

	private void getAvailableTurns()
	{
		var turns = this._turnCalculator.GetValidTurnsForBoard(this._boardModel);
		this._validTurns = turns;
	}

	#region Turn Highlight
	private void unhighlightAvailableTurns()
	{
		this.highlightAvailableTurns(false);
	}

	private void highlightAvailableTurns()
	{
		this.highlightAvailableTurns(true);
	}

	private void highlightAvailableTurns(bool shouldHighlight)
	{
		var turns = this._validTurns;

		foreach (IReversiTurn singleTurn in turns)
		{
			ICellCoordinates turnCell = singleTurn.Position;
			GameObject cellCube = this._cellsMatrix[turnCell.Row, turnCell.Column];

			Material cellColour = null;

			if (shouldHighlight)
			{
				cellColour = this._highlightedCellMaterial;
			} 
			else
			{
				cellColour = 
					turnCell.IsBlack 		? 
					this._blackCellMaterial : 
					this._whiteCellMaterial ;
				
			}
			cellCube.GetComponent<Renderer>().material = cellColour;
		}
	}
	#endregion	

	#region Prototyping
	private void doSampleTurn()
	{
		ICellCoordinates newItemPosition = BoardCoordinatesConverter.CellNameToCoordinates("C5");
		//UnityEngine.Debug.LogError("C5 coordinates : " + newItemPosition.Row.ToString() + ", " + newItemPosition.Column.ToString());

        this.createBallWithColourAtCell(this._blackItemMaterial, newItemPosition);



		// change material
		ICellCoordinates d5Position = BoardCoordinatesConverter.CellNameToCoordinates("D5");
		//		GameObject cellD5 = this._cellsMatrix[d5Position.Row, d5Position.Column];
		GameObject ballD5 = this._ballsMatrix[d5Position.Row, d5Position.Column];
		{
			var d5Renderer = ballD5.GetComponent<Renderer>();
			d5Renderer.material = this._blackItemMaterial;
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

	#endregion

	#region Cells Initialization
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
	
    private void populateCellColours()
    {
        // Hot fix : the material references from the editor turn out to be "null"

        this._blackCellMaterial = this._cellsMatrix[0, 0].GetComponent<Renderer>().material;
        this._whiteCellMaterial = this._cellsMatrix[1, 0].GetComponent<Renderer>().material;
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

	private const int    BOARD_SIZE = 			8;
	private const string CELL_TAG   = "FieldCell";
	private const string BALL_TAG   = 	   "Ball";

	private IEnumerable<IReversiTurn> _validTurns	;
}
