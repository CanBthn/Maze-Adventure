using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    CharacterController controller;
    AudioSource source;

    Vector3 velocity;
    bool isGrounded;
    bool isMoving;

    public Transform ground;
    public float distance = 0.3f;

    public float speed;
    public float jumpHeight;
    public float gravity;

    public float originalHeight;
    public float crouchHeight;

    public LayerMask mask;
    public float timeBetweenSteeps;
    float timer;
    public AudioClip[] stepsSounds;

    public float maxStamina = 100f;
    private float currentStamina;
    public float staminaRegenRate = 5f;
    public float sprintDrainRate = 10f; //Stamina reduce
    private bool isTired = false;
    public Slider StaminaBar;
    private void Start()
    {
        controller = GetComponent<CharacterController>();
        source = GetComponent<AudioSource>();

        currentStamina = maxStamina;

        Cursor.visible = false; // invisible mouse
        Cursor.lockState = CursorLockMode.Locked; //Lock mouse
    }

    private void Update()
    {
        #region Movement
        float horizantal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 move = transform.right * horizantal + transform.forward * vertical;
        controller.Move(move * speed * Time.deltaTime);
        #endregion

        #region Jump
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            velocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravity); //When up, veloctiy is reduce
        }
        #endregion

        #region Gravity
        isGrounded = Physics.CheckSphere(ground.position, distance, mask);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = 0;
        }
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
        #endregion

        #region Crouch
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            gravity = -300f;
            speed = 7.0f;
            timeBetweenSteeps = 0.8f;
            controller.height = crouchHeight;
        }
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            gravity = -15f;
            speed = 15.0f;
            timeBetweenSteeps = 0.6f;
            controller.height = originalHeight;
        }
        #endregion

        #region Run
        if (Input.GetKeyDown(KeyCode.LeftShift) && currentStamina > 0)
        {
            timeBetweenSteeps = 0.3f;
            speed = 30.0f;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            timeBetweenSteeps = 0.6f;
            speed = 15.0f;
        }
        #endregion

        #region FootSteps
        if (horizantal != 0 || vertical != 0)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }
        if (isMoving)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                timer = timeBetweenSteeps;
                source.clip = stepsSounds[Random.Range(0, stepsSounds.Length)];
                source.pitch = Random.Range(0.5f, 1.0f);
                source.Play();
            }
        }
        else
        {
            timer = timeBetweenSteeps;
        }
        #endregion

        HandleStamina();
        if (StaminaBar != null)
        {
            StaminaBar.value = currentStamina;
        }
    }

    private void HandleStamina()
    {   
        if (Input.GetKey(KeyCode.LeftShift) && currentStamina > 0)
        {
            speed = 30.0f;
            currentStamina -= sprintDrainRate * Time.deltaTime;
        }
        else
        {

            if (currentStamina < maxStamina)
            {
                currentStamina += staminaRegenRate * Time.deltaTime;
                if (isTired)
                {
                    speed = 15.0f;
                    isTired = false;
                }
            }
        }

        if (currentStamina <= 0 && !isTired)
        {
            isTired = true;
        }
    }
}