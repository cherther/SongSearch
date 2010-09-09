using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using NLog;
using SongSearch.Web.Logging;

namespace SongSearch.Web {
	public static class Log {

		public static void Debug(string message) {
#if DEBUG
			Console.WriteLine(message);
			System.Diagnostics.Debug.WriteLine(message);
#else
			App.Logger.Info(message);
#endif
		}
		public static void Error(string message) {
#if DEBUG
			Console.WriteLine(message);
			System.Diagnostics.Debug.WriteLine(message);
#else
			Mail.SendMail(SystemConfig.AdminEmailAddress, SystemConfig.AdminEmailAddress, 
				String.Format("[WorldSongNet Exception]: {0}", message), 
				String.Format("Exception occured at {0}:\n{1}", DateTime.Now, message));

			App.Logger.Error(message);
#endif
		}
		public static void Error(Exception x) {
#if DEBUG
			System.Diagnostics.Debug.WriteLine(LogUtility.BuildExceptionMessage(x));
#else
			var message = LogUtility.BuildExceptionMessage(x);
			Mail.SendMail(SystemConfig.AdminEmailAddress, SystemConfig.AdminEmailAddress,
				String.Format("[WorldSongNet Exception]: {0}", message),
				String.Format("Exception occured at {0}:\n{1}", DateTime.Now, message));
			App.Logger.Error(x);
#endif
		}
	}
}
namespace SongSearch.Web.Logging {
	public class NLogLogger:ILogger {

		private Logger _logger;

		public NLogLogger() {
			_logger = LogManager.GetCurrentClassLogger();
		}

		public void Info(string message) {
			_logger.Info(message);
		}

		public void Warn(string message) {
			_logger.Warn(message);
		}

		public void Debug(string message) {
			_logger.Debug(message);
		}

		public void Error(string message) {
			_logger.Error(message);
		}
		public void Error(Exception x) {
			Error(LogUtility.BuildExceptionMessage(x));
		}
		public void Fatal(string message) {
			_logger.Fatal(message);
		}
		public void Fatal(Exception x) {
			Fatal(LogUtility.BuildExceptionMessage(x));
		}
	}
}
