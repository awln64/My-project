using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour {
    public Vector3 spawnPosition = new Vector3(-2, 1.3f, -20.65f);

    private NavMeshAgent agent;
    private Animator animator;
    private bool hasMoved = false;
    

    void Start() {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    public void StartMovement(Vector3 correctPos, Vector3 incorrectPos) {
        if (hasMoved) {
            return;
        }

        StartCoroutine(MoveToAnswer(correctPos, incorrectPos));
    }

    IEnumerator MoveToAnswer(Vector3 LeftSign, Vector3 RightSign) {
        yield return new WaitForSeconds(Random.Range(1f, 2f));
        hasMoved = true;

        Vector3 target = Random.value > 0.5f ? LeftSign : RightSign;
        animator.SetBool("walks", true);
        if (agent != null) {
            agent.SetDestination(target);
        } else {
            transform.position = Vector3.MoveTowards(transform.position, target, 5f);
        }
    }

    void Update() {
        if (agent.remainingDistance < 0.2f && agent.hasPath) {
            animator.SetBool("walks", false);
        }
    }

    public void Respawn() {
        StopAllCoroutines();

        Vector2 randomCircle = Random.insideUnitCircle * 5;
        Vector3 rawSpawnPos = new Vector3(spawnPosition.x + randomCircle.x, spawnPosition.y, spawnPosition.z + randomCircle.y);

        if (NavMesh.SamplePosition(rawSpawnPos, out NavMeshHit hit, 2f, NavMesh.AllAreas)) {
            if (agent != null) {
                agent.ResetPath();
                agent.Warp(hit.position);
            } else {
                transform.position = hit.position;
            }

            gameObject.SetActive(true);
            hasMoved = false;
            animator.SetBool("walks", false);
        } else {
            Debug.LogError($"{name} failed to respawn: no NavMesh nearby at {rawSpawnPos}");
        }
    }
}
