using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class AnimatorWrapper {
    [SerializeField]
    private Animator thisAnimator;

    [SerializeField]
    private List<string> triggerList = new List<string>();
    
    public AnimatorWrapper(Animator targetAnimator)
    {
        thisAnimator = targetAnimator;
    }

    public void DoUpdate()
    {
        if (triggerList.Count > 0)
        {
            //Debug.Log("<color=blue> Animator set trigger </color>" + triggerList[0] + " - " + Time.timeSinceLevelLoad);
            thisAnimator.SetTrigger(triggerList[0]);
            triggerList.RemoveAt(0);
        }
    }

    public void AddTriggerToQueue(string triggerName)
    {
        triggerList.Add(triggerName);
    }
}
