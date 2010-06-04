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
	public interface IDataSession : IDisposable {

		void CommitChanges();

		IQueryable<T> All<T>() where T : class, new();
//		IQueryable<T> All<T>(string loadWith = null) where T : class, new();
		IQueryable<T> All<T>(string commandText, params object[] parameters) where T : class, new();
		ObjectQuery<T> GetObjectQuery<T>() where T : class, new();

		T Single<T>(Expression<Func<T, bool>> expression) where T : class, new();

		void Add<T>(T item) where T : class, new();
		void Add<T>(IEnumerable<T> items) where T : class, new();
		void Update<T>(T item) where T : class, new();
		void Delete<T>(Expression<Func<T, bool>> expression) where T : class, new();
		void Delete<T>(T item) where T : class, new();
		//void DeleteMany<T>() where T : class;
		void DeleteMany<T>(IEnumerable<T> items) where T : class, new();
		//void Save<T>(T item) where T : class;		

	}

}
