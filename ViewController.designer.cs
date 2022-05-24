// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace detnswautologinmacos
{
	[Register ("ViewController")]
	partial class ViewController
	{
		[Outlet]
		AppKit.NSSecureTextField passwordBox { get; set; }

		[Outlet]
		AppKit.NSTextField passwordLabel { get; set; }

		[Outlet]
		AppKit.NSTextField usernameBox { get; set; }

		[Outlet]
		AppKit.NSTextField usernameLabel { get; set; }

		[Action ("loginButton:")]
		partial void loginButton (AppKit.NSButton sender);

		[Action ("saveButton:")]
		partial void saveButton (AppKit.NSButton sender);

		[Action ("startupCheckbox:")]
		partial void startupCheckbox (AppKit.NSButton sender);

		[Action ("urlButton:")]
		partial void urlButton (AppKit.NSButton sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (passwordBox != null) {
				passwordBox.Dispose ();
				passwordBox = null;
			}

			if (passwordLabel != null) {
				passwordLabel.Dispose ();
				passwordLabel = null;
			}

			if (usernameBox != null) {
				usernameBox.Dispose ();
				usernameBox = null;
			}

			if (usernameLabel != null) {
				usernameLabel.Dispose ();
				usernameLabel = null;
			}
		}
	}
}
