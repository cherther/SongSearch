using System;
using System.Collections.Generic;
using System.Configuration;

namespace SongSearch.Web {

	// **************************************
	// Configuration
	// **************************************
	public static class Configuration {
		// **************************************
		// Get
		// **************************************
		public static string Get(string key) {
			string value = ConfigurationManager.AppSettings[key];
			if (String.IsNullOrEmpty(value)) {
				throw new KeyNotFoundException(String.Format("{0} is not specified in config file", key));
			}
			return value;
		}

		
	}
}