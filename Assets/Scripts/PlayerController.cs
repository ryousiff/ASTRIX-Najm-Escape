using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    public float thrustForce = 8f;
    public float maxSpeed = 12f;
    public GameObject BoosterFlame;
    public float scoreMultiplier = 10f;
    public UIDocument uiDocument;
    public GameObject explosionEffect;
    public GameObject borderParent;

    public InputAction moveForward;
    public InputAction lookPosition;

    private float elapsedTime = 0f;
    private Rigidbody2D rb;
    private Button restartButton;
    public AudioClip explosionSound;
    private AudioSource audioSource;
    private int score = 0;

    void OnEnable()
    {
        moveForward.Enable();
        lookPosition.Enable();
    }

    void OnDisable()
    {
        moveForward.Disable();
        lookPosition.Disable();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();

        if (uiDocument != null)
        {
            restartButton = uiDocument.rootVisualElement.Q<Button>("RestartButton");
            if (restartButton != null)
            {
                restartButton.style.display = DisplayStyle.None;
                restartButton.clicked += ReloadScene;
            }
        }

        if (BoosterFlame != null)
            BoosterFlame.SetActive(false);
    }

    void Update()
    {
        MovePlayer();

        elapsedTime += Time.deltaTime;
        score = Mathf.FloorToInt(elapsedTime * scoreMultiplier);
        GameUi gameUi = FindFirstObjectByType<GameUi>();
        if (gameUi != null)
            gameUi.UpdateScore(score);
    }

    void MovePlayer()
    {
        bool isMoving = moveForward.IsPressed();

        if (BoosterFlame != null)
            BoosterFlame.SetActive(isMoving);
        if (!isMoving)
            return;

        Vector2 screenPosition = lookPosition.ReadValue<Vector2>();
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(screenPosition);
        mousePos.z = 0f;

        Vector2 direction = (mousePos - transform.position).normalized;

        transform.up = direction;
        rb.AddForce(direction * thrustForce);

        if (rb.linearVelocity.magnitude > maxSpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
        }
    }

    // void OnCollisionEnter2D(Collision2D collision)
    // {
    //     // Play explosion sound
    //     if (audioSource != null && explosionSound != null)
    //     {
    //         audioSource.PlayOneShot(explosionSound);
    //     }
    //     FindFirstObjectByType<GameUi>()?.ShowGameOver();

    //     if (borderParent != null)
    //         borderParent.SetActive(false);

    //     Destroy(gameObject);
    // }
    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("HIT!");
        // Spawn explosion (with sound)
        Instantiate(explosionEffect, transform.position, Quaternion.identity);

        FindFirstObjectByType<GameUi>()?.ShowGameOver();

        if (borderParent != null)
            borderParent.SetActive(false);

        Destroy(gameObject);
    }

    void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}