using FluentValidation.Attributes;
using Village.Idiot.ViewModels.Validations;

namespace Village.Idiot.ViewModels
{
  [Validator(typeof(CredentialsViewModelValidator))]
  public class CredentialsViewModel
  {
    public string UserName { get; set; }
    public string Password { get; set; }
  }
}
