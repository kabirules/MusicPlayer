using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelOpener : MonoBehaviour {

	public GameObject fileLoadPanel;
	public GameObject playerPanel;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OpenPanel() {
		this.fileLoadPanel.SetActive(true);
		this.playerPanel.SetActive(false);
	}
}
