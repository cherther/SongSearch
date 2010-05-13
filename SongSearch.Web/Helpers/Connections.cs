using System;
using System.Collections.Generic;
using System.Configuration;

namespace SongSearch.Web.Data
{
	public enum ConnectionStrings
	{
		FordMusicConnectionString,
		SongSearchContext
	}

    // **************************************
    // Configuration
    // **************************************
    public static class Connections
    {
        // **************************************
        // Get
        // **************************************
		//public static string Get(string key)
		//{
		//    string value = ConfigurationManager.AppSettings[key];
		//    if (String.IsNullOrEmpty(value)) { throw new KeyNotFoundException(String.Format("{0} is not specified in config file", key)); }
		//    return value;
		//}

		public static string ConnectionString(ConnectionStrings connection)
		{
			var key = String.Format("{0}", connection);

			string value = ConfigurationManager.ConnectionStrings[key].ConnectionString;
			if (String.IsNullOrEmpty(value)) { throw new KeyNotFoundException(String.Format("{0} is not specified in config file", key)); }
			return value;
		}
    }
}