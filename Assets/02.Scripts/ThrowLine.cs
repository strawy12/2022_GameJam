using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ThrowLine : MonoBehaviour
{
    private LineRenderer _lineRenderer;

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    public void DrawGuideLine(Rigidbody2D rigid, Vector2 pos, Vector2 velocity, int steps)
    {
        Vector2[] posList = CalcDrawPoint(rigid, pos, velocity, steps);

        Vector3[] posVector3List = posList.Select(p => (Vector3)p).ToArray();
        _lineRenderer.positionCount = posList.Length;
        _lineRenderer.SetPositions(posVector3List);
    }

    private Vector2[] CalcDrawPoint(Rigidbody2D rigid, Vector2 pos, Vector2 velocity, int steps)
    {
        Vector2[] results = new Vector2[steps];
        float timeStep = Time.fixedDeltaTime / Physics2D.velocityIterations;
        Vector2 gravityAccel = Physics2D.gravity * rigid.gravityScale* timeStep * timeStep;
        

        float drag = 1f - timeStep * rigid.drag;
        Vector2 moveStep = velocity * timeStep;

        for (int i = 0; i < steps; i++)
        {
            moveStep += gravityAccel;
            moveStep *= drag;
            pos += moveStep;

            results[i] = pos;
        }

        return results;
    }

    
}
