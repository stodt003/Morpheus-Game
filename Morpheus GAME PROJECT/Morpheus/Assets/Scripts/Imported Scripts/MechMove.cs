using UnityEngine;
using System.Collections;

public class MechMove : MonoBehaviour {

    public float speed;
    public Vector3 destination;
    public Vector3 destRotation;

    private Rigidbody body;
   
	// Use this for initialization
	void Start () {
        body = GetComponent<Rigidbody>();
        
    }


	
    void FixedUpdate()
    {
        Vector3 direction = (destination - transform.position).normalized;
        body.MovePosition(transform.position + direction * speed * Time.deltaTime);
        Quaternion rot = transform.rotation;
        rot.eulerAngles = new Vector3(transform.rotation.x , transform.rotation.y + 170, transform.rotation.z);
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime);
        
    }


}
