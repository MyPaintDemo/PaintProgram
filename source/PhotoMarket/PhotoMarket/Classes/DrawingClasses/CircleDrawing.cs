﻿using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace PhotoMarket.DrawingClasses {
    public class CircleDrawing : DeepCopier {
        PointF startRatio;
        PointF endRatio;

        MainWindow parent;

        RectangleF toDraw;

        Pen pen;

        bool temp = true;

        //constructors
        public CircleDrawing(PointF _startCoord, Pen _pen, MainWindow _parent) {
            parent = _parent;
            startRatio = new PointF(parent.canvasSizeX / _startCoord.X, parent.canvasSizeY / _startCoord.Y);
            pen = _pen;
            DeepCopy(_pen);
        }
        public CircleDrawing(MainWindow _parent) {
            parent = _parent;
        }

        //gets the end point for the rectangle
        public void SetEndPoint(PointF _endPoint, bool shiftDown, bool finalPoint) {

            //gets the final position of the mouse
            if (shiftDown == false)
                endRatio = new PointF(parent.canvasSizeX / _endPoint.X, parent.canvasSizeY / _endPoint.Y);
            else {

                //if shift was pressed, then the end point distance from the start is equal in x and y
                endRatio.X = parent.canvasSizeX / _endPoint.X;
                endRatio.Y = parent.canvasSizeY / (startRatio.Y + (endRatio.X - startRatio.X));
            }

            if (finalPoint == true)
                temp = false;
            //lets the program know that the final point has been placed
        }

        //creates the rectangle used to draw the circle
        public void CreateRectangle() {

            //creates a rectangle to be used to draw the circle out
            toDraw = RectangleF.FromLTRB(parent.canvasSizeX / startRatio.X, parent.canvasSizeY / startRatio.Y, parent.canvasSizeX / endRatio.X, parent.canvasSizeY / endRatio.Y);
        }

        //draws out the circle using the rectangle made in the set end point
        public void Draw(Graphics g) {

            //sets up the rectangle each time, incase there was a change in window size
            CreateRectangle();

            if (temp == true)
                g.DrawEllipse(tempPen, toDraw);
            else
                g.DrawEllipse(pen, toDraw);
        }

        //saves the data abou this object
        public void SaveData(StreamWriter sw) {

            //saves the rectangle used to draw the circle
            sw.WriteLine(startRatio.X);
            sw.WriteLine(startRatio.Y);
            sw.WriteLine(endRatio.X);
            sw.WriteLine(endRatio.Y);

            //saves the different argb values of the pen
            sw.WriteLine(pen.Color.A);
            sw.WriteLine(pen.Color.R);
            sw.WriteLine(pen.Color.G);
            sw.WriteLine(pen.Color.B);

            //saves the width of the pen
            sw.WriteLine(pen.Width);
        }

        //loads in each value for the object from a file
        public void LoadData(StreamReader sr) {

            //sets the rectangle used to draw the circle

            startRatio.X = Convert.ToSingle(sr.ReadLine());
            startRatio.Y = Convert.ToSingle(sr.ReadLine());
            endRatio.X = Convert.ToSingle(sr.ReadLine());
            endRatio.Y = Convert.ToSingle(sr.ReadLine());

            //sets the pen's color
            pen = new Pen(
                Color.FromArgb(
                    Convert.ToInt16(sr.ReadLine()),
                    Convert.ToInt16(sr.ReadLine()),
                    Convert.ToInt16(sr.ReadLine()),
                    Convert.ToInt16(sr.ReadLine())));

            //sets the pen's width
            pen.Width = Convert.ToInt16(sr.ReadLine());

            //sets up the pen
            pen.SetLineCap(
                System.Drawing.Drawing2D.LineCap.Round,
                System.Drawing.Drawing2D.LineCap.Round,
                System.Drawing.Drawing2D.DashCap.Round);

            //lets the program know not to draw the dashed lines
            temp = false;
        }
    }
}
