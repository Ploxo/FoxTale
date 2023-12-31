using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DrawLine : MonoBehaviour
{
    LineDrawer lineDrawer;
    public Vector3 positionOne;
    public Vector3 positionTwo;
    public Color color1;
    public Color color2;

    void Start()
    {
        lineDrawer = new LineDrawer();
    }

    void Update()
    {
        lineDrawer.DrawLineInGameView(positionOne, positionTwo, color1, color2);
    }

    //public void Draw(Color color)
    //{
    //    lineDrawer.DrawLineInGameView(positionOne, positionTwo, color);
    //}
}


public struct LineDrawer
{
    private LineRenderer lineRenderer;
    private float lineSize;

    public LineDrawer(float lineSize = 0.1f)
    {
        GameObject lineObj = new GameObject("LineObj");
        lineRenderer = lineObj.AddComponent<LineRenderer>();
        //Particles/Additive
        lineRenderer.material = new Material(Shader.Find("Hidden/Internal-Colored"));

        this.lineSize = lineSize;
    }

    private void init(float lineSize = 0.1f)
    {
        if (lineRenderer == null)
        {
            GameObject lineObj = new GameObject("LineObj");
            lineRenderer = lineObj.AddComponent<LineRenderer>();
            //Particles/Additive
            lineRenderer.material = new Material(Shader.Find("Hidden/Internal-Colored"));

            this.lineSize = lineSize;
        }
    }

    //Draws lines through the provided vertices
    public void DrawLineInGameView(Vector3 start, Vector3 end, Color color1, Color color2)
    {
        if (lineRenderer == null)
        {
            init(0.1f);
        }

        //Set color
        lineRenderer.startColor = color1;
        lineRenderer.endColor = color2;

        //Set width
        lineRenderer.startWidth = lineSize;
        lineRenderer.endWidth = lineSize;

        //Set line count which is 2
        lineRenderer.positionCount = 2;

        //Set the postion of both two lines
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
    }

    public void Destroy()
    {
        if (lineRenderer != null)
        {
            UnityEngine.Object.Destroy(lineRenderer.gameObject);
        }
    }
}
