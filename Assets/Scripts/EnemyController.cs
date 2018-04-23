using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;


public class EnemyController : NetworkBehaviour
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

	public static bool click = false;
    

    // Update is called once per frame
    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        countText.text = "Resources: " + pickupCount.ToString();        
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

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        // Adjust the position of the tank based on the player's input.
        transform.Translate((m_Speed - pickupCount * pickupSlow) * m_TurnInputValue * Time.deltaTime, 0f, (m_Speed - pickupCount * pickupSlow) * m_MovementInputValue * Time.deltaTime);
    }

    //JUMP
    private bool isGrounded = false;
    private bool isOffCooldown = true;
    public float jumpForce = 1f;
    public float cooldownTime = 5f;
    public Button jumpButton;

    private void OnCollisionStay(Collision other)
    {
        isGrounded = true;
    }
    
    private void OnCollisionExit(Collision other)
    {
        isGrounded = false;
    }

    public void Jump()
    {
        if (isGrounded && isOffCooldown)
        {
            Debug.Log("Adding force");
            m_Rigidbody.AddRelativeForce(new Vector3(0,jumpForce,0),ForceMode.Impulse);
            StartCoroutine(startCooldown());
        }
    }
    
    private IEnumerator startCooldown()
    {
        isOffCooldown = false;
        jumpButton.GetComponent<Image>().color = Color.red;
        yield return new WaitForSeconds(cooldownTime);
        jumpButton.GetComponent<Image>().color = Color.white;
        isOffCooldown = true;
    }

    //**********************************ENABLE AND DISABLE MOVEMENT*************************************
    private bool canMove = false;
    public void disableMovement()
    {
        canMove = false;
    }

    public void enableMovement()
    {
        canMove = true;
    }

    private void ignoreInputs()
    {
        m_MovementInputValue = 0;
        m_TurnInputValue = 0;
    }
}