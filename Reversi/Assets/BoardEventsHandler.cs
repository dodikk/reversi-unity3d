using UnityEngine;
using System.Collections;

using Unity.Linq;
using System.Linq;

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

		populateCellsList ();
		populateCellsMatrix ();
		setSampleTurnText ();
	}
	
	// Update is called once per frame
	void Update () 
	{
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
	#endregion

	public TextMesh _turnLabel; 

	private GameObject _root;
	private GameObject[] _cellsList;
	private GameObject[,] _cellsMatrix;

	private static int BOARD_SIZE = 8;
}
