using System;
using System.Collections.Generic;
using System.Linq;

namespace CRM.ServiceCommon.Services.CodeService.Models
{
    public abstract class CodeResult
    {
        public ICollection<string> Errors { get; } = new List<string>();

        public bool IsSucceed()
        {
            return !Errors.Any();
        }

        public string GetErrorsString()
        {
            return string.Join(Environment.NewLine, Errors);
        }
    }
}