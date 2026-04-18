using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class Neighborhood : MonoBehaviour
{
    [Header("Set Dynamically")]
    public List<Boid> neighbors = new List<Boid>();

    SphereCollider coll;

    public Vector3 avgPos => GetAveragePosition(false);
    public Vector3 avgClosePos => GetAveragePosition(true);

    public Vector3 avgVel
    {
        get
        {
            if (neighbors.Count == 0)
            {
                return Vector3.zero;
            }

            Vector3 avg = Vector3.zero;
            int validCount = 0;

            for (int i = neighbors.Count - 1; i >= 0; i--)
            {
                Boid boid = neighbors[i];
                if (boid == null || boid.rigid == null)
                {
                    neighbors.RemoveAt(i);
                    continue;
                }

                avg += boid.rigid.linearVelocity;
                validCount++;
            }

            return validCount == 0 ? Vector3.zero : avg / validCount;
        }
    }

    void Awake()
    {
        coll = GetComponent<SphereCollider>();
        neighbors ??= new List<Boid>();
        ConfigureCollider();
    }

    void OnEnable()
    {
        ConfigureCollider();
    }

    void OnDisable()
    {
        neighbors.Clear();
    }

    void FixedUpdate()
    {
        ConfigureCollider();
    }

    void OnValidate()
    {
        if (coll == null)
        {
            coll = GetComponent<SphereCollider>();
        }

        if (coll != null)
        {
            ConfigureCollider();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Boid boid = other.GetComponent<Boid>();
        if (boid != null && boid != GetComponent<Boid>() && !neighbors.Contains(boid))
        {
            neighbors.Add(boid);
        }
    }

    void OnTriggerExit(Collider other)
    {
        Boid boid = other.GetComponent<Boid>();
        if (boid != null)
        {
            neighbors.Remove(boid);
        }
    }

    void ConfigureCollider()
    {
        if (coll == null)
        {
            return;
        }

        coll.isTrigger = true;

        if (Spawner.S != null)
        {
            coll.radius = Mathf.Max(0.01f, Spawner.S.neighborDist);
        }
    }

    Vector3 GetAveragePosition(bool onlyCloseNeighbors)
    {
        if (neighbors.Count == 0 || Spawner.S == null)
        {
            return Vector3.zero;
        }

        Vector3 avg = Vector3.zero;
        int validCount = 0;

        for (int i = neighbors.Count - 1; i >= 0; i--)
        {
            Boid boid = neighbors[i];
            if (boid == null)
            {
                neighbors.RemoveAt(i);
                continue;
            }

            if (onlyCloseNeighbors)
            {
                float distance = Vector3.Distance(boid.pos, transform.position);
                if (distance > Spawner.S.collDist)
                {
                    continue;
                }
            }

            avg += boid.pos;
            validCount++;
        }

        return validCount == 0 ? Vector3.zero : avg / validCount;
    }
}
