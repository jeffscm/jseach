using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MaterialUI;

public enum LABELSINFO {
	TITLE = 0,
	DESCR,
	START,
	END,
	STATUS,
	TYPE,
	DURATION,
	LABELBUTTON
}

public class SearchController : AppController {

	int offset = 0;
	string currentSearch = string.Empty;
	bool canLoadMore = false;

	Coroutine searchCo = null;

	List<ButtonItem> pool = new List<ButtonItem>();

	[SerializeField]
	ButtonItem prefabItem;
	[SerializeField]
	Transform basePrefabs;
	[SerializeField]
	ScrollRect scroller;
	[SerializeField]
	Transform poolStorage;
	[SerializeField]
	Image dummyImage;

	public override void OnNotification(NOTIFYEVENT p_event_path,Object p_target,params object[] p_data)
	{
		switch (p_event_path)
		{


		case NOTIFYEVENT.BACKBUTTON:

			var temp = app.pushedScreens[app.pushedScreens.Count-1];

			app.pushedScreens.RemoveAt(app.pushedScreens.Count-1);

			app.sv.Transition(temp);

			if (app.pushedScreens.Count == 0)
			{
				app.SetBackButton (false);	
			}

			if (temp == "PlayScreen")
			{
				app.Notify(NOTIFYEVENT.STOP, null, null);
			}
				
			break;

		case NOTIFYEVENT.SEARCH:

			string q = p_data[0].ToString ();
			offset = 0;
			ResetData ();

			if (string.IsNullOrEmpty(q))
			{
				
			}
			else
			{
				if (searchCo != null)
				{
					StopCoroutine(searchCo);						
				}


				app.ShowLoader(true);
				searchCo = StartCoroutine(
					app.model.Search(
						q, 
						(res) => 
						{
							CreateSeachObjects (res);

						},
						offset
					)
				);
			}
			break;

		case NOTIFYEVENT.CHANGESEARCH:

			currentSearch = p_data[0].ToString();
			CancelInvoke("SearchInputChanged");
			Invoke("SearchInputChanged", 0.25f);
			break;

		case NOTIFYEVENT.LOADMORE:

			if (canLoadMore && scroller.verticalNormalizedPosition < 0.15f && offset >= 10)
			{
				canLoadMore = false;

				StartCoroutine(
					app.model.Search(
						currentSearch, 
						(res) => 
						{
							CreateSeachObjects (res);
						},
						offset
					)
				);

			}
			break;

		

		}
	}


	void CreateSeachObjects (List<Datum> res) 
	{
		if (res == null)
		{
			app.ShowToast(ERROR.ERRNET);
		}
		else if (res.Count == 0)
		{
			if (offset < 10) app.ShowToast(ERROR.NORESULT);
		}
		else
		{
			dummyImage.gameObject.SetActive(false);
			if (res.Count == 10)
			{
				offset += 10;
				canLoadMore = true;
			}
			else
			{
				canLoadMore = false;
			}				

			for (int i = 0 ; i < res.Count; i++)
			{
				var obj = GetPool();
				obj.transform.SetParent(basePrefabs);
				obj.transform.localScale = Vector3.one;
				obj.Setup(res[i]);
			}
		}
		app.ShowLoader(false);
	}

	ButtonItem GetPool()
	{

		for (int i = 0 ; i < pool.Count; i++)
		{
			if (!pool[i].gameObject.activeSelf)
			{
				return pool[i];
			}
		}
		var temp = Instantiate(prefabItem);
		pool.Add(temp);
		return temp;
	}


	void SearchInputChanged ()
	{
		app.Notify(NOTIFYEVENT.SEARCH, null, new object [] {currentSearch});
	}

	void ResetData ()
	{
		dummyImage.gameObject.SetActive(true);
		for (int i = 0 ; i < pool.Count; i++)
		{
			pool[i].Reset ();
			pool[i].transform.SetParent(poolStorage);
			pool[i].gameObject.SetActive(false);
		}
	}


}
