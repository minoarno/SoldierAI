using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SeekBehavior : SteeringBehavior
{
    // Update is called once per frame
    public void Update()
    {
        
    }

    public override SteeringOutput CalculateSteering(SteeringAgent agent)
    {
        SteeringOutput steering = new SteeringOutput();

        steering.LinearVelocity = m_Target - agent.GetPosition();
        steering.LinearVelocity.Normalize();
        steering.LinearVelocity *= agent.GetMaxLinearSpeed();
        steering.IsValid = true;

        return steering;
    }
}
