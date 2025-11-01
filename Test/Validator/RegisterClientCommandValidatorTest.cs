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
    public class RegisterClientCommandValidatorTest
    {
        private readonly RegisterClientCommandValidator _validator = new();

        [Fact]
        public void SuccessScenario()
        {
            var cmd = new RegisterClientCommand
            {
                Nome = "Maria Silva",
                Cpf = "39053344705",
                ValorLimite = 500
            };

            var result = _validator.TestValidate(cmd);
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void EmptyName()
        {
            var cmd = new RegisterClientCommand
            {
                Nome = "",
                Cpf = "39053344705",
                ValorLimite = 100
            };

            var result = _validator.TestValidate(cmd);
            result.ShouldHaveValidationErrorFor(x => x.Nome)
                  .WithErrorMessage("nome é obrigatório");
        }

        [Fact]
        public void EmptyCPF()
        {
            var cmd = new RegisterClientCommand
            {
                Nome = "João",
                Cpf = "",
                ValorLimite = 100
            };

            var result = _validator.TestValidate(cmd);
            result.ShouldHaveValidationErrorFor(x => x.Cpf)
                  .WithErrorMessage("cpf é obrigatório");
        }

        [Fact]
        public void InvalidCPF()
        {
            var cmd = new RegisterClientCommand
            {
                Nome = "João",
                Cpf = "12345678900",
                ValorLimite = 100
            };

            var result = _validator.TestValidate(cmd);
            result.ShouldHaveValidationErrorFor(x => x.Cpf)
                  .WithErrorMessage("CPF inválido.");
        }

        [Fact]
        public void CPFLessThan11digits()
        {
            var cmd = new RegisterClientCommand
            {
                Nome = "João",
                Cpf = "12345678",
                ValorLimite = 100
            };

            var result = _validator.TestValidate(cmd);
            result.ShouldHaveValidationErrorFor(x => x.Cpf)
                  .WithErrorMessage("CPF inválido.");
        }

        [Fact]
        public void NegativeValue()
        {
            var cmd = new RegisterClientCommand
            {
                Nome = "Maria",
                Cpf = "39053344705",
                ValorLimite = -50
            };

            var result = _validator.TestValidate(cmd);
            result.ShouldHaveValidationErrorFor(x => x.ValorLimite)
                  .WithErrorMessage("valorLimite não pode ser negativo");
        }

        [Fact]
        public void LimitZero()
        {
            var cmd = new RegisterClientCommand
            {
                Nome = "Ana",
                Cpf = "39053344705",
                ValorLimite = 0
            };

            var result = _validator.TestValidate(cmd);
            result.ShouldNotHaveAnyValidationErrors();
        }

    }
}
