/*******************************************************************************************

Model Data

Note:

Commented members from Serializable classes are only to save memory, they are not used
at this moment.

*******************************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Newtonsoft.Json;
using UnityEngine.Networking;

public class AppModel {


	const string baseUrl = "https://external.api.yle.fi/v1/programs/";

	const string appid = "app_id=[APPID]&app_key=[APPKEY]";

	const string baseUrlImage = "http://images.cdn.yle.fi/image/upload/";

	YSearch _currentSearchObj;

	public YSearch currentSearchObj { 
		get
		{
			return _currentSearchObj;	
		}
	}

	public AppModel ()
	{
		
	}

	public IEnumerator Search (string q, Action<List<Datum>> onFinished, int offset)
	{

		using (var www = UnityWebRequest.Get(baseUrl +"items.json?"+ appid +"&availability=ondemand&mediaobject=video&order=playcount.24h:desc&limit=10&offset="+offset.ToString()+"&q=" + WWW.EscapeURL(q)))
		{
			yield return www.Send();

			if (www.isNetworkError)
			{
				onFinished(null);
			}
			else
			{
				if (www.responseCode == 200)
				{
					var temp = www.downloadHandler.text;
					if (string.IsNullOrEmpty(temp))
					{
						onFinished(null);
					}
					else
					{
						_currentSearchObj = JsonConvert.DeserializeObject<YSearch>(temp);
						if (_currentSearchObj != null && _currentSearchObj.data != null)
						{
							onFinished(_currentSearchObj.data);	
						}
						else
						{
							onFinished(new List<Datum> ());
						}
					}
				}
			}
		}

	}

	public IEnumerator DownloadThumb (string id, Action<Texture2D> onFinished)
	{
		using (var www = UnityWebRequestTexture.GetTexture(baseUrlImage + id + ".jpg"))
		{
			yield return www.Send();

			Texture2D temp = null;
				
			if (!www.isNetworkError)
			{
				temp = DownloadHandlerTexture.GetContent(www);
			}

			if (temp == null)
			{
				temp = Resources.Load("noimg") as Texture2D;
			}
			onFinished(temp);

		}

	
	}


	public IEnumerator GetMediaInfo (string pid, string mid, Action<string> onFinished)
	{


		string url = "https://external.api.yle.fi/v1/media/playouts.json?program_id="+pid+"&media_id="+mid+"&protocol=HLS&" + appid;


		using (var www = UnityWebRequest.Get(url))
		{
			yield return www.Send();

			if (www.isNetworkError)
			{
				onFinished(string.Empty);
			}
			else
			{
				if (www.responseCode == 200)
				{
					var temp = www.downloadHandler.text;
					if (string.IsNullOrEmpty(temp))
					{
						onFinished(string.Empty);
					}
					else
					{
						var obj = JsonConvert.DeserializeObject<UrlResponse>(temp);

						if (obj.data.Count > 0)
						{
							onFinished(obj.data[0].url);	
						}
						else
						{
							onFinished(string.Empty);
						}
					}
				}
			}
		}
	}


}







[System.Serializable]
public class Meta
{
	public string q;
	public string availability;
	public int clip;
	public string mediaobject;
	public string limit;
	public string offset;
	public int count;
	public string order;
	public int program;
}
[System.Serializable]
public class Video
{
	public List<object> language;
	public List<Format> format;
	public string type;
}
[System.Serializable]
public class Creator
{
	public string name;
	public string type;
}
	
[System.Serializable]
public class Subject
{
	public string id;
	public Dictionary<string, string> title;
	public string inScheme;
	public string type;
	public List<Notation> notation;
	public Dictionary<string, string> broader;
	public string key;
}

[System.Serializable]
public class PartOfSeason
{
	public Dictionary<string, string> description;
	public int seasonNumber;
	public List<Creator> creator;
	public Dictionary<string, object>  partOfSeries;
	public DateTime indexDataModified;
	public string type;
	public string productionId;
	public Dictionary<string, string>  title;
	public List<object> countryOfOrigin;
	public string id;
	public List<Subject> subject;
}

[System.Serializable]
public class PromotionTitle
{
	public string fi;
}

[System.Serializable]
public class ContentRating
{
	public Dictionary<string, string> title;
	public int ageRestriction;
	public List<object> reason;
	public string ratingSystem;
	public string type;
}


[System.Serializable]
public class ImageContainer
{
	public string id;
//	public bool available;
//	public string type;
//	public int version;
}

[System.Serializable]
public class Format
{
	public string inScheme;
	public string type;
	public string key;
}

[System.Serializable]
public class Audio
{
	public List<string> language;
	public List<Format> format;
	public string type;
}

[System.Serializable]
public class PartOfProduct
{
	public Dictionary<string, string> description;
	public IList<object> creator;
	public DateTime indexDataModified;
	public Dictionary<string, string> title;
	public IList<object> countryOfOrigin;
	public IList<object> interactions;
	public string id;
	public ImageContainer image;
	public List<Subject> subject;
}

[System.Serializable]
public class OriginalTitle
{
	public string und;
}

[System.Serializable]
public class Tags
{
	public bool catalog;
}

[System.Serializable]
public class Service
{
	public string id;
}

[System.Serializable]
public class Publisher
{
	public string id;
}

[System.Serializable]
public class ContentProtection
{
	public string id;
	public string type;
}

[System.Serializable]
public class Media
{
	public string id;
	public string duration;
//	public List<ContentProtection> contentProtection;
//	public bool available;
	public string type;
//	public bool downloadable;
	public int version;
}

[System.Serializable]
public class PublicationEvent
{
//	public Tags tags;
//	public Service service;
//	public IList<Publisher> publisher;
	public DateTime startTime; // List
	public string temporalStatus;// List
	public DateTime endTime; // List
	public string type;// List
	public string duration;// List
//	public string region;
	public string id;
//	public int version;
	public Media media;
}

[System.Serializable]
public class Notation
{
	public string value;
	public string valueType;
}

[System.Serializable]
public class Subtitling
{
	public List<string> language;
	public string type;
}

[System.Serializable]
public class Datum
{
	public Dictionary<string, string> description; // List
//	public Video video;
//	public string typeMedia;
//	public List<Creator> creator;
//	public PartOfSeason partOfSeason;
//	public Dictionary<string, object>  partOfSeries;
//	public PromotionTitle promotionTitle;
//	public DateTime indexDataModified;
//	public List<string> alternativeId;
	public string type;
	public string duration;
//	public string productionId;
//	public ContentRating contentRating;
	public Dictionary<string, string> title; // List
//	public Dictionary<string, string> itemTitle;
//	public IList<string> countryOfOrigin;
	public string id;
	public string typeCreative;
	public ImageContainer image;
//	public List<Audio> audio;
//	public PartOfProduct partOfProduct;
//	public OriginalTitle originalTitle;
	public List<PublicationEvent> publicationEvent; // List
//	public string collection;
//	public List<Subject> subject;
//	public List<Subtitling> subtitling;
}

[System.Serializable]
public class YSearch
{
//	public string apiVersion;
//	public Meta meta;
	public List<Datum> data;
}





[System.Serializable]
public class DataUrl
{
	public string url;
}

[System.Serializable]
public class UrlResponse
{
	public List<DataUrl> data;
}