using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSurvivor : MonoBehaviour
{
    private static AudioSurvivor instance = null;
    public static AudioSurvivor Instance
    {
        get { return instance; }
    }
    // Update is called once per frame
    void Update()
    {

    }

    void Awake()
    {
        if (instance != null & instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }
}