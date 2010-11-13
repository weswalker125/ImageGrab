using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Web;


namespace chanRipper
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			
			//Check input argument:
			if(args.Length != 1)
				return;
			
			//arg1 : URL to 4chan thread
			string URL = args[0];
			
			//http://boards.4chan.org/s/res/10822814#q10822814
			string saveDirectory = "/4chan_download/" + URL.Substring(URL.LastIndexOf("/"));
			
			if(!Directory.Exists(saveDirectory))
				Directory.CreateDirectory(saveDirectory);
			
			/*UTF8Encoding utf8;
			Byte[] encodedBody;*/
			WebRequest request;
			HttpWebResponse response;
			
			//Create http request:
			request = HttpWebRequest.Create(URL);
			request.Proxy = null;
			request.ContentType = "text/xml";
			request.Method = "GET";
			
			//Send the request:
			Stream chanThread = null;
			try
			{
				chanThread = request.GetRequestStream();
			}
			catch
			{
				Console.WriteLine("Error sending HTTP Request");
			}
			
			finally
			{
				if(chanThread != null)
					chanThread.Close();
			}
			
			Console.WriteLine("Sending Request...");
			
			//Get the response:
			try
			{
				response = (HttpWebResponse) request.GetResponse();
				if(response == null)
					Console.WriteLine("Null web response");
				Console.WriteLine("Code: " + response.StatusCode);
				Console.WriteLine("Details: " + response.StatusDescription);
				//Read Response:
				StreamReader chanReply = new StreamReader(response.GetResponseStream());
				//Console.WriteLine(chanReply.ReadToEnd());
				string[] tokens = chanReply.ReadToEnd().Split('"');
				
				foreach(string imageUrl in tokens)
				{
					if(imageUrl.Contains("http://images.4chan.org"))
					{
						string imageName;
						imageName = imageUrl.Substring(imageUrl.LastIndexOf("/"));
						imageName = saveDirectory + imageName;
							
						ImageRetriever imgRetriever = new ImageRetriever();
						//Get image:
						Image currentPicture = null;
						currentPicture = imgRetriever.GetImage(imageUrl);
						//Write image to disk:
						imgRetriever.SaveImage(currentPicture, imageName);
						
						
					}
				}
			}
			catch(WebException ex)
			{
				//Read error:
				WebResponse Response = (HttpWebResponse)ex.Response;
				using (Stream reply = Response.GetResponseStream())
				{
					string ErrorTxt = new StreamReader(reply).ReadToEnd();
					Console.WriteLine("Error: unable to retrieve web response");
					Console.WriteLine(ErrorTxt);
				}
			}
		}
	}
}

