using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetTreeVals : MonoBehaviour
{
    public Button MonoButton;
    public Button SympButton;
    public Button LeavesButton;
    public GameObject Branch; // branch object to instantiate


    public Slider iterations;       // contraction ratios 
    public Slider r1;       // contraction ratios 
    public Slider r2;
    public Slider a0;       // branching angles
    public Slider a2;
    public Slider d;        // divergence angles
    void Start()
    {
        MonoButton.onClick.AddListener(ConfigureMonopodial);
        SympButton.onClick.AddListener(ConfigureSympodial);
        LeavesButton.onClick.AddListener(ConfigureLeaves);        
    }

    void ConfigureMonopodial() {
        iterations.value = 7;
        r1.value = 0.9f;
        r2.value = 0.6f;
        a0.value = 45f;
        a2.value = 45f;
        d.value = 137.5f;
        TreeGlobals.type = 0;
        TreeGlobals.updated = true;

    }
    void ConfigureSympodial() {
        iterations.value = 7;
        r1.value = 0.9f;
        r2.value = 0.8f;
        a0.value = 35f;
        a2.value = 35f;
        d.value = 0f;
        TreeGlobals.type = 1;
        TreeGlobals.updated = true;

    }
    void ConfigureLeaves() {
        TreeGlobals.toggleLeaves = !TreeGlobals.toggleLeaves;
        TreeGlobals.updated = true;
    }
}
