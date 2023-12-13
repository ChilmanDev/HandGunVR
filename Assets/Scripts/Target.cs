using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] float speed = 5;
    [HideInInspector] public bool isActive;

    [SerializeField] HandPose destroyPose;



    [SerializeField] GameObject particleConst;
    [SerializeField] GameObject particleDestroy;
    [SerializeField] AudioSource soundSource;

    // Start is called before the first frame update
    void Start()
    {
        isActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(0f,1f,0f) * speed * Time.deltaTime);
    }

    public void Activate(){
        isActive = true;
    }

    public void handleHit(HandPose pose)
    {
        if(!isActive)
            return;

        if(pose != destroyPose)
        {
            wrongPose();
            return;
        }

        hitTarget();
    }

    void hitTarget()
    {
        particleConst.SetActive(false);
        GetComponent<SphereCollider>().enabled = false;
        particleDestroy.SetActive(true);
        soundSource.Play();
        StartCoroutine("DelayDestroy", 1f);
    }

    IEnumerator DelayDestroy(float sec)
    {
        yield return new WaitForSeconds(sec);

        GameManager.Instance.TargetDestroyed();
        Destroy(gameObject);
    }

    void wrongPose()
    {
        GameManager.Instance.TargetMissed();
    }
}
