using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BezierCurve))]
public class BezierDrawer : Editor
{
    private Vector3 controlPointDelta = Vector3.zero;
    private Vector3 directionToControl = Vector3.zero;
    private Color defaultColor = Color.grey;
    private Color previewColor = new Color(0.2f, 0.2f, 0.2f);
    public static List<BezierCurve> ActiveBezier;
    private BezierCurve curve;
    private bool doubleClickReset = true;
    private Color UNSELECTED_COLOR = Color.gray;
    private bool m_IsSelected = true;

    protected void Awake()
    {
        if (ActiveBezier == null)
        {
            ActiveBezier = new List<BezierCurve>();
        }
    }

    protected void OnEnable()
    {
        defaultColor = GUI.color;
    }

    public void AddToAlwaysRender()
    {
        //SceneView.onSceneGUIDelegate += HandleOnSceneFunc;
        if (SceneGUI.InEditorActiveBeziers != null && !SceneGUI.InEditorActiveBeziers.Contains(this))
        {
            curve.IsLockedDisplay = true;
            SceneGUI.InEditorActiveBeziers.Add(this);
        }
    }


    public void RemoveFromAlwaysRender()
    {
        //SceneView.onSceneGUIDelegate += HandleOnSceneFunc;

        if (SceneGUI.InEditorActiveBeziers != null)
        {
            curve.IsLockedDisplay = false;
            for (int i = 0; i < SceneGUI.InEditorActiveBeziers.Count; ++i)
            {
                if (SceneGUI.InEditorActiveBeziers[i].curve == curve)
                {
                    SceneGUI.InEditorActiveBeziers.RemoveAt(i);
                    return;
                }
            }
        }
    }

