using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Shared;
using System.IO;
using System;

public class Preferences : MonoBehaviour {

	private bool isFirstStart = true;
	private string configFilePath = "config.txt";

	//assets - the components that are mapped to gameObjects in Scene
	public InputField TechNameInput;
	public InputField TechEmailInput;
	public Text BadInputText;
	public Toggle MirrorActive;

	/*
	 * Any time the Preferences Scene is loaded it first populates the input fields with their stored values.
	 * If no stored values exist, then they are empty.
	*/
	void Start () {
		TechNameInput.text = PlayerPrefs.HasKey("techName") ? PlayerPrefs.GetString ("techName") : string.Empty; 
		TechEmailInput.text = PlayerPrefs.HasKey ("techEmail") ? PlayerPrefs.GetString ("techEmail") : string.Empty;

		if (PlayerPrefs.HasKey ("mirror")) {
			MirrorActive.isOn = PlayerPrefs.GetInt ("mirror") == 1 ? true : false;
		} else {
			MirrorActive.isOn = false;
		}

		if (isFirstStart) {
			if (GameSetup.Input == null) {
				GetGameInput();
			}
			isFirstStart = false;
		}
	}
		
	/*
	 * Update() is called once per frame. In this Script it's job is to check whether the fields
	 * are valid. If the fields do not have valid input, an error message alerts the user.
	*/
	void Update () {
		if (TechNameInput.text == string.Empty || TechEmailInput.text == string.Empty) {
			BadInputText.text = "Fields must not be Empty";
			BadInputText.enabled = true;
		}else{
			BadInputText.enabled = false;
		}
	}

	/*
	 * If the input fields are empty the user cannot proceed to the MainMenu Scene.
	 * If they are not empty than the values are stored in PlayerPrefs ( a file on the computer, to be used later ).
	*/
	public void LoadMainScene()
	{ 
		if (TechNameInput.text != string.Empty && TechEmailInput.text != string.Empty) {
			BadInputText.enabled = false;

			PlayerPrefs.SetString("techName", TechNameInput.text);
			PlayerPrefs.SetString ("techEmail", TechEmailInput.text);
			PlayerPrefs.SetInt ("mirror", MirrorActive.isOn ? 1 : 0);

			SceneManager.LoadScene ("MainMenuScene");
		}
	}

	/* 
	 * Determines input source for game.  Currently, if a config file is found, it uses TcpInput.
	 * If there is no config file, it uses simulated input.  Can be expanded in the future to handle
	 * things like the ability to create a direct connection from the bellows to the game (i.e. no Server).
	 */
	private void GetGameInput()
	{
		if (!File.Exists (configFilePath))
		{
			GameSetup.Input = new SimInput ();
		} else {
			try
			{
				using (StreamReader reader = new StreamReader(configFilePath))
				{
					string address = reader.ReadLine ().Trim();
					int port = Int32.Parse (reader.ReadLine ());
					GameSetup.Input = new TcpInput (address, port);
				}
			}
			catch (Exception e)
			{
				Debug.Log("Problem reading config.txt " + e.Message);
				GameSetup.Input = new SimInput ();  // Cannot open config file, so use simulated
			}
		}
	}
}
