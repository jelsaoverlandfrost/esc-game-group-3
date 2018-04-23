using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CustomNetworkManager : NetworkManager
{
	public void Start() {
		StartMatchMaker ();
		matchMaker.ListMatches(0, 3, "", true, 0, 0, HandleListMatchesComplete);
	}

	private void OnMatchCreated(bool success, string extendedinfo, MatchInfo responsedata) {
		base.StartHost (responsedata);
	}

	private void HandleJoinedMatch(bool success, string extendedinfo, MatchInfo responsedata) {
		StartClient(responsedata);
	}

	private void HandleListMatchesComplete(bool success, 
		string extendedinfo, 
		List<MatchInfoSnapshot> responsedata) {
		Debug.Log ("count is " + responsedata.Count);
		if (responsedata.Count == 0) {
			Debug.Log ("created match");
			matchMaker.CreateMatch ("Jasons Match", 3, true, "", "", "", 0, 0, OnMatchCreated);
		} else if (responsedata.Count < 3) {
			Debug.Log ("joined match");
			matchMaker.JoinMatch (responsedata[0].networkId, "", "", "", 0, 0, HandleJoinedMatch);
		}
	}


}
