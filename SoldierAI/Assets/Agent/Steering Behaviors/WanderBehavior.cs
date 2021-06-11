using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WanderBehavior : SeekBehavior
{
    private float m_OffsetDistance = 6.0f;
    [SerializeField]
    protected float m_Radius = 4.0f;
    private const float m_MaxAngleChange = 45 * Mathf.Deg2Rad;
    
    private float m_WanderAngle = 0.0f;

    public WanderBehavior()
    {
    }

    public override SteeringOutput CalculateSteering(SteeringAgent agent)
    {
        Vector3 offset = agent.GetVelocity();
        offset.Normalize();
        offset *= m_OffsetDistance;

        m_WanderAngle += Random.Range(-0.5f, 0.5f) * m_MaxAngleChange;
        Vector3 circleOffset = new Vector3(Mathf.Cos(m_WanderAngle) * m_Radius, 0, Mathf.Sin(m_WanderAngle) * m_Radius);

        m_Target = agent.GetPosition() + offset + circleOffset;

        return base.CalculateSteering(agent);
    }
}

