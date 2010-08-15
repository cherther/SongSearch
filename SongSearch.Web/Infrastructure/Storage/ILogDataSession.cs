using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Data.Objects;

namespace SongSearch.Web
{
	// **************************************
	// IDataSession
	// **************************************    
	public interface ILogDataSession : IDisposable {

		void CommitChanges();

		void Add<T>(T item) where T : class, new();
		void Add<T>(IEnumerable<T> items) where T : class, new();
		void QuickAdd<T>(T item) where T : class, new();
	}

}
