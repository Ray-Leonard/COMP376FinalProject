using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class RulePowerVisual : MonoBehaviour
{
    [SerializeField] private Image fillImage;
    [SerializeField] private Progress_bar progress;

    private GameLogic gameLogic;

    private int maxRulePower;

    private Color highColor = new Color(0.094f, 0.306f, 0.792f);
    private Color lowColor = new Color(0.384f, 0, 0.027f);
    // Start is called before the first frame update
    void Start()
    {
        gameLogic = GameObject.FindGameObjectsWithTag("GameLogic")[0].GetComponent<GameLogic>();
        maxRulePower = gameLogic.maxRulePower;
        progress.max = maxRulePower;
    }

    // Update is called once per frame
    void Update()
    {
        progress.current = gameLogic.rulePower;
        float percentage = gameLogic.rulePower / (float)maxRulePower;
        fillImage.color = new Color(Mathf.Lerp(lowColor.r, highColor.r, percentage),
                                    Mathf.Lerp(lowColor.g, highColor.g, percentage),
                                    Mathf.Lerp(lowColor.b, highColor.b, percentage));
    }
}
