using UnityEngine;

public enum HandPose { None, Gun, Spidey, Fist, Paper, Point}

public class Player : MonoBehaviour
{
    //[SerializeField] JustRead ardData;
    //public GameObject aim;
    //public GameObject aimAssist;

    //[SerializeField] float sense = 0.0005f;
    [SerializeField] float aimAssistSize = 5f;
    [SerializeField] float maxRange = 30f;
    //[SerializeField] float X = 2, Y = 2;

    [SerializeField] HandPose handPose;

    [SerializeField] GameObject gunParticle, fistParticle;
    
    [SerializeField] Transform aimOrigin, aimTarget;

    Vector3 startPos;
    
    void Awake() {
        
    }
    void Start()
    {
        //startPos = aim.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateParticles();
        //Trigger();
        //Aim();
    }

    void Shoot()
    {
        if (Physics.SphereCast(aimOrigin.position, aimAssistSize, (aimTarget.position - aimOrigin.position).normalized, out RaycastHit raycastHit, maxRange))
        {
            if(raycastHit.collider.gameObject.GetComponent<Target>())
            {                    
                raycastHit.collider.gameObject.GetComponent<Target>().handleHit(handPose);
            }
        }
        else if (Physics.SphereCast(transform.position, aimAssistSize * 2, (aimTarget.position - aimOrigin.position).normalized, out RaycastHit raycastHit2, maxRange))
        {
            if(raycastHit2.collider.gameObject.GetComponent<Target>())
            { 
                raycastHit2.collider.gameObject.GetComponent<Target>().handleHit(handPose);
            }
        }
        else
        {
           GameManager.Instance.TargetMissed(); 
        }
    }

    public void TryShoot()
    {
        Shoot();
    }
    
    public void OnTriggerInput()
    {
        TryShoot();
    }
    
    //enum HandPose { None, Gun, Spidey, Fist, Paper, Point}
    public void setHandPose(HandPose pose)
    {
        handPose = pose;
    }

    void UpdateParticles()
    {
        gunParticle.SetActive(handPose == HandPose.Gun);

        fistParticle.SetActive(handPose == HandPose.Fist);
    }
}
