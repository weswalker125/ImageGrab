using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Web;


namespace chanRipper
{
	public class ImageRetriever
	{
		public ImageRetriever ()
		{
		}
		
		public Image GetImage(string imageUrl)
		{
			Image currentImage = null;
			
			//Get this URL, save in folder
			HttpWebRequest imageRequest = (HttpWebRequest) HttpWebRequest.Create(imageUrl);
			imageRequest.AllowWriteStreamBuffering = true;
			
			//imageRequest.Method = "GET";
			//20 second timeout:
			imageRequest.Timeout = 20000;
			
			/*
			//Send request:
			Stream imageRequestStream = null;
	
			try
			{
				imageRequestStream = imageRequest.GetRequestStream();
			}
			catch
			{
				Console.WriteLine("Error sending HTTP request for image " + imageUrl);
			}
			finally
			{
				if(imageRequestStream != null)
					imageRequestStream.Close();
			}
			*/
			
			Stream imageResponseStream = null;
			//Read response:
			try
			{
				WebResponse imageResponse = imageRequest.GetResponse();
				imageResponseStream = imageResponse.GetResponseStream();
				
				//Convert stream to image:
				currentImage = Image.FromStream(imageResponseStream);
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error getting HTTP response for image " + imageUrl);
				Console.WriteLine(ex.ToString());
			}
			finally
			{
				if(imageResponseStream != null)
					imageResponseStream.Close();
			}
			
			return currentImage;
		}
		
		public void SaveImage(Image picture, string savePath)
		{
			picture.Save(savePath);
		}
	}
}

