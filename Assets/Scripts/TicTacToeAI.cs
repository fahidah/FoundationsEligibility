using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public enum TicTacToeState{none, cross, circle}

[System.Serializable]
public class WinnerEvent : UnityEvent<int>
{
}

public class TicTacToeAI : MonoBehaviour
{

	int _aiLevel;

	int count = 0;
	int zeroCounter = 0;

	int pickedCell;

	public GameObject playerTurnNotice;
	public GameObject aiTurnNotice;

	
	TicTacToeState[,] boardState;
    List<(TicTacToeState, TicTacToeState)> availableBoardCells;

	[SerializeField]
	private bool _isPlayerTurn;

	[SerializeField]
	private int _gridSize = 3;
	
	[SerializeField]
	private TicTacToeState playerState = TicTacToeState.cross;
	TicTacToeState aiState = TicTacToeState.circle;

	[SerializeField]
	private GameObject _xPrefab;

	[SerializeField]
	private GameObject _oPrefab;

	public UnityEvent onGameStarted;

	//Call This event with the player number to denote the winner
	public WinnerEvent onPlayerWin;

	ClickTrigger[,] _triggers;
	
	private void Awake()
	{
		if(onPlayerWin == null){
			onPlayerWin = new WinnerEvent();
		}
	}

	public void StartAI(int AILevel){
		_aiLevel = AILevel;
		playerTurnNotice.SetActive(true);
		StartGame();
	}

	public void RegisterTransform(int myCoordX, int myCoordY, ClickTrigger clickTrigger)
	{
		_triggers[myCoordX, myCoordY] = clickTrigger;
	}

	private void StartGame()
	{
		_triggers = new ClickTrigger[3,3];
		boardState = new TicTacToeState[_gridSize, _gridSize];

		//best to use list for dynamic size,,,,still figuring it out..memory leak might occur cos of this
		//availableBoardCells = new TicTacToeState[_gridSize, _gridSize];


		_isPlayerTurn = true;
		onGameStarted.Invoke();
	}

	public void PlayerSelects(int coordX, int coordY){

		SetVisual(coordX, coordY, playerState);
	}

	public void AiSelects(int coordX, int coordY){

		SetVisual(coordX, coordY, aiState);
	}

	private void SetVisual(int coordX, int coordY, TicTacToeState targetState)
	{
		Instantiate(
			targetState == TicTacToeState.circle ? _oPrefab : _xPrefab,
			_triggers[coordX, coordY].transform.position,
			Quaternion.identity
		);
	}

	public void SwitchTurns(int coordX, int coordY)
    {
        if (_isPlayerTurn)
        {

			aiTurnNotice.SetActive(true);
			playerTurnNotice.SetActive(false);

			PlayerSelects(coordX, coordY);
			_isPlayerTurn = false;

			UpdateBoardStateCross(coordX, coordY);

		}
        else
        {
			aiTurnNotice.SetActive(false);
			playerTurnNotice.SetActive(true);
			
			AiSelects(coordX, coordY);
			_isPlayerTurn = true;

			UpdateBoardStateCircle(coordX, coordY);
		}
	}

	public void UpdateBoardStateCross(int coordX, int coordY)
	{

		boardState[coordX, coordY] = TicTacToeState.cross;

		DisplayBoardState();

	}
	public void UpdateBoardStateCircle(int coordX, int coordY)
	{

		boardState[coordX, coordY] = TicTacToeState.circle;
		DisplayBoardState();


	}

	public void DisplayBoardState()
    {
		for(int row = 0; row < _gridSize; row++)
        {
			for (int column = 0; column < _gridSize; column++)
            {
				if((boardState[row, column])== 0)
				{
					count++;
				}
			}
		}

		pickedCell = Random.Range(0, count);

		Debug.Log("Length of available cell: " + count);
		Debug.Log("Picked Cell: " + pickedCell);

		count = 0;
	}
}

