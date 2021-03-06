using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringAgent : MonoBehaviour
{
    [SerializeField]
    protected Color m_Color = new Color(0f, 0f, 0f);

    [SerializeField]
    protected float m_MaxLinearSpeed = 20f;

    [SerializeField]
    protected Vector3 m_Velocity = new Vector3(0, 0, 0);

    protected SteeringBehavior m_SteeringBehavior = null;

    [SerializeField]
    protected Rigidbody m_RigidBody = null;

    bool m_IsEnabled = false;

    // Start is called before the first frame up = nulldate
    void Start()
    {
        m_SteeringBehavior = new WanderBehavior();

        Color color = new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f));
        foreach (Renderer renderer in gameObject.GetComponentsInChildren<Renderer>())
        {
            renderer.materials[0].color = color;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (m_IsEnabled)
        {
            if (m_SteeringBehavior != null)
            {
                SteeringOutput output = m_SteeringBehavior.CalculateSteering(this);

                Vector3 steeringForce = output.LinearVelocity - m_Velocity;
                m_Velocity = (m_Velocity + steeringForce);
                m_Velocity.Normalize();
            }
            //transform.position += m_Velocity;
            m_RigidBody.velocity = m_Velocity * Time.fixedDeltaTime * m_MaxLinearSpeed;
        }
        else
        {
            m_RigidBody.velocity = Vector3.zero;
        }
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public float GetMaxLinearSpeed()
    {
        return m_MaxLinearSpeed;
    }

    public Vector3 GetVelocity()
    {
        return m_Velocity;
    }

    public void SetSteeringBehavior(SteeringBehavior behavior, int delay =0)
    {
        if (behavior == null)
        {
            Debug.LogError("NULL");
        }
        m_SteeringBehavior = behavior;
        m_IsEnabled = false;
        Invoke("Enable", delay);
    }

    private void Enable()
    {
        m_IsEnabled = true;
    }

    public void SetTarget(Vector3 newTarget)
    {
        m_SteeringBehavior.SetTarget(newTarget);
    }

    public Vector3 GetTarget()
    {
        return m_SteeringBehavior.GetTarget();
    }

    public Color GetColor()
    {
        return m_Color;
    }
}
