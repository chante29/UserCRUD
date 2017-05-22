using System;
using System.ServiceModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using SharedLibrary;
using UserCRUDRest;
using UserCRUDRestTest.Utils;
using System.Threading;
using System.Net.Http;
using System.Collections.Generic;
using System.Globalization;

namespace UserCRUDRestTest
{
    [TestClass]
    public class UserCRUDTest
    {
        private const string ApiUriUserCRUD = "http://localhost:50419/UserCRUD.svc";
        private static readonly object LockTestMethod = new object();
        private static ServiceHost _apiUserCRUDService;
        private static GenericApiCall _genericApiCaller;

        #region Initialize and Cleanup Methods

        [ClassInitialize]
        public static void MyClassInitialize(TestContext testContext)
        {
            _apiUserCRUDService = new ServiceHost(typeof(UserCRUD), new Uri(ApiUriUserCRUD));

            _apiUserCRUDService.Open();

            _genericApiCaller = new GenericApiCall(ApiUriUserCRUD);
        }

        [ClassCleanup]
        public static void MyClassCleanup()
        {
            if (_apiUserCRUDService.State != CommunicationState.Closed)
            {
                _apiUserCRUDService.Close();
            }
        }

        [TestInitialize]
        public void MyTestInitialize()
        {
            Monitor.Enter(LockTestMethod);
        }

        [TestCleanup]
        public void MyTestCleanup()
        {
            Monitor.Exit(LockTestMethod);
        }

        #endregion

        #region CreateUserTests
        [TestMethod]
        public void WhenCreateUser_ThenMethodValidateNewUserHasToBeCalled()
        {
            var userTransactionProvider = MockRepository.GenerateStub<IUserTransaction>();
            var user = MockRepository.GenerateStub<UserCRUDRest.User>();

            userTransactionProvider.Stub(userTransaction => userTransaction.ValidateNewUser(Arg<SharedLibrary.User>.Is.Anything)).Return(true);

            var userCRUD = new UserCRUD(userTransactionProvider);
            var userId = userCRUD.CreateUser(user);

            userTransactionProvider.AssertWasCalled(userTransaction => userTransaction.ValidateNewUser(Arg<SharedLibrary.User>.Is.Anything));

        }

        [TestMethod]
        public void WhenCreateUser_ThenMethodValidateNewUserIsTrue_ThenAddNewUserHasToBeCalled()
        {
            var userTransactionProvider = MockRepository.GenerateStub<IUserTransaction>();
            var user = MockRepository.GenerateStub<UserCRUDRest.User>();

            userTransactionProvider.Stub(userTransaction => userTransaction.ValidateNewUser(Arg<SharedLibrary.User>.Is.Anything)).Return(true);
            userTransactionProvider.Stub(userTransaction => userTransaction.AddNewUser(Arg<SharedLibrary.User>.Is.Anything)).Return(1);

            var userCRUD = new UserCRUD(userTransactionProvider);
            var userId = userCRUD.CreateUser(user);

            userTransactionProvider.AssertWasCalled(userTransaction => userTransaction.AddNewUser(Arg<SharedLibrary.User>.Is.Anything));

        }

        [TestMethod]
        public void WhenCreateUser_ThenMethodValidateNewUserIsFalse_ThenAddNewUserNoHasToBeCalled()
        {
            var userTransactionProvider = MockRepository.GenerateStub<IUserTransaction>();
            var user = MockRepository.GenerateStub<UserCRUDRest.User>();

            userTransactionProvider.Stub(userTransaction => userTransaction.ValidateNewUser(Arg<SharedLibrary.User>.Is.Anything)).Return(false);
            userTransactionProvider.Stub(userTransaction => userTransaction.AddNewUser(Arg<SharedLibrary.User>.Is.Anything)).Return(1);

            var userCRUD = new UserCRUD(userTransactionProvider);
            var userId = userCRUD.CreateUser(user);

            userTransactionProvider.AssertWasNotCalled(userTransaction => userTransaction.AddNewUser(Arg<SharedLibrary.User>.Is.Anything));

        }

        [TestMethod]
        public void WhenCreateUser_GivenCorrectData_ShouldReturnIdDiferentZero()
        {
            const string name = "User Test";
            const string date = "1985/02/28";
            var userTest = GetUserTest(name, date);

            var requestApiCall = new ObjectGenericApiCall
                {
                    MethodRequest = HttpMethod.Post,
                    ParamsResource = new List<string>()
                };
            int UserId = _genericApiCaller.LaunchTest<UserCRUDRest.User, int>(requestApiCall, userTest);
            Assert.AreNotEqual(UserId, 0);
        }

        [TestMethod]
        public void WhenCreateTwoUsers_GivenCorrectData_ShouldReturnDifferentIds()
        {
            const string name = "User Test";
            const string date = "1985/02/28";
            var userTest = GetUserTest(name, date);

            var requestApiCall = new ObjectGenericApiCall
            {
                MethodRequest = HttpMethod.Post,
                ParamsResource = new List<string>()
            };
            int UserId = _genericApiCaller.LaunchTest<UserCRUDRest.User, int>(requestApiCall, userTest);

            const string name2 = "User Test2";
            const string date2 = "1985/10/11";
            var user2Test = GetUserTest(name2, date2);
            int User2Id = _genericApiCaller.LaunchTest<UserCRUDRest.User, int>(requestApiCall, user2Test);

            Assert.AreNotEqual(UserId, User2Id);
        }

        [TestMethod]
        public void WhenCreateUser_GivenIdDifferentZero_ShouldReturnCodeErrorForbidden()
        {
            const int id = 1;
            const string name = "User Test";
            const string date = "1985/02/28";
            var userTest = GetUserTest(name, date);

            userTest.Id = id;

            var requestApiCall = new ObjectGenericApiCall
            {
                MethodRequest = HttpMethod.Post,
                ParamsResource = new List<string>()
            };
            _genericApiCaller.LaunchForbiddenTest<UserCRUDRest.User>(requestApiCall, userTest);
        }

        [TestMethod]
        public void WhenCreateUser_GivenNameEmpty_ShouldReturnCodeErrorForbidden()
        {
            const string name = "";
            const string date = "1985/02/28";
            var userTest = GetUserTest(name, date);

            var requestApiCall = new ObjectGenericApiCall
            {
                MethodRequest = HttpMethod.Post,
                ParamsResource = new List<string>()
            };
            _genericApiCaller.LaunchForbiddenTest<UserCRUDRest.User>(requestApiCall, userTest);
        }

        [TestMethod]
        public void WhenCreateUser_GivenNameWith129Characters_ShouldReturnCodeErrorForbidden()
        {
            const string name = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
            const string date = "1985/02/28";
            var userTest = GetUserTest(name, date);

            var requestApiCall = new ObjectGenericApiCall
            {
                MethodRequest = HttpMethod.Post,
                ParamsResource = new List<string>()
            };
            _genericApiCaller.LaunchForbiddenTest<UserCRUDRest.User>(requestApiCall, userTest);
        }

        [TestMethod]
        public void WhenCreateUser_GivenIncorrectFormatBirthday_ShouldReturnCodeErrorForbidden()
        {
            const string name = "User Tests";
            const string date = "1985-28-02";
            var userTest = GetUserTest(name, date);

            var requestApiCall = new ObjectGenericApiCall
            {
                MethodRequest = HttpMethod.Post,
                ParamsResource = new List<string>()
            };
            _genericApiCaller.LaunchForbiddenTest<UserCRUDRest.User>(requestApiCall, userTest);
        }

        [TestMethod]
        public void WhenCreateUser_GivenBirthdayAboveCurrentDate_ShouldReturnCodeErrorForbidden()
        {
            const string name = "User Tests";
            const int numAddDays = 1;

            string date = DateTime.Now.AddDays(numAddDays).ToString("yyyy/MM/dd", CultureInfo.InvariantCulture);
            var userTest = GetUserTest(name, date);

            var requestApiCall = new ObjectGenericApiCall
            {
                MethodRequest = HttpMethod.Post,
                ParamsResource = new List<string>()
            };
            _genericApiCaller.LaunchForbiddenTest<UserCRUDRest.User>(requestApiCall, userTest);
        }

        #endregion

        #region Private Methods

        private UserCRUDRest.User GetUserTest(string name, string date)
        {
            return new UserCRUDRest.User
            {
                Name = name,
                Birthday = date
            };
        }
        #endregion
    }
}
