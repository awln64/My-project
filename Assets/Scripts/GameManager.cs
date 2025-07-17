using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public List<MemeData> memes = new List<MemeData>();

    public BannerDisplay bannerDisplay;

    public AnswerSign SignRight;
    public AnswerSign SignLeft;

    public List<Enemy> enemies;
    public Player player;
    public GameObject enemyPrefab;

    public TextMeshProUGUI timerText;
    public float roundTime = 10f;

    private float timer = 0f;
    private MemeData currentMeme;
    private bool roundActive = false;

    private void Awake() {
        memes = new List<MemeData>(Resources.LoadAll<MemeData>("MemeData"));
        enemies = FindObjectsOfType<Enemy>().ToList();
    }

    private void Start() {
        StartCoroutine(RoundLoop());
    }

    private void Update() {
        if (roundActive) {
            timer -= Time.deltaTime;
            timer = Mathf.Max(0, timer);
            UpdateTimerUI();
        }
    }

    IEnumerator RoundLoop() {
        while (true) {
            yield return StartCoroutine(StartNewRound());
            yield return new WaitForSeconds(0.2f);
        }
    }

    IEnumerator StartNewRound() {
        if(enemies.Count <= 0) {
            player.Win();
        }

        roundActive = false;
        currentMeme = memes[Random.Range(0, memes.Count)];
        bannerDisplay.ShowMeme(currentMeme);

        bool correctIsOnLeft = Random.value > 0.5f;

        if (correctIsOnLeft) {
            SignRight.SetText(GetWrongAnswer(currentMeme.name));
            SignLeft.SetText(currentMeme.name);
        } else {
            SignLeft.SetText(GetWrongAnswer(currentMeme.name));
            SignRight.SetText(currentMeme.name);
        }
        

        yield return new WaitForSeconds(2f);

        timer = roundTime;
        roundActive = true;

        foreach (var ai in enemies) {
            ai.StartMovement(SignLeft.transform.position, SignRight.transform.position);
        }

        yield return new WaitForSeconds(roundTime);

        roundActive = false;

        CheckResults();

        yield return new WaitForSeconds(1f);

        RespawnSurvivors();
    }

    void CheckResults() {
        string playerAnswer = GetAnswerAtPosition(player.transform.position);
        if (playerAnswer != currentMeme.name) {
            player.Die();
        }

        foreach (var ai in enemies.ToArray()) {
            string aiAnswer = GetAnswerAtPosition(ai.transform.position);
            if (aiAnswer != currentMeme.name) {
                enemies.Remove(ai);
                Destroy(ai.gameObject);
            }
        }
    }

    public string GetAnswerAtPosition(Vector3 position) {
        float distToLeft = Vector3.Distance(position, SignLeft.transform.position);
        float distToRight = Vector3.Distance(position, SignRight.transform.position);

        if (distToLeft < distToRight) {
            return SignLeft.textElement.text;
        } else {
            return SignRight.textElement.text;
        }
    }

    void RespawnSurvivors() {
        player.Respawn();

        foreach (var ai in enemies) {
            ai.Respawn();
        }
    }

    string GetWrongAnswer(string correct) {
        List<string> options = new List<string>();
        foreach (var meme in memes) {
            if (meme.name != correct) {
                options.Add(meme.name);
            }
        }

        return options[Random.Range(0, options.Count)];
    }

    void UpdateTimerUI() {
        int seconds = Mathf.CeilToInt(timer);
        timerText.text = "Time left: " + seconds.ToString();
    }
}
