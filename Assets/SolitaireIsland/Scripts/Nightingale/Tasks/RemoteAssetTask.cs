using com.F4A.MobileThird;
using Nightingale.Extensions;
using Nightingale.JSONUtilitys;
using Nightingale.Utilitys;
using SolitaireTripeaks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

namespace Nightingale.Tasks
{
	public class RemoteAssetTask : NormalTask
	{
		protected string Tag;

		protected int Index;

		protected string LocalPath;

		public RemoteAssetTask(string root, string path)
		{
			LocalPath = Path.Combine(Application.persistentDataPath, path);
#if ENABLE_ASSET_ONLINE
			base.TaskId = Path.Combine(root, path);
#else

			base.TaskId = StreamingAssetsPathUtility.StreamingAssetsPath(path);

#endif
            DoSomething = DoNoCacheAssetTask;
		}

		public void Rest()
		{
			Index = 0;
		}

		private IEnumerator DoNoCacheAssetTask(UnityAction<object, float> completed)
		{
            if (completed == null)
			{
				yield return new WaitForEndOfFrame();
				yield break;
			}
#if ENABLE_LOG_DOWNLOAD
			Debug.Log("@LOG DoNoCacheAssetTask TaskId:" + TaskId);
#endif
			AssetBundle bundle = AssetBundle.GetAllLoadedAssetBundles().ToList().Find((AssetBundle e) => FileUtility.IsSameFile(TaskId, e.name));
			if (bundle != null)
			{
				Debug.Log("@LOG DoNoCacheAssetTask bundle contain");
                completed(bundle, 1f);
				yield return new WaitForEndOfFrame();
				yield break;
			}
#if ENABLE_ASSET_ONLINE
#if ENABLE_LOG_DOWNLOAD
			UnityEngine.Debug.LogFormat("-----Start Download {0}", base.TaskId);
#endif
			completed(null, 0f);
			UnityWebRequest Request = UnityWebRequest.Get(base.TaskId);
			Request.SetRequestHeader("Cache-Control", "max-age=0, no-cache, no-store");
			Request.SetRequestHeader("Pragma", "no-cache");
			Request.SetRequestHeader("___did", SingletonClass<NightingaleSystemInfo>.Get().DeviceUniqueIdentifier);
			Request.SetRequestHeader("___pid", SolitaireTripeaksData.Get().GetPlayerId());
			Request.SetRequestHeader("___pf", Application.platform.ToString());
			Request.SetRequestHeader("___cv", Application.version);
			Request.SetRequestHeader("___sv", "1");
			Request.SetRequestHeader("___aid", "tripeaks");
			Request.SetRequestHeader("___pp", base.Process.ToString());
			Request.SetRequestHeader("___tag", Tag.TOBase64());
#if ENABLE_LOG_DOWNLOAD
			UnityEngine.Debug.LogFormat("下载进度:{0}", base.Process);
#endif
			yield return Request.SendWebRequest();
			if (!Request.isDone)
			{
				yield break;
			}
#if ENABLE_LOG_DOWNLOAD
			UnityEngine.Debug.LogFormat("-----{0}", Request.downloadHandler.text);
#endif
			Dictionary<string, object> datas = Json.Deserialize(Request.downloadHandler.text) as Dictionary<string, object>;
			if (datas != null && datas.ContainsKey("Url") && !string.IsNullOrEmpty(datas["Url"].ToString()))
			{
				Request = UnityWebRequest.Get(datas["Url"].ToString());
				Request.SetRequestHeader("Cache-Control", "max-age=0, no-cache, no-store");
				Request.SetRequestHeader("Pragma", "no-cache");
				UnityWebRequestAsyncOperation Operation = Request.SendWebRequest();
				do
				{
					base.Process = Request.downloadProgress;
					if (Request.isDone)
					{
						Tag = string.Empty;
						if (string.IsNullOrEmpty(Request.error))
						{
							bundle = AssetBundle.GetAllLoadedAssetBundles().ToList().Find((AssetBundle e) => FileUtility.IsSameFile(TaskId, e.name));
							if (bundle != null)
							{
								bundle.Unload(unloadAllLoadedObjects: false);
							}
							if (Request.downloadHandler.data.Length > 0)
							{
								yield return AssetBundle.LoadFromMemoryAsync(Request.downloadHandler.data);
								bundle = AssetBundle.GetAllLoadedAssetBundles().ToList().Find((AssetBundle e) => FileUtility.IsSameFile(TaskId, e.name));
								if (bundle == null)
								{
									Tag = "decompress error";
								}
							}
							else
							{
								Tag = "data=0";
							}
#if ENABLE_LOG_DOWNLOAD
							UnityEngine.Debug.LogFormat("-----END Download {0} {1}", bundle != null, Request.url);
#endif
						}
						else
						{
#if ENABLE_LOG_DOWNLOAD
							UnityEngine.Debug.LogFormat("-----END Download {0}", Request.error);
#endif
							Tag = Request.error;
						}
						
						if (bundle != null)
						{
                            try
                            {
                                FileUtility.DeleteFile(LocalPath);
                                FileUtility.CreateDirectory(LocalPath);
                                File.WriteAllBytes(LocalPath, Request.downloadHandler.data);
                            }
                            catch (Exception)
                            {
                                Tag = "save error";
                            }
                            completed(bundle, 1f);
							bundle = AssetBundle.GetAllLoadedAssetBundles().ToList().Find((AssetBundle e) => FileUtility.IsSameFile(TaskId, e.name));
							if (bundle != null)
							{
								bundle.Unload(unloadAllLoadedObjects: false);
							}
						}
						else
						{
							float delay2 = Mathf.Min(Mathf.Pow(3f, ++Index) + 12f, 300f);
#if ENABLE_LOG_DOWNLOAD
							UnityEngine.Debug.LogFormat("下载等待时间:{0}", delay2);
#endif
							yield return new WaitForSeconds(delay2);
							completed(null, 1f);
						}
						Request.Dispose();
						break;
					}
					completed(null, Mathf.Min(base.Process, 0.99f));
					yield return new WaitForSeconds(0.5f);
				}
				while (Application.isPlaying);
				yield return Operation;
			}
			else
			{
#if ENABLE_LOG_DOWNLOAD
				UnityEngine.Debug.Log("-----End Download for Shield....");
#endif
				if (datas != null && datas.ContainsKey("Url") && string.IsNullOrEmpty(datas["Url"].ToString()))
				{
					Tag = "shield user";
				}
				float delay = Mathf.Min(Mathf.Pow(3f, ++Index) + 12f, 300f);
#if ENABLE_LOG_DOWNLOAD
				UnityEngine.Debug.LogFormat("下载等待时间:{0}", delay);
#endif
				yield return new WaitForSeconds(delay);
				completed(null, 1f);
			}
#else
				UnityWebRequest request = new UnityWebRequest();
            request = UnityWebRequestAssetBundle.GetAssetBundle(base.TaskId);
            yield return request.SendWebRequest();
            if (request.isDone)
            {
                var assetBundle = ((DownloadHandlerAssetBundle)request.downloadHandler).assetBundle;
                if(assetBundle != null)
                {
                    try
                    {
                        FileUtility.DeleteFile(LocalPath);
                        FileUtility.CreateDirectory(LocalPath);
                        File.WriteAllBytes(LocalPath, request.downloadHandler.data);
                    }
                    catch (Exception)
                    {
                        Tag = "save error";
                    }
                    completed(assetBundle, 1f);
                }
                yield break;
            }
            yield return new WaitForSeconds(1f);
            completed(null, 1f);
#endif
            yield return null;
		}
	}
}
