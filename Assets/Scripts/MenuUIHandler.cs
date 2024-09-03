using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuUIHandler : MonoBehaviour
{
    public TMP_InputField TMP_InputField;

    public void SaveName()
    {
        MainManager.Instance.name = TMP_InputField.text;
    }

}
