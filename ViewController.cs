using System;
using System.Text;
using System.Text.Json;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.IO;
using System.Runtime.InteropServices;
using AppKit;
using Foundation;

namespace detnswautologinmacos
{
	public partial class ViewController : NSViewController
	{
		public void RenderStream(Stream stream)
		{
			var reader = new StreamReader(stream);
			InvokeOnMainThread(() => {
				/*
				string clientType = HandlerType != null ? $"HttpClient is using {HandlerType.Name}\n" : string.Empty;
				TheLog.Value = $"{clientType} The HTML returned by the server: {reader.ReadToEnd()}";
				TheButton.Enabled = true;
				*/
				var alert = new NSAlert()
				{
					AlertStyle = NSAlertStyle.Informational,
					InformativeText = reader.ReadToEnd(),
					MessageText = "Response from server"
				};
				alert.RunModal();
			});
		}

		void RunTls12Request()
		{
			var actual = ServicePointManager.SecurityProtocol;

			try
			{
				ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                // "https://edgeportal.forti.net.det.nsw.edu.au/portal/selfservice/IatE_CP/"
                // "https://httpbin.org/post"
                HttpWebRequest request = WebRequest.CreateHttp(new Uri("https://edgeportal.forti.net.det.nsw.edu.au/portal/selfservice/IatE_CP/"));
				request.Method = "POST";
				byte[] byteArray = Encoding.UTF8.GetBytes($"csrfmiddlewaretoken=&username={usernameBox.StringValue}&password={passwordBox.StringValue}");
				request.ContentLength = byteArray.Length;
				request.ContentType = "application/x-www-form-urlencoded";
				Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
				var msg = request.GetResponse();
				bool debug = true;
				if (debug)
				{
					using (var stream = msg.GetResponseStream())
						RenderStream(stream);
				}
				
			}
			catch (WebException ex)
			{
				Console.WriteLine(ex.Message);
			}
			finally
			{
				ServicePointManager.SecurityProtocol = actual;
			}
		}

		public ViewController (IntPtr handle) : base (handle)
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			// Do any additional setup after loading the view.
		}

		public override NSObject RepresentedObject {
			get {
				return base.RepresentedObject;
			}
			set {
				base.RepresentedObject = value;
				// Update the view, if already loaded.
			}
		}

        partial void loginButton(NSButton sender)
        {
			RunTls12Request();
        }

        partial void saveButton(NSButton sender)
        {
            
        }

        partial void urlButton(NSButton sender)
        {
            
        }
        
        partial void startupCheckbox(NSButton sender)
        {
			if (System.IO.File.Exists(Environment.GetEnvironmentVariable("HOME") + @"/Applications/detnsw-autologin/Contents/Info.plist"))
            {
				var alert = new NSAlert()
				{
					AlertStyle = NSAlertStyle.Warning,
					InformativeText = "The application must be in the Applications folder.",
					MessageText = "whoops, something went wrong"
				};
				alert.RunModal();
				return;
			}
			//Startup.StartAtLogin(sender.State == NSCellStateValue.On);
			if (sender.State == NSCellStateValue.On)
            {
				string path = Environment.GetEnvironmentVariable("HOME") + @"/Library/LaunchAgents";
				try
				{
					using (FileStream fs = File.Create(path))
					{
						byte[] info = new UTF8Encoding(true).GetBytes("<?xml version=\"1.0\" encoding=\"UTF - 8\"?>\n" +
							"<!DOCTYPE plist PUBLIC \" -//Apple Computer//DTD PLIST 1.0//EN\" \"http://www.apple.com/DTDs/PropertyList-1.0.dtd\">\n" +
							"<plist version=\"1.0\">\n" +
                            "<dict>\n" +
                            "	<key>Label</key>\n" +
                            "	<string>com.circularsprojects.detnsw-autologin</string>\n" +
                            "	<key>ProgramArguments</key>\n" +
                            "	<array><string>executable</string></array>\n" +
                            "	<key>RunAtLoad</key>\n" +
                            "	<true/>\n" +
                            "</dict>\n" +
                            "</plist>\n");
						fs.Write(info, 0, info.Length);
					}
				} catch(Exception err)
                {
					var alert = new NSAlert()
					{
						AlertStyle = NSAlertStyle.Critical,
						InformativeText = err.ToString(),
						MessageText = "whoops, something went wrong"
					};
					alert.RunModal();
				}
				} else if (sender.State == NSCellStateValue.Off)
            {

            }
        }
    }
}
