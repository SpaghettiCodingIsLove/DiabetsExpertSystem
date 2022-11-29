using DiabetsAPI.Models.Requests;

namespace DiabetsAPITests
{
    public class ServicesTests : IClassFixture<ServiceFixture>
    {
        private ServiceFixture serviceFixture;

        public ServicesTests(ServiceFixture serviceFixture)
        {
            this.serviceFixture = serviceFixture;
        }

        [Fact]
        public void CryptoTest()
        {
            var testString = "test";
            var encrypted = serviceFixture.CryptoService.Encrypt(testString);
            Assert.Equal(testString, serviceFixture.CryptoService.Decrypt(encrypted));
        }

        [Fact]
        public void CorrectLogin()
        {
            var response = serviceFixture.Controller.Authenticate(new AuthenticateRequest() { Login = "TestD1", Password = "TestD1" });
            Assert.NotNull(response);
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData("", null)]
        [InlineData(null, "")]
        [InlineData("TestD2", "TestD1")]
        [InlineData("TestD2", null)]
        [InlineData("TestD3", "TestD3")]
        public void IncorrectLogin(string login, string password)
        {
            var response = serviceFixture.Controller.Authenticate(new AuthenticateRequest() { Login = login, Password = password });
            Assert.Null(response);
        }

        [Fact]
        public void CreateDoctor()
        {
            CreateDoctorRequest request = new CreateDoctorRequest()
            {
                Login = "TestD3",
                Password = "TestD3",
                Name = "TestD3",
                LastName = "TestD3",
                IsAdmin = true
            };

            var response = serviceFixture.Controller.CreateDoctor(request);
            Assert.NotNull(response);
        }

        [Theory]
        [MemberData(nameof(TestData.DoctorsToCreatIncorrect), MemberType = typeof(TestData))]
        public void CreateIncorrectDoctor(CreateDoctorRequest createDoctorRequest)
        {
            var response = serviceFixture.Controller.CreateDoctor(createDoctorRequest);
            Assert.Null(response);
        }

        [Theory]
        [InlineData(1)]
        public void DeleteDoctor(long doctorId)
        {
            bool before = serviceFixture.Context.Doctors.Any(x => x.Id == doctorId);
            serviceFixture.Controller.DeleteDoctor(doctorId);
            bool after = serviceFixture.Context.Doctors.Any(x => x.Id == doctorId);
            Assert.True(before != after);
        }

        [Theory]
        [InlineData(1)]
        public void DeletePatient(long patientId)
        {
            bool before = serviceFixture.Context.Patients.Any(x => x.Id == patientId);
            serviceFixture.Controller.DeletePatient(patientId);
            bool after = serviceFixture.Context.Patients.Any(x => x.Id == patientId);
            Assert.True(before != after);
        }

        [Fact]
        public void CreatePatientCorrect()
        {
            var request = new CreatePatientRequest { Name = serviceFixture.CryptoService.Encrypt("TestP1"), LastName = serviceFixture.CryptoService.Encrypt("TestP2"), Pesel = serviceFixture.CryptoService.Encrypt("1234567"), BirthDate = new DateTime(2000, 1, 1) };
            var response = serviceFixture.Controller.CreatePatient(request);
            Assert.NotNull(response);
        }

        [Theory]
        [MemberData(nameof(TestData.PatientsToCreatIncorrect), MemberType = typeof(TestData))]
        public void CreatePatientIncorrect(CreatePatientRequest createPatientRequest)
        {
            var response = serviceFixture.Controller.CreatePatient(createPatientRequest);
            Assert.Null(response);
        }

        [Fact]
        public void GetPatients()
        {
            var patients = serviceFixture.Controller.GetPatients();
            Assert.NotEmpty(patients);
        }

        [Fact]
        public void Train()
        {
            TrainingRequest trainingRequest = new TrainingRequest() { DataSet = File.ReadAllText("pima-indians-diabetes.data.csv") };
            var response = serviceFixture.Controller.Train(trainingRequest);
            Assert.True(response.Score != 0);
        }

        [Fact]
        public void Classify()
        {
            var request = new AddExaminationRequest()
            {
                DiabetesPedigreeFunction = 1,
                DoctorId = 1,
                BloodPreasure = 1,
                Glucose = 1,
                Height = 1,
                Insulin = 1,
                PatientId = 1,
                Pregnancies = 1,
                SkinThickness = 1,
                Weight = 1
            };

            var response = serviceFixture.Controller.AddExamination(request);
            Assert.True(response.Outcome == 0 || response.Outcome == 1);
        }

        [Fact]
        public void GetExaminationsOk()
        {
            var examinations = serviceFixture.Controller.GetExaminations(1);
            Assert.NotEmpty(examinations);
        }

        [Fact]
        public void GetExaminationsEmpty()
        {
            var examinations = serviceFixture.Controller.GetExaminations(2);
            Assert.Empty(examinations);
        }

        [Fact]
        public void ChangePasswordCorrect()
        {
            var request = new ChangePasswordRequest()
            {
                OldPassword = "TestD1",
                NewPassword = "NewTestD1"
            };

            Assert.True(serviceFixture.Controller.ChangePassword(request, 1));
        }

        [Theory]
        [MemberData(nameof(TestData.ChangePasswordIncorrct), MemberType = typeof(TestData))]
        public void ChangePasswordIncorrect(ChangePasswordRequest request, long id)
        {
            Assert.False(serviceFixture.Controller.ChangePassword(request, id));
        }
    }
}