using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

enum Finger{Index, Middle, Ring, Pinky}

public class Controller : MonoBehaviour
{
    protected float[] fingerAction = {0,0,0,0};
    
    [SerializeField] protected Player player;
    protected HandPose handPose;

    // Update is called once per frame
    protected void Start() {
        handPose = HandPose.None;
        //handPose = HandPose.Gun;
    }
    protected void Update()
    {
        UpdateHandPose();
        setPlayerHandPose();
    }
    // public void GripPressedEvent(float state)
    // {
    //     int i = state > 0 ? 1 : 0;
        
    //     fingerAction[(int)Finger.Middle] = i;
    //     fingerAction[(int)Finger.Ring] = i;
    //     fingerAction[(int)Finger.Pinky] = i;
    // }
    public void GripPressedEvent(InputAction.CallbackContext ctx)
    {
        var state = ctx.ReadValue<float>();
        
        int i = state > 0 ? 1 : 0;
        
        fingerAction[(int)Finger.Middle] = i;
        fingerAction[(int)Finger.Ring] = i;
        fingerAction[(int)Finger.Pinky] = i;
    }
    
    // public void TriggerPressedEvent(float state)
    // {
    //     fingerAction[(int)Finger.Index] = state > 0 ? 1 : 0;
    // }
    public void ShootButtonPressedEvent(InputAction.CallbackContext ctx)
    {
        player.TryShoot();
    }
    public void TriggerPressedEvent(InputAction.CallbackContext ctx)
    {
        var state = ctx.ReadValue<float>();
        
        fingerAction[(int)Finger.Index] = state > 0 ? 1 : 0;
    }

    protected float getFingerValue(int _finger)
    {
        return fingerAction[_finger];
    }
    
    protected void UpdateHandPose()
    {
        if (checkHandPose(false, true, true, true))
            handPose = HandPose.Gun;
        
        else if (checkHandPose(false, true, true, false))
            handPose = HandPose.Spidey;
        
        else if (checkHandPose(true, true, true, true))
            handPose = HandPose.Fist;
        
        else if (checkHandPose(false, false, false, false))
            handPose = HandPose.Paper;

        else if (checkHandPose(true, false, false, false))
            handPose = HandPose.Point;

        else handPose = HandPose.None;
        
    }

    protected bool checkHandPose(bool index, bool middle, bool ring, bool pinky)
    {
        bool[] pose = {index, middle, ring, pinky};

        for(int i=0; i < 4; i++)
        {
            if((fingerAction[i] == 1 ? true : false) != pose[i])
                return false;
        }
        return true;
    }

    protected void setPlayerHandPose()
    {
        player.setHandPose(handPose);
    }
}
