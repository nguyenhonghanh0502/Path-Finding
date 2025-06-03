using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private MapGenerate mapCtrl;

    [Header("Manual Update Start and Goal point")]
    [SerializeField] 
    private TMP_InputField startXIF;

    [SerializeField]
    private TMP_InputField startYIF;

    [SerializeField] 
    private TMP_InputField goalXIF;

    [SerializeField]
    private TMP_InputField goalYIF;

    public void UpdateStartGoalPoint()
    {
        mapCtrl.UpdateStartIndex(Convert(startXIF, startYIF));
        mapCtrl.UpdateGoalIndex(Convert(goalXIF, goalYIF));
        mapCtrl.UpdateStartGoal();
    }

    private Vector2Int Convert(TMP_InputField X, TMP_InputField Y)
    {
        return new Vector2Int(ConvertToInt(X), ConvertToInt(Y));
    }

    private int ConvertToInt(TMP_InputField input)
    {
        int num = -1;
        if (int.TryParse(input.text, out num))
        {
            
        }
        else
        {
            num = -1;
            Debug.LogWarning("Không hợp lệ!");
        }
        return num;
    }

    public void FindPath()
    {
        mapCtrl.FindPath();
    }
}
