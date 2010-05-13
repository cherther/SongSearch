using System;
using System.Linq;
using System.Linq.Expressions;

namespace SongSearch.Web.Data
{
	// **************************************
	// ISession
	// **************************************    
	public interface IReadOnlyRepository : IDisposable {

		T Single<T>(Expression<Func<T, bool>> expression) where T : class;
		IQueryable<T> All<T>() where T : class;

	}

}
