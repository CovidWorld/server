using System.Linq;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sygic.Corona.Domain.Common;

namespace Sygic.Corona.Application.Validations
{
    public class ValidationProcessor
    {
        public ValidationProblemDetails ProcessErrors(DomainException ex)
        {
            var problemDetails = new ValidationProblemDetails
            {
                Instance = "CreateDeviceProfile",
                Status = StatusCodes.Status400BadRequest,
                Detail = "Please refer to the errors property for additional details."
            };

            if (ex.InnerException is ValidationException validationException)
            {
                var propertyErrors = validationException.Errors.GroupBy(x => x.PropertyName);

                foreach (var propertyError in propertyErrors)
                {
                    problemDetails.Errors.Add(propertyError.Key, propertyError.Select(x => x.ErrorMessage).ToArray());
                }
            }
            else
            {
                problemDetails.Errors.Add("DomainValidations", new[] { ex.Message });
            }

            return problemDetails;
        }
    }
}
