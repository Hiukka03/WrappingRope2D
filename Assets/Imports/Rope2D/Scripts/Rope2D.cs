using System;
using System.Collections.Generic;
using Vectrosity;
using WrappingRopeLibrary.Scripts;

namespace Gamee_Hiukka.WrappingRope2D

{
    using UnityEngine;

    public class Rope2D : MonoBehaviour
    {
        public float capLineWidth;
        public VectorLine line;
        public Texture2D lineTexture;
        public Color lineColor;
        public Color lineColorUpdate;
        public Rope rope;
        public float distanceRope;
        public List<Vector3> WayPoints => wayPoints;
        public VectorLine Line => line;
        public float Length
        {
            get 
            {
                float length = 0f;
                for(int i = 1; i < wayPoints.Count; i++) 
                {
                    length += (wayPoints[i] - wayPoints[i - 1]).magnitude;
                }
                return length;
            }
        }

        bool rending;
        List<Vector3> wayPoints = new List<Vector3>();
        private bool _oneFrame;
        private Color lineColorCurrent = new Color();

        public void CreateLine(string name)
        {
            float aspectRatio = Screen.width / 1080f;
            line = new VectorLine(name,
                new List<Vector3>(),
                lineTexture,
                capLineWidth * aspectRatio,
                LineType.Continuous,
                Joins.Weld);
            line.textureScale = 1f;
            line.drawDepth = 2;
            lineColorCurrent = lineColor;
            line.Draw();
            Calculate();
        }

        public void SetCameraCanvasLine(Camera cam = null) 
        {
            if (cam == null) cam = Camera.main;
            VectorLine.SetCanvasCamera(cam);
        }
        public void SetBackEndPositionStart(Vector3 pos) 
        {
            rope.BackEnd.transform.position = pos;
        }

        public void SetFrontEndPositionStart(Vector3 pos) 
        {
            rope.FrontEnd.transform.position = pos;
        }

        private void Update()
        {
            if (_oneFrame)
            {
                _oneFrame = false;
                rope.enabled = false;
            }

            if (!rending) return;
            RenderLine();
        }

        public void EnableRender() { rending = true; }
        public void HideRender() { rending = false; }

        public void RenderLine()
        {
            if (line == null) return;
            Calculate();
            line.textureOffset -= distanceRope;
            line.points3 = wayPoints;
            line.color = lineColorCurrent;
            line?.Draw();
        }

        public void UpdateLineCollor() 
        {
            lineColorCurrent = lineColorUpdate;
        }
        public void DefautLineCollor()
        {
            lineColorCurrent = lineColor;
        }
        private void AddPiece(Piece piece)
        {
            if (!wayPoints.Contains(piece.PrevFrontBandPoint)) wayPoints.Add(piece.PrevFrontBandPoint);
            if (!wayPoints.Contains(piece.PrevBackBandPoint)) wayPoints.Add(piece.PrevBackBandPoint);
        }

        public void Calculate()
        {
            wayPoints.Clear();
            if (transform == null) return;

            var rootPiece = transform.GetChild(0).GetComponent<Piece>();
            AddPiece(rootPiece);

            for (int i = 1; i < transform.childCount; i++)
            {
                var nextPiece = rootPiece.BackPiece;
                if (nextPiece.FrontPiece == rootPiece)
                {
                    rootPiece = nextPiece;
                    AddPiece(rootPiece);
                }
            }
        }

        public Rope2D()
        {
            capLineWidth = 20f;
            _oneFrame = false;
            wayPoints = new List<Vector3>();
        }
    }
}