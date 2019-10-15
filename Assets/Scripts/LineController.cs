using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineController : MonoBehaviour
{
    public LayerMask mask;

    LineRenderer line;
    Vector3 previousPoint = new Vector3();
    // Start is called before the first frame update
    void Start()
    {
        line = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (line.positionCount < 2) return;
        RaycastHit hit;
        if (Physics.Linecast(line.GetPosition(line.positionCount-2), line.GetPosition(line.positionCount-1), out hit, mask))
        {
            line.positionCount = 1;
            Debug.Log("Line Snapped");
        }
    }
}
