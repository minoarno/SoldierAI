using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheFlock : MonoBehaviour
{
    [SerializeField]
    GameObject m_AgentTemplate = null;
    [SerializeField]
    int m_AmountOfAgents = 0;
    [SerializeField]
    List<LegionAgent> m_Agents = null;

    [SerializeField]
    [Range(0f, 1f)]
    float m_Weight;

    [SerializeField]
    int m_RowsLegion = 4;

    int m_CollumsLegion = 5;

    [SerializeField]
    float m_Offset = 10f;

    static Vector3 m_Hitpoint;

    List<List<bool>> m_LegionLayout;
    bool m_HasFormed = false;

    // Start is called before the first frame update
    void Start()
    {
        m_CollumsLegion = m_AmountOfAgents / m_RowsLegion;

        m_Agents = new List<LegionAgent>();
        for (int i = 0; i < m_AmountOfAgents; i++)
        {
            LegionAgent agent = Instantiate(m_AgentTemplate, transform.position, m_AgentTemplate.transform.rotation, transform).GetComponent<LegionAgent>();
            if (agent)
            {
                m_Agents.Add(agent);
                agent.gameObject.name = i.ToString();
            }
        }

        if (m_AmountOfAgents > m_RowsLegion * m_CollumsLegion)
        {
            m_CollumsLegion++;
        }

        m_LegionLayout = new List<List<bool>>();
        for (int r = 0; r < m_RowsLegion; r++)
        {
            List<bool> list = new List<bool>();
            for (int c = 0; c < m_CollumsLegion; c++)
            {
                list.Add(false);
            }
            m_LegionLayout.Add(list);
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 1000.0f))
            {
                Debug.Log("You selected the " + hit.transform.name); // ensure you picked right object
            }
            m_Hitpoint = hit.point;
            ClearLegionLayout();
            FillInTheGrid();
        }
    }

    public void ClearLegionLayout()
    {
        for (int r = 0; r < m_RowsLegion; r++)
        {
            for (int c = 0; c < m_CollumsLegion; c++)
            {
                m_LegionLayout[r][c] = false;
            }
        }
    }

    public void FillInTheGrid()
    {
        Vector3 halfTheSize = new Vector3(m_Offset * (m_CollumsLegion * 0.5f), 0, m_Offset * (m_RowsLegion * 0.5f));
        Vector3 vec = m_Hitpoint - halfTheSize;
        
        m_Agents.Sort(SortByDistance);

        for (int i = 0; i < m_AmountOfAgents; i++)
        {
            int closestRow = 0;
            int closestCol = 0;
            float closestDistance = float.MaxValue;

            if (m_Agents[i].GetCurrentRow() != -1 && m_Agents[i].GetCurrentCol() != -1)
            {
                if (m_LegionLayout[m_Agents[i].GetCurrentRow()][m_Agents[i].GetCurrentCol()] == false)
                {
                    closestRow = m_Agents[i].GetCurrentRow();
                    closestCol = m_Agents[i].GetCurrentCol();
                    closestDistance = Vector3.Distance(m_Agents[i].GetPosition(), new Vector3(vec.x + closestCol * m_Offset, vec.y, vec.z + closestRow * m_Offset));
                }
            }

            if (m_HasFormed == false)
            {
                for (int r = 0; r < m_RowsLegion; r++)
                {
                    for (int c = 0; c < m_CollumsLegion; c++)
                    {
                        if (m_LegionLayout[r][c] == false)
                        {
                            float distanceToGridPoint = Vector3.Distance(m_Agents[i].GetPosition(), new Vector3(vec.x + c * m_Offset, vec.y, vec.z + r * m_Offset));
                            if (closestDistance > distanceToGridPoint)
                            {
                                closestDistance = distanceToGridPoint;
                                closestCol = c;
                                closestRow = r;
                            }
                        }
                    }
                }
            }
            
            m_LegionLayout[closestRow][closestCol] = true;
            m_Agents[i].SetCurrentCol(closestCol);
            m_Agents[i].SetCurrentRow(closestRow);

            Vector3 target = vec + new Vector3(closestRow * m_Offset, 0, closestCol * m_Offset);

            SeekBehavior seek = new SeekBehavior();
            seek.SetTarget(target);
            m_Agents[i].SetSteeringBehavior(seek);       
        }

        if (!m_HasFormed) m_HasFormed = true;
    }

    void OnDrawGizmos()
    {
        Vector3 vec = m_Hitpoint - new Vector3(m_Offset * (m_RowsLegion * 0.5f), 0, m_Offset * (m_CollumsLegion * 0.5f));

        if (m_Agents.Count > 1)
        {
            for (int i = 0; i < m_AmountOfAgents; i++)
            {
                Gizmos.color = m_Agents[i].GetColor();
                Gizmos.DrawLine(m_Agents[i].GetPosition(), m_Agents[i].GetTarget());
            }
        }

    }

    static int SortByDistance(SteeringAgent a1, SteeringAgent a2)
    {
        float distanceToPointA1 = Vector3.Distance(m_Hitpoint, a1.GetPosition());
        float distanceToPointA2 = Vector3.Distance(m_Hitpoint, a2.GetPosition());
        return distanceToPointA1.CompareTo(distanceToPointA2) * -1;
        //return distanceToPointA1.CompareTo(distanceToPointA2);
    }
}
