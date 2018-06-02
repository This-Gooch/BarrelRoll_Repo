//////////////////////////////////////////
//	Create by Leonard Marineau-Quintal  //
//		www.leoquintgames.com			//
//////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class BezierCurve : MonoBehaviour {

    ////////////////////////////////
    ///			Constants		 ///
    ////////////////////////////////
    private const int POINTS_PER_BEZIER = 50;
    ////////////////////////////////
    ///			Statics			 ///
    ////////////////////////////////

    ////////////////////////////////
    ///	  Serialized In Editor	 ///
    ////////////////////////////////
    [SerializeField]
    protected Color _CurveColor = Color.red;
    [SerializeField]
    protected List<Vector3> _Points = new List<Vector3>() { Vector3.zero, Vector3.up , new Vector3(1f,1f,0f) , Vector3.right };
    [SerializeField]
    protected float _Width = 5f;
    [SerializeField]
    private float _HandleSize = 0.5f;
    [SerializeField]
    private bool _ForceSmoothPath = true;
    [SerializeField]
    private bool _UseLocalMatrix = true;
    [SerializeField]
    [InEditorReadOnly]
    private List<Vector3> m_Path = new List<Vector3>();
    [SerializeField]
    [InEditorReadOnly]
    protected int m_NumberOfBezierInPath = 1;
    [SerializeField]
    [InEditorReadOnly]
    private float m_TotalLength = -1f;
    [SerializeField]
    [InEditorReadOnly]
    private List<float> m_Lengths = new List<float>();

    ////////////////////////////////
    ///			Public			 ///
    ////////////////////////////////
    [HideInInspector]
    public bool IsLockedDisplay = false;
    ////////////////////////////////
    ///			Protected		 ///
    ////////////////////////////////

    ////////////////////////////////
    ///			Private			 ///
    ////////////////////////////////
#if UNITY_EDITOR
    private bool _PreviewOn = false;
#endif
    ///proterties
    ///
    public bool UseLocalMatrix
    {
        get { return _UseLocalMatrix; }
    }

    public List<Vector3> Points
    {
        get { return _Points;  }
        set { _Points = value; }
    }

    public float Width
    {
        get { return _Width; }
    }

    public Color CurveColor
    {
        get { return _CurveColor; }
    }

    public float HandleSize
    {
        get { return _HandleSize; }
    }

    public int NumberOfBezier
    {
        get { return m_NumberOfBezierInPath;  }
        set { m_NumberOfBezierInPath = value; }
    }

    public float Length
    {
        get
        {
            if (m_TotalLength == -1f)
            {
                Debug.Log("Bezier length not calculated. Use PreCalculatePoints to calculate.");
            }
            return m_TotalLength;
        }
    }

    public bool ForceSmoothPath
    {
        get { return _ForceSmoothPath; }
    }
#if UNITY_EDITOR
    public bool PreviewOn
    {
        get { return _PreviewOn; }
        set
        {
            if (_PreviewOn != value)
            {
                UnityEditor.SceneView.RepaintAll();
            }
            _PreviewOn = value;
        }
    }
