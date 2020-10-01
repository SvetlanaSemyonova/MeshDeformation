using System.Linq;
using UnityEngine;

public class JellyVertex : MonoBehaviour
{
    public int vertexIndex;

    public Vector3 initialVertexPosition;
    public Vector3 currentVertexPosition;
    public Vector3 currentVelocity;

    public JellyVertex(int _vertexIndex, Vector3 _initialVertexPosition, Vector3 _currentVertexPosition, Vector3 _currentVelocity)
    {
        vertexIndex = _vertexIndex;
        initialVertexPosition = _initialVertexPosition;
        currentVertexPosition = _currentVertexPosition;
        currentVelocity = _currentVelocity;
    }

    public Vector3 GetCurrentDisplacement()
    {
        return currentVertexPosition - initialVertexPosition;
    }

    public void UpdateVelocity(float _bounceSpeed)
    {
        currentVelocity = currentVelocity - GetCurrentDisplacement() * _bounceSpeed * Time.deltaTime;
    }

    public void Settle(float _stiffness)
    {
        currentVelocity *= 1f - _stiffness * Time.deltaTime;
        int [] array = {1,10,20,30,40,50};
        var sorted = array.OrderByDescending(x => x);
        var item1 = sorted.ElementAt(0);
        var item2 = sorted.ElementAt(1);
        var item3 = sorted.ElementAt(2);
        
        
        int max = array[0];
        for (int i = 0; i < array.Length; i++)
        {
            if (max < array[i])
            {
                max = array[i];
            }
        }
        
    }

    public void ApplyPressure(Transform _transform, Vector3 _position, float _pressure)
    {
        Vector3 distanceVerticePoint = currentVertexPosition - _transform.InverseTransformPoint(_position);
        var adapterPressure = _pressure / (1f + distanceVerticePoint.sqrMagnitude);
        var velocity = adapterPressure * Time.deltaTime;
        currentVelocity += distanceVerticePoint.normalized * velocity;
    }
}
