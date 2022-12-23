using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassController : MonoBehaviour
{
    public bool isBroken;

    public GameObject glass, glassBroken;

    public void BreakGlass()
    {
        glass.SetActive(false);
        glassBroken.SetActive(true);
    }
}
