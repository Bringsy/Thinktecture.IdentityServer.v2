using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Caching;
using BrockAllen.MembershipReboot;

namespace Thinktecture.IdentityServer.Web.Areas.Account
{
    public class TemplateNotificationService : CustomNotificationService
    {
        public TemplateNotificationService(IMessageDelivery messageDelivery, ApplicationInformation appInfo)
            : base(messageDelivery, appInfo)
        {
        }

        protected virtual string GetTemplateFormat(string name)
        {
            var cache = MemoryCache.Default;
            var fileContents = cache[name] as string;

            if (string.IsNullOrWhiteSpace(fileContents))
            {
                var fileInfo = new FileInfo(string.Format(@"{0}Templates\{1}.html", AppDomain.CurrentDomain.BaseDirectory, name));
                if (fileInfo.Exists)
                {
                    var filePath = fileInfo.FullName;

                    // clean cache in case file gets modified 
                    var policy = new CacheItemPolicy();
                    var files = new List<string> { filePath };
                    policy.ChangeMonitors.Add(new HostFileChangeMonitor(files));

                    // Fetch the file contents.
                    fileContents = File.ReadAllText(filePath);

                    cache.Set(name, fileContents, policy);
                }
            }

            return fileContents;
        }

        protected override string GetAccountCreateFormat()
        {
            var format = this.GetTemplateFormat("AccountCreate");
            return !String.IsNullOrWhiteSpace(format) ? format : base.GetAccountCreateFormat();
        }

        protected override string GetAccountDeleteFormat()
        {
            var format = this.GetTemplateFormat("AccountAccountDelete");
            return !String.IsNullOrWhiteSpace(format) ? format : base.GetAccountDeleteFormat();
        }

        protected override string GetAccountNameReminderFormat()
        {
            var format = this.GetTemplateFormat("AccountNameReminder");
            return !String.IsNullOrWhiteSpace(format) ? format : base.GetAccountNameReminderFormat();
        }

        protected override string GetAccountVerifiedFormat()
        {
            var format = this.GetTemplateFormat("AccountVerified");
            return !String.IsNullOrWhiteSpace(format) ? format : base.GetAccountVerifiedFormat();
        }

        protected override string GetChangeEmailRequestNoticeFormat()
        {
            var format = this.GetTemplateFormat("ChangeEmailRequestNotice");
            return !String.IsNullOrWhiteSpace(format) ? format : base.GetChangeEmailRequestNoticeFormat();
        }

        protected override string GetEmailChangedNoticeFormat()
        {
            var format = this.GetTemplateFormat("EmailChangedNotice");
            return !String.IsNullOrWhiteSpace(format) ? format : base.GetEmailChangedNoticeFormat();
        }

        protected override string GetPasswordChangeNoticeFormat()
        {
            var format = this.GetTemplateFormat("PasswordChangeNotice");
            return !String.IsNullOrWhiteSpace(format) ? format : base.GetPasswordChangeNoticeFormat();
        }

        protected override string GetResetPasswordFormat()
        {
            var format = this.GetTemplateFormat("ResetPassword");
            return !String.IsNullOrWhiteSpace(format) ? format : base.GetResetPasswordFormat();
        }

        protected override string GetUsernameChangedNoticeFormat()
        {
            var format = this.GetTemplateFormat("UsernameChangedNotice");
            return !String.IsNullOrWhiteSpace(format) ? format : base.GetUsernameChangedNoticeFormat();
        }
    }
}