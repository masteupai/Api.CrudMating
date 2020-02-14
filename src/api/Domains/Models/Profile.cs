using Google.Protobuf.WellKnownTypes;
using System.Runtime.Serialization;

namespace API.Domains.Models
{
    public enum Profile
    {
        [EnumMember(Value = "Administrator")]
        Administrator = 1,

        [EnumMember(Value = "Funcionario")]
        Funcionario = 2,

        [EnumMember(Value = "Cliente")]
        Cliente = 3
    }
}