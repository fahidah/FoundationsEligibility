using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Collections;

public enum TicTacToeState{none, cross, circle}

[System.Serializable]
public class WinnerEvent : UnityEvent<int>
{
	
}

public class TicTacToeAI : MonoBehaviour
{

	int _aiLevel;

	int count = 0;

	int pickedCell;

	public GameObject playerTurnNotice;
	public GameObject aiTurnNotice;

	
	TicTacToeState[,] boardState;
    List<TicTacToeState> availableBoardCells;

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
	ClickTrigger clickTrigger;
	
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


		_isPlayerTurn = true;
		onGameStarted.Invoke();
	}

	public void PlayerSelects(int coordX, int coordY)
	{

		aiTurnNotice.SetActive(true);
		playerTurnNotice.SetActive(false);


		UpdateBoardStateCross(coordX, coordY);
		SetVisual(coordX, coordY, playerState);

		_isPlayerTurn = false;
		StartCoroutine(AIChoice());		
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


	public void UpdateBoardStateCross(int coordX, int coordY)
	{
		boardState[coordX, coordY] = TicTacToeState.cross;
	}


	public void UpdateBoardStateCircle(int coordX, int coordY)
	{
		boardState[coordX, coordY] = TicTacToeState.circle;
	}

	public IEnumerator AIChoice()
    {
		yield return new WaitForSeconds(.5f);

		for(int row = 0; row < _gridSize; row++)
        {
			for (int column = 0; column < _gridSize; column++)
            {
				if(((int)boardState[row, column])== 0)
				{ 
					count++;

					if(count == pickedCell)
                    {
						//Debug.Log("(" + row + "," + column + "): " + (int)boardState[row, column]);

                        if (!_isPlayerTurn)
                        {
							aiTurnNotice.SetActive(false);
							playerTurnNotice.SetActive(true);

							UpdateBoardStateCircle(row, column);

							AiSelects(row, column);

							_isPlayerTurn = true;

						}

					}
					if (count < 5)
					{
						WiningCondition();
					}
				}
			}
		}
		
		pickedCell = Random.Range(0, count);

		//Debug.Log("Length of available cell: " + count);
		//Debug.Log("Picked Cell: " + pickedCell);

		count = 0;
	}


	private void WiningCondition()
    {
			int s1 = (int)boardState[0,0] + (int)boardState[0, 1] + (int)boardState[0, 2];
			int s2 = (int)boardState[1,0] + (int)boardState[1,1] + (int)boardState[1, 2];
			int s3 = (int)boardState[2,0] + (int)boardState[2, 1] + (int)boardState[2, 2];
			int s4 = (int)boardState[0, 0] + (int)boardState[1,0] + (int)boardState[0, 2];
			int s5 = (int)boardState[0, 1] + (int)boardState[1, 1] + (int)boardState[2, 1];
			int s6 = (int)boardState[0, 2] + (int)boardState[1, 2] + (int)boardState[2, 2];
			int s7 = (int)boardState[0, 0] + (int)boardState[1, 1] + (int)boardState[2, 2];
			int s8 = (int)boardState[0, 2] + (int)boardState[1, 1] + (int)boardState[0, 2];

			var solutions = new int[] { s1, s2, s3, s4, s5, s6, s7, s8 };

			for(int i = 0; i<solutions.Length; i++)
			{
				if(solutions[i] == (int)TicTacToeState.cross)
				{
					Debug.Log("Player Won!");
				}

				else if (solutions[i] == (int)TicTacToeState.cross)
				{
				Debug.Log("AI Won!");
				}

				else
				{
					Debug.Log("Draw!");
				}
			}
	}
}

