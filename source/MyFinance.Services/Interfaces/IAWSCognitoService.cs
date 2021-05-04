using Amazon.CognitoIdentityProvider.Model;
using MyFinance.Services.DataTransferObjects;
using System.Threading.Tasks;

namespace MyFinance.Services.Interfaces
{
    public interface IAWSCognitoService
    {
        Task<SignUpResponse> PerformSignUpAsync(UserForCreationDto user);
        Task<AdminInitiateAuthResponse> PerformAuthAsync(UserForCreationDto user);
        SignUpRequest CreateSignUpRequestFromUserDto(UserForCreationDto user);
    }
}