    public void OnSceneGUI()
    {
        curve = target as BezierCurve;
        m_IsSelected = Selection.activeGameObject == curve.gameObject;

        if (curve.UseLocalMatrix)
        {
            Handles.matrix = curve.transform.localToWorldMatrix;
        }

        if (ActiveBezier != null && !ActiveBezier.Contains(this.curve))
        {
            ActiveBezier.Add(curve);
        }
        List<Vector3> newPositions = new List<Vector3>(curve.Points);
        Event e = Event.current;
        if (e.clickCount < 2)
        {
            doubleClickReset = true;
        }
        if (m_IsSelected && (e.command || e.control) && e.shift && e.clickCount >= 2 && doubleClickReset && curve.Points.Count > 4)
        {
            doubleClickReset = false;
            e.clickCount = 0;
            //delete nearest bezier
            EditorGUI.BeginChangeCheck();
            //save the new bezier inserted to undo.       
            List<Vector3> newPath = curve.RemoveControlPoint(HandleUtility.GUIPointToWorldRay(Event.current.mousePosition).origin);
            EditorGUI.EndChangeCheck();

            Undo.RecordObject(target, "Removed Bezier Curve");
            curve.Points = newPath;
            curve.NumberOfBezier--;
            SceneView.RepaintAll();
            return;
        }
        else if (m_IsSelected && (e.command || e.control) && e.clickCount >= 2 && doubleClickReset)
        {
            doubleClickReset = false;
            e.clickCount = 0;
            EditorGUI.BeginChangeCheck();
            //save the new bezier inserted to undo.       
            List<Vector3> newPath = curve.InsertControlPoint(HandleUtility.GUIPointToWorldRay(Event.current.mousePosition).origin);
            EditorGUI.EndChangeCheck();

            Undo.RecordObject(target, "Added Bezier Curve");
            curve.Points = newPath;
            curve.NumberOfBezier++;
            SceneView.RepaintAll();
            return;
        }
        int curveFrom = 0;
        int curveTo = 1;

        for (int i = 0; i < curve.NumberOfBezier; ++i)
        {
            Handles.DrawBezier(curve.Points[0 + (i * 3)], curve.Points[3 + (i * 3)], curve.Points[1 + (i * 3)], curve.Points[2 + (i * 3)], m_IsSelected ? curve.CurveColor : UNSELECTED_COLOR, null, curve.Width);
            if (!curve.PreviewOn)
            {
                Handles.DrawLine(curve.Points[curveFrom], curve.Points[curveTo]);
                curveFrom += 2;
                curveTo += 2;
                Handles.DrawLine(curve.Points[curveFrom], curve.Points[curveTo]);
                ++curveFrom;
                ++curveTo;
            }
        }
        //No editing in preview mode
        if (!curve.PreviewOn)
        {
            EditorGUI.BeginChangeCheck();

            for (int i = 0; i < curve.Points.Count; ++i)
            {
                if (i % 3 == 0)//Control points
                {
                    newPositions[i] = Handles.FreeMoveHandle(curve.Points[i], Quaternion.identity, curve.HandleSize, Vector3.zero, Handles.RectangleHandleCap);
                    controlPointDelta = newPositions[i] - curve.Points[i];
                    if (i > 0)
                    {
                        newPositions[i - 1] += controlPointDelta;
                    }
                    if (newPositions.Count > i + 2)
                    {
                        newPositions[i + 1] += controlPointDelta;
                        curve.Points[i + 1] = newPositions[i + 1];
                    }
                }
                else//tangents
                {
                    if (curve.ForceSmoothPath)
                    {
                        if (i % 3 == 1 && i > 1)//after the control point
                        {
                            newPositions[i] = Handles.FreeMoveHandle(curve.Points[i], Quaternion.identity, curve.HandleSize, Vector3.zero, Handles.RectangleHandleCap);
                            if (newPositions[i] != curve.Points[i])
                            {
                                directionToControl = curve.Points[i]/*target*/ - curve.Points[i - 1]/*origin*/;
                                float distance = Vector3.Distance(newPositions[i - 2], newPositions[i - 1]);
                                newPositions[i - 2] = newPositions[i - 1] + (-directionToControl.normalized * distance);
                            }
                        }
                        else if (i % 3 == 2 && i + 2 < newPositions.Count)//before the control point
                        {
                            newPositions[i] = Handles.FreeMoveHandle(curve.Points[i], Quaternion.identity, curve.HandleSize, Vector3.zero, Handles.RectangleHandleCap);
                            if (newPositions[i] != curve.Points[i])
                            {
                                directionToControl = curve.Points[i]/*target*/ - curve.Points[i + 1]/*origin*/;
                                float distance = Vector3.Distance(newPositions[i + 2], newPositions[i + 1]);
                                newPositions[i + 2] = newPositions[i + 1] + (-directionToControl.normalized * distance);
                            }
                        }
                        else
                        {
                            newPositions[i] = Handles.FreeMoveHandle(curve.Points[i], Quaternion.identity, curve.HandleSize, Vector3.zero, Handles.RectangleHandleCap);
                        }
                    }
                    else
                    {
                        newPositions[i] = Handles.FreeMoveHandle(curve.Points[i], Quaternion.identity, curve.HandleSize, Vector3.zero, Handles.RectangleHandleCap);
                    }
                }
            }
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(target, "Change Bezier curve.");
                for (int i = 0; i < curve.Points.Count; ++i)
                {
                    curve.Points[i] = newPositions[i];
                }
            }
        }
    }

    public override void OnInspectorGUI()
    {


        BezierCurve curve = target as BezierCurve;

        if (curve.PreviewOn)
        {
            GUI.color = previewColor;
        }
        DrawDefaultInspector();


        if (GUILayout.Button("Extend"))
        {
            curve.Extend();
        }
        if (GUILayout.Button("Shorten"))
        {
            curve.Shorten();
        }
        if (GUILayout.Button("Pre Calculate"))
        {
            curve.PreCalculatePoints();
        }

        if (curve.PreviewOn)
        {
            GUI.color = Color.red;
        }
        if (GUILayout.Button(curve.PreviewOn ? "Preview Mode" : "Edit Mode"))
        {
            curve.PreviewOn = !curve.PreviewOn;
        }

        if (curve.IsLockedDisplay)
        {
            GUI.color = Color.red;
            if (GUILayout.Button("Unlock Display"))
            {
                RemoveFromAlwaysRender();
            }
        }
        else
        {
            if (GUILayout.Button("Lock Display"))
            {
                AddToAlwaysRender();
            }
        }

        if (curve.PreviewOn)
        {
            GUI.color = defaultColor;
        }
    }
}

public class SceneGUI : EditorWindow
{
    public static List<BezierDrawer> InEditorActiveBeziers = new List<BezierDrawer>();
    public static bool ShowLockedBezier = true;

    [MenuItem("Bezier/Toggle Always ON %g")]
    public static void ToggleBezier()
    {
        if (ShowLockedBezier)
        {
            ShowLockedBezier = false;
            SceneView.onSceneGUIDelegate -= OnScene;
        }
        else
        {
            SceneView.onSceneGUIDelegate += OnScene;
            ShowLockedBezier = true;
        }

    }

    private static void OnScene(SceneView sceneview)
    {

        for (int i = 0; i < InEditorActiveBeziers.Count; ++i)
        {
            InEditorActiveBeziers[i].OnSceneGUI();
        }
    }
}
