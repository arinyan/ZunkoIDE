using Microsoft.VisualStudio.Text.Editor;
using System;
using System.IO;
using System.Reflection;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ZunkoIDE
{
	/// <summary>
	/// Adornment class that draws a square box in the top right hand corner of the viewport
	/// </summary>
	internal class ZunkoIDE
	{
		private Image _image;
		private IWpfTextView _view;
		private IAdornmentLayer _adornmentLayer;
		private BitmapImage _bitmap;

		/// <summary>
		/// Creates a square image and attaches an event handler to the layout changed event that
		/// adds the the square in the upper right-hand corner of the TextView via the adornment layer
		/// </summary>
		/// <param name="view">The <see cref="IWpfTextView"/> upon which the adornment will be drawn</param>
		public ZunkoIDE(IWpfTextView view)
		{
			_view = view;

			try
			{
			_image = new Image();

			var imagepath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Images\a1zunko01.png");
			_bitmap = new BitmapImage(new Uri(imagepath, UriKind.Absolute));

			_image.Source = _bitmap;
			_image.Opacity = 0.3;
			_image.Stretch = Stretch.Uniform;

			//Grab a reference to the adornment layer that this adornment should be added to
			_adornmentLayer = view.GetAdornmentLayer("ZunkoIDE");

			_view.ViewportHeightChanged += delegate { this.onSizeChange(); };
			_view.ViewportWidthChanged += delegate { this.onSizeChange(); };
			}
			catch (Exception) { }
		}

		public void onSizeChange()
		{
			//clear the adornment layer of previous adornments
			_adornmentLayer.RemoveAllAdornments();

			var ratio = _view.ViewportHeight / _bitmap.Height;

			if (_view.ViewportHeight >= _bitmap.Height)
			{
				_image.Height = _bitmap.Height;
				_image.Width = _bitmap.Width;
			}
			else
			{
				_image.Height = _bitmap.Height * ratio;
				_image.Width = _bitmap.Width * ratio;
			}

			//Place the image in the top right hand corner of the Viewport
			Canvas.SetLeft(_image, _view.ViewportRight - _image.Width);
			Canvas.SetTop(_image, _view.ViewportBottom - _image.Height);

			//add the image to the adornment layer and make it relative to the viewport
			_adornmentLayer.AddAdornment(AdornmentPositioningBehavior.ViewportRelative, null, null, _image, null);
		}
	}
}