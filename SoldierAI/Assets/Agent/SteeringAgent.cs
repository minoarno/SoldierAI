using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringAgent : MonoBehaviour
{
    [SerializeField]
    private Color m_Color = new Color(0f,0f,0f);

    [SerializeField]
    private float m_MaxLinearSpeed = 20f;

    [SerializeField]
    private Vector3 m_Velocity = new Vector3(0,0,0);

    [SerializeField]
    private SteeringBehavior m_SteeringBehavior = null;

    [SerializeField]
    private Rigidbody m_RigidBody = null;

    // Start is called before the first frame up = nulldate
    void Start()
    {
        Color color = new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f));
        foreach (Renderer renderer in gameObject.GetComponentsInChildren<Renderer>())
        {
            renderer.materials[0].color = color;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
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

    public void SetSteeringBehavior(SteeringBehavior behavior)
    {
        m_SteeringBehavior = behavior;
    }
}
