using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Microsoft.Extensions.Options;
using MyFinance.Services.DataTransferObjects;
using MyFinance.Services.Helpers;
using MyFinance.Services.Interfaces;
using System.Threading.Tasks;

namespace MyFinance.Services
{
    public class AWSCognitoService : IAWSCognitoService
    {
        private readonly IOptions<AWSCognitoSettings> _settings;

        public AWSCognitoService(IOptions<AWSCognitoSettings> settings)
        {
            _settings = settings;
        }

        public async Task<SignUpResponse> PerformSignUpAsync(UserForCreationDto user)
        {
            var signUpRequest = CreateSignUpRequestFromUserDto(user);

            AmazonCognitoIdentityProviderClient _client = new AmazonCognitoIdentityProviderClient();

            var result = await _client.SignUpAsync(signUpRequest);

            return result;
        }

        public SignUpRequest CreateSignUpRequestFromUserDto(UserForCreationDto user)
        {
            var signUpRequest = new SignUpRequest
            {
                ClientId = _settings.Value.UserPoolClientId,
                Password = user.Password,
                Username = user.UserName,
            };

            var emailAttribute = new AttributeType
            {
                Name = "email",
                Value = user.Email
            };

            var firstNameAttr = new AttributeType
            {
                Name = "given_name",
                Value = user.FirstName
            };

            var lastName = new AttributeType
            {
                Name = "family_name",
                Value = user.LastName
            };
            signUpRequest.UserAttributes.Add(emailAttribute);
            signUpRequest.UserAttributes.Add(firstNameAttr);
            signUpRequest.UserAttributes.Add(lastName);

            return signUpRequest;
        }

        public async Task<AdminInitiateAuthResponse> PerformAuthAsync(UserForCreationDto user)
        {
            var authRequest = new AdminInitiateAuthRequest
            {
                UserPoolId = _settings.Value.UserPoolId,
                ClientId = _settings.Value.UserPoolClientId,
                AuthFlow = AuthFlowType.ADMIN_NO_SRP_AUTH
            };

            authRequest.AuthParameters.Add("USERNAME", user.UserName);
            authRequest.AuthParameters.Add("PASSWORD", user.Password);

            AmazonCognitoIdentityProviderClient _client = new AmazonCognitoIdentityProviderClient();

            AdminInitiateAuthResponse authResponse = await _client.AdminInitiateAuthAsync(authRequest);

            return authResponse;
        }
    }
}
