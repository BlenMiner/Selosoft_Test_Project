using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipe : MonoBehaviour
{
    [SerializeField] Transform _start;
    [SerializeField] Transform _end;
    [SerializeField] float _width;

    public Vector3 StartPosition => _start.position;

    public Vector3 EndPosition => _end.position;

    public float Width => _width;

# if UNITY_EDITOR

    bool validGizmos => (_end != null && _start != null);

    private void OnDrawGizmosSelected()
    {
        if (!validGizmos) return;

        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.color = new Color(0, 1, 0, 0.5f);

        Vector3 center = _start.position + (_end.position - _start.position) * 0.5f;
        float length = Vector3.Distance(_start.position, _end.position);

        Gizmos.DrawCube(
            transform.InverseTransformPoint(center),
            new Vector3(_width / transform.localScale.x, length / transform.localScale.y, 0));
    }

    private void OnDrawGizmos()
    {
        if (!validGizmos) return;

        Gizmos.color = Color.black;

        Gizmos.DrawLine(_start.position, _end.position);
    }
#endif
}
