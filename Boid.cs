using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Neighborhood))]
public class Boid : MonoBehaviour
{
    [Header("Set Dynamically")]
    public Rigidbody rigid;

    Neighborhood neighborhood;
    TrailRenderer trailRenderer;

    public Vector3 pos
    {
        get => transform.position;
        set => transform.position = value;
    }

    void Awake()
    {
        neighborhood = GetComponent<Neighborhood>();
        rigid = GetComponent<Rigidbody>();
        trailRenderer = GetComponent<TrailRenderer>();

        if (Spawner.S == null)
        {
            Debug.LogError("Boid requires an active Spawner in the scene.", this);
            enabled = false;
            return;
        }

        pos = Random.insideUnitSphere * Spawner.S.spawnRadius;
        rigid.linearVelocity = Random.onUnitSphere * Spawner.S.velocity;

        ApplyRandomColor();
        LookAhead();
    }

    void FixedUpdate()
    {
        if (Spawner.S == null || rigid == null || neighborhood == null)
        {
            return;
        }

        Vector3 vel = rigid.linearVelocity;
        Spawner spn = Spawner.S;

        Vector3 velAvoid = Vector3.zero;
        Vector3 tooClosePos = neighborhood.avgClosePos;

        if (tooClosePos != Vector3.zero)
        {
            velAvoid = (pos - tooClosePos).normalized * spn.velocity;
        }

        Vector3 velAlign = neighborhood.avgVel;
        if (velAlign != Vector3.zero)
        {
            velAlign = velAlign.normalized * spn.velocity;
        }

        Vector3 velCenter = neighborhood.avgPos;
        if (velCenter != Vector3.zero)
        {
            velCenter = (velCenter - transform.position).normalized * spn.velocity;
        }

        Vector3 delta = Attractor.POS - pos;
        bool attracted = delta.magnitude > spn.attractPushDist;
        Vector3 velAttract = delta == Vector3.zero ? Vector3.zero : delta.normalized * spn.velocity;

        float fdt = Time.fixedDeltaTime;
        if (velAvoid != Vector3.zero)
        {
            vel = Vector3.Lerp(vel, velAvoid, spn.collAvoid * fdt);
        }
        else if (velAlign != Vector3.zero)
        {
            vel = Vector3.Lerp(vel, velAlign, spn.velMatching * fdt);
        }

        if (velCenter != Vector3.zero)
        {
            vel = Vector3.Lerp(vel, velCenter, spn.flockCentering * fdt);
        }

        if (velAttract != Vector3.zero)
        {
            vel = attracted
                ? Vector3.Lerp(vel, velAttract, spn.attractPull * fdt)
                : Vector3.Lerp(vel, -velAttract, spn.attractPush * fdt);
        }

        rigid.linearVelocity = vel.normalized * spn.velocity;
        LookAhead();
    }

    void ApplyRandomColor()
    {
        Color randColor = Color.black;
        while (randColor.r + randColor.g + randColor.b < 1f)
        {
            randColor = new Color(Random.value, Random.value, Random.value);
        }

        Renderer[] rends = GetComponentsInChildren<Renderer>();
        foreach (Renderer rendererComponent in rends)
        {
            rendererComponent.material.color = randColor;
        }

        if (trailRenderer != null)
        {
            Material trailMaterial = trailRenderer.material;
            if (trailMaterial.HasProperty("_TintColor"))
            {
                trailMaterial.SetColor("_TintColor", randColor);
            }
            else if (trailMaterial.HasProperty("_Color"))
            {
                trailMaterial.SetColor("_Color", randColor);
            }
        }
    }

    void LookAhead()
    {
        Vector3 lookTarget = pos + rigid.linearVelocity;
        if (lookTarget != pos)
        {
            transform.LookAt(lookTarget);
        }
    }
}
