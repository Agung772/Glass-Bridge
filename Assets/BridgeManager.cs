using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeManager : MonoBehaviour
{
    public Transform leftSideBridge, rightSideBridge;
    public Transform leftBridgeParent, rightBridgeParent, goalTransform, goalPivot;
    public GameObject glassPrefab;

    public int totalRow;
    public float glassOffset;
    public float bridgeLengthMultipier;
    public GlassController[,] glasses;

    public static BridgeManager instance;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        SpawnGlass();
    }

    void SpawnGlass()
    {
        glasses = new GlassController[totalRow, 2];
        bridgeLengthMultipier *= totalRow;
        leftBridgeParent.localScale = new Vector3(leftBridgeParent.localScale.x, leftBridgeParent.localScale.y, bridgeLengthMultipier);
        rightBridgeParent.localScale = leftBridgeParent.localScale;
        goalTransform.position = new Vector3(0, 0, totalRow * 3.5f + glassOffset);

        for (int i = 0; i < totalRow; i++)
        {
            GameObject glassGOLeft = Instantiate(glassPrefab, leftSideBridge);
            GameObject glassGORight =  Instantiate(glassPrefab, rightSideBridge);

            glassGOLeft.transform.localPosition = new Vector3(0, 0, i * 3 + glassOffset);
            glassGORight.transform.localPosition = new Vector3(0, 0, i * 3 + glassOffset);

            GlassController glassLeft = glassGOLeft.GetComponent<GlassController>();
            GlassController glassRight = glassGORight.GetComponent<GlassController>();

            glasses[i, 0] = glassLeft;
            glasses[i, 1] = glassRight;

            bool rightIsTheBrokenGlass = RightIsTheBrokenGlass();
            if (rightIsTheBrokenGlass)
            {
                glassRight.isBroken = true;
            }
            else
            {
                glassLeft.isBroken = true;
            }
        }
    }

    private bool RightIsTheBrokenGlass()
    {
        bool rightIsBroken = false;

        int randomNumber = Random.Range(0, 2);

        if (randomNumber == 0)
        {
            rightIsBroken = true;
        }
        else
        {
            rightIsBroken = false;
        }

        return rightIsBroken;

    }
}
