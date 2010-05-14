using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace SongSearch.Web.Data
{
	// **************************************
	// ISession
	// **************************************    
	public interface ISession : IDisposable {

		void CommitChanges();

		void Delete<T>(Expression<Func<T, bool>> expression) where T : class;
		void Delete<T>(T item) where T : class;
		//void DeleteMany<T>() where T : class;
		void DeleteMany<T>(IEnumerable<T> items) where T : class;
		T Single<T>(Expression<Func<T, bool>> expression) where T : class;
		T Single<T>(Expression<Func<T, bool>> expression, string loadWith) where T : class;
		IQueryable<T> All<T>() where T : class;
		IQueryable<T> All<T>(string loadWith) where T : class;
		void Add<T>(T item) where T : class;
		void Add<T>(IEnumerable<T> items) where T : class;
		void Update<T>(T item) where T : class;
		void Save<T>(T item) where T : class;		

	}

}
