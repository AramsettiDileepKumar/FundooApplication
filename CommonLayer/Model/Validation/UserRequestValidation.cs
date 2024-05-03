using CommonLayer.Model.RequestDTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CommonLayer.Model.Validation
{
    public class UserRequestValidation : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            UserRequest valid = validationContext.ObjectInstance as UserRequest;
            if (valid != null)
            {
                if (Regex.IsMatch(valid.UserFirstName, @"^[a-zA-Z]{1,20}$"))
                {
                    if (Regex.IsMatch(valid.UserLastName, "^[a-zA-Z]{1,20}$"))
                    {
                        if (!Regex.IsMatch(valid.UserEmail, @"^([a-z0-9]+)@([a-z]+)((\.[a-z]{2,3})+)$"))
                        {

                            return new ValidationResult("Give Email id in correct Format");
                        }
                        else
                        {
                            if (!Regex.IsMatch(valid.UserPassword, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d!@#$%^&*]{6,16}$"))

                            {
                                return new ValidationResult("Given Password is in correct Format");
                            }
                        }
                    }
                    else return new ValidationResult("Given LastName is in correct Format");
                }
                else return new ValidationResult("Given FirstName is in correct Format");

                return ValidationResult.Success;
            }
            return new ValidationResult("The given object is null");
        }
    }

}
