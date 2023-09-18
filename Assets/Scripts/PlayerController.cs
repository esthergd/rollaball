using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float speed = 0;
    public TextMeshProUGUI countText;
    public float startTime = 20;  // Starting time in seconds
    public TextMeshProUGUI timerText;
    public AudioClip collectibleSound;

    private float timeRemaining;
    private int timeRemainingInt;
    private Rigidbody rb;
    private int count;
    private float movementX;
    private float movementY;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        count = 0;
        timeRemaining = startTime;

        SetCountText();

        audioSource = GetComponent<AudioSource>();
    }

    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();

        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    void SetCountText()
    {
        countText.text = "Count: " + count.ToString();
        if(count >= 13)
        {
            SceneManager.LoadScene("GameWin");
        }
    }

    void FixedUpdate()
    {
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);

        rb.AddForce(movement * speed);

        // Decrease the time remaining
        timeRemaining -= Time.deltaTime;
        timeRemainingInt = (int) timeRemaining;

        timerText.text = "Time Remaining: " + timeRemainingInt.ToString() + "s";

        // Check for timer reaching zero
        if (timeRemaining <= 0)
        {
            SceneManager.LoadScene("GameOver");  // Replace with your Dead-End Scene name
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);
            count += 1;

            SetCountText();

            audioSource.PlayOneShot(collectibleSound);
        }
        
    }
}