#endif
    #region Unity API

    #endregion

    #region Public API
    /// <summary>
    /// Add an extra curve.
    /// </summary>
    public void Extend()
    {
        _Points.Add( /*Rotate the first point so its handle is 180 degree from the opposite one.*/
            new Vector3(_Points[_Points.Count - 2].x, 
                        _Points[_Points.Count - 2].y, 
                        _Points[_Points.Count - 2].z)
                        .RotatePoint(_Points[_Points.Count - 1], Mathf.PI/*180Degrees*/));
        _Points.Add(new Vector3(_Points[_Points.Count - 1].x + 1f, _Points[_Points.Count - 1].y + 1f, _Points[_Points.Count - 1].z));
        _Points.Add(new Vector3(_Points[_Points.Count - 1].x + 1f, _Points[_Points.Count - 1].y + 1f, _Points[_Points.Count - 1].z));
        ++m_NumberOfBezierInPath;
#if UNITY_EDITOR
        UnityEditor.SceneView.RepaintAll();
#endif
    }

    public List<Vector3> InsertControlPoint(Vector3 screenPoint)
    {
        PreCalculatePoints();
        screenPoint.z = 0;
        List<Vector3> path = new List<Vector3>(_Points);
        Vector4 results = GetNearestPoint(screenPoint);        
        Vector3 newControlPoint = new Vector3(results.x, results.y, results.z);
        float ratio = GetRatio(newControlPoint);
        path.Insert((int)results.w, newControlPoint);//control point
        if (ratio < 0.975f)
        {
            Vector3 point2 = GetPointOnPath(ratio + 0.02f);
            Vector3 tangent1 = newControlPoint - point2;
            tangent1 = new Vector3(tangent1.y, -tangent1.x, tangent1.z);
            path.Insert((int)results.w, newControlPoint + (tangent1.normalized * HandleSize * 5f));
            path.Insert((int)results.w + 2, newControlPoint + (-tangent1.normalized * HandleSize * 5f));
        }
        else
        {
            Vector3 point2 = GetPointOnPath(ratio - 0.02f);
            Vector3 tangent1 = newControlPoint - point2;
            tangent1 = new Vector3(tangent1.y, -tangent1.x, tangent1.z);
            path.Insert((int)results.w, newControlPoint +( tangent1.normalized * HandleSize * 5f));
            path.Insert((int)results.w + 2, newControlPoint +( - tangent1.normalized * HandleSize * 5f));
        }
        return path;
    }

    public List<Vector3> RemoveControlPoint(Vector3 screenPoint)
    {
        PreCalculatePoints();
        screenPoint.z = 0;
        List<Vector3> path = new List<Vector3>(_Points);
        int nearestControlPoint = GetNearestControlPoint(screenPoint);
        if (nearestControlPoint == -1 || path.Count <= 4)
        {
            return path;
        }

        if (nearestControlPoint == 0)
        {
            path.RemoveAt(0);
            path.RemoveAt(0);
            path.RemoveAt(0);
        }
        else if (nearestControlPoint + 1 == path.Count)
        {
            path.RemoveAt(path.Count - 1);
            path.RemoveAt(path.Count - 1);
            path.RemoveAt(path.Count - 1);
        }
        else
        {
            path.RemoveAt(nearestControlPoint - 1);
            path.RemoveAt(nearestControlPoint - 1);
            path.RemoveAt(nearestControlPoint - 1);
        }

        return path;
    }
    /// <summary>
    /// Remove a curve
    /// </summary>
    public void Shorten()
    {
        if (m_NumberOfBezierInPath > 1)
        {
            _Points.RemoveAt(_Points.Count - 1);
            _Points.RemoveAt(_Points.Count - 1);
            _Points.RemoveAt(_Points.Count - 1);
            --m_NumberOfBezierInPath;
#if UNITY_EDITOR
            UnityEditor.SceneView.RepaintAll();
#endif
        }
    }

    [ContextMenu("PreCalculatePoints")]
    public void PreCalculatePoints()
    {
        m_Path.Clear();
        m_Path.Capacity = m_NumberOfBezierInPath * POINTS_PER_BEZIER;
        m_Lengths.Clear();
        m_Lengths.Capacity = m_NumberOfBezierInPath;
        m_TotalLength = 0f;
        for (int i = 0; i < m_NumberOfBezierInPath; ++i)
        {
            for (int j = 0; j < POINTS_PER_BEZIER; ++j)
            {
                m_Path.Add(GetPointOnCurve(((float)j / (float)POINTS_PER_BEZIER), i));
                if (i + 1 == m_NumberOfBezierInPath && j + 1 == POINTS_PER_BEZIER)//Add an extra pts to cap the curve.
                {
                    m_Path.Add(GetPointOnCurve(((float)(j+1) / (float)POINTS_PER_BEZIER), i));
                }                
            }
            //Calculate the length of each bezier.
            float length = CalculateBezierLength(i);
            m_Lengths.Add(length);
            m_TotalLength += length;
        }        
    }
    /// <summary>
    /// Vector4 (w) is used to add the point index where to insert new controls.
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public Vector4 GetNearestPoint(Vector3 position)
    {
        float nearest = float.MaxValue;
        int index = -1;
        int pointIndex = 0;
        for (int i = 0; i < m_NumberOfBezierInPath; ++i)
        {
            for (int j = 0; j < POINTS_PER_BEZIER; ++j)
            {
                Vector3 point = m_Path[(i * POINTS_PER_BEZIER) + j];
                if (_UseLocalMatrix)
                {
                    point = transform.TransformPoint(point); ;
                }
                float distance = Vector3.Distance(position, point);
                if (distance < nearest)
                {
                    nearest = distance;
                    index =( i * POINTS_PER_BEZIER) +j;
                    pointIndex = i * 3;
                }
            }            
        }
        if (index != -1)
        {
            return new Vector4( m_Path[index].x, m_Path[index].y, m_Path[index].z, pointIndex + 2);
        }
        return Vector4.one;

    }

    public int GetNearestControlPoint(Vector3 position)
    {
        float nearest = float.MaxValue;
        int index = -1;
        for (int i = 0; i < _Points.Count; ++i)
        {
            if (i % 3 == 0)//control point
            {                
                float distance = Vector3.Distance(position, _Points[i]);
                if (distance < nearest)
                {
                    nearest = distance;
                    index = i;
                }
            }
        }
       return index;
    }

    public float GetRatio(Vector3 position)
    {
        float nearest = float.MaxValue;
        float ratio = -1;
        for (int i = 0; i < m_NumberOfBezierInPath; ++i)
        {
            for (int j = 0; j < POINTS_PER_BEZIER; ++j)
            {
                float distance = Vector3.Distance(position, m_Path[(i * POINTS_PER_BEZIER) + j]);
                if (distance < nearest)
                {
                    nearest = distance;
                    ratio = ((float)((i * POINTS_PER_BEZIER) + j)/(float)(m_NumberOfBezierInPath * POINTS_PER_BEZIER));
                }
            }
        }
        if (ratio != -1)
        {
            return ratio;
        }
        return 0f;
    }

    public Vector3 GetNormalOnPath(float ratio)
    {
        Vector3 point1 = GetPointOnPath(ratio);
        Vector3 point2 = Vector3.zero;
        if (ratio < 0.975f)
        {
            point2 = GetPointOnPath(ratio + 0.025f);
        }
        else
        {
            point2 = GetPointOnPath(ratio - 0.025f);
        }

        Vector3 direction = point1 - point2;

        return new Vector3(direction.y, -direction.x, direction.z);
    }
    /// <summary>
    /// Get the nearest point on path
    /// </summary>
    /// <param name="ratio"></param>
    /// <returns></returns>
    public Vector3 GetPathPoint(float ratio)
    {
        float targetDistance = m_TotalLength * ratio;
        float currentDistance = 0f;
        for (int i = 0; i < m_NumberOfBezierInPath; ++i)
        {
            if (currentDistance + m_Lengths[i] >= targetDistance)
            {
                return m_Path[(POINTS_PER_BEZIER * i) + (int)((targetDistance - currentDistance) / m_Lengths[i] * (float)POINTS_PER_BEZIER)];
            }
            currentDistance += m_Lengths[i];
        }

        return m_Path[m_Path.Count - 1];
    }
    /// <summary>
    /// Calculate the exact point on path
    /// </summary>
    /// <param name="ratio"></param>
    /// <returns></returns>
    public Vector3 GetPointOnPath(float ratio)
    {
        Vector3 point = Vector3.zero;
        float targetDistance = m_TotalLength * ratio;
        float currentDistance = 0f;
        for (int i = 0; i < m_NumberOfBezierInPath; ++i)
        {
            if (currentDistance + m_Lengths[i] >= targetDistance)
            {
                float distanceOnCurve = targetDistance - currentDistance;
                return GetPointOnCurve(distanceOnCurve / m_Lengths[i], i);
            }
            currentDistance += m_Lengths[i];
        }

        return point;
    }
#endregion

#region Protect

#endregion

#region Private
    private Vector3 GetPointOnCurve(float ratio, int bezier = 0)
    {
        float u = 1f - ratio;
        float t2 = ratio * ratio;
        float u2 = u * u;
        float u3 = u2 * u;
        float t3 = t2 * ratio;

        Vector3 point = (u3) * _Points[0 + (bezier * 3)] +
                        (3f * u2 * ratio) * _Points[1 + (bezier * 3)] +
                        (3f * u * t2) * _Points[2 + (bezier * 3)] +
                        (t3) * _Points[3 + (bezier * 3)];
        return point;
    }

    private float CalculateBezierLength(int bezier = 0)
    {
        float length = 0f;
        for (int i = 0; i < POINTS_PER_BEZIER-1; ++i)
        {
            length += Vector3.Distance(m_Path[i + (bezier * POINTS_PER_BEZIER)], m_Path[i + 1 + (bezier * POINTS_PER_BEZIER)]);
        }
        return length;
    }
#endregion
}

