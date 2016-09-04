using UnityEngine;
using System.Collections;

public class MissleLaunch : MonoBehaviour {

    public GameObject projectile;

    public Transform originPos;
    public float shotForce = 1f;
    
    public float numberOfShots = 1;

    private Rigidbody projectileRigidBody;


	// Use this for initialization
	void Start () {
        
        projectileRigidBody = projectile.GetComponent<Rigidbody>();
        //StartCoroutine("FireMissles");
    }
	
    void FireLaser()
    {
        GameObject.Find("MechWarriorLArmDigit11").GetComponent<LaserBeam>().FiringMode(true);
        //GetComponent<GvrAudioSource>().Play();
    }

    IEnumerator FireMissiles()
    {
        for(int i = 0; i < numberOfShots; i++)
        {
            GameObject missile = Instantiate(projectile, originPos.position, originPos.rotation) as GameObject;
            missile.GetComponent<Rigidbody>().AddForce(originPos.forward * shotForce, ForceMode.Impulse);
            
            yield return new WaitForSeconds(0.33f);
        }

        yield return null; 
    }
}
