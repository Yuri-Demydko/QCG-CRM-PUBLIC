using System.Collections.Generic;

namespace CRM.DAL.Models.ResponseModels.Error
{
    public class ErrorResponse
    {
        /// <summary>
        /// Список ошибок
        /// </summary>
        public List<Error> Errors { get; set; }

        public ErrorResponse(string error, string parameter = null)
        {
            Errors = new List<Error>
            {
                new Error(error, parameter)
            };
        }

        public ErrorResponse(List<Error> errors)
        {
            Errors = errors;
        }
    }

    public class Error
    {
        /// <summary>
        /// Сообщение ошибки
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Некорректный параметр
        /// </summary>
        public string Parameter { get; set; }

        public Error(string message, string parameter = null)
        {
            Message = message;
            Parameter = parameter;
        }
    }
}