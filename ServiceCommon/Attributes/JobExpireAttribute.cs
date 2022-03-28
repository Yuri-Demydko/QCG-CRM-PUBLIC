using System;
using Hangfire.Common;
using Hangfire.States;
using Hangfire.Storage;

namespace CRM.ServiceCommon.Attributes
{
    public class JobExpireAttribute : JobFilterAttribute, IApplyStateFilter
    {
        private readonly int jobExpiration;

        public JobExpireAttribute(int jobExpiration)
        {
            this.jobExpiration = jobExpiration;
        }

        public void OnStateApplied(ApplyStateContext context, IWriteOnlyTransaction transaction)
        {
            context.JobExpirationTimeout = TimeSpan.FromSeconds(jobExpiration);
        }

        public void OnStateUnapplied(ApplyStateContext context, IWriteOnlyTransaction transaction)
        {
            context.JobExpirationTimeout = TimeSpan.FromSeconds(jobExpiration);
        }
    }
}