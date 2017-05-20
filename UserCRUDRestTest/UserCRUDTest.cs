using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using SharedLibrary;
using UserCRUDRest;

namespace UserCRUDRestTest
{
    [TestClass]
    public class UserCRUDTest
    {
        #region CreateUserTests
        [TestMethod]
        public void WhenCreateUserThenMethodValidateNewUserHasToBeCalled()
        {
            var userTransactionProvider = MockRepository.GenerateStub<IUserTransaction>();
            var user = MockRepository.GenerateStub<UserCRUDRest.User>();

            userTransactionProvider.Stub(userTransaction => userTransaction.ValidateNewUser(Arg<SharedLibrary.IUser>.Is.Anything)).Return(true);

            var userCRUD = new UserCRUD(userTransactionProvider);
            var userId = userCRUD.CreateUser(user);

            userTransactionProvider.AssertWasCalled(userTransaction => userTransaction.ValidateNewUser(Arg<SharedLibrary.IUser>.Is.Anything));

        }

        [TestMethod]
        public void WhenCreateUserThenMethodValidateNewUserIsTrueThenAddNewUserHasToBeCalled()
        {
            var userTransactionProvider = MockRepository.GenerateStub<IUserTransaction>();
            var user = MockRepository.GenerateStub<UserCRUDRest.User>();

            userTransactionProvider.Stub(userTransaction => userTransaction.ValidateNewUser(Arg<SharedLibrary.IUser>.Is.Anything)).Return(true);
            userTransactionProvider.Stub(userTransaction => userTransaction.AddNewUser(Arg<SharedLibrary.IUser>.Is.Anything)).Return(true);

            var userCRUD = new UserCRUD(userTransactionProvider);
            var userId = userCRUD.CreateUser(user);

            userTransactionProvider.AssertWasCalled(userTransaction => userTransaction.AddNewUser(Arg<SharedLibrary.IUser>.Is.Anything));

        }

        [TestMethod]
        public void WhenCreateUserThenMethodValidateNewUserIsFalseThenAddNewUserNoHasToBeCalled()
        {
            var userTransactionProvider = MockRepository.GenerateStub<IUserTransaction>();
            var user = MockRepository.GenerateStub<UserCRUDRest.User>();

            userTransactionProvider.Stub(userTransaction => userTransaction.ValidateNewUser(Arg<SharedLibrary.IUser>.Is.Anything)).Return(false);
            userTransactionProvider.Stub(userTransaction => userTransaction.AddNewUser(Arg<SharedLibrary.IUser>.Is.Anything)).Return(true);

            var userCRUD = new UserCRUD(userTransactionProvider);
            var userId = userCRUD.CreateUser(user);

            userTransactionProvider.AssertWasNotCalled(userTransaction => userTransaction.AddNewUser(Arg<SharedLibrary.IUser>.Is.Anything));

        }
        #endregion
        
    }
}
