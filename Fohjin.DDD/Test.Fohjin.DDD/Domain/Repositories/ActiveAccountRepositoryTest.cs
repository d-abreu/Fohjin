using System;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using Fohjin.DDD.Configuration;
using Fohjin.DDD.Contracts;
using Fohjin.DDD.Domain;
using Fohjin.DDD.Domain.Entities;
using Fohjin.DDD.Domain.Entities.Mementos;
using Fohjin.DDD.Domain.Exceptions;
using Fohjin.DDD.Domain.ValueObjects;
using Fohjin.DDD.EventStore.SQLite;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace Test.Fohjin.DDD.Domain.Repositories
{
    [TestFixture]
    public class ActiveAccountRepositoryTest
    {
        private const string dataBaseFile = "domainDataBase.db3";

        private IDomainRepository _repository;
        private Storage _storage;

        [SetUp]
        public void SetUp()
        {
            new DomainDatabaseBootStrapper().ReCreateDatabaseSchema();

            var sqliteConnectionString = string.Format("Data Source={0}", dataBaseFile);

            _storage = new Storage(sqliteConnectionString, new BinaryFormatter());

            _repository = new SQLiteDomainRepository(_storage, _storage);
        }

        [Test]
        public void When_calling_Save_it_will_add_the_domain_events_to_the_domain_event_storage()
        {
            var activeAccount = ActiveAccount.CreateNew(Guid.NewGuid(), "AccountName");
            activeAccount.Deposite(new Amount(1));
            activeAccount.Deposite(new Amount(1));

            _repository.Save(activeAccount);

            Assert.That(_storage.GetEventsSinceLastSnapShot(activeAccount.Id).Count(), Is.EqualTo(3));
            Assert.That(_storage.GetAllEvents(activeAccount.Id).Count(), Is.EqualTo(3));
        }

        [Test]
        public void When_calling_Save_it_will_reset_the_domain_events()
        {
            var activeAccount = ActiveAccount.CreateNew(Guid.NewGuid(), "AccountName");
            activeAccount.Deposite(new Amount(1));
            activeAccount.Deposite(new Amount(1));

            _repository.Save(activeAccount);

            var activeAccountForRepository = (IEventProvider)activeAccount;

            Assert.That(activeAccountForRepository.GetChanges().Count(), Is.EqualTo(0));
        }

        [Test]
        public void When_calling_Save_after_more_than_9_events_a_new_snap_shot_will_be_created_9_events_will_not()
        {
            var activeAccount = ActiveAccount.CreateNew(Guid.NewGuid(), "AccountName");
            activeAccount.Deposite(new Amount(1));
            activeAccount.Deposite(new Amount(1));
            activeAccount.Deposite(new Amount(1));
            activeAccount.Deposite(new Amount(1));
            activeAccount.Deposite(new Amount(1));
            activeAccount.Deposite(new Amount(1));
            activeAccount.Deposite(new Amount(1));
            activeAccount.Deposite(new Amount(1));

            _repository.Save(activeAccount);

            Assert.That(_storage.GetSnapShot(activeAccount.Id), Is.Null);
        }

        [Test]
        public void When_calling_Save_after_more_than_9_events_a_new_snap_shot_will_be_created_10_events()
        {
            var activeAccount = ActiveAccount.CreateNew(Guid.NewGuid(), "AccountName");
            activeAccount.Deposite(new Amount(1));
            activeAccount.Deposite(new Amount(1));
            activeAccount.Deposite(new Amount(1));
            activeAccount.Deposite(new Amount(1));
            activeAccount.Deposite(new Amount(1));
            activeAccount.Deposite(new Amount(1));
            activeAccount.Deposite(new Amount(1));
            activeAccount.Deposite(new Amount(1));
            activeAccount.Deposite(new Amount(1));

            _repository.Save(activeAccount);

            var snapShot = _storage.GetSnapShot(activeAccount.Id);

            Assert.That(snapShot, Is.Not.Null);
            Assert.That(snapShot.Memento, Is.InstanceOfType(typeof(ActiveAccountMemento)));
        }

        [Test]
        public void When_calling_Save_after_more_than_9_events_a_new_snap_shot_will_be_created_11_events()
        {
            var activeAccount = ActiveAccount.CreateNew(Guid.NewGuid(), "AccountName");
            activeAccount.Deposite(new Amount(1));
            activeAccount.Deposite(new Amount(1));
            activeAccount.Deposite(new Amount(1));
            activeAccount.Deposite(new Amount(1));
            activeAccount.Deposite(new Amount(1));
            activeAccount.Deposite(new Amount(1));
            activeAccount.Deposite(new Amount(1));
            activeAccount.Deposite(new Amount(1));
            activeAccount.Deposite(new Amount(1));
            activeAccount.Deposite(new Amount(1));

            _repository.Save(activeAccount);

            var snapShot = _storage.GetSnapShot(activeAccount.Id);

            Assert.That(snapShot, Is.Not.Null);
            Assert.That(snapShot.Memento, Is.InstanceOfType(typeof(ActiveAccountMemento)));
        }

        [Test]
        public void When_calling_Save_after_more_than_9_events_after_the_last_snap_shot_a_new_snapshot_will_be_created_10_events_after_last_snapshot()
        {
            var activeAccount = ActiveAccount.CreateNew(Guid.NewGuid(), "AccountName");
            activeAccount.Deposite(new Amount(1));
            activeAccount.Deposite(new Amount(1));
            activeAccount.Deposite(new Amount(1));
            activeAccount.Deposite(new Amount(1));
            activeAccount.Deposite(new Amount(1));
            activeAccount.Deposite(new Amount(1));
            activeAccount.Deposite(new Amount(1));
            activeAccount.Deposite(new Amount(1));
            activeAccount.Deposite(new Amount(1));

            _repository.Save(activeAccount);

            activeAccount.Deposite(new Amount(1));
            activeAccount.Deposite(new Amount(1));
            activeAccount.Deposite(new Amount(1));
            activeAccount.Deposite(new Amount(1));
            activeAccount.Deposite(new Amount(1));
            activeAccount.Deposite(new Amount(1));
            activeAccount.Deposite(new Amount(1));
            activeAccount.Deposite(new Amount(1));
            activeAccount.Deposite(new Amount(1));
            activeAccount.Deposite(new Amount(1));
            activeAccount.Deposite(new Amount(1));
            activeAccount.Deposite(new Amount(1));

            _repository.Save(activeAccount);

            var snapShot = _storage.GetSnapShot(activeAccount.Id);

            Assert.That(snapShot, Is.Not.Null);
            Assert.That(snapShot.Memento, Is.InstanceOfType(typeof(ActiveAccountMemento)));
        }

        [Test]
        public void When_calling_Save_after_more_than_9_events_after_the_last_snap_shot_a_new_snapshot_will_be_created_10_events_after_last_snapshot_9_events_after_last_snapshot()
        {
            var activeAccount = ActiveAccount.CreateNew(Guid.NewGuid(), "AccountName");
            activeAccount.Deposite(new Amount(1));
            activeAccount.Deposite(new Amount(1));
            activeAccount.Deposite(new Amount(1));
            activeAccount.Deposite(new Amount(1));
            activeAccount.Deposite(new Amount(1));
            activeAccount.Deposite(new Amount(1));
            activeAccount.Deposite(new Amount(1));
            activeAccount.Deposite(new Amount(1));
            activeAccount.Deposite(new Amount(1));

            _repository.Save(activeAccount);

            activeAccount.Deposite(new Amount(1));
            activeAccount.Deposite(new Amount(1));
            activeAccount.Deposite(new Amount(1));
            activeAccount.Deposite(new Amount(1));
            activeAccount.Deposite(new Amount(1));
            activeAccount.Deposite(new Amount(1));
            activeAccount.Deposite(new Amount(1));
            activeAccount.Deposite(new Amount(1));
            activeAccount.Deposite(new Amount(1));

            _repository.Save(activeAccount);

            var snapShot = _storage.GetSnapShot(activeAccount.Id);

            Assert.That(snapShot, Is.Not.Null);
            Assert.That(snapShot.Memento, Is.InstanceOfType(typeof(ActiveAccountMemento)));
        }

        [Test]
        public void When_calling_Save_after_more_than_9_events_after_the_last_snap_shot_a_new_snapshot_will_be_created_10_events_after_last_snapshot_9_events_after_last_snapshot_verify_all_event_counts()
        {
            var activeAccount = ActiveAccount.CreateNew(Guid.NewGuid(), "AccountName");
            activeAccount.Deposite(new Amount(1));
            activeAccount.Deposite(new Amount(1));
            activeAccount.Deposite(new Amount(1));
            activeAccount.Deposite(new Amount(1));
            activeAccount.Deposite(new Amount(1));
            activeAccount.Deposite(new Amount(1));
            activeAccount.Deposite(new Amount(1));
            activeAccount.Deposite(new Amount(1));
            activeAccount.Deposite(new Amount(1));

            _repository.Save(activeAccount);

            activeAccount.Deposite(new Amount(1));
            activeAccount.Deposite(new Amount(1));
            activeAccount.Deposite(new Amount(1));
            activeAccount.Deposite(new Amount(1));
            activeAccount.Deposite(new Amount(1));
            activeAccount.Deposite(new Amount(1));
            activeAccount.Deposite(new Amount(1));
            activeAccount.Deposite(new Amount(1));
            activeAccount.Deposite(new Amount(1));

            _repository.Save(activeAccount);

            Assert.That(_storage.GetEventsSinceLastSnapShot(activeAccount.Id).Count(), Is.EqualTo(9));
            Assert.That(_storage.GetAllEvents(activeAccount.Id).Count(), Is.EqualTo(19));
        }

        [Test]
        [ExpectedException(typeof(AccountBalanceIsToLowException))]
        public void When_calling_GetById_after_9_events_a_new_ActiveAcount_will_be_populated()
        {
            var activeAccount = ActiveAccount.CreateNew(Guid.NewGuid(), "AccountName");
            activeAccount.Deposite(new Amount(1));
            activeAccount.Deposite(new Amount(2));
            activeAccount.Deposite(new Amount(3));
            activeAccount.Deposite(new Amount(4));
            activeAccount.Deposite(new Amount(5));
            activeAccount.Deposite(new Amount(6));
            activeAccount.Deposite(new Amount(7));
            activeAccount.Deposite(new Amount(8));

            _repository.Save(activeAccount);

            var sut = _repository.GetById<ActiveAccount>(activeAccount.Id);

            try
            {
                sut.Withdrawl(new Amount(36));
            }
            catch (Exception Ex)
            {
                Assert.Fail(string.Format("This should not fail: {0}", Ex.Message));
            }

            sut.Withdrawl(new Amount(1));
        }

        [Test]
        [ExpectedException(typeof(AccountBalanceIsToLowException))]
        public void When_calling_GetById_after_every_10_events_a_new_snap_shot_will_be_created()
        {
            var activeAccount = ActiveAccount.CreateNew(Guid.NewGuid(), "AccountName");
            activeAccount.Deposite(new Amount(1));
            activeAccount.Deposite(new Amount(2));
            activeAccount.Deposite(new Amount(3));
            activeAccount.Deposite(new Amount(4));
            activeAccount.Deposite(new Amount(5));
            activeAccount.Deposite(new Amount(6));
            activeAccount.Deposite(new Amount(7));
            activeAccount.Deposite(new Amount(8));
            activeAccount.Deposite(new Amount(9));

            _repository.Save(activeAccount);

            var sut = _repository.GetById<ActiveAccount>(activeAccount.Id);

            try
            {
                sut.Withdrawl(new Amount(45));
            }
            catch (Exception Ex)
            {
                Assert.Fail(string.Format("This should not fail: {0}", Ex.Message));
            }

            sut.Withdrawl(new Amount(1));
        }

        [Test]
        [ExpectedException(typeof(AccountBalanceIsToLowException))]
        public void When_calling_GetById_after_every_10_events_a_new_snap_shot_will_be_created_11_events()
        {
            var activeAccount = ActiveAccount.CreateNew(Guid.NewGuid(), "AccountName");
            activeAccount.Deposite(new Amount(1));
            activeAccount.Deposite(new Amount(2));
            activeAccount.Deposite(new Amount(3));
            activeAccount.Deposite(new Amount(4));
            activeAccount.Deposite(new Amount(5));
            activeAccount.Deposite(new Amount(6));
            activeAccount.Deposite(new Amount(7));
            activeAccount.Deposite(new Amount(8));
            activeAccount.Deposite(new Amount(9));
            activeAccount.Deposite(new Amount(10));

            _repository.Save(activeAccount);

            var sut = _repository.GetById<ActiveAccount>(activeAccount.Id);

            try
            {
                sut.Withdrawl(new Amount(55));
            }
            catch (Exception Ex)
            {
                Assert.Fail(string.Format("This should not fail: {0}", Ex.Message));
            }

            sut.Withdrawl(new Amount(1));
        }
    }
}