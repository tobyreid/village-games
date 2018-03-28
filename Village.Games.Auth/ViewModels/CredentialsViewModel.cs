using FluentValidation.Attributes;
using Village.Games.Auth.ViewModels.Validations;

namespace Village.Games.Auth.ViewModels
{
  [Validator(typeof(CredentialsViewModelValidator))]
  public class CredentialsViewModel
  {
    public string UserName { get; set; }
    public string Password { get; set; }
  }
}
