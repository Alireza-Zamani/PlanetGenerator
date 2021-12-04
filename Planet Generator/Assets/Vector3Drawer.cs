using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vector3Drawer : MonoBehaviour
{
    [SerializeField] Vector3 vectorOne = new Vector3(1,2,3);
    [SerializeField] Vector3 vectorTwo = new Vector3(2,3,1);

    [SerializeField] Vector3 localUp = Vector3.up;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(Vector3.up);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, localUp);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, new Vector3(localUp.y, localUp.z, localUp.x));

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, Vector3.Cross(localUp, new Vector3(localUp.y, localUp.z, localUp.x)));
    }
}
