using System;
using System.Linq;
using System.Linq.Expressions;
using System.Data.Objects;

namespace SongSearch.Web
{
	// **************************************
	// IDataSession
	// **************************************    
	public interface IDataSessionReadOnly : IDisposable {

		T Single<T>(Expression<Func<T, bool>> expression) where T : class, new();
		IQueryable<T> All<T>() where T : class, new();
		ObjectQuery<T> GetObjectQuery<T>() where T : class, new();
	}

}
