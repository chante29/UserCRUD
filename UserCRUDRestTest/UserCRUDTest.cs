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
using System.Linq;

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
            int UserId = _genericApiCaller.LaunchOkCreateTest<UserCRUDRest.User, int>(requestApiCall, userTest);
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
            int UserId = _genericApiCaller.LaunchOkCreateTest<UserCRUDRest.User, int>(requestApiCall, userTest);

            const string name2 = "User Test2";
            const string date2 = "1985/10/11";
            var user2Test = GetUserTest(name2, date2);
            int User2Id = _genericApiCaller.LaunchOkCreateTest<UserCRUDRest.User, int>(requestApiCall, user2Test);

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
            _genericApiCaller.LaunchBadRequestTest<UserCRUDRest.User>(requestApiCall, userTest);
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
            _genericApiCaller.LaunchBadRequestTest<UserCRUDRest.User>(requestApiCall, userTest);
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
            _genericApiCaller.LaunchBadRequestTest<UserCRUDRest.User>(requestApiCall, userTest);
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
            _genericApiCaller.LaunchBadRequestTest<UserCRUDRest.User>(requestApiCall, userTest);
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
            _genericApiCaller.LaunchBadRequestTest<UserCRUDRest.User>(requestApiCall, userTest);
        }

        #endregion

        #region UpdateUserTests
        [TestMethod]
        public void WhenUpdateUser_ThenMethodValidateUpdateUserHasToBeCalled()
        {
            var userTransactionProvider = MockRepository.GenerateStub<IUserTransaction>();
            var user = MockRepository.GenerateStub<UserCRUDRest.User>();

            userTransactionProvider.Stub(userTransaction => userTransaction.ValidateUpdateUser(Arg<SharedLibrary.User>.Is.Anything)).Return(true);

            var userCRUD = new UserCRUD(userTransactionProvider);
            userCRUD.UpdateUser(user);

            userTransactionProvider.AssertWasCalled(userTransaction => userTransaction.ValidateUpdateUser(Arg<SharedLibrary.User>.Is.Anything));

        }

        [TestMethod]
        public void WhenUpdateUser_ThenMethodValidateUpdateUserIsTrue_ThenAddUpdateUserHasToBeCalled()
        {
            var userTransactionProvider = MockRepository.GenerateStub<IUserTransaction>();
            var user = MockRepository.GenerateStub<UserCRUDRest.User>();

            userTransactionProvider.Stub(userTransaction => userTransaction.ValidateUpdateUser(Arg<SharedLibrary.User>.Is.Anything)).Return(true);
            userTransactionProvider.Stub(userTransaction => userTransaction.UpdateUser(Arg<SharedLibrary.User>.Is.Anything)).Return(true);

            var userCRUD = new UserCRUD(userTransactionProvider);
            userCRUD.UpdateUser(user);

            userTransactionProvider.AssertWasCalled(userTransaction => userTransaction.UpdateUser(Arg<SharedLibrary.User>.Is.Anything));

        }

        [TestMethod]
        public void WhenUpdateUser_ThenMethodValidateUpdateUserIsFalse_ThenUpdateUserNoHasToBeCalled()
        {
            var userTransactionProvider = MockRepository.GenerateStub<IUserTransaction>();
            var user = MockRepository.GenerateStub<UserCRUDRest.User>();

            userTransactionProvider.Stub(userTransaction => userTransaction.ValidateUpdateUser(Arg<SharedLibrary.User>.Is.Anything)).Return(false);
            userTransactionProvider.Stub(userTransaction => userTransaction.UpdateUser(Arg<SharedLibrary.User>.Is.Anything)).Return(true);

            var userCRUD = new UserCRUD(userTransactionProvider);
            userCRUD.UpdateUser(user);

            userTransactionProvider.AssertWasNotCalled(userTransaction => userTransaction.UpdateUser(Arg<SharedLibrary.User>.Is.Anything));

        }

        [TestMethod]
        public void WhenUpdateUser_GivenCorrectData_ShouldReturnCode200()
        {
            const string name = "User Test";
            const string date = "1985/02/28";
            var userTest = GetUserTest(name, date);
            userTest.Id = CreateUser(userTest);

            userTest.Name = "User Test Modify";
            userTest.Birthday = "2000/10/25";

            var requestApiCall = new ObjectGenericApiCall
            {
                MethodRequest = HttpMethod.Put,
                ParamsResource = new List<string>()
            };
            _genericApiCaller.LaunchTest<UserCRUDRest.User>(requestApiCall, userTest);
        }

        [TestMethod]
        public void WhenUpdateUser_GivenCorrectData_ShouldChangeValues()
        {
            const string name = "User Test";
            const string date = "1985/02/28";
            var userTest = GetUserTest(name, date);
            userTest.Id = CreateUser(userTest);

            var userUpdate = new UserCRUDRest.User
                                 {
                                     Id = userTest.Id,
                                     Name = "User Test Modify",
                                     Birthday = "2000/10/25"
                                 };

            var requestApiCall = new ObjectGenericApiCall
            {
                MethodRequest = HttpMethod.Put,
                ParamsResource = new List<string>()
            };
            _genericApiCaller.LaunchTest<UserCRUDRest.User>(requestApiCall, userUpdate);

            var userStored = GetUser(userTest.Id);

            Assert.AreEqual(userStored.Id, userTest.Id);
            Assert.AreNotEqual(userStored.Name, userTest.Name);
            Assert.AreNotEqual(userStored.Birthday, userTest.Birthday);

        }
        
        [TestMethod]
        public void WhenUpdateUser_GivenIdDoentExist_ShouldReturnCodeErrorForbidden()
        {
            const int id = 0;
            const string name = "User Test";
            const string date = "1985/02/28";
            var userTest = GetUserTest(name, date);

            userTest.Id = id;

            var requestApiCall = new ObjectGenericApiCall
            {
                MethodRequest = HttpMethod.Put,
                ParamsResource = new List<string>()
            };
            _genericApiCaller.LaunchBadRequestTest<UserCRUDRest.User>(requestApiCall, userTest);
        }

        [TestMethod]
        public void WhenUpdateUser_GivenNameEmpty_ShouldReturnCodeErrorForbidden()
        {
            const string name = "User Test";
            const string date = "1985/02/28";
            var userTest = GetUserTest(name, date);

            userTest.Id = CreateUser(userTest);

            userTest.Name = "";

            var requestApiCall = new ObjectGenericApiCall
            {
                MethodRequest = HttpMethod.Put,
                ParamsResource = new List<string>()
            };
            _genericApiCaller.LaunchBadRequestTest<UserCRUDRest.User>(requestApiCall, userTest);
        }

        [TestMethod]
        public void WhenUpdateUser_GivenNameWith129Characters_ShouldReturnCodeErrorForbidden()
        {
            const string name = "User Test";
            const string date = "1985/02/28";
            var userTest = GetUserTest(name, date);

            userTest.Id = CreateUser(userTest);

            userTest.Name = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";

            var requestApiCall = new ObjectGenericApiCall
            {
                MethodRequest = HttpMethod.Put,
                ParamsResource = new List<string>()
            };
            _genericApiCaller.LaunchBadRequestTest<UserCRUDRest.User>(requestApiCall, userTest);
        }

        [TestMethod]
        public void WhenUpdateUser_GivenIncorrectFormatBirthday_ShouldReturnCodeErrorForbidden()
        {
            const string name = "User Test";
            const string date = "1985/02/28";
            var userTest = GetUserTest(name, date);

            userTest.Id = CreateUser(userTest);

            userTest.Birthday = "1985-28-02";

            var requestApiCall = new ObjectGenericApiCall
            {
                MethodRequest = HttpMethod.Put,
                ParamsResource = new List<string>()
            };
            _genericApiCaller.LaunchBadRequestTest<UserCRUDRest.User>(requestApiCall, userTest);
        }

        [TestMethod]
        public void WhenUpdateUser_GivenBirthdayAboveCurrentDate_ShouldReturnCodeErrorForbidden()
        {

            const string name = "User Test";
            const string date = "1985/02/28";

            const int numAddDays = 1;

            var userTest = GetUserTest(name, date);

            userTest.Id = CreateUser(userTest);

            userTest.Birthday = DateTime.Now.AddDays(numAddDays).ToString("yyyy/MM/dd", CultureInfo.InvariantCulture);

            var requestApiCall = new ObjectGenericApiCall
            {
                MethodRequest = HttpMethod.Put,
                ParamsResource = new List<string>()
            };
            _genericApiCaller.LaunchBadRequestTest<UserCRUDRest.User>(requestApiCall, userTest);

        }

        #endregion

        #region GetUserByIdTests

        [TestMethod]
        public void WhenGetUserById_GivenCorrectId_ShouldReturnCorrectUser()
        {
            const string queryParamId = "id";

            const string name = "User Test";
            const string date = "1985/02/28";
            var userTest = GetUserTest(name, date);
            userTest.Id = CreateUser(userTest);

            var requestApiCall = new ObjectGenericApiCall
            {
                MethodRequest = HttpMethod.Get,
                ParamsResource = new List<string>(),
                QueryStringParams = new Dictionary<string,object>()
            };

            requestApiCall.QueryStringParams.Add(queryParamId, userTest.Id);

            var user = _genericApiCaller.LaunchTest<UserCRUDRest.User>(requestApiCall);

            Assert.AreEqual(user.Id, userTest.Id);
            Assert.AreEqual(user.Name, userTest.Name);
            Assert.AreEqual(user.Birthday, userTest.Birthday);
        }

        [TestMethod]
        public void WhenGetUserById_GivenIdDoesntExists_ShouldReturnNull()
        {
            const string queryParamId = "id";
            const int id = 0;

            var requestApiCall = new ObjectGenericApiCall
            {
                MethodRequest = HttpMethod.Get,
                ParamsResource = new List<string>(),
                QueryStringParams = new Dictionary<string, object>()
            };

            requestApiCall.QueryStringParams.Add(queryParamId, id);

            var user = _genericApiCaller.LaunchNoContentTest<UserCRUDRest.User>(requestApiCall);

            Assert.IsNull(user);
        }

        [TestMethod]
        public void WhenGetUserById_GivenIdDoentExists_ShouldReturnCode204()
        {
            const string queryParamId = "id";
            const int id = 0;

            var requestApiCall = new ObjectGenericApiCall
            {
                MethodRequest = HttpMethod.Get,
                ParamsResource = new List<string>(),
                QueryStringParams = new Dictionary<string, object>()
            };

            requestApiCall.QueryStringParams.Add(queryParamId, id);

            var user = _genericApiCaller.LaunchNoContentTest<UserCRUDRest.User>(requestApiCall);
        }

        #endregion

        #region GetUsersTests

        [TestMethod]
        public void WhenGetUsers_GivenAnyData_ShouldReturnCorrectUsers()
        {
            const string name = "User Test";
            const string date = "1985/02/28";
            var userTest = GetUserTest(name, date);
            userTest.Id = CreateUser(userTest);

            string name2 = "User Test2";
            string date2 = "2000/01/28";
            var userTest2 = GetUserTest(name2, date2);
            userTest2.Id = CreateUser(userTest2);

            string name3 = "User Test3";
            string date3 = "2005/01/28";
            var userTest3 = GetUserTest(name3, date3);
            userTest3.Id = CreateUser(userTest3);

            var requestApiCall = new ObjectGenericApiCall
                {
                    MethodRequest = HttpMethod.Get,
                    ParamsResource = new List<string>(),
                };

            var users = _genericApiCaller.LaunchTest<List<UserCRUDRest.User>>(requestApiCall);

            Assert.IsTrue(users.Count >= 3);

            var user1 = users.FirstOrDefault(user => user.Id == userTest.Id);
            Assert.AreEqual(user1.Name, userTest.Name);
            Assert.AreEqual(user1.Birthday, userTest.Birthday);

            var user2 = users.FirstOrDefault(user => user.Id == userTest2.Id);
            Assert.AreEqual(user2.Name, userTest2.Name);
            Assert.AreEqual(user2.Birthday, userTest2.Birthday);

            var user3 = users.FirstOrDefault(user => user.Id == userTest3.Id);
            Assert.AreEqual(user3.Name, userTest3.Name);
            Assert.AreEqual(user3.Birthday, userTest3.Birthday);
        }

       [TestMethod]
        public void WhenGetUsers_GivenAnyData_ShouldReturnCode204()
        {
            DeleteAllUsers();

            var requestApiCall = new ObjectGenericApiCall
            {
                MethodRequest = HttpMethod.Get,
                ParamsResource = new List<string>(),
            };

            var users = _genericApiCaller.LaunchNoContentTest<List<UserCRUDRest.User>>(requestApiCall);

            Assert.IsNull(users);
        }



        #endregion

        #region DeleteUserTests

        [TestMethod]
        public void WhenDeleteUser_GivenExistsId_ShouldReturnCode200()
        {
            const string name = "User Test";
            const string date = "1985/02/28";
            var userTest = GetUserTest(name, date);
            userTest.Id = CreateUser(userTest);

            var requestApiCall = new ObjectGenericApiCall
            {
                MethodRequest = HttpMethod.Delete,
                ParamsResource = new List<string>(),
            };

            requestApiCall.ParamsResource.Add(userTest.Id.ToString());

            var users = _genericApiCaller.LaunchTest<List<UserCRUDRest.User>>(requestApiCall);

        }

        [TestMethod]
        public void WhenDeleteUser_GivenExistsId_ShouldGetUsersReturnUsersLess()
        {
            const string name = "User Test";
            const string date = "1985/02/28";
            var userTest = GetUserTest(name, date);
            userTest.Id = CreateUser(userTest);

            List<UserCRUDRest.User> usersBefore = GetUsers();

            var requestApiCall = new ObjectGenericApiCall
            {
                MethodRequest = HttpMethod.Delete,
                ParamsResource = new List<string>(),
            };

            requestApiCall.ParamsResource.Add(userTest.Id.ToString());

            _genericApiCaller.LaunchTest(requestApiCall);

            List<UserCRUDRest.User> usersAfter = GetUsers();

            Assert.IsTrue(usersBefore.Count - 1 == usersAfter.Count);

        }
        
        [TestMethod]
        public void WhenDeleteUser_GivenNoExistsId_ShouldReturnCode204()
        {
            string userIdDoesntExists = "0";

            var requestApiCall = new ObjectGenericApiCall
            {
                MethodRequest = HttpMethod.Delete,
                ParamsResource = new List<string>(),
            };

            requestApiCall.ParamsResource.Add(userIdDoesntExists);

            _genericApiCaller.LaunchBadRequestTest(requestApiCall);
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

        private int CreateUser(UserCRUDRest.User user)
        {
            var requestApiCall = new ObjectGenericApiCall
            {
                MethodRequest = HttpMethod.Post,
                ParamsResource = new List<string>()
            };
            return _genericApiCaller.LaunchOkCreateTest<UserCRUDRest.User, int>(requestApiCall, user);
        }

        private UserCRUDRest.User GetUser(int id)
        {
            const string queryParamId = "id";

            var requestApiCall = new ObjectGenericApiCall
            {
                MethodRequest = HttpMethod.Get,
                ParamsResource = new List<string>(),
                QueryStringParams = new Dictionary<string, object>()
            };

            requestApiCall.QueryStringParams.Add(queryParamId, id);

            return _genericApiCaller.LaunchTest<UserCRUDRest.User>(requestApiCall);
        }

        private List<UserCRUDRest.User> GetUsers()
        {
            var requestApiCall = new ObjectGenericApiCall
            {
                MethodRequest = HttpMethod.Get,
                ParamsResource = new List<string>()
            };

            return _genericApiCaller.LaunchTest<List<UserCRUDRest.User>>(requestApiCall);
        }


        private void DeleteAllUsers()
        {
            var users = GetUsers();

            users.ForEach(user =>
                        {

                            var requestApiCall = new ObjectGenericApiCall
                            {
                                MethodRequest = HttpMethod.Delete,
                                ParamsResource = new List<string>(),
                            };

                            requestApiCall.ParamsResource.Add(user.Id.ToString());

                            _genericApiCaller.LaunchTest(requestApiCall);
                        });

        }

        #endregion

    }
}
