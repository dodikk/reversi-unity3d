using UnityEngine;
using UnityEngine.Rendering;
using System.Collections;

using Unity.Linq;
using System.Linq;
using ReversiKit;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class BoardEventsHandler : MonoBehaviour 
{
	#region MonoBehaviour override
	// Use this for initialization
	void Start() 
	{
        this._isGameOver = false;

		this._mutableBoardModel = new MatrixBoard();
		this._turnCalculator    = new TurnCalculator();
		this._boardModel        = this._mutableBoardModel;
        this._turnSelector      = new GreedyTurnSelector();

		this._root = GameObject.Find("root"); 



		// Using lowercase methods in this class 
		// to distinguish own methods from built-in unity methods
		
        populateLabels();
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
        this.updateScoreLabels();
        this.updateBallColours();
		this.highlightAvailableTurns();

		bool isMouseUpEvent = Input.GetMouseButtonUp(0);
		if (isMouseUpEvent)
		{
			this.handleMouseUpEvent();
		}
	}
	#endregion

    private void getAvailableTurns()
    {
        var turns = this._turnCalculator.GetValidTurnsForBoard(this._boardModel);
        this._validTurns = turns;
    }


    #region Human Player Turn
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
        if (this.IsTurnOfAI)
        {
            return;
        }


		// TODO : maybe compute matrix index by reference
		string cellName = cellCube.name;

        if (null == this._validTurns || 0 == this._validTurns.Count())
        {
            // Passing the turn if current user can't make it.
            this._boardModel.PassTurn();


            // Game over ???
            this.getAvailableTurns();
            if (null == this._validTurns || 0 == this._validTurns.Count())
            {
                // Yes. Game over.

                this._turnLabel.text = "Game Over";
                this._isGameOver = true;
            }

            return;
        }


        var turnsSetToMatchInput = this._validTurns.Where(t =>
        {
            string turnPositionName = BoardCoordinatesConverter.CoordinatesToCellName(t.Position);
            return cellName.Equals(turnPositionName);
        });

        if (null == turnsSetToMatchInput || 0 == turnsSetToMatchInput.Count())
        {
            // TODO : maybe show alert
            return;
        }
        IReversiTurn turn = turnsSetToMatchInput.First();
		this.makeTurn(turn);


        if (IS_OPPONENT_PLAYER_AI)
        {
            while (this.IsTurnOfAI)
            {
                this.makeTurnByAI();
            }
        }
	}

	private void makeTurn(IReversiTurn turn)
    {
		this.unhighlightAvailableTurns();
        this.drawChangesForTurn(turn);
        this._boardModel.ApplyTurn(turn);
        this.getAvailableTurns();
        this.highlightAvailableTurns();

        if (0 == this._boardModel.NumberOfFreeCells)
        {
            this._turnLabel.text = "Game Over";
            this._isGameOver = true;

            return;
        }
	}
    #endregion 

    private void makeTurnByAI()
    {
        IReversiTurn selectedTurn = 
            this._turnSelector.SelectBestTurnOnBoard(
                this._validTurns, 
                this._boardModel);

        this.makeTurn(selectedTurn);
    }

    private bool IsTurnOfAI
    { 
        get
        {
            if (this._isGameOver)
            {
                return false;
            }

            if (!this.IS_OPPONENT_PLAYER_AI)
            {
                return false;
            }

            return !this._boardModel.IsTurnOfBlackPlayer;
        }
    }

        
    #region Turn Drawing
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

        this._ballsMatrix[cell.Row, cell.Column] = sphere;
    }

    private void setColourForBallAtCell(Material activePlayerColour, ICellCoordinates cell)
    {
        this._ballsMatrix[cell.Row, cell.Column].GetComponent<Renderer>().material = activePlayerColour;
    }

	private void updateTurnLabel()
	{
        if (this._isGameOver)
        {
            return;
        }

		this._turnLabel.text = 
			this._boardModel.IsTurnOfBlackPlayer ? 
			"Black Player Turn" :
			"White Player Turn" ;
	}

    private void updateBallColours()
    {
        for (int row = 0; row != BOARD_SIZE; ++row)
            for (int col = 0; col != BOARD_SIZE; ++col)
            {
                ICellCoordinates cell = new CellCoordinates(row, col);
                if (this._boardModel.IsCellTakenByBlack(cell))
                {
                    this.setColourForBallAtCell(this._blackItemMaterial, cell);
                } 
                else if (this._boardModel.IsCellTakenByWhite(cell))
                {
                    this.setColourForBallAtCell(this._whiteItemMaterial, cell);
                }
            }
    }

    private void updateScoreLabels()
    {
        int blackScore = this._boardModel.NumberOfBlackPieces;
        int whiteScore = this._boardModel.NumberOfWhitePieces;

        this._blackScoreLabel.text = "Black : " + Convert.ToString(blackScore);
        this._whiteScoreLabel.text = "White : " + Convert.ToString(whiteScore);
    }
    #endregion

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
        if (null == turns)
        {
            // win condition will be checked in user input processing code
            return;
        }

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

        if (null == this._blackItemMaterial)
        {
            this._blackItemMaterial = this._ballsMatrix[3, 3].GetComponent<Renderer>().material;
        }
        if (null == this._whiteItemMaterial)
        {
            this._whiteItemMaterial = this._ballsMatrix[3, 4].GetComponent<Renderer>().material;
        }

        if (null == this._blackCellMaterial)
        {
            this._blackCellMaterial = this._cellsMatrix[0, 0].GetComponent<Renderer>().material;
        }

        if (null == this._whiteCellMaterial)
        {
            this._whiteCellMaterial = this._cellsMatrix[0, 1].GetComponent<Renderer>().material;
        }

//        this._blackCellMaterial = Resources.Load("chessboard_min2-czarny.mat", typeof(Material)) as Material;
//        this._whiteCellMaterial = Resources.Load("wood_ash_clear.mat", typeof(Material)) as Material;
//
//        this._blackItemMaterial = Resources.Load("chessboard_min2-bialy.mat", typeof(Material)) as Material;
//        this._whiteItemMaterial = Resources.Load("wood_mahogany.mat", typeof(Material)) as Material;
//
//        this._highlightedCellMaterial = Resources.Load("HighlightedCell.mat", typeof(Material)) as Material;
    }

    private void populateLabels()
    {
        var labels = FindObjectsOfType<Text>();


        this._turnLabel       = labels.Where(l => "TurnLabel"  == l.name).First();
        this._blackScoreLabel = labels.Where(l => "BlackScore" == l.name).First();
        this._whiteScoreLabel = labels.Where(l => "WhiteScore" == l.name).First();
    }
    #endregion

	#region GUI elements
	public Text _turnLabel; 
    public Text _blackScoreLabel;
    public Text _whiteScoreLabel;

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
    private ITurnSelector   _turnSelector     ;

	#endregion

    private bool IS_OPPONENT_PLAYER_AI = false;

	private const int    BOARD_SIZE = 			8;
	private const string CELL_TAG   = "FieldCell";
	private const string BALL_TAG   = 	   "Ball";

	private IEnumerable<IReversiTurn> _validTurns	;
    bool _isGameOver;
}
