using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.UI;

public class FileSelector : MonoBehaviour {

	private string[] directories;
	private string[] files;
	private string upDirectory;	
	private string currentDir;

	public Button fileButton; // Prefab
	public GameObject scrollViewContent;

	// Use this for initialization
	void Start () {
#if UNITY_ANDROID
		Debug.Log("Unity_Android");
		this.currentDir = "/";
#endif
#if UNITY_EDITOR
		Debug.Log("Unity_Editor");
		this.currentDir = "c:/";
#endif
		this.BuildFileSystem();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void BuildFileSystem() {
		this.directories = Directory.GetDirectories(this.currentDir, "*");
		this.files = Directory.GetFiles(this.currentDir, "*");
		if (!this.currentDir.Equals("c:/")) {
			Button button = Instantiate(fileButton) as Button;
			button.transform.SetParent(scrollViewContent.transform,false);
			button.GetComponentInChildren<Text>().text = "..";
			button.onClick.AddListener(UpDirectory);
		}
		foreach (string directory in this.directories) {
			string directoryModded = directory.Replace("\\", "/");
			Button button = Instantiate(fileButton) as Button;
			button.transform.SetParent(scrollViewContent.transform,false);
			button.GetComponentInChildren<Text>().text = directoryModded.Substring(directoryModded.IndexOf("/")+1);
			button.onClick.AddListener(delegate {EnterDirectory(directoryModded); });
		}
		foreach (string file in this.files) {
			// file.Replace("\\", "/");
			Button button = Instantiate(fileButton) as Button;
			button.transform.SetParent(scrollViewContent.transform,false);
			button.GetComponentInChildren<Text>().text = file.Substring(file.IndexOf("/")+1);
			button.onClick.AddListener(delegate{ShowFile(file); });
		}
		this.scrollViewContent.transform.position = new Vector2(this.scrollViewContent.transform.position.x, -99999f);
	}

	public void UpDirectory() {
		string newDir = this.currentDir.Substring(0, this.currentDir.LastIndexOf("/"));
		if (newDir.IndexOf("/") == -1) {
			newDir = newDir + "/";
		}
		this.currentDir = newDir;
		this.EmptyScrollView();
		this.BuildFileSystem();
	}

	public void EnterDirectory(string directory) {
		try {
			string newDir = directory;
			Debug.Log(newDir);
			this.currentDir = newDir;
			this.EmptyScrollView();
			this.BuildFileSystem();
		} catch (Exception e) {
			Debug.Log(e); //TODO Show error message somehow to the user
			this.UpDirectory();
		}
	}

	public void ShowFile(string file) {
		Debug.Log(file); //TODO Do something with the file
	}	

	public void EmptyScrollView()
	{
		foreach (Transform child in scrollViewContent.transform) {
			GameObject.Destroy(child.gameObject);
		}
	}	
}
