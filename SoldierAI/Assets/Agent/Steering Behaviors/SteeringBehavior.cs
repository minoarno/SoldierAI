using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SteeringOutput
{
    public Vector3 LinearVelocity;
    public bool IsValid;
}

[System.Serializable]
public class SteeringBehavior : MonoBehaviour
{
    [SerializeField]
    protected Vector3 m_Target = new Vector3(0, 0, 0);

    // Update is called once per frame
    public virtual SteeringOutput CalculateSteering(SteeringAgent agent)
    {
        SteeringOutput steering = new SteeringOutput();
        return steering;
    }

    public void SetTarget(Vector3 newTarget)
    {
        m_Target = newTarget;
    }

    public Vector3 GetTarget()
    {
        return m_Target;
    }

}
