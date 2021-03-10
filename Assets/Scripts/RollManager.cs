using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RollManager : MonoBehaviour
{
    public InputField sampleSizeInput;
    public InputField diceInput;
    public InputField modifierInput;
    public InputField multiplierInput;
    public InputField safeLineInput;
    public Text resultText;

    private int sampleSize;
    private int dice;
    private int modifier;
    private int multiplier;
    private int safeline;

    private int safelineCountAC;
    private int safelineCountDC;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Roll()
    {
        int.TryParse(sampleSizeInput.text, out sampleSize);
        if (sampleSize < 1) sampleSize = 1;

        int.TryParse(diceInput.text, out dice);
        if (dice < 4) dice = 4;

        int.TryParse(modifierInput.text, out modifier);

        int.TryParse(multiplierInput.text, out multiplier);
        if (multiplier < 1) multiplier = 1;

        int.TryParse(safeLineInput.text, out safeline);
        if (safeline < 1) safeline = 1;

        safelineCountAC = 0;
        safelineCountDC = 0;

        resultText.text = "";

        if (sampleSize > 0)
        {     
            int min = multiplier + modifier;
            if (min < 1) min = 1;

            int max = multiplier * dice + modifier;
            if (max < 1) max = 1;

            int ceiling = max + 1;

            if (min == max)
            {
                //Debug.Log("Roll " + min + " : " + sampleSize);
                resultText.text += string.Format("Roll {0} : {1}\n", min, sampleSize);
            }
            else
            {
                int[] pool = new int[ceiling - min];

                int poolModifier = 1 + modifier;
                if (poolModifier < 1) poolModifier = 1;

                for (int i = 0; i < sampleSize; i++)
                {
                    int rn = 0;

                    for (int j = 0; j < multiplier; j++)
                    {
                        rn += Random.Range(1, dice + 1);
                    }

                    rn += modifier;
                    if (rn < min) rn = min;
                    //Debug.Log(rn);

                    pool[rn - min]++;

                }

                for (int i = 0; i < pool.Length; i++)
                {
                    int roll = i + min;
                    //Debug.Log("Roll " + roll + " : " + pool[i]);

                    resultText.text += string.Format("Roll {0} : {1}\n", roll, pool[i]);

                    if (roll > safeline) safelineCountAC += pool[i];

                    if (roll > safeline - 1) safelineCountDC += pool[i];

                }

                float accAC = (float)safelineCountAC / sampleSize;
                //Debug.Log(string.Format("AC : {0}, DC : {1}, sample : {2}", safeline, safeline - 1,safelineCountDC));
                resultText.text += string.Format("Accuracy AC {0} : {1}\n", safeline,  accAC.ToString("F3"));

                float evaDC = (float)safelineCountDC / sampleSize;
                resultText.text += string.Format("Evasion DC {0} : {1}\n", safeline,  evaDC.ToString("F3"));

            }

        }
            
        
    }
}
