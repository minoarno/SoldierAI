using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class LegionAgent : SteeringAgent
{
    int m_CurrentRow;
    int m_CurrentCol;

    public Vector3 m_Hitpoint;

    void Start()
    {
        m_SteeringBehavior = new WanderBehavior();

        Color color = new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f));
        foreach (Renderer renderer in gameObject.GetComponentsInChildren<Renderer>())
        {
            renderer.materials[0].color = color;
        }

        m_CurrentCol = -1;
        m_CurrentRow = -1;
    }

    public void SetCurrentRow(int value)
    {
        m_CurrentRow = value;
    }

    public void SetCurrentCol(int value)
    {
        m_CurrentCol = value;
    }

    public int GetCurrentRow()
    {
        return m_CurrentRow;
    }

    public int GetCurrentCol()
    {
        return m_CurrentCol;
    }
}
