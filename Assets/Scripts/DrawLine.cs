using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DrawLine : MonoBehaviour
{
    private int TotalPointInEachSide;
    private float PointDistance;
    public float StartingPointX = -5;
    public float StartingPointY = -5;
    public GameObject GlobalPointObject;
    public GameObject GlobalLineObject;
    public GameObject GlobalBoxObject;
    private int TotalPoints;
    private int TotalLines;
    private int TotalBox;
    private String STotalPointInEachSide  = "totalpointineachside";
    public PlayerManagement PlayerManagement;
    public Boolean IsPlayingWithMobile;
    [Header("For online")]
    public Boolean IsPlayingOnline;
    public GameObject GameManager;

    [Header("UI")]
    //public GameObject PlayerLabel1;
    //public GameObject PlayerLabel2;
   // public GameObject PlayerScore1;
   // public GameObject PlayerScore2;
   // public GameObject Score;
  //  public GameObject Turn;


    public List<GameObject> lines = new List<GameObject>();
    private bool hasPreviousClick = false;
    public bool isFirstPlayerTurn = true;
    public int LineDistance = 5;
    public Material NotClicked;
    public Material Clicked;
    private Point previousClickedPoint;
    private bool newBoxDrawn;
 
    public class Point
    {
        public GameObject PointObject;
        public Vector3 Position;
        public Point(GameObject pointObject, Vector3 position)
        {
            PointObject = Instantiate(pointObject);
            Position = position;
            PointObject.GetComponent<Transform>().position = position;
        }
        public Boolean IsThisPoint(GameObject pointObject)
        {
            return PointObject == pointObject;
        }
    }
    
    public class Line
    {
        public GameObject LineObject;
        public Point StartPoint;
        public Point EndPoint;
        public Boolean IsDrawn;
        public Line(GameObject lineObject, Point startPoint, Point endPoint)
        {
            this.LineObject = lineObject;
            this.StartPoint = startPoint;
            this.EndPoint = endPoint;
            IsDrawn = false;
        }
        public void DrawLine(bool isFirstPlayerTurn, PlayerManagement playerManagement)
        {
            IsDrawn = true;
            LineObject = Instantiate(LineObject);
            //Vector3 middlePoint = (StartPoint.Position + EndPoint.Position) / 2;
            Vector3 startPoint = StartPoint.Position;
            startPoint.z = 0;
            Vector3 endPoint = EndPoint.Position;
            endPoint.z = 0;
            LineObject.GetComponent<LineRenderer>().SetPositions(new Vector3[] { startPoint, endPoint });
            if (isFirstPlayerTurn)
            {
                LineObject.GetComponent<LineRenderer>().GetComponent<Renderer>().material.SetColor("_EmissionColor", playerManagement.Player1.Color);
            }
            else
            {
                LineObject.GetComponent<LineRenderer>().GetComponent<Renderer>().material.SetColor("_EmissionColor", playerManagement.Player2.Color);
            }
            LineObject.GetComponent<Animator>().Play("LineFadeAndShow");
            FindObjectOfType<AudioManager>().Play("draw_line");

        }
        public Boolean IsThisLine(Point firstPoint, Point secondPoint)
        {
            if((StartPoint == firstPoint && EndPoint == secondPoint) || (StartPoint == secondPoint && EndPoint == firstPoint))
            {
                return true;
            }
            return false;
        }
        public Boolean IsThisLine(Vector3 firstPoint, Vector3 secondPoint)
        {
            if ((StartPoint.Position == firstPoint && EndPoint.Position == secondPoint) || (StartPoint.Position == secondPoint && EndPoint.Position == firstPoint))
            {
                return true;
            }
            return false;
        }

    }
   
    public class Box
    {
        public GameObject BoxObject;
        public Vector3 Position;
        public Line TopLine;
        public Line BottomLine;
        public Line LeftLine;
        public Line RightLine;
        public Boolean IsDrawn;
        public Box(GameObject boxObject, Line topLine, Line bottomLine, Line leftLine, Line rightLine)
        {
            BoxObject = boxObject;
            TopLine = topLine;
            BottomLine = bottomLine;
            LeftLine = leftLine;
            RightLine = rightLine;
            IsDrawn = false;
            Position = new Vector3((topLine.StartPoint.Position.x + topLine.EndPoint.Position.x) / 2, (leftLine.StartPoint.Position.y + leftLine.EndPoint.Position.y) / 2);
        }
        public void DrawBox(bool isFirstPlayerTurn, PlayerManagement playerManagement)
        {
            BoxObject = Instantiate(BoxObject);
            BoxObject.GetComponent<Transform>().position = Position;
            IsDrawn = true;
            if (isFirstPlayerTurn)
            {
                BoxObject.GetComponent<Renderer>().material.SetColor("_EmissionColor", playerManagement.Player1.Color);
            }
            else
            {
                BoxObject.GetComponent<Renderer>().material.SetColor("_EmissionColor", playerManagement.Player2.Color);
            }
            FindObjectOfType<AudioManager>().Play("draw_box");

            BoxObject.GetComponent<Animator>().Play("BoxAnimation");
        }
        public Boolean IsLineOfThisBox(Line line)
        {
            return (TopLine == line) || (BottomLine == line) || (LeftLine == line) || (RightLine == line);
        }
        public Boolean CanDrawBox()
        {
            return TopLine.IsDrawn && BottomLine.IsDrawn && LeftLine.IsDrawn && RightLine.IsDrawn;
        }
        public int TotalLineDrawn()
        {
            int lineNumber = 0;
            if (TopLine.IsDrawn)
            {
                lineNumber++;
            }
            if (BottomLine.IsDrawn)
            {
                lineNumber++;
            }
            if (LeftLine.IsDrawn)
            {
                lineNumber++;
            }
            if (RightLine.IsDrawn)
            {
                lineNumber++;
            }
            return lineNumber;
        }

    }
    
    public List<List<Point>> Points = new List<List<Point>>();
    public List<List<Line>> HorizontalLines = new List<List<Line>>();
    public List<List<Line>> VerticalLines = new List<List<Line>>();
    public List<List<Box>> Boxes = new List<List<Box>>();
    void Start()
    {
        Debug.Log(PlayerPrefs.HasKey(STotalPointInEachSide)); 
        TotalPointInEachSide = PlayerPrefs.GetInt(GetComponent<Constants>().TOTALPOINTS, 6);
        Debug.Log(TotalPointInEachSide.ToString());
        PointDistance = (10*1.0f) / ((TotalPointInEachSide * 1.0f) - 1);
        GlobalBoxObject.GetComponent<Transform>().localScale = new Vector3(PointDistance - 1, PointDistance - 1);
        PlayerManagement = gameObject.GetComponent<PlayerManagement>();
        PlayerManagement.SetPlayers( IsPlayingWithMobile);
        if (!IsPlayingOnline)
        {
         //   PlayerLabel1.GetComponent<TextMeshProUGUI>().SetText(PlayerManagement.Player1.Name);
         //   PlayerLabel2.GetComponent<TextMeshProUGUI>().SetText(PlayerManagement.Player2.Name);
         //   PlayerScore1.GetComponent<TextMeshProUGUI>().SetText("0");
         //   PlayerScore2.GetComponent<TextMeshProUGUI>().SetText("0");
        //    Score.GetComponent<TextMeshProUGUI>().SetText("");
        //    Turn.GetComponent<TextMeshProUGUI>().SetText("First player turn");
        }
        
        
        TotalPoints = TotalPointInEachSide * TotalPointInEachSide;
        TotalLines = (TotalPointInEachSide * (TotalPointInEachSide - 1)) * 2;
        TotalBox = (TotalPointInEachSide - 1) * (TotalPointInEachSide - 1);
        DrawPoints();
        DrawLines();
        DrawBoxes();
    }

    public void OnlineRestart()
    {
        Points.ForEach(point => { 
            point.ForEach(p => { 
                GameObject.Destroy(p.PointObject); 
            });
        });
        HorizontalLines.ForEach(lines =>
        {
            lines.ForEach(line =>
            {
                GameObject.Destroy(line.LineObject);
            });
        });
        VerticalLines.ForEach(lines =>
        {
            lines.ForEach(line =>
            {
                GameObject.Destroy(line.LineObject);
            });
        });
        Boxes.ForEach(boxes =>
        {
            boxes.ForEach(box =>
            {
                GameObject.Destroy(box.BoxObject);
            });
        });
        DrawPoints();
        DrawLines();
        DrawBoxes();
    }

    private void DrawBoxes()
    {
        Boxes.Clear();
        for (int i = 0; i < TotalPointInEachSide - 1; i++)
        {
            List<Box> boxes = new List<Box>();
            for (int j = 0; j < TotalPointInEachSide - 1; j++)
            {
                //Line topLine, Line bottomLine, Line leftLine, Line rightLine
                Box box = new Box(GlobalBoxObject, HorizontalLines[i+1][j], HorizontalLines[i][j], VerticalLines[j][i], VerticalLines[j+1][i]);
                boxes.Add(box);
            }
            Boxes.Add(boxes);
        }
    }

    private void DrawLines()
    {
        HorizontalLines.Clear();
        VerticalLines.Clear();
        for(int i = 0; i<TotalPointInEachSide; i++)
        {
            List<Line> horizontalLines = new List<Line>();
            List<Line> verticalLines = new List<Line>();
            for (int j = 0; j < TotalPointInEachSide-1; j++)
            {
                Line horizontalLine = new Line(GlobalLineObject, Points[i][j], Points[i][j + 1]);

                horizontalLines.Add(horizontalLine);
                Line verticalLine = new Line(GlobalLineObject, Points[j][i], Points[j+1][i]);

                verticalLines.Add(verticalLine);
            }
            
            HorizontalLines.Add(horizontalLines);
            VerticalLines.Add(verticalLines);
        }
    }

    private void DrawPoints()
    {
        Points.Clear();
        float pointX = StartingPointX;
        float pointY = StartingPointY;
        for(int i = 0; i<TotalPointInEachSide; i++)
        {
            List<Point> points = new List<Point>();
 
            for (int j = 0; j < TotalPointInEachSide; j++)
            {
                Vector3 position = new Vector3(pointX, pointY, -10);
                points.Add(new Point(GlobalPointObject, position));
                pointX += PointDistance;
            }
            Points.Add(points);
            pointY += PointDistance;
            pointX = StartingPointX;
        }
    }

    public void OnResetClick()
    {
        //PlayerPrefs.SetInt("STotalPointInEachSide", 5);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    } 

    public void OnPointClick(GameObject ClickObject)
    {
        Point point = null;
        for (int i = 0; i < Points.Count; i++)
        {
            for (int j = 0; j < Points[i].Count; j++)
            {
                if (Points[i][j].IsThisPoint(ClickObject))
                {
                    point = Points[i][j];
                }
            }
        }

        if (!hasPreviousClick)
        {
            FirstClick(point);
        }
        else
        {
            if (previousClickedPoint == point)
            {
                RemoveClickedColor(previousClickedPoint);
                hasPreviousClick = false;
                previousClickedPoint = null;
                Debug.Log("Already Click in this");
            }

            else
            {
                SecondClick(point);
                Debug.Log("Line Drawn");
            }
        }
    }


    private void FirstClick(Point point)
    {
        if(previousClickedPoint != null)
        {
            RemoveClickedColor(previousClickedPoint);
        }
        previousClickedPoint = point;
        hasPreviousClick = true;
        ApplyClickedColor(point);
    }
    private void SecondClick(Point clickPoint)
    {
        //Lines.Add(new Line(GlobalLineObject, previousClickedObject.GetComponent<Transform>().position, ClickObject.GetComponent<Transform>().position, isFirstPlayerTurn, PlayerManagement));
        Line line = null;
        for (int i = 0; i < HorizontalLines.Count; i++)
        {
            for (int j = 0; j < HorizontalLines[i].Count; j++)
            {
                if(HorizontalLines[i][j].IsThisLine(previousClickedPoint, clickPoint))
                {
                    line = HorizontalLines[i][j];
                }
            }
        }
        if (line == null)
        {
            for (int i = 0; i < VerticalLines.Count; i++)
            {
                for (int j = 0; j < VerticalLines[i].Count; j++)
                {
                    if (VerticalLines[i][j].IsThisLine(previousClickedPoint, clickPoint))
                    {
                        line = VerticalLines[i][j];
                    }
                }
            }
        }
       
        line.DrawLine(isFirstPlayerTurn, PlayerManagement);
        OnSuccessfulLineDraw(line);
        isFirstPlayerTurn = !isFirstPlayerTurn;
       // RemoveClickedColor(previousClickedPoint);
        previousClickedPoint = null;
        hasPreviousClick = false;
        //StartCoroutine(CoroutineMobileSecondMove());
        if (isFirstPlayerTurn)
        {
            GameManager.GetComponent<GameManagerPlayMobile>().PlayerSwitch(isFirstPlayerTurn);
        }
        else
        {
            GameManager.GetComponent<GameManagerPlayMobile>().PlayerSwitch(isFirstPlayerTurn);
            StartCoroutine(CoroutineMobileMove());
        }
        
    }
    public IEnumerator CoroutineMobileMove()
    {
        Debug.Log("inside coroutine");
        yield return new WaitForSeconds(2f);
        ComputerTurn();
       // Turn.GetComponent<TextMeshProUGUI>().SetText("Computer turn");
    }
    public void OnlineDrawLine(Vector3 startPoint, Vector3 endPoint)
    {
        Line line = null;
        for (int i = 0; i < HorizontalLines.Count; i++)
        {
            for (int j = 0; j < HorizontalLines[i].Count; j++)
            {
                if (HorizontalLines[i][j].IsThisLine(startPoint, endPoint))
                {
                    line = HorizontalLines[i][j];
                }
            }
        }
        if (line == null)
        {
            for (int i = 0; i < VerticalLines.Count; i++)
            {
                for (int j = 0; j < VerticalLines[i].Count; j++)
                {
                    if (VerticalLines[i][j].IsThisLine(startPoint, endPoint))
                    {
                        line = VerticalLines[i][j];
                    }
                }
            }
        }
        line.DrawLine(isFirstPlayerTurn, PlayerManagement);
        OnSuccessfulLineDraw(line);
        //isFirstPlayerTurn = !isFirstPlayerTurn;
        GameManager.GetComponent<GameManager>().OnTurnComplete(startPoint, endPoint, true);
        return;
        

    }
    public void ReflectOtherNetworkPlayerTurn(Vector3 startPoint, Vector3 endPoint, bool isAdminPlayer)
    {
        Line line = null;
        for (int i = 0; i < HorizontalLines.Count; i++)
        {
            for (int j = 0; j < HorizontalLines[i].Count; j++)
            {
                if (HorizontalLines[i][j].IsThisLine(startPoint, endPoint))
                {
                    line = HorizontalLines[i][j];
                }
            }
        }
        if (line == null)
        {
            for (int i = 0; i < VerticalLines.Count; i++)
            {
                for (int j = 0; j < VerticalLines[i].Count; j++)
                {
                    if (VerticalLines[i][j].IsThisLine(startPoint, endPoint))
                    {
                        line = VerticalLines[i][j];
                    }
                }
            }
        }
        line.DrawLine(isAdminPlayer, PlayerManagement);
        for (int i = 0; i < Boxes.Count; i++)
        {
            for (int j = 0; j < Boxes[i].Count; j++)
            {
                if (Boxes[i][j].IsLineOfThisBox(line))
                {

                    if (Boxes[i][j].CanDrawBox() && !Boxes[i][j].IsDrawn)
                    {
                        Boxes[i][j].DrawBox(isAdminPlayer, PlayerManagement);
                    }
                }
            }
        }
    }
    
    /// <summary>
    /// Called when a new line is drawn by dragging one point to another
    /// </summary>
    /// <param name="line">Represt the line object</param>
    public void PlayerDrawLine(Line line)
    {
        
        if (IsPlayingOnline)
        {
            line.DrawLine(PhotonNetwork.IsMasterClient, PlayerManagement);
        }
        else
        {
            line.DrawLine(isFirstPlayerTurn, PlayerManagement);
        }
        newBoxDrawn = false;
        OnSuccessfulLineDraw(line);
        isFirstPlayerTurn = !isFirstPlayerTurn;

        if (IsPlayingOnline)
        {
            GameManager.GetComponent<GameManager>().OnTurnComplete(line.StartPoint.Position, line.EndPoint.Position, newBoxDrawn);
            return;
        }

        if (IsPlayingWithMobile)
        {
            GameManager.GetComponent<GameManagerPlayMobile>().PlayerSwitch(isFirstPlayerTurn);
            if (!isFirstPlayerTurn)
            {
                StartCoroutine(CoroutineMobileMove());
            }
            return;
        }


        
            //Turn.GetComponent<TextMeshProUGUI>().SetText("First player turn");
        GameManager.GetComponent<GameManagerPlayFriend>().PlayerSwitch(isFirstPlayerTurn);
       
        //else
        //{
        //    if (IsPlayingWithMobile)
        //    {
        //        ComputerTurn();
        //        //Turn.GetComponent<TextMeshProUGUI>().SetText("Computer turn");
        //    }
        //    else
        //    {
        //        //Turn.GetComponent<TextMeshProUGUI>().SetText("Second player turn");
        //        GameManager.GetComponent<GameManagerPlayFriend>().PlayerSwitch(isFirstPlayerTurn);
        //    }

        //}
    }
    private void OnSuccessfulLineDraw(Line line)
    {
        Boolean anyBoxDrawn = false;
        for (int i = 0; i < Boxes.Count; i++)
        {
            for (int j = 0; j < Boxes[i].Count; j++)
            {
                if (Boxes[i][j].IsLineOfThisBox(line))
                {
 
                    if (Boxes[i][j].CanDrawBox() && !Boxes[i][j].IsDrawn)
                    {
                        anyBoxDrawn = true;
                        if (IsPlayingOnline)
                        {
                            Boxes[i][j].DrawBox(PhotonNetwork.IsMasterClient, PlayerManagement);
                            GameManager.GetComponent<GameManager>().IncrementScoreForBoxDrawn();
                        }
                        else if (IsPlayingWithMobile)
                        {
                            Boxes[i][j].DrawBox(isFirstPlayerTurn, PlayerManagement);
                            GameManager.GetComponent<GameManagerPlayMobile>().OnScoreUpdate(isFirstPlayerTurn);
                        }
                        else
                        {
                            Boxes[i][j].DrawBox(isFirstPlayerTurn, PlayerManagement);
                            GameManager.GetComponent<GameManagerPlayFriend>().OnScoreUpdate(isFirstPlayerTurn);
                        }
                    }
                }
            }
        }
        if (anyBoxDrawn)
        {
            newBoxDrawn = true;
            //if(IsPlayingOnline)
            //    GameManager.GetComponent<GameManager>().IncrementScoreForBoxDrawn();
            isFirstPlayerTurn = !isFirstPlayerTurn;
        }
       // if (IsPlayingOnline) return;
        //if((PlayerManagement.Player1.Score + PlayerManagement.Player2.Score) >= TotalBox)
        //{
        //    if(PlayerManagement.Player1.Score> PlayerManagement.Player2.Score)
        //    {
        //        Score.GetComponent<TextMeshProUGUI>().SetText("Player 1 Wins");
        //    }
        //    else if (PlayerManagement.Player1.Score < PlayerManagement.Player2.Score)
        //    {
        //        Score.GetComponent<TextMeshProUGUI>().SetText("Player 2 Wins");
        //    }
        //    else
        //    {
        //        Score.GetComponent<TextMeshProUGUI>().SetText("It's a Tie");
        //    }
        //}
    }
    private void ApplyClickedColor(Point point)
    {
        point.PointObject.GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.blue);
    }
    private void RemoveClickedColor(Point point)
    {
        point.PointObject.GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.white);
    }
    private void ComputerTurn()
    {
        Boolean gotAMove = false;
        Box box= null;
        //check if can complete a box
        for (int i = 0; i < Boxes.Count; i++)
        {
            for (int j = 0; j < Boxes[i].Count; j++)
            {
                if (Boxes[i][j].TotalLineDrawn() == 3)
                {
                    gotAMove = true;
                    box = Boxes[i][j];
                    
                    break;
                }
                if (gotAMove)
                {
                    break;
                }
            }   
        }
        
        if (gotAMove)
        {
            ComputerCompleteBox(box);
            return;
        }
        //find a box that has less then 2 lines to drawn
        for (int i = 0; i < Boxes.Count; i++)
        {
            for (int j = 0; j < Boxes[i].Count; j++)
            {
                if (Boxes[i][j].TotalLineDrawn() < 2)
                {
                    gotAMove = true;
                    box = Boxes[i][j];
                    
                    break;
                }
                if (gotAMove)
                {
                    break;
                }
            }
        }
        if (gotAMove)
        {
            ComputerCompleteBox(box);
            return;
        }
        //if not go for box that has two line drawn
        for (int i = 0; i < Boxes.Count; i++)
        {
            for (int j = 0; j < Boxes[i].Count; j++)
            {
                if (Boxes[i][j].TotalLineDrawn() < 4)
                {
                    gotAMove = true;
                    box = Boxes[i][j];
                    
                    break;
                }
                if (gotAMove)
                {
                    break;
                }
            }
        }
        if (gotAMove)
        {
            ComputerCompleteBox(box);
            return;
        }

    }
    private void ComputerCompleteBox(Box box)
    {
        if (!box.LeftLine.IsDrawn)
        {
            ComputerDrawLine(box.LeftLine);
        }
        else if (!box.RightLine.IsDrawn)
        {
            ComputerDrawLine(box.RightLine);
        }
        else if (!box.TopLine.IsDrawn)
        {
            ComputerDrawLine(box.TopLine);
        }
        else
        {
            ComputerDrawLine(box.BottomLine);
        }
    }
    private void ComputerDrawLine(Line line)
    {
        FirstClick(line.StartPoint);
        SecondClick(line.EndPoint);
    }


}
