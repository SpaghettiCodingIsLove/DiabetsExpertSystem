using DiabetsAPI.Models.Requests;

namespace DiabetsAPITests
{
    public class TestData
    {
        public static IEnumerable<object[]> DoctorsToCreatIncorrect
        {
            get
            {
                yield return new object[]
                {
                    new CreateDoctorRequest()
                    {
                        Login = "TestD3",
                        Password = null,
                        Name = "TestD3",
                        LastName = "TestD3",
                        IsAdmin = true
                    }
                };
                yield return new object[]
                {
                    new CreateDoctorRequest()
                    {
                        Login = "TestD3",
                        Password = " ",
                        Name = "TestD3",
                        LastName = "TestD3",
                        IsAdmin = true
                    }
                };
                yield return new object[]
                {
                    new CreateDoctorRequest()
                    {
                        Login = "TestD2",
                        Password = "TestD3",
                        Name = "TestD3",
                        LastName = "TestD3",
                        IsAdmin = true
                    }
                };
            }
        }

        public static IEnumerable<object[]> PatientsToCreatIncorrect
        {
            get
            {
                yield return new object[]
                {
                    new CreatePatientRequest()
                    {
                        Name = "TestD3",
                        LastName = "TestD3",
                        BirthDate = DateTime.Now,
                        Pesel = null
                    }
                };
                yield return new object[]
                {
                    new CreatePatientRequest()
                    {
                        Name = "TestD3",
                        LastName = "TestD3",
                        BirthDate = DateTime.Now,
                        Pesel = "  "
                    }
                };
                yield return new object[]
                {
                    new CreatePatientRequest()
                    {
                        Name = "TestD3",
                        LastName = "TestD3",
                        BirthDate = DateTime.Now,
                        Pesel = "123456"
                    }
                };
            }
        }

        public static IEnumerable<object[]> ChangePasswordIncorrct
        {
            get
            {
                yield return new object[]
                {
                    new ChangePasswordRequest()
                    {
                        OldPassword = null,
                        NewPassword = "    ",
                    },
                    1
                };
                yield return new object[]
                {
                    new ChangePasswordRequest()
                    {
                        OldPassword = null,
                        NewPassword = "TestD3",
                    },
                    1
                };
                yield return new object[]
                {
                    new ChangePasswordRequest()
                    {
                        OldPassword = "TestD3",
                        NewPassword = null,
                    },
                    1
                };
                yield return new object[]
                {
                    new ChangePasswordRequest()
                    {
                        OldPassword = "TestD2",
                        NewPassword = "TestD1",
                    },
                    1
                };
                yield return new object[]
                {
                    new ChangePasswordRequest()
                    {
                        OldPassword = "TestD1",
                        NewPassword = "TestD1",
                    },
                    3
                };
            }
        }
    }
}
