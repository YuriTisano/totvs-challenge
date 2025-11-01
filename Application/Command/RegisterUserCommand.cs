using MediatR;

namespace Application.Command
{
    public class RegisterUserCommand : IRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
