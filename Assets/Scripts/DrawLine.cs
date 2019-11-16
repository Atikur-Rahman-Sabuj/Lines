using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLine : MonoBehaviour
{
    public GameObject lineObject;
    public List<GameObject> lines = new List<GameObject>();
    private bool hasPreviousClick = false;
    private Vector3 previousPosition;

    public void OnStartClick()
    {
        Debug.Log("Click on draw line");
        LineRenderer lineRenderer = lineObject.GetComponent<LineRenderer>();
        lineRenderer.SetPosition(1, new Vector3(5, 0));
        lines.Add(Instantiate(lineObject));
        lines[0].GetComponent<LineRenderer>().SetPositions(new Vector3[] { new Vector3(5, 0) , new Vector3(5, 5)});
    }

    public void DrawNewLine(Vector3 startPosition, Vector3 endPosition)
    {
        lines.Add(Instantiate(lineObject));
        lines[lines.Count-1].GetComponent<LineRenderer>().SetPositions(new Vector3[] { startPosition, endPosition });
    }
    public void OnPointClick(GameObject point)
    {
        if (!hasPreviousClick)
        {
            previousPosition = point.GetComponent<Transform>().position;
            hasPreviousClick = true;
        }
        else
        {
            DrawNewLine(previousPosition, point.GetComponent<Transform>().position);
            hasPreviousClick = false;

        }
    }
}
