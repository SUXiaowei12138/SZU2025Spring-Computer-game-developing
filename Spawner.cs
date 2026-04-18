using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public static Spawner S;
    public static List<Boid> boids;

    [Header("Boid Prefab")]
    public GameObject boidPrefab;
    public Transform boidAnchor;
    [Min(0)] public int numBoids = 100;
    [Min(0f)] public float spawnRadius = 100f;
    [Min(0f)] public float spawnDelay = 0.1f;

    [Header("Flocking Settings")]
    [Min(0.01f)] public float velocity = 10f;
    [Min(0f)] public float neighborDist = 30f;
    [Min(0f)] public float collDist = 4f;
    [Min(0f)] public float velMatching = 0.25f;
    [Min(0f)] public float flockCentering = 0.2f;
    [Min(0f)] public float collAvoid = 2f;
    [Min(0f)] public float attractPull = 2f;
    [Min(0f)] public float attractPush = 2f;
    [Min(0f)] public float attractPushDist = 5f;

    Coroutine spawnRoutine;

    void Awake()
    {
        S = this;
        boids = new List<Boid>();

        if (boidAnchor == null)
        {
            boidAnchor = transform;
        }
    }

    void Start()
    {
        if (boidPrefab == null)
        {
            Debug.LogError("Spawner requires a boid prefab reference.", this);
            enabled = false;
            return;
        }

        spawnRoutine = StartCoroutine(SpawnBoids());
    }

    void OnValidate()
    {
        numBoids = Mathf.Max(0, numBoids);
        spawnRadius = Mathf.Max(0f, spawnRadius);
        spawnDelay = Mathf.Max(0f, spawnDelay);
        velocity = Mathf.Max(0.01f, velocity);
        collDist = Mathf.Max(0f, collDist);
        neighborDist = Mathf.Max(collDist, neighborDist);
        attractPushDist = Mathf.Max(0f, attractPushDist);
    }

    void OnDisable()
    {
        if (spawnRoutine != null)
        {
            StopCoroutine(spawnRoutine);
            spawnRoutine = null;
        }
    }

    void OnDestroy()
    {
        if (S == this)
        {
            S = null;
        }
    }

    IEnumerator SpawnBoids()
    {
        for (int i = 0; i < numBoids; i++)
        {
            InstantiateBoid();

            if (spawnDelay > 0f)
            {
                yield return new WaitForSeconds(spawnDelay);
            }
            else
            {
                yield return null;
            }
        }

        spawnRoutine = null;
    }

    public void InstantiateBoid()
    {
        GameObject go = Instantiate(boidPrefab, boidAnchor);
        Boid boid = go.GetComponent<Boid>();

        if (boid == null)
        {
            Debug.LogError("The boid prefab must contain a Boid component.", boidPrefab);
            Destroy(go);
            return;
        }

        boids.Add(boid);
    }
}
