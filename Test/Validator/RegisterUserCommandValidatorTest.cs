using Application.Command;
using Application.Validator;
using FluentValidation.TestHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Validator
{
    public class RegisterUserCommandValidatorTest
    {
        private readonly RegisterUserCommandValidator _validator = new();

        [Fact]
        public void SuccessScenario()
        {
            var cmd = new RegisterUserCommand
            {
                Email = "john.doe@test.com",
                Password = "Abcdef1!"
            };

            var result = _validator.TestValidate(cmd);
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void EmptyEmail()
        {
            var cmd = new RegisterUserCommand { Email = "", Password = "Abcdef1!" };

            var result = _validator.TestValidate(cmd);
            result.ShouldHaveValidationErrorFor(x => x.Email)
                  .WithErrorMessage("User email is required.");
        }

        [Fact]
        public void EmailExceedsCharacters()
        {
            var longLocal = new string('a', 101);
            var cmd = new RegisterUserCommand { Email = $"{longLocal}@mail.com", Password = "Abcdef1!" };

            var result = _validator.TestValidate(cmd);
            result.ShouldHaveValidationErrorFor(x => x.Email)
                  .WithErrorMessage("user email must not exceed 100 characters.");
        }

        [Fact]
        public void EmptyPassword()
        {
            var cmd = new RegisterUserCommand { Email = "john@doe.com", Password = "" };

            var result = _validator.TestValidate(cmd);
            result.ShouldHaveValidationErrorFor(x => x.Password)
                  .WithErrorMessage("User password is required.");
        }

        [Fact]
        public void PasswordExceedsChacters()
        {
            var longPwd = new string('A', 51);
            var cmd = new RegisterUserCommand { Email = "john@doe.com", Password = longPwd };

            var result = _validator.TestValidate(cmd);
            result.ShouldHaveValidationErrorFor(x => x.Password)
                  .WithErrorMessage("user password must not exceed 50 characters.");
        }

        [Fact]
        public void ValidPassword()
        {
            var cmd = new RegisterUserCommand
            {
                Email = "john@doe.com",
                Password = "ValidPass1!"
            };

            var result = _validator.TestValidate(cmd);
            result.ShouldNotHaveValidationErrorFor(x => x.Password);
        }

    }
}
