using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace MusicWord.Models
{
    class TimeModel : PropertyChangedBase
	{
		public event EventHandler TimeOver;
		private int _secondes;
		private Timer timer;
		public TimeModel(int secondes)
		{
			this._secondes = secondes;
		}
		

		public int Secondes
		{
			get { return _secondes; }
		}

		private void OnTimedEvent(Object source, ElapsedEventArgs e)
		{
			_secondes -= 1;
			if (_secondes == 0)
			{
				TimeOver(this, EventArgs.Empty);
			}
			NotifyOfPropertyChange(() => Secondes);
		}
		public void start()
		{
			// timer for one seconed intervals
			timer = new Timer(1000);
			timer.Elapsed += OnTimedEvent;
			timer.AutoReset = true;
			timer.Enabled = true;
		}
		public void stop()
		{
			timer.Stop();
			timer.Dispose();
		}
	}
}
