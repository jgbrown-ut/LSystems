using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeGlobals : MonoBehaviour
{
    public static bool toggleLeaves;
    public static int type;
    public static int iterations;
    public static float r1;
    public static float r2;
    public static float a0;
    public static float a2;
    public static float d;
    public static float wr;
    public static bool updated;


    // Start is called before the first frame update
    void Start()
    {
        toggleLeaves = true;
        iterations = 4;
        r1 = 0.9f;
        r2 = 0.6f;
        a0 = 45f;
        a2 = 45f;
        d = 137.5f;
        wr = 0.707f;
        updated = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // type range [0, 1]
    public void setType(float val)
    {
        iterations = (int) val;
        updated = true;
    }

    // r1 range [0, 1]
    public void setIterations(float val)
    {
        iterations = (int) val;
        updated = true;
    }

    // r1 range [0, 1]
    public void setR1(float val)
    {
        r1 = val;
        updated = true;
    }

    // r1 range [0, 1]
    public void setR2(float val)
    {
        r2 = val;
        updated = true;
    }

    // a0 range [0, 180]
    public void setA0(float val)
    {
        a0 = val;
        updated = true;
    }

    // a2 range [0, 180]
    public void setA2(float val)
    {
        a2 = val;
        updated = true;
    }

    // d range [0, 180]
    public void setD(float val)
    {
        d = val;
        updated = true;
    }

    // wr range [0, 1]
    public void setWr(float val)
    {
        wr = val;
        updated = true;
    }
}
