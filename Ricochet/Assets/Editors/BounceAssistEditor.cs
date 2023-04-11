using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DiscMovement))]
public class BounceAssistEditor : Editor
{
    private Vector3 DirectionFromAngle(float angleInDegrees)
    {
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    private void OnSceneGUI()
    {
        DiscMovement disc = (DiscMovement)target;
        float viewAngle = disc.viewAngle;
        float viewRadius = disc.viewRadius;
        Handles.color = Color.black;
        Vector3 viewAngleA = DirectionFromAngle(-viewAngle / 2 + disc.transform.eulerAngles.y);
        Vector3 viewAngleB = DirectionFromAngle(viewAngle / 2 + disc.transform.eulerAngles.y);

        Handles.DrawWireArc(disc.transform.position, Vector3.up, Vector3.forward, 360, viewRadius);
        Handles.DrawLine(disc.transform.position, disc.transform.position + viewAngleA * viewRadius);
        Handles.DrawLine(disc.transform.position, disc.transform.position + viewAngleB * viewRadius);
    }
}
