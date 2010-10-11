using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace SongSearch.Web {
	public static class Log {

		public static void Debug(string message) {
			Console.WriteLine(message);
			System.Diagnostics.Debug.WriteLine(message);
		}

		public static void Error(Exception x) {
#if DEBUG
			System.Diagnostics.Debug.WriteLine(x.Message);
#else

			Elmah.ErrorSignal.FromCurrentContext().Raise(x);

#endif
		}
	}
}

