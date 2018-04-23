using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerNetworkObj : NetworkBehaviour
{
	private _GameManager _gameManager;
	private int playerCount = 0;
	
	private void Awake()
	{
		_gameManager = FindObjectOfType<_GameManager>();
		
	}

	private void Start()
	{
		if (isLocalPlayer && isServer)
		{
			CmdSpawnEnemy();
		}
		else if(isLocalPlayer)
		{
			CmdSpawnPlayer();
		}
		_gameManager.ActivateGlobalText();
	}

	public GameObject playerObjPrefab;
	
	[Command]
	private void CmdSpawnPlayer()
	{
		//spawn player at playerNetworkObj position
		GameObject playerObj = Instantiate(playerObjPrefab, transform.position, Quaternion.identity);
		NetworkServer.SpawnWithClientAuthority(playerObj,connectionToClient);
		//increase player count
		playerObj.GetComponent<PlayerController>().disableMovement();
		_gameManager.CmdAddPlayerCount();
	}

	public GameObject enemyObjPrefab;
	
	[Command]
	private void CmdSpawnEnemy()
	{
		GameObject enemyObj = Instantiate(enemyObjPrefab, transform.position, Quaternion.identity);
		NetworkServer.SpawnWithClientAuthority(enemyObj,connectionToClient);
		enemyObj.GetComponent<EnemyController>().disableMovement();
	}
}
