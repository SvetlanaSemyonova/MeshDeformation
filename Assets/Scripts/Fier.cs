using System;
using UnityEngine;

public class Fier : MonoBehaviour
{
    public float bounceSpeed;
    public float fallForce;
    public float stiffness;

    private MeshFilter _meshFilter;
    private Mesh _mesh;

    private JellyVertex[] _jellyVertices;
    private Vector3[] _currentMeshVertices;

    private void Start()
    {
        _meshFilter = GetComponent<MeshFilter>();
        _mesh = _meshFilter.mesh;

        GetVertices();
    }

    private void GetVertices()
    {
        _jellyVertices = new JellyVertex[_mesh.vertices.Length];
        _currentMeshVertices = new Vector3[_mesh.vertices.Length];

        for (int i = 0; i < _mesh.vertices.Length; i++)
        {
            _jellyVertices[i] = new JellyVertex(i, _mesh.vertices[i], _mesh.vertices[i], Vector3.zero );
            _currentMeshVertices[i] = _mesh.vertices[i];
        }
    }

    private void Update()
    {
        UpdateVertices();
    }

    private void UpdateVertices()
    {
        for (var i = 0; i < _jellyVertices.Length; i++)
        {
            _jellyVertices[i].UpdateVelocity(bounceSpeed);
            _jellyVertices[i].Settle(stiffness);

            _jellyVertices[i].currentVertexPosition += _jellyVertices[i].currentVelocity * Time.deltaTime;
            _currentMeshVertices[i] = _jellyVertices[i].currentVertexPosition;
        }

        _mesh.vertices = _currentMeshVertices;
        _mesh.RecalculateBounds();
        _mesh.RecalculateNormals();
        _mesh.RecalculateTangents();
    }

    public void OnCollisionEnter(Collision other)
    {
        ContactPoint[] collisionPoints = other.contacts;

        for (int i = 0; i < collisionPoints.Length; i++)
        {
            var inputPoint = collisionPoints[i].point + (collisionPoints[i].point * .1f);
            ApplyPressureToPoint(inputPoint, fallForce);
        }
    }

    public void ApplyPressureToPoint(Vector3 point, float pressure)
    {
        for (int i = 0; i < _jellyVertices.Length; i++)
        {
            _jellyVertices[i].ApplyPressure(transform, point, pressure);
        }
    }
}
