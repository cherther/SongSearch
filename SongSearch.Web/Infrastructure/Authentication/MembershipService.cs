﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SongSearch.Web.Services {

	public class AccountMembershipService : IMembershipService {

        public IAccountService AccountService { get; set; }

        private const int _MIN_PASSWORD_LENGTH = 5;

        public AccountMembershipService()
        {
            if (AccountService == null) { AccountService = new AccountService(); }
        }

        public int MinPasswordLength
        {
            get
            {
                return _MIN_PASSWORD_LENGTH; // _provider.MinRequiredPasswordLength;
            }
        }

        public static int MIN_PASSWORD_LENGTH
        {
            get
            {
                return _MIN_PASSWORD_LENGTH; // _provider.MinRequiredPasswordLength;
            }
        }


        public bool ValidateUser(string userName, string password)
        {
            if (String.IsNullOrEmpty(userName)) throw new ArgumentException("Value cannot be null or empty.", "userName");
            if (String.IsNullOrEmpty(password)) throw new ArgumentException("Value cannot be null or empty.", "password");

            return AccountService.UserIsValid(userName, password);
        }

        public bool RegisterUser(RegisterModel model)
        {
//            if (String.IsNullOrEmpty(model.Email)) throw new ArgumentException("Value cannot be null or empty.", "userName");
            if (String.IsNullOrEmpty(model.Email)) throw new ArgumentException("Value cannot be null or empty.", "Email");
            if (String.IsNullOrEmpty(model.Password)) throw new ArgumentException("Value cannot be null or empty.", "Password");
            if (String.IsNullOrEmpty(model.InviteId)) throw new ArgumentException("Value cannot be null or empty.", "InviteId");

            DisplayUser user = new DisplayUser()
            {
                UserName = model.Email,
                Password = model.Password,
                FirstName = model.FirstName,
                LastName = model.LastName,
                ParentUserId = model.Invitation.InvitedByUserId,
                Invitation = model.Invitation
            };
            AccountService.RegisterUser(user);
            return true;
        }

        public bool UpdateProfile(UpdateProfileModel model)
        {
            if (String.IsNullOrEmpty(model.Email)) throw new ArgumentException("Value cannot be null or empty.", "userName");
            //if (String.IsNullOrEmpty(model.OldPassword)) throw new ArgumentException("Value cannot be null or empty.", "oldPassword");
            //if (String.IsNullOrEmpty(model.NewPassword)) throw new ArgumentException("Value cannot be null or empty.", "newPassword");

            // The underlying UpdateProfile() will throw an exception rather
            // than return false in certain failure scenarios.
            try
            {
                //MembershipUser currentUser = _provider.GetUserSimple(userName, true /* userIsOnline */);
                DisplayUser currentUser = new DisplayUser() 
                { 
                        UserName = model.Email,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Signature = model.Signature,
                        Password = model.OldPassword 
                };
                return AccountService.UpdateProfile(currentUser, model.NewPassword);
            }
            catch (ArgumentException)
            {
                return false;
            }
            catch (MembershipPasswordException)
            {
                return false;
            }
        }

        public bool ResetPasswordProcessRequest(ResetPasswordModel model)
        {
            if (String.IsNullOrEmpty(model.Email)) throw new ArgumentException("Value cannot be null or empty.", "NewPassword");

            try
            {
                DisplayUser currentUser = new DisplayUser() { UserName = model.Email, Password = model.ResetCode };
                //reset temp password to hashed username
                return AccountService.ResetPassword(currentUser, model.Email);
            }
            catch (ArgumentException)
            {
                return false;
            }
            catch (MembershipPasswordException)
            {
                return false;
            }
        }
        
        public bool ResetPassword(ResetPasswordModel model)
        {
            if (String.IsNullOrEmpty(model.Email)) throw new ArgumentException("Value cannot be null or empty.", "NewPassword");
            if (String.IsNullOrEmpty(model.NewPassword)) throw new ArgumentException("Value cannot be null or empty.", "NewPassword");

            // The underlying UpdateProfile() will throw an exception rather
            // than return false in certain failure scenarios.
            try
            {
                //MembershipUser currentUser = _provider.GetUserSimple(userName, true /* userIsOnline */);
                DisplayUser currentUser = new DisplayUser() { UserName = model.Email, Password = model.ResetCode };
                return AccountService.ResetPassword(currentUser, model.NewPassword);
            }
            catch (ArgumentException)
            {
                return false;
            }
            catch (MembershipPasswordException)
            {
                return false;
            }
        }

    }
}