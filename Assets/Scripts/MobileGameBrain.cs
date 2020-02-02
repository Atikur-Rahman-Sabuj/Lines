using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileGameBrain : MonoBehaviour
{


    public void ComputerTurnMedium(List<List<DrawLine.Box>> Boxes)
    {
        bool gotAMove = false;
        DrawLine.Box box = null;
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
            GetComponent<DrawLine>().ComputerCompleteBox(box);
            return;
        }
        //find a box that has other then 2 lines to drawn
        for (int i = 0; i < Boxes.Count; i++)
        {
            for (int j = 0; j < Boxes[i].Count; j++)
            {
                if (Boxes[i][j].TotalLineDrawn() != 2)
                {
                    // gotAMove = true;
                    box = Boxes[i][j];
                    if (!box.LeftLine.IsDrawn)
                    {
                        if (j != 0 && Boxes[i][j - 1].TotalLineDrawn() != 2)
                        {
                            GetComponent<DrawLine>().ComputerDrawLine(box.LeftLine);
                            return;
                        }
                        else if (j == 0)
                        {
                            GetComponent<DrawLine>().ComputerDrawLine(box.LeftLine);
                            return;
                        }
                    }
                    if (!box.RightLine.IsDrawn)
                    {
                        if (j != Boxes.Count - 1 && Boxes[i][j + 1].TotalLineDrawn() != 2)
                        {
                            GetComponent<DrawLine>().ComputerDrawLine(box.RightLine);
                            return;
                        }
                        else if (j == Boxes.Count - 1)
                        {
                            GetComponent<DrawLine>().ComputerDrawLine(box.RightLine);
                            return;
                        }
                    }
                    if(!box.TopLine.IsDrawn)
                    {
                        if (i != Boxes.Count - 1 && Boxes[i + 1][j].TotalLineDrawn() != 2)
                        {
                            GetComponent<DrawLine>().ComputerDrawLine(box.TopLine);
                            return;
                        }
                        else if (i == Boxes.Count - 1)
                        {
                            GetComponent<DrawLine>().ComputerDrawLine(box.TopLine);
                            return;
                        }
                    }
                    if (!box.BottomLine.IsDrawn)
                    {
                        if (i != 0 && Boxes[i - 1][j].TotalLineDrawn() != 2)
                        {
                            GetComponent<DrawLine>().ComputerDrawLine(box.BottomLine);
                            return;
                        }
                        else if (i == 0)
                        {
                            GetComponent<DrawLine>().ComputerDrawLine(box.BottomLine);
                            return;
                        }
                    }

                }
                if (gotAMove)
                {
                    break;
                }
            }
        }
        if (gotAMove)
        {
            GetComponent<DrawLine>().ComputerCompleteBox(box);
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
            GetComponent<DrawLine>().ComputerCompleteBox(box);
            return;
        }
    }
}
