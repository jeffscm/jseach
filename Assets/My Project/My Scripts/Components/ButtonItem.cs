using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonItem : AppElement {

	string videoId;

	[SerializeField]
	Text titleLabel, durationLabel;

	[SerializeField]
	RawImage thumb;


	Coroutine co = null;
	Datum data;

	void OnDisable ()
	{
		if (co != null)
		{
			StopCoroutine(co);
			co = null;
		}

		if (thumb.texture != null && string.IsNullOrEmpty(videoId))
		{
			Destroy(thumb.texture);
		}
	}

	public void OnClickPlay () {
		app.Notify(NOTIFYEVENT.PLAY, this, new object[] {data});
	}

	public void Reset ()
	{
		videoId = string.Empty;
	}

	public void Setup (Datum res)
	{

		data = res;

		titleLabel.text = Util.ExtractStr(res.title);

		durationLabel.text = Util.FormatDuration(res.duration);

		this.videoId = res.id;

		this.gameObject.SetActive(true);

		if (co != null)
		{
			StopCoroutine(co);
			co = null;
		}

		co = StartCoroutine(app.model.DownloadThumb(res.image.id, (t) => {
			co = null;
			thumb.texture = t;

		}));

	}

	

}
