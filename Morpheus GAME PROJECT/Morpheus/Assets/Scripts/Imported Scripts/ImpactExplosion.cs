using UnityEngine;
using System.Collections;

public class ImpactExplosion : MonoBehaviour {
    
    public GameObject explosion;
    public GameObject explosionAudio;
    private MeshCollider groundCol;
    // Use this for initialization

    void Start()
    {
        groundCol = GameObject.Find("Ground").GetComponent<MeshCollider>() as MeshCollider;
    }

    // Missile explodes on impact.
    void OnCollisionEnter(Collision coll)
    {
        
        Instantiate(explosion, transform.position, Quaternion.identity);
        Instantiate(explosionAudio, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
       

    }

}
