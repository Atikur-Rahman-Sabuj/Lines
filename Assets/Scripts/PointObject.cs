using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointObject : MonoBehaviour
{
    private GameObject LineRenderer;
    private GameObject Script;
    private bool IsDraging;
    private DrawLine.Line lineToDraw;
    public GameObject GameManager;
    //public DrawLine;

    void Start()
    {
        
        LineRenderer = GameObject.Find("line_for_drag");
        GameManager = GameObject.Find("GameManager");
        IsDraging = false;
        lineToDraw = null;

    }

    private void OnMouseDown()
    {
        //Debug.Log(gameObject.GetComponent<Transform>().position.ToString());
        //GameObject script = GameObject.Find("script");
        //script.GetComponent<DrawLine>().OnPointClick(gameObject);
    }
    void OnMouseDrag()
    {
        if (GameManager != null && GameManager.GetComponent<GameManager>().Turn == false)
        {
            return;
        }
        IsDraging = true;
        lineToDraw = null;
        Script = GameObject.Find("script");
        if (Script.GetComponent<DrawLine>().IsPlayingOnline)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                LineRenderer.GetComponent<LineRenderer>().GetComponent<Renderer>().material.SetColor("_EmissionColor", Script.GetComponent<DrawLine>().PlayerManagement.Player1.Color);
            }
            else
            {
                LineRenderer.GetComponent<LineRenderer>().GetComponent<Renderer>().material.SetColor("_EmissionColor", Script.GetComponent<DrawLine>().PlayerManagement.Player2.Color);
            }
        }
        else
        {
            if (Script.GetComponent<DrawLine>().isFirstPlayerTurn)
            {
                LineRenderer.GetComponent<LineRenderer>().GetComponent<Renderer>().material.SetColor("_EmissionColor", Script.GetComponent<DrawLine>().PlayerManagement.Player1.Color);
            }
            else
            {
                LineRenderer.GetComponent<LineRenderer>().GetComponent<Renderer>().material.SetColor("_EmissionColor", Script.GetComponent<DrawLine>().PlayerManagement.Player2.Color);
            }
        }
        
        Script = GameObject.Find("script");
        Vector3 endPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        endPosition.z = 0;
        List<List<DrawLine.Line>> horizontalLines = Script.GetComponent<DrawLine>().HorizontalLines;
        List<List<DrawLine.Line>> verticalLines = Script.GetComponent<DrawLine>().VerticalLines;
        List<List<DrawLine.Point>> allPoints = Script.GetComponent<DrawLine>().Points;
        DrawLine.Point point = null;
        for (int i = 0; i < allPoints.Count; i++)
        {
            for (int j = 0; j < allPoints[i].Count; j++)
            {
                if(allPoints[i][j].PointObject == gameObject)
                {
                    point = allPoints[i][j];
                   // Debug.Log("got object" + i +" "+j);
                    break;
                }
            }
        }
        List<DrawLine.Line> adjacentLines = new List<DrawLine.Line>();
        for (int i = 0; i < horizontalLines.Count; i++)
        {
            for (int j = 0; j < horizontalLines[i].Count; j++)
            {
                if ((horizontalLines[i][j].StartPoint == point) || (horizontalLines[i][j].EndPoint == point))
                {
                    adjacentLines.Add(horizontalLines[i][j]);
                }
            }
        }
        for (int i = 0; i < verticalLines.Count; i++)
        {
            for (int j = 0; j < verticalLines[i].Count; j++)
            {
                if ((verticalLines[i][j].StartPoint == point) || (verticalLines[i][j].EndPoint == point))
                {
                    adjacentLines.Add(verticalLines[i][j]);
                }
            }
        }
        Debug.Log("Adjacent Lines : "+ adjacentLines.Count);
        bool allLineDrawn = true;
        for (int i = 0; i < adjacentLines.Count; i++)
        {
            if (!adjacentLines[i].IsDrawn)
            {
                Debug.Log("Inside here : ");
                allLineDrawn = false;
                DrawLine.Point otherPoint = adjacentLines[i].StartPoint;
                if(otherPoint == point)
                {
                    otherPoint = adjacentLines[i].EndPoint;       
                }
                Vector3 position = otherPoint.Position;
                position.z = 0;
                //Debug.Log(otherPoint.PointObject.GetComponent<Transform>().position + "   " + endPosition);
                //Debug.Log(Vector3.Distance(otherPoint.PointObject.GetComponent<Transform>().position, endPosition));
                if (Vector3.Distance(position, endPosition) < 2)
                {
                
                    Debug.Log("Inside here : ");
                    lineToDraw = adjacentLines[i];
                    endPosition = position;
                    break;
                }
            }
        }
        if (!allLineDrawn)
        {
            Vector3 startPosition = gameObject.GetComponent<Transform>().position;
            startPosition.z = 0;
            LineRenderer.GetComponent<LineRenderer>().SetPositions(new Vector3[] { startPosition, endPosition });
        }
        else
        {
            LineRenderer.GetComponent<LineRenderer>().SetPositions(new Vector3[] { new Vector3(500,500), new Vector3(500, 500) });
        }

    }
    private void OnMouseUp()
    {
        if (GameManager != null && GameManager.GetComponent<GameManager>().Turn == false)
        {
            return;
        }

        if (IsDraging)
        {
            if (lineToDraw != null)
            {
                Script.GetComponent<DrawLine>().PlayerDrawLine(lineToDraw);

            }
        }
        LineRenderer.GetComponent<LineRenderer>().SetPositions(new Vector3[] { new Vector3(500, 500), new Vector3(500, 500) });
        IsDraging = false;
        lineToDraw = null;
    }
}
