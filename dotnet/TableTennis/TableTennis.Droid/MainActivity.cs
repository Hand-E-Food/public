using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System;

namespace TableTennis.Droid
{
	[Activity(Label = "@string/main_activity", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity
	{
        private Button StartButton;

        /// <summary>
        /// Initialises this <see cref="Activity"/>.
        /// </summary>
        /// <param name="bundle">The <see cref="Bundle"/> passed from the previous <see cref="Activity"/>.</param>
		protected override void OnCreate(Bundle bundle)
		{
            RequestWindowFeature(WindowFeatures.NoTitle);

            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Main);
            StartButton = FindViewById<Button>(Resource.Id.StartButton);

            StartButton.Click += StartButton_Click;
        }

        /// <summary>
        /// Moves to the next screen.
        /// </summary>
        private void StartButton_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(TeamInputActivity));
        }

        /// <summary>
        /// Removes all view events and nulls all fields.
        /// </summary>
        /// <param name="disposing">True if this object is being disposed programatically; false if this object is being finalised.</param>
        #region protected override void Dispose(bool disposing)
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing && !IsDisposed)
                {
                    StartButton.Click -= StartButton_Click;

                    StartButton = null;
                }
                IsDisposed = true;
            }
            finally
            {
                base.Dispose(disposing);
            }
        }
        /// <summary>
        /// True if this object is disposed; otherwise, false.
        /// </summary>
        public bool IsDisposed { get; private set; } = false;
        #endregion
    }
}
