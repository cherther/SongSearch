using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SongSearch.Web.Data;
using SongSearch.Web.Services;
using Ninject;
using Ninject.Modules;
using Ninject.Web.Mvc;
namespace SongSearch.Web.Services {

	public static class PlanBalanceService {

		const int SUPER_ADMIN_BALANCE = 1;

		// ----------------------------------------------------------------------------
		// (Public)
		// ----------------------------------------------------------------------------
		// **************************************
		// AddToUserBalance
		// **************************************
		public static void AddToUserBalance(this User user) {
			using (var ctx = new SongSearchContext()) {

				ctx.AddToUserBalance(user);
				ctx.SaveChanges();

			}
		}

		public static void AddToUserBalance(this SongSearchContext ctx, User user) {
			ctx.UpdateBalance(new PlanBalance() {
				PlanBalanceId = user.PlanBalanceId,
				NumberOfSongs = 0,
				NumberOfInvitedUsers = 1,
				NumberOfCatalogAdmins = 0
			});
		}

		// **************************************
		// AddToAdminBalance
		// **************************************
		public static void AddToAdminBalance(this User user) {
			using (var ctx = new SongSearchContext()) {

				ctx.AddToAdminBalance(user);
				ctx.SaveChanges();

			}
		}
		public static void AddToAdminBalance(this SongSearchContext ctx, User user) {
			ctx.UpdateBalance(new PlanBalance() {
				PlanBalanceId = user.PlanBalanceId,
				NumberOfSongs = 0,
				NumberOfInvitedUsers = 0,
				NumberOfCatalogAdmins = 1
			});
		}

		// **************************************
		// AddToSongsBalance
		// **************************************
		public static void AddToSongsBalance(this User user) {
			using (var ctx = new SongSearchContext()) {

				ctx.AddToSongsBalance(user);
				ctx.SaveChanges();

			}
		}
		public static void AddToSongsBalance(this SongSearchContext ctx, User user) {
			ctx.UpdateBalance(new PlanBalance() {
				PlanBalanceId = user.PlanBalanceId,
				NumberOfSongs = 1,
				NumberOfInvitedUsers = 0,
				NumberOfCatalogAdmins = 0
			});
		}

		// **************************************
		// RemoveFromUserBalance
		// **************************************
		public static void RemoveFromUserBalance(this User user) {
			using (var ctx = new SongSearchContext()) {

				ctx.RemoveFromUserBalance(user);
				if (user.RoleId == (int)Roles.Admin) {
					ctx.RemoveFromAdminBalance(user);
				}
				ctx.SaveChanges();

			}
		}
		public static void RemoveFromUserBalance(this SongSearchContext ctx, User user) {
			ctx.UpdateBalance(new PlanBalance() {
				PlanBalanceId = user.PlanBalanceId,
				NumberOfSongs = 0,
				NumberOfInvitedUsers = -1,
				NumberOfCatalogAdmins = 0
			});
		}

		// **************************************
		// RemoveFromAdminBalance
		// **************************************
		public static void RemoveFromAdminBalance(this User user) {
			using (var ctx = new SongSearchContext()) {

				ctx.RemoveFromAdminBalance(user);
				ctx.SaveChanges();
			}
		}
		public static void RemoveFromAdminBalance(this SongSearchContext ctx, User user) {
			ctx.UpdateBalance(new PlanBalance() {
				PlanBalanceId = user.PlanBalanceId,
				NumberOfSongs = 0,
				NumberOfInvitedUsers = 0,
				NumberOfCatalogAdmins = -1
			});
		}

		// **************************************
		// RemoveFromAdminBalance
		// **************************************
		public static void RemoveFromSongsBalance(this User user) {
			using (var ctx = new SongSearchContext()) {

				ctx.RemoveFromSongsBalance(user);
				ctx.SaveChanges();
			}
		}
		public static void RemoveFromSongsBalance(this SongSearchContext ctx, User user) {
			ctx.UpdateBalance(new PlanBalance() {
				PlanBalanceId = user.PlanBalanceId,
				NumberOfSongs = -1,
				NumberOfInvitedUsers = 0,
				NumberOfCatalogAdmins = 0
			});
		}

		public static void Delete(PlanBalance planBalance) {
			using (var ctx = new SongSearchContext()) {
				ctx.Attach(planBalance);
				ctx.PlanBalances.DeleteObject(planBalance);
				ctx.SaveChanges();
			}
		}
		public static void Delete(this SongSearchContext ctx, PlanBalance planBalance) {
			ctx.PlanBalances.DeleteObject(planBalance);
		}
		// ----------------------------------------------------------------------------
		// (Private)
		// ----------------------------------------------------------------------------
		
		
		// **************************************
		// UpdateBalanceAmounts
		// **************************************
		private static void UpdateBalance(this SongSearchContext ctx, PlanBalance balanceDelta) {
			
			var balance = ctx.PlanBalances.SingleOrDefault(x => x.PlanBalanceId == balanceDelta.PlanBalanceId);
			balance.UpdateAmountsWithDelta(balanceDelta);

			// Update balance for SuperAdmins (=1)
			var superBalance = ctx.PlanBalances.SingleOrDefault(x => x.PlanBalanceId == SUPER_ADMIN_BALANCE);
			superBalance.UpdateAmountsWithDelta(balanceDelta);

		}

		// **************************************
		// NewBalanceAmount
		// **************************************
		private static PlanBalance UpdateAmountsWithDelta(this PlanBalance balance, PlanBalance balanceDelta) {

			if (balance != null && balanceDelta != null) {
				balance.NumberOfSongs = NewBalanceAmount(balance.NumberOfSongs, balanceDelta.NumberOfSongs);
				balance.NumberOfInvitedUsers = NewBalanceAmount(balance.NumberOfInvitedUsers, balanceDelta.NumberOfInvitedUsers);
				balance.NumberOfCatalogAdmins = NewBalanceAmount(balance.NumberOfCatalogAdmins, balanceDelta.NumberOfCatalogAdmins);
			}
			return balance;

		}
		// **************************************
		// NewBalanceAmount
		// **************************************
		private static int NewBalanceAmount(int currentBalance, int balanceTransaction) {
			var newBalance = currentBalance + balanceTransaction;
			return newBalance >= 0 ? newBalance : 0;
		}

	}
}