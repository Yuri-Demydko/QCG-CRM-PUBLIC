using System;

namespace CRM.ServiceCommon.Services.DbEventHandlerService
{
    public class DbEventHandler
    {
        public Func<DbEvent, bool> Pattern { get; set; }

        public Action<DbEvent> Handler { get; set; }
    }
}