using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class _GameManager : NetworkBehaviour
{
	
	
	public int NumOfResourcesToWin = 5;
	public GameObject globalText;
	
	public void ActivateGlobalText()
	{
		globalText.SetActive(true);
	}
	
//***********************************ADD AND MINUS FROZEN COUNT***************************************
	public Text FrozenText;
	
	private int frozenCount = 0;
	private int totalPlayer = 0;

	
	[Command]
	public void CmdAddFrozenCount()
	{
		frozenCount++;
		RpcChangeFrozenCount(frozenCount);
		Debug.Log("plus" + frozenCount);
		if (frozenCount == totalPlayer)
		{
			Debug.Log("GAME END");
			CmdEnemyWins();
		}
	}
	
	[Command]
	public void CmdMinusFrozenCount()
	{
		frozenCount--;
		Debug.Log("minus" + frozenCount);
		RpcChangeFrozenCount(frozenCount);
	}
    
	[ClientRpc]
	private void RpcChangeFrozenCount(int serverFrozenCount)
	{
		Debug.Log("serverFrozenCount " + serverFrozenCount);
		FrozenText.text = "Players Frozen: " + serverFrozenCount;
	}
    
    
	//******************************ADD PLAYER COUNT(used when player join game)***************************************
	[Command]
	public void CmdAddPlayerCount()
	{
		totalPlayer++;
		RpcChangePlayerCount(totalPlayer);
	}
    
	[ClientRpc]
	public void RpcChangePlayerCount(int total)
	{
		if (total == 2)
		{
			//each client will disable their own waiting text under global text
			disableLocalWaitingText();
			
			if (isServer)
			{
				StartCoroutine(StartTimer());
				FindObjectOfType<EnemyController>().enableMovement();
			}
			else
			{
				PlayerController[] playerControllers = FindObjectsOfType<PlayerController>();
				Debug.Log(playerControllers.Length);
				foreach (PlayerController player in playerControllers)
				{
					player.enableMovement();
				}
			}			
		}	
	}
	
	//********************************* WAITING FOR PLAYER TEXT*****************************
	public Text waitingText;

	public void disableLocalWaitingText()
	{
		waitingText.enabled = false;
	}
	
    
	//*******************************GAME TIMER****************************************
	public float durationOfGame = 10;
	private float timeLeft;
	public Text TimerText;
	
	[ClientRpc]
	public void RpcUpdateTimer(float time)
	{
		timeLeft = time;
		TimerText.text = timeLeft.ToString();
	}
	
	//call this method to start game timer
	public IEnumerator StartTimer()
	{
		timeLeft = durationOfGame;
		while (timeLeft > 0)
		{
			yield return new WaitForSeconds(1.0f);
			timeLeft--;
			RpcUpdateTimer(timeLeft);

			if (timeLeft == 0)
			{
				if (totalResourceCollected >= NumOfResourcesToWin)
				{
					CmdPlayerWins();
				}
				else
				{
					CmdEnemyWins();
				}
			}
		}
	}
	
	//************************************KEEPTRACK OF total RESOURCE*****************************************
	private int totalResourceCollected = 0;


	[Command]
	public void CmdPlusGlobalResource()
	{
		//update server total resource
		totalResourceCollected++;
		Debug.Log(totalResourceCollected);
		RpcChangeGlobalResource(totalResourceCollected);
	}
	
	[Command]
	public void CmdMinusGlobalResource()
	{
		totalResourceCollected--;
		Debug.Log(totalResourceCollected);
		RpcChangeGlobalResource(totalResourceCollected);
	}

	public Text TotalRText;
	
	[ClientRpc]
	public void RpcChangeGlobalResource(int i)
	{
		TotalRText.text = "Total Resource: " + i + "/" + NumOfResourcesToWin;
	}
	

//********************************END GAME SCENES********************************
	
	public GameObject PlayerWinText;
	public GameObject EnemyWinText;
	

	[Command]
	private void CmdPlayerWins()
	{
		RpcPlayerWins();
		StartCoroutine(EndGame());
	}
	
	[ClientRpc]
	private void RpcPlayerWins()
	{
		PlayerWinText.SetActive(true);
	}

	[Command]
	private void CmdEnemyWins()
	{
		RpcEnemyWins();
		StartCoroutine(EndGame());
	}
	[ClientRpc]
	private void RpcEnemyWins()
	{
		EnemyWinText.SetActive(true);
	}
	
	

	public int RestartTimer = 5;
	public Text endGameText;

	IEnumerator EndGame()
	{
		RpcEnableCountdownText();
		int countDown = RestartTimer;
		while ( countDown > 0 )
		{
			RpcUpdateCountdownText(countDown);
			yield return new WaitForSeconds(1.0f);
			countDown--;
			if (countDown == 0)
			{
				NetworkManager.singleton.ServerChangeScene("World");
			}
		}
	}

	[ClientRpc]
	public void RpcEnableCountdownText()
	{
		endGameText.gameObject.SetActive(true);
	}

	[ClientRpc]
	public void RpcUpdateCountdownText(int count)
	{
		endGameText.text = "Game Ending in ... " + count;
	}
}
