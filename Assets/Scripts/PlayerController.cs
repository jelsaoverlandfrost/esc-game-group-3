using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;


public class PlayerController : NetworkBehaviour
{

    public float m_TurnSpeed = 180f;
    public float m_Speed = 2f;
	public VirtualJoystick joystick;
    private float m_MovementInputValue;
    private float m_TurnInputValue;
    private Rigidbody m_Rigidbody;

    public float pickupSlow = 0.5f;
    public Text countText;
	public static int pickupCount;
    private int pickupTriggerDuration = 0;
    public GameObject pickupPrefab;
    public static bool froze;
	public static bool click = false;
    public Button dropButton;
    public Text dropButtonText;

    private _GameManager _gameManager;

    // Update is called once per frame
    private void Awake()
    {
        pickupCount = 0;
        m_Rigidbody = GetComponent<Rigidbody>();
        countText.text = "Resource: " + pickupCount.ToString();
        _gameManager = FindObjectOfType<_GameManager>();
    }


    private void OnEnable()
    {
        
        m_Rigidbody.isKinematic = false;
        m_MovementInputValue = 0f;
        m_TurnInputValue = 0f;
    }

    private void OnDisable()
    {
        m_Rigidbody.isKinematic = true;
    }

    void Update()
    {
        if (hasAuthority == true)
        {
            transform.Find("Main Camera").gameObject.SetActive(true);
            transform.Find("Canvas").gameObject.SetActive(true);
        }


		m_MovementInputValue = joystick.InputDirection[2];
		m_TurnInputValue = joystick.InputDirection[0];

        if (canMove == false)
        {
            ignoreInputs();
        }

    }

    public void OnDropClick()
    {
        if (hasAuthority == false)
            return;

        if (pickupCount >= 1)
        {
            CmdDropPickup();
            pickupCount--;
            GetComponent<Powerup>().SpeedUpPlayer();
            countText.text = "Resource: " + pickupCount.ToString();
            dropButtonText.text = "Dash/Drop\n" + pickupCount;
            if (pickupCount <= 0)
            {
                dropButton.GetComponent<Image>().color = Color.white;
                countText.color = Color.white;
            }
            
        }
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        // Adjust the position of the tank based on the player's input.
        transform.Translate((m_Speed - pickupCount * pickupSlow) * m_TurnInputValue * Time.deltaTime, 0f, (m_Speed - pickupCount * pickupSlow) * m_MovementInputValue * Time.deltaTime);
    }

    //COLLECTING PICKUPS
	public void OnTriggerStay(Collider other)
    {
        if (hasAuthority == false)
            return;
        
        if (other.gameObject.CompareTag("pickup"))
        {
            
            pickupTriggerDuration++;
        }

        if (pickupTriggerDuration > 100)
        {
            CmdtakePickup(other.gameObject);
            pickupTriggerDuration = 0;
            pickupCount++;
            countText.text = "Resource: " + pickupCount.ToString();
            if (pickupCount > 0)
            {
                dropButton.GetComponent<Image>().color = Color.green;
                dropButtonText.text = "Dash/Drop\n" + pickupCount;
                countText.color = Color.green;
                
            }
        }


    }

    private void OnTriggerExit(Collider other)
    {
        if (hasAuthority == false || gameObject.CompareTag("Enemy"))
            return;
        
        if (other.gameObject.CompareTag("pickup"))
        {
            pickupTriggerDuration = 0;
        }
    }


    [Command]
    private void CmdtakePickup(GameObject pickupObj)
    {
        //later add func to update total pickup count
        RpctakePickup(pickupObj);
        _gameManager.CmdPlusGlobalResource();
        
    }
    
    [ClientRpc]
    private void RpctakePickup(GameObject pickupObj)
    {
        Destroy(pickupObj);
    }

    [Command]
    private void CmdDropPickup()
    {
        GameObject pickup = Instantiate(pickupPrefab, transform.position, Quaternion.identity);
        //later add func to decrease total pickup count
        NetworkServer.Spawn(pickup);
        _gameManager.CmdMinusGlobalResource();
    }

    //**********************************ENABLE AND DISABLE MOVEMENT*************************************
    private bool canMove = false;
    public void disableMovement()
    {
        canMove = false;
    }

    public void enableMovement()
    {
        Debug.Log("canMove = true");
        canMove = true;
    }

    private void ignoreInputs()
    {
        m_MovementInputValue = 0;
        m_TurnInputValue = 0;
    }

    /*
     * GET AND SET METHODS
     */
    public void AddSpeed(float speed)
    {
        m_Speed += speed;
    }

    public int getPickupCount()
    {
        return pickupCount;
    }

    public void setPickupCount(int i)
    {
        pickupCount = i;
    }


}