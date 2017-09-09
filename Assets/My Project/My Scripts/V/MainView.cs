using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainView : AppElement {

	public void OnChangeSearch (string newinput)
	{

		app.Notify(NOTIFYEVENT.CHANGESEARCH, null, new object[] {newinput});

	}


	public void OnClickBackButton ()
	{

		app.Notify(NOTIFYEVENT.BACKBUTTON, null, null);

	}


	public void OnClickSelectPublication ()
	{
		app.Notify(NOTIFYEVENT.SELECTPUB, null, null);

	}

	public void OnChangeScroller ( Vector2 delta )
	{
		app.Notify(NOTIFYEVENT.LOADMORE, null, null);
	}
		
	public void OnClickPlayVideo ()
	{
		app.Notify(NOTIFYEVENT.PLAYVIDEO, null, null);
	}

	public void OnClickPlayPause ()
	{
		app.Notify(NOTIFYEVENT.PLAYPAUSE, null, null);
	}
}
