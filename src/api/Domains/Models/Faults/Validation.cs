using System.Runtime.Serialization;

namespace API.Domains.Models.Faults
{
    public enum Validation
    {
        PaginationExceedsLimits = 1,
        UserNotInformed = 2,
        UserProfileNotInformed = 3,
        UserCountryNotInfored = 4,
        UserNameNotInformed = 5,
        UserNameExceedsLimit = 6,
        UserEmailNotInformed = 7,
        UserEmailExceedsLimit = 8,
        UserEmailNotValid = 9,
        UserDocumentNotInformed = 10,
        UserDocumentInvalid = 11,
        UserBirthdateNotInformed = 12,
        UserBirthdateInvalid = 13,
        UserRepeatedDocument = 14,
        UserRepeatedEmail = 15,
        ProductExists = 16,
        ProductNotExists = 17,
        FuncionarioExists = 18,
        FuncionarioNotExists = 19,
        FuncionarioNotInformed = 20,
        ClienteExists = 21,
        ClienteNotExists = 22,
        ClienteNotInformed = 23,
        ContatoExists= 24,
        ContatoNotExists= 25,
        ContatoNotInformed= 26,
        EnderecoExist = 27,
        EnderecoNotExists = 28,
        EnderecoNotInformed = 29,
    }
}
