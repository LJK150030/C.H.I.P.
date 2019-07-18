using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CleanButton : MonoBehaviour {
    public Button slot1;
    public Button slot2;

    Button cleanButton;

	void Start () {
        cleanButton = GetComponent<Button>();
        cleanButton.onClick.AddListener(OnBtnClicked);
	}

	void Update () {
		
	}

    private void OnBtnClicked()
    {
        slot1.onClick.Invoke();
        slot2.onClick.Invoke();
    }
}
