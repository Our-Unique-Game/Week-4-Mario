using UnityEngine;

public class Rotator : MonoBehaviour
{

    [SerializeField]
    public Vector3 rotationSpeed = new Vector3(0, 100, 0);

    void Update()
    {
        transform.Rotate(rotationSpeed * Time.deltaTime);
    }
}