using UnityEngine;

public static class GizmosExtension
{
    public static void DrawConnectedLine(Vector3 _from, Vector3 _to, float arrowHeadLength = 1.25f, float arrowHeadAngle = 20.0f)
    {
        Gizmos.DrawLine(_from, _to);

        Vector3 direction = _to - _from;
        Vector3 right = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 + arrowHeadAngle, 0) * new Vector3(0, 0, 1);
        Vector3 left = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 - arrowHeadAngle, 0) * new Vector3(0, 0, 1);

        Vector3 middlePoint = (_to + _from) * 0.5f;
        Gizmos.DrawRay(middlePoint, right * arrowHeadLength);
        Gizmos.DrawRay(middlePoint, left * arrowHeadLength);
    }
}

