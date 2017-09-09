using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MaterialUI;
using System;

public enum ERROR {NORESULT = 0, ERRNET, NOITEMS, NOMEDIA};

public enum NOTIFYEVENT 
	{

		START,
		SEARCH,
		PLAY,
		CHANGESEARCH,
		LOADMORE,
		BACKBUTTON,
		SELECTPUB,
		PLAYVIDEO,
		PLAYPAUSE,
		STOP

	};

public class App : MonoBehaviour {

	public AppModel model;

	public string [] errMsg;

	public GameObject loaderObj;

	public ScreenView sv;

	public GameObject backButton, menuButton;

	public List<string> pushedScreens = new List<string>();

	void Awake ()
	{
		model = new AppModel();
	}

	public void Notify(NOTIFYEVENT p_event_path, UnityEngine.Object p_target, params object[] p_data)
	{
		AppController[] controller_list = GetAllControllers();
		foreach(AppController c in controller_list)
		{
			c.OnNotification(p_event_path,p_target,p_data);
		}
	}

	public AppController[] GetAllControllers() { return GameObject.FindObjectsOfType<AppController>(); }


	public void ShowLoader (bool a)
	{
		loaderObj.SetActive(a);
	}

	public void ShowToast (ERROR err) 
	{
		ToastManager.Show(errMsg[(int)err]);
	}

	public void SetBackButton (bool activateBack)
	{
		backButton.SetActive(activateBack); 
		menuButton.SetActive(!activateBack);
	}
}
