using System;

namespace CRM.ServiceCommon.Services.CodeService
{
    public class EmailCodeConfiguration
    {
        public readonly TimeSpan CodeLifeTime;

        public EmailCodeConfiguration(TimeSpan codeLifeTime)
        {
            CodeLifeTime = codeLifeTime;
        }
    }
}