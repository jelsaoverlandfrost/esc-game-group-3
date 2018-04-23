using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Powerup : NetworkBehaviour
{
	public float speedIncrease = 1f;
	public float speedIncreaseDuration = 5f;
	private TrailRenderer trailRenderer;
	public Material gold;
	public Material green;
	public Material red;
	
    private PlayerController playerController;
	// Use this for initialization
	void Start () {
		playerController = GetComponent<PlayerController>();
		trailRenderer = GetComponent<TrailRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	private void SpeedUp(){
	    playerController.AddSpeed(speedIncrease);
		
	}

	private void SlowDown()
	{
		playerController.AddSpeed(-speedIncrease);
	}

	private int NumOfCoroutineRunning;
	public void SpeedUpPlayer()
	{
		StartCoroutine(SetTrailVisibleForDuration());
	}

	IEnumerator SetTrailVisibleForDuration()
	{
		NumOfCoroutineRunning++;
		SpeedUp();
		CmdTrailActive();
		yield return new WaitForSeconds(speedIncreaseDuration);
		SlowDown();
		NumOfCoroutineRunning--;
		//in case of stacking powerups, the trail will still show
		if (NumOfCoroutineRunning == 0)
		{
			CmdTrailInactive();
		}
	}
	[Command]
	private void CmdTrailActive()
	{
		RpcSetTrail(this.gameObject, 1.2f);
		RpcSetPlayerMaterialGold(this.gameObject);
	}

	[Command]
	private void CmdTrailInactive()
	{
		RpcSetTrail(this.gameObject, 0.7f);
		RpcSetPlayerMaterialRed(this.gameObject);
	}
	
	[ClientRpc]
	public void RpcSetTrail(GameObject obj, float m)
	{
		obj.GetComponent<TrailRenderer>().time = m;
	}

	[ClientRpc]
	public void RpcSetPlayerMaterialGold(GameObject obj)
	{
		obj.GetComponent<MeshRenderer>().material = gold;
	}
	
	[ClientRpc]
	public void RpcSetPlayerMaterialRed(GameObject obj)
	{
		obj.GetComponent<MeshRenderer>().material = red;
	}
}
