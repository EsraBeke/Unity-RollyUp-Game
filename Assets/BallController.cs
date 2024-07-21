using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    [SerializeField] float gravityPower = 1f;
    [SerializeField] float range = 3f;
    [SerializeField] SphereCollider collisonCollider;
    [SerializeField] MainBallControl mainBallControl;
    [SerializeField] Rigidbody myRigidbody;

    Vector3 gravityDirection;
    Collider[] cubes;
    int maxCubeNumber = 15;
    bool firedBall = false;
    bool lookLocation = false;
    bool targetAchieved = false;
    private void Update()
    {
        if (firedBall)
        {
            if (myRigidbody.velocity.magnitude < 0.15f && myRigidbody.velocity.magnitude != 0)
            // ana topun finish kisminda hangi skor diliminde oldugunu anlamamiz icin
            // belli bir hizin altinda dustugu yeri alacagiz
            {
                lookLocation = true;
                return;
            }
        }
    }
    private void FixedUpdate()
    {
        if (GameManager.Instance.hasTheGameStart)
        {
            transform.Rotate(Vector3.forward, 400 * Time.fixedDeltaTime);
            cubes = new Collider[maxCubeNumber];
            int totalCubeNumber = Physics.OverlapSphereNonAlloc(transform.position, range, cubes);
            // bu fizik fonksiyonu bu objenin gittigi yon boyunce range icerisine giren colliderlari algilar ve cubes dizisine ekler
            for (int i = 0; i < totalCubeNumber; i++)
            {
                Rigidbody rbs = cubes[i].GetComponent<Rigidbody>();

                if (rbs != null)
                {
                    gravityDirection = new Vector3(transform.position.x, 0, transform.position.z) - cubes[i].transform.position;
                    rbs.AddForceAtPosition(gravityPower * gravityDirection.normalized, transform.position);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Cube"))
        {
            other.attachedRigidbody.isKinematic = true;
            other.gameObject.transform.SetParent(transform);
            AddedCube();
        }
        if (other.CompareTag("Finish"))
        {
            Transform[] allChild = GetComponentsInChildren<Transform>();

            GameManager.Instance.hasTheGameStart = false;
            transform.SetParent(null); // parent objesi yok dedim
            myRigidbody.isKinematic = false;
            myRigidbody.AddForce(allChild.Length * Time.deltaTime * Vector3.forward, ForceMode.Impulse); // topladigi cube sayisina gore ana topa guc uygular
            // **********
            // burada onemli bir kisim var ana ball cubelerle carpisarak finish kisminda ilerlemiyordu bunu engellemek icin cubelara layer ekledik
            // ve main ball un 2 tane collideri vardi biri carpismalari algilasin diye isTriggeri acik olan digeri sert govde olan 
            //sert govde olan colliderin icindeki LayerOverrides kisminda Exclude Layers kismindan cube secerek sert govdenin cubeler ile olan 
            //temasini kestik 
            //***********
            firedBall = true;

            collisonCollider.enabled = false;

            foreach (var item in allChild)
            {
                if (item.gameObject.CompareTag("Cube")) //toplanan cubeler oldugu yere dussun ana toptan bagimsiz olsun diye yapildi
                {
                    item.gameObject.transform.SetParent(null);
                    item.GetComponent<Rigidbody>().isKinematic = false;
                }
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (lookLocation)
        {
            if (!other.gameObject.CompareTag("Floor") && !other.gameObject.CompareTag("Cube") && !targetAchieved)
            {
                targetAchieved = true;  // buraya 2. kez girmesin diye olan bir bool
                myRigidbody.velocity = Vector3.zero; // hedefe ulasildi direkt durduruyoruz main ball u
                GameManager.Instance.GameDone(int.Parse(other.name)); // su an temas halinde oldugu score zemininin ismini score olarak alacagiz
            }
        }
    }

    void AddedCube() // belli bir kup sayisindan sonra cekim rangesini kurenin colliderini arttirmassan etrafi kaplandigi icin sorun cikarir 
    {
        range += 0.0009f;
        collisonCollider.radius += 0.0010f;
        transform.position = new Vector3(transform.position.x, transform.position.y + 0.0018f, transform.position.z);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
