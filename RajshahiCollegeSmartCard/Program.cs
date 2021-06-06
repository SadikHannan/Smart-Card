using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
//using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace RajshahiCollegeSmartCard
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]

		public static string ByteArrayToString(byte[] bytes)
		{
			System.Text.Encoding Encoding = System.Text.Encoding.ASCII;
			return (Encoding.GetString(bytes));
		}

		public static byte[] StringToByteArray(string str)
		{
			byte[] ByteArray = new byte[str.Length + 1];
			System.Text.Encoding Encoding = System.Text.Encoding.ASCII;
			ByteArray = new byte[Encoding.GetByteCount(str) + 1];
			ByteArray = Encoding.GetBytes(str + " ");
			ByteArray[ByteArray.Length - 1] = 0;
			return ByteArray;
		}

		public static byte[] GetSize(Int32 Value)
		{
			byte[] val = new byte[3];
			for (int indx = 0; indx < 3; indx++)
			{
				val[indx] = (byte)(Value & 0xFF);
				Value >>= 8;
			}
			return val;
		}

		public static byte[] GetBytes(Int32 Value)
		{
			byte[] val = new byte[4];
			for (int indx = 0; indx < 4; indx++)
			{
				val[indx] = (byte)(Value & 0x000000FF);
				Value >>= 8;
			}
			return val;
		}

		public static Int32 GetValue(byte[] byteVal)
		{
			Int32 Value = 0;
			for (int indx = byteVal.Length - 1; indx >= 0; indx--)
			{
				Value <<= 8;
				Value = Value | ((Int32)byteVal[indx] & 0x000000FF);
			}
			return Value;
		}        

		public static void ImageCopressSave(Image img, short CompressRation, ref byte[] CompressImageArray)
		{            
			Bitmap bm = new Bitmap(img);
			int x = Convert.ToInt16(bm.Width * 0.05);
			Rectangle r = new Rectangle(x, 0, bm.Width - 2 * x, bm.Height);
			ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
			ImageCodecInfo ici = null;

			foreach (ImageCodecInfo codec in codecs)
			{
				if (codec.MimeType == "image/jpeg")
					ici = codec;
			}

			EncoderParameters ep = new EncoderParameters(2);
			if (CompressRation == 1)
			{
				ep.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, (long)30);
				ep.Param[1] = new EncoderParameter(System.Drawing.Imaging.Encoder.ColorDepth, 24L);
			}
			else if (CompressRation == 2)
			{
				ep.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, (long)20);
				ep.Param[1] = new EncoderParameter(System.Drawing.Imaging.Encoder.ColorDepth, 24L);
			}
			else if (CompressRation == 3)
			{
				ep.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, (long)10);
				ep.Param[1] = new EncoderParameter(System.Drawing.Imaging.Encoder.ColorDepth, 18L);
			}
			else
			{
				ep.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, (long)10);
				ep.Param[1] = new EncoderParameter(System.Drawing.Imaging.Encoder.ColorDepth, 12L);
			}


			Bitmap resized = new Bitmap(bm, 150, 200);
			using (MemoryStream ms = new MemoryStream())           
			{                
				resized.Save(ms, ici, ep);
				CompressImageArray = ms.ToArray();
			}            
		}

		public static Student GetResponseData(string id)
		{
            //10.10.10.253
            //WebRequest request = WebRequest.Create("http://rajcourtcollege.edu.bd/ecm/public/api/adminapi/" + id);
            WebRequest request = WebRequest.Create("http://10.10.10.253/EasyCollegeMate/admin_api.php?student_id=" + id);
            request.Credentials = CredentialCache.DefaultCredentials;
			WebResponse response = request.GetResponse();
			//Console.WriteLine(((HttpWebResponse)response).StatusDescription);
			Stream dataStream = response.GetResponseStream();
			StreamReader reader = new StreamReader(dataStream);
			string responseFromServer = reader.ReadToEnd();
		 //   JObject googleSearch = JObject.Parse(responseFromServer);
			List<Student> searchResult = JsonConvert.DeserializeObject<List<Student>>(responseFromServer.ToString());
			return searchResult.FirstOrDefault();

		}

		public static Teacher GetResponseDataTeacher(string id)
		{
            //WebRequest request = WebRequest.Create("http://rajcourtcollege.edu.bd/ecm/public/api/teacherapi/" + id);
            WebRequest request = WebRequest.Create("http://10.10.10.253/EasyCollegeMate/teacher_api.php?teacher_id=" + id);
            request.Credentials = CredentialCache.DefaultCredentials;
			WebResponse response = request.GetResponse();
			//Console.WriteLine(((HttpWebResponse)response).StatusDescription);
			Stream dataStream = response.GetResponseStream();
			StreamReader reader = new StreamReader(dataStream);
			string responseFromServer = reader.ReadToEnd();
			//   JObject googleSearch = JObject.Parse(responseFromServer);
			List<Teacher> searchResult = JsonConvert.DeserializeObject<List<Teacher>>(responseFromServer.ToString());
			return searchResult.FirstOrDefault();

		}

        public static ESMStudent GetResponseDataEasySchoolmatetudent(String institutecode,string id)
        {
            //WebRequest request = WebRequest.Create("http://rajcourtcollege.edu.bd/ecm/public/api/teacherapi/" + id);
            WebRequest request = WebRequest.Create("http://easyschoolmate.net/sclupdateapi/public/api/studentidcardinfo/"+ institutecode + "/"+id);
            request.Credentials = CredentialCache.DefaultCredentials;
            WebResponse response = request.GetResponse();
            //Console.WriteLine(((HttpWebResponse)response).StatusDescription);
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();
            //   JObject googleSearch = JObject.Parse(responseFromServer);
           List<ESMStudent> searchResult = JsonConvert.DeserializeObject<List<ESMStudent>>(responseFromServer.ToString());
            return searchResult.FirstOrDefault();

        }

        //public void Request(string url, string metode)
        //{
        //    try
        //    {
        //        //Enviem la petició a la URL especificada i configurem el tipus de connexió
        //        HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(url);

        //        myReq.KeepAlive = true;
        //        myReq.Headers.Set("Cache-Control", "no-store");
        //        myReq.Headers.Set("Pragma", "no-cache");
        //        myReq.Headers.Set("Authorization", usuari.token_type + " " + usuari.access_token);

        //        if (metode.Equals("GET") || metode.Equals("POST"))
        //        {
        //            myReq.Method = metode;  // Set the Method property of the request to POST or GET.
        //            if (body == true)
        //            {
        //                // add request body with chat search filters
        //                List<paramet> p = new List<paramet>();
        //                paramet p1 = new paramet();
        //                p1.value = "1";
        //                string jsonBody = JsonConvert.SerializeObject(p1);
        //                var requestBody = Encoding.UTF8.GetBytes(jsonBody);
        //                myReq.ContentLength = requestBody.Length;
        //                myReq.ContentType = "application/json";
        //                using (var stream = myReq.GetRequestStream())
        //                {
        //                    stream.Write(requestBody, 0, requestBody.Length);
        //                }
        //                body = false;
        //            }
        //        }
        //        else throw new Exception("Invalid Method Type");

        //        //Obtenim la resposta del servidor
        //        HttpWebResponse myResponse = (HttpWebResponse)myReq.GetResponse();
        //        Stream rebut = myResponse.GetResponseStream();
        //        StreamReader readStream = new StreamReader(rebut, Encoding.UTF8); // Pipes the stream to a higher level stream reader with the required encoding format. 
        //        string info = readStream.ReadToEnd();
        //        var jsondata = JsonConvert.DeserializeObject<Usuari.Client>(info);

        //        myResponse.Close();
        //        readStream.Close();
        //     }
        //    catch (WebException ex)
        //    {
        //        // same as normal response, get error response
        //        var errorResponse = (HttpWebResponse)ex.Response;
        //        string errorResponseJson;
        //        var statusCode = errorResponse.StatusCode;
        //        var errorIdFromHeader = errorResponse.GetResponseHeader("Error-Id");
        //        using (var responseStream = new StreamReader(errorResponse.GetResponseStream()))
        //        {
        //            errorResponseJson = responseStream.ReadToEnd();
        //        }
        //    }
        //}

        static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);           
			Application.Run(new StudentDataView());

		}
	}
}
