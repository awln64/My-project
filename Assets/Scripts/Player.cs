using TMPro;
using UnityEngine;


[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour {
    public TextMeshProUGUI scoreText;
    public Animator animator;
    public float speed = 5f;
    public Joystick joystick;
    public GameObject gameOverUI;
    public GameObject WinUI;

    private Vector3 spawnPosition = new Vector3(-2, 2f, -20.65f);
    private int score = 0;

    private CharacterController controller;

    private void Start() {
        controller = GetComponent<CharacterController>();
    }

    private void Update() {
        scoreText.text = "Score: " + score;
        Vector3 move = new Vector3(joystick.Horizontal, 0, joystick.Vertical);

        if (move.magnitude > 0.1f) {
            transform.forward = move;
            controller.Move(move.normalized * speed * Time.deltaTime);
            animator.SetBool("walks", true);
        } else {
            animator.SetBool("walks", false);
        }
    }

    public void Respawn() {
        Vector2 randomCircle = Random.insideUnitCircle * 5;
        Vector3 randomPos = new Vector3(spawnPosition.x + randomCircle.x, spawnPosition.y, spawnPosition.z + randomCircle.y);

        if (controller != null) {
            controller.enabled = false;
        }

        transform.position = randomPos;

        if (controller != null) {
            controller.enabled = true;
        }
        
        gameObject.SetActive(true);
        score++;
    }

    public void Die() {
        gameOverUI.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Win() {
        WinUI.SetActive(true);
        Time.timeScale = 0f;
    }
}
