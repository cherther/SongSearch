﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;

namespace SongSearch.Web {
	public static class Mail {

		public static void SendMail(string from, string to, string subject, string message) {
			//            string from = Config"Ford Music Services<info@fordmusicservices.com>";
			using (var cl = new SmtpClient()) {
				using (var msg = new MailMessage()) {
					msg.From = new MailAddress(from);
					msg.To.Add(to);
					msg.CC.Add(SiteProfileData.SiteProfile().AdminEmail);
					msg.Subject = subject;
					msg.IsBodyHtml = true;
					msg.Body = message;

					cl.EnableSsl = true;
					cl.Send(msg); //from, to, subject, message);
				}
			}
		}

	}
}