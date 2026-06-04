using MediatR;

namespace IdentityService.Application.Features.Users.Commands.RegisterUser;


// IRequest<Guid> : Bu işlemin sonucunda bize bir Guid (Kullanıcı ID'si) dönecek demektir.
// record : Sadece veri taşıyan (DTO) değişmez (immutable) nesneler için best practice'tir.
public record RegisterUserCommand(string FirstName, string LastName, string Email, string Password) : IRequest<Guid>;