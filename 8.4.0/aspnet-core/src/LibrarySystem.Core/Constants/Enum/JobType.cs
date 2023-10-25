using System.Runtime.Serialization;

namespace LibrarySystem.Constants.Enum
{
    public enum JobType
    {
        [EnumMember(Value = "None")]
        None = 0,

        [EnumMember(Value = "Teacher")]
        Teacher = 1,

        [EnumMember(Value = "Student")]
        Student = 2,

        [EnumMember(Value = "Developer")]
        Developer = 3,
    }
}
