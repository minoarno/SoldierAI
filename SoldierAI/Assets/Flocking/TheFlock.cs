using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheFlock : MonoBehaviour
{
    [SerializeField]
    GameObject m_AgentTemplate = null;
    [SerializeField]
    int m_AmountOfAgents = 20;
    [SerializeField]
    List<SteeringAgent> m_Agents = null;

    [SerializeField]
    [Range(0f, 1f)]
    float m_Weight;

    [SerializeField]
    int m_RowsLegion = 4;

    int m_CollumsLegion = 5;

    [SerializeField]
    float m_Offset = 10f;

    float m_WidthLegion = 200;
    float m_LengthLegion = 200;

    static Vector3 m_Hitpoint;

    List<List<bool>> m_LegionLayout;
    // Start is called before the first frame update
    void Start()
    {
        m_CollumsLegion = m_AmountOfAgents / m_RowsLegion;

        m_Agents = new List<SteeringAgent>();
        for (int i = 0; i < m_AmountOfAgents; i++)
        {
            SteeringAgent agent = Instantiate(m_AgentTemplate, transform.position, m_AgentTemplate.transform.rotation, transform).GetComponent<SteeringAgent>();
            if (agent)
            {
                m_Agents.Add(agent);
            }
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

        m_WidthLegion = m_CollumsLegion * m_Offset;
        m_LengthLegion = m_LengthLegion * m_Offset;
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
            FillInTheGrid(hit.point);
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

    public void FillInTheGrid(Vector3 hitpoint)
    {
        Vector3 vec = hitpoint + new Vector3(m_WidthLegion / 2, 0, m_LengthLegion / 2);

        m_Agents.Sort(SortByDistance);

        for (int i = 0; i < m_AmountOfAgents; i++)
        {
            int closestRow = 0;
            int closestCol = 0;
            float closestDistance = float.MaxValue;
            for (int r = 0; r < m_RowsLegion; r++)
            {
                for (int c = 0; c < m_CollumsLegion; c++)
                {
                    if(m_LegionLayout[r][c] == false)
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
            m_LegionLayout[closestRow][closestCol] = true;
            
            Vector3 target = vec + new Vector3(closestRow * m_Offset, 0, closestCol * m_Offset);

            SeekBehavior seek = new SeekBehavior();
            seek.SetTarget(target);
            m_Agents[i].SetSteeringBehavior(seek);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }

    static int SortByDistance(SteeringAgent a1, SteeringAgent a2)
    {
        float distanceToPointA1 = Vector3.Distance(m_Hitpoint, a1.GetPosition());
        float distanceToPointA2 = Vector3.Distance(m_Hitpoint, a1.GetPosition());
        return distanceToPointA1.CompareTo(distanceToPointA2);
    }
}
