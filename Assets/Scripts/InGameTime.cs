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
        time = 720;
        pastTime = 0;
    }

    // Update is called once per frame
    public void Update()
    {
        pastTime += Time.deltaTime;
        if(pastTime >= timeTick)
        {
            time++;
            if (time > 1440)
            {
                time = 0;
            }
            pastTime -= timeTick;
        }
    }
}
