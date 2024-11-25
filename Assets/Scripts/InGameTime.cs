using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class InGameTime : MonoBehaviour
{
    public int time;
    public float timeTick;
    public float pastTime;
    // Start is called before the first frame update
    public void Start()
    {
        // Time : 1 ~ 1440 : 0:00 ~ 23:59
        time = 720;
        pastTime = 0;
    }

    // Update is called once per frame
    public void Update()
    {
        // calculate game time on time tick
        pastTime += Time.deltaTime;
        if(pastTime >= timeTick)
        {
            time++;
            if (time > 1440)
            {
                time = 1;
            }
            pastTime -= timeTick;
        }
    }
}
