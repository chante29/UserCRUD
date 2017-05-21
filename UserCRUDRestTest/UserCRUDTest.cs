﻿using System;
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
            var userTest = GetUserTest();

            var requestApiCall = new ObjectGenericApiCall
                {
                    MethodRequest = HttpMethod.Post,
                    ParamsResource = new List<string>()
                };
            int UserId = _genericApiCaller.LaunchTest<UserCRUDRest.User, int>(requestApiCall, userTest);
            Assert.AreNotEqual(UserId, 0);
        }
        #endregion

        #region Private Methods

        private UserCRUDRest.User GetUserTest()
        {
            DateTime date;
            DateTime.TryParseExact("1985/02/28", "yyyy/MM/dd", null, DateTimeStyles.None, out date);

            return new UserCRUDRest.User
            {
                Name = "User Test",
                Birthday = date
            };
        }

        #endregion
    }
}
