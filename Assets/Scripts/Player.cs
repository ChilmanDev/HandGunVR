using UnityEngine;

public enum HandPose { None, Gun, Spidey, Fist, Paper, Point}

public class Player : MonoBehaviour
{
    //[SerializeField] JustRead ardData;
    public GameObject aim;
    public GameObject aimAssist;

    //[SerializeField] float sense = 0.0005f;
    [SerializeField] float aimAssistSize = 5f;
    [SerializeField] float maxRange = 30f;
    //[SerializeField] float X = 2, Y = 2;

    [SerializeField] HandPose handPose;

    [SerializeField] GameObject gunParticle, fistParticle;
    
    [SerializeField] Camera camera;
    [SerializeField] Transform aimOrigin, aimTarget;

    // Start is called before the first frame update
    bool triggerHold = false;

    Vector3 startPos;

    int bulletCount = 111;
    void Awake() {
        
    }
    void Start()
    {
        //startPos = aim.transform.position;
    }

    // Update is called once per frame
    void Update()
    {

        // if (Physics.SphereCast(aimOrigin.position, aimAssistSize, (aimTarget.position - aimOrigin.position).normalized, out RaycastHit hit, maxRange))
        // {
        //     aimAssist.transform.position = hit.point;
        // }
        // else if (Physics.SphereCast(aimOrigin.position, aimAssistSize * 3, (aimTarget.position - aimOrigin.position).normalized, out RaycastHit hitCast, maxRange))
        // {
        //     aimAssist.transform.position = hitCast.point;
        // }
        // else
        // {
        //     aimAssist.transform.position = Vector3.zero;
        // }

        UpdateParticles();
        //Trigger();
        //Aim();
    }

    public void Shoot()
    {
        bulletCount--;
        if (Physics.SphereCast(aimOrigin.position, aimAssistSize, (aimTarget.position - aimOrigin.position).normalized, out RaycastHit raycastHit, maxRange))
        {
            if(raycastHit.collider.gameObject.GetComponent<Target>())
            {                    
                raycastHit.collider.gameObject.GetComponent<Target>().handleHit(handPose);
            }
        }
        else if (Physics.SphereCast(transform.position, aimAssistSize * 2, (aim.transform.position - transform.position).normalized, out RaycastHit raycastHit2, maxRange))
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

    void TryShoot()
    {
        if(bulletCount > 0) Shoot();
        else
        {
            //TryShoot();
        }
    }
    
    public void OnTriggerInput()
    {
        TryShoot();
    }

    // void Trigger()
    // {
    //     if(!ardData.trigger)
    //     {
    //         triggerHold = false;
    //     }
    //     else if(triggerHold == false)
    //     {
    //         TryShoot();
    //         triggerHold = true;
    //     }
    // }

    // void Aim()
    // {
    //     if(Mathf.Abs(ardData.gyro.x)  > 3)
    //         aim.transform.position = new Vector3(aim.transform.position.x + ardData.gyro.x * sense, aim.transform.position.y, aim.transform.position.z);

    //     if(Mathf.Abs(ardData.gyro.z) > 3)
    //     aim.transform.position = new Vector3(aim.transform.position.x, aim.transform.position.y + ardData.gyro.z * sense, aim.transform.position.z);

    //     if(ardData.reload) Reload();
    // }
    
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

    void Reload()
    {
        bulletCount = 3;
        aim.transform.position = startPos;
    }

    public int getBulletCount()
    {
        return bulletCount;
    }
}
