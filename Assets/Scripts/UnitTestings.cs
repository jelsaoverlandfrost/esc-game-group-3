using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class PickUpNumberTest
{
	private string initialScenePath;
	GameObject playerTest;
	public static GameObject pickUpTest;
	public static int initialCount;
	private int pickupTriggerDuration;
	public static GameObject[] pickups;
	private static bool checker = false;

	[SetUp]
	public void Setup()
	{
		Debug.Log("Load Scene");
		initialScenePath = SceneManager.GetActiveScene().path;
		SceneManager.LoadScene("Scenes/World");	
	}

	[TearDown]
	public void TearDown()
	{
		SceneManager.LoadScene(initialScenePath);
	}

	public static bool Checker() {
		if (PlayerController.pickupCount > 0) {
			return true;
		}
		return false;
	}

	[UnityTest]
	public IEnumerator PickUpTestEnumerator()
	{
		UnityEvent pickUpEvent;
		Debug.Log("**The test started**");
		playerTest = (GameObject) MonoBehaviour.Instantiate (Resources.Load<GameObject> ("PlayerObj"));
		pickups = GameObject.FindGameObjectsWithTag("pickup");
		initialCount = pickups.Length;
		Debug.Log (initialCount);
		yield return new WaitUntil (Checker);
	}

}


public class BasicMovementTest
{
	private string initialScenePath;
	static GameObject playerTest;
	private static Vector3 initialPosition;
	private int pickupTriggerDuration;

	[SetUp]
	public void Setup()
	{
		Debug.Log("Load Scene");
		initialScenePath = SceneManager.GetActiveScene().path;
		SceneManager.LoadScene("Scenes/World");
	}

	[TearDown]
	public void TearDown()
	{
		SceneManager.LoadScene(initialScenePath);
	}

	public static bool CheckerUp() {
		if (playerTest.transform.position.y > initialPosition.y) {
			return true;
		}
		return false;
	}

	public static bool CheckerDown() {
		if (playerTest.transform.position.y < initialPosition.y) {
			return true;
		}
		return false;
	}

	public static bool CheckerLeft() {
		if (playerTest.transform.position.x < initialPosition.x) {
			return true;
		}
		return false;
	}

	public static bool CheckerRight() {
		if (playerTest.transform.position.x > initialPosition.x) {
			return true;
		}
		return false;
	}

	[UnityTest]
	public IEnumerator TestMovingUp()
	{
		Debug.Log("**The test started**");
		Debug.Log("**Move Up Now**");
		playerTest = (GameObject) MonoBehaviour.Instantiate (Resources.Load<GameObject> ("PlayerObj"));
		initialPosition = playerTest.transform.position;
		yield return new WaitUntil(CheckerUp);
	}

	[UnityTest]
	public IEnumerator TestMovingDown()
	{
		Debug.Log("**The test started**");
		Debug.Log("**Move Down Now**");
		playerTest = (GameObject) MonoBehaviour.Instantiate (Resources.Load<GameObject> ("PlayerObj"));
		initialPosition = playerTest.transform.position;
		yield return new WaitUntil(CheckerDown);
	}

	[UnityTest]
	public IEnumerator TestMovingLeft()
	{
		Debug.Log("**The test started**");
		Debug.Log("**Move Left Now**");
		playerTest = (GameObject) MonoBehaviour.Instantiate (Resources.Load<GameObject> ("PlayerObj"));
		initialPosition = playerTest.transform.position;
		yield return new WaitUntil(CheckerLeft);
	}

	[UnityTest]
	public IEnumerator TestMovingRight()
	{
		Debug.Log("**The test started**");
		Debug.Log("**Move Right Now**");
		playerTest = (GameObject) MonoBehaviour.Instantiate (Resources.Load<GameObject> ("PlayerObj"));
		initialPosition = playerTest.transform.position;
		yield return new WaitUntil(CheckerRight);
	}

		
}

public class FrozenTest
{
	private string initialScenePath;
	GameObject enemyTest;
	GameObject playerTest;

	[SetUp]
	public void Setup()
	{
		Debug.Log("Load Scene");
		initialScenePath = SceneManager.GetActiveScene().path;
		SceneManager.LoadScene("Scenes/World");
	}

	[TearDown]
	public void TearDown()
	{
		SceneManager.LoadScene(initialScenePath);
	}

	[UnityTest]
	public IEnumerator FrozenTestEnumerator() 
	{
		yield return new WaitForSeconds (100);
		Debug.Log("**The test started**");
		enemyTest = (GameObject) MonoBehaviour.Instantiate (Resources.Load<GameObject> ("EnemyObj"));
		playerTest = (GameObject) MonoBehaviour.Instantiate (Resources.Load<GameObject> ("PlayerObj"));
		Debug.Log (playerTest == null);
	}

	void OnCollisionEnter(Collision collision) {
		if (collision.gameObject.CompareTag("EnemyObj")) {
			Assert.IsTrue(playerTest.GetComponent<MeshRenderer>().material == Freeze.frozen);
		}
	}
}


 public class PickUpDropTest
{
	private string initialScenePath;
	GameObject playerTest;
	public static GameObject pickUpTest;
	public static int initialCount;
	public static int currentCount;
	private int pickupTriggerDuration;
	public static GameObject[] pickups;

	[SetUp]
	public void Setup()
	{
		Debug.Log("Load Scene");
		initialScenePath = SceneManager.GetActiveScene().path;
		SceneManager.LoadScene("Scenes/World");	
	}

	[TearDown]
	public void TearDown()
	{
		SceneManager.LoadScene(initialScenePath);
	}

	public static bool Checker() {
		if (currentCount == initialCount) {
			return true;
		}
		return false;
	}

	[UnityTest]
	public IEnumerator PickUpTestEnumerator()
	{
		UnityEvent pickUpEvent;
		Debug.Log("**The test started**");
		playerTest = (GameObject) MonoBehaviour.Instantiate (Resources.Load<GameObject> ("PlayerObj"));
		pickUpTest = (GameObject) MonoBehaviour.Instantiate (Resources.Load<GameObject> ("BackGround"));
		pickups = GameObject.FindGameObjectsWithTag("pickup");
		initialCount = pickups.Length;
		yield return new WaitUntil(Checker);
	}

	public void OnDropClick()
	{
		currentCount = pickups.Length;
	}

}
	