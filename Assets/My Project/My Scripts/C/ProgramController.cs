using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MaterialUI;
using UnityEngine.Video;

public class ProgramController : AppController {

	List<ButtonItem> pool = new List<ButtonItem>();

	Datum currentItem = null;

	[SerializeField]
	Text[] labelsInfo;
	[SerializeField]
	GameObject buttonPlayVideo, videoLoader;
	[SerializeField]
	MediaPlayerCtrl videoPlayer;

	int currIdx = -1;

	public override void OnNotification(NOTIFYEVENT p_event_path,Object p_target,params object[] p_data)
	{

		switch (p_event_path)
		{
		case NOTIFYEVENT.PLAY:

			currentItem = p_data[0] as Datum;

			app.sv.Transition("PlayScreen");

			app.pushedScreens.Add("SearchScreen");

			app.SetBackButton (true);

			SetupDataItem (-1);

			break;
		case NOTIFYEVENT.PLAYVIDEO:


			if (currentItem.publicationEvent[currIdx].media != null)
			{

				app.sv.Transition("PlayVideo");
				app.pushedScreens.Add("PlayScreen");
				app.SetBackButton (true);

				videoPlayer.Stop();
				videoLoader.SetActive(true);

				StartCoroutine(app.model.GetMediaInfo(currentItem.id, currentItem.publicationEvent[currIdx].media.id, (res) => {
				
					if (string.IsNullOrEmpty(res))
					{
						app.ShowToast(ERROR.NOMEDIA);
					}
					else
					{
						string tempUrl = Util.GetUrl(res);

						#if UNITY_IOS || UNITY_ANDROID

						videoPlayer.Load( tempUrl );

						#else

						Application.OpenURL(tempUrl);

						#endif



					}
					videoLoader.SetActive(false);
				}));
			}
			else
			{
				app.ShowToast(ERROR.NOMEDIA);
			}

			break;
		case NOTIFYEVENT.SELECTPUB:

			if (currentItem.publicationEvent != null && currentItem.publicationEvent.Count > 0)
			{

				var options = new List<string>();

				foreach(var p in currentItem.publicationEvent)
				{

					options.Add(p.id);

				}

				DialogManager.ShowSimpleList(options.ToArray(), (int selectedIndex) => {

					SetupDataItem(selectedIndex);

				});

			}
			else
			{
				app.ShowToast(ERROR.NOITEMS);
			}


			break;
		case NOTIFYEVENT.PLAYPAUSE:
			videoPlayer.Stop ();
			videoPlayer.Play ();
			break;


		case NOTIFYEVENT.STOP :

			videoPlayer.Stop ();
			videoPlayer.UnLoad ();

			break;
		}

	}

	void SetupDataItem (int idx)
	{
		labelsInfo[(int)LABELSINFO.TITLE].text = Util.ExtractStr(currentItem.title);
		labelsInfo[(int)LABELSINFO.DESCR].text = Util.ExtractStr(currentItem.description);

		if (currentItem.publicationEvent != null && currentItem.publicationEvent.Count > idx && idx >= 0)
		{
			labelsInfo[(int)LABELSINFO.LABELBUTTON].text = currentItem.publicationEvent[idx].id;
			labelsInfo[(int)LABELSINFO.START].text 		= "Start: " + currentItem.publicationEvent[idx].startTime.ToString("G");
			labelsInfo[(int)LABELSINFO.END].text 		= "End: " + currentItem.publicationEvent[idx].endTime.ToString("G");
			labelsInfo[(int)LABELSINFO.STATUS].text 	= "Status: " + currentItem.publicationEvent[idx].temporalStatus;
			labelsInfo[(int)LABELSINFO.TYPE].text 		= "Type: " + currentItem.publicationEvent[idx].type;
			labelsInfo[(int)LABELSINFO.DURATION].text 	= "Duration: " + Util.FormatDuration(currentItem.publicationEvent[idx].duration);
			currIdx = idx;
			buttonPlayVideo.SetActive( (currentItem.publicationEvent[idx].temporalStatus == "currently" && currentItem.publicationEvent[idx].type == "OnDemandPublication") );
		}
		else
		{
			currIdx = -1;
			labelsInfo[(int)LABELSINFO.LABELBUTTON].text = "Select publication";
			labelsInfo[(int)LABELSINFO.START].text 		= string.Empty;
			labelsInfo[(int)LABELSINFO.END].text 		= string.Empty;
			labelsInfo[(int)LABELSINFO.STATUS].text 	= string.Empty;
			labelsInfo[(int)LABELSINFO.TYPE].text 		= string.Empty;
			labelsInfo[(int)LABELSINFO.DURATION].text 	= string.Empty;
			buttonPlayVideo.SetActive(false);

		}
	}
}
