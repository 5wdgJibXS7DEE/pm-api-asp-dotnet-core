using System;
using System.Collections.Generic;
using System.Linq;
using JsonFlatFileDataStore;
using Moq;
using ProjectManagement.Models;
using Xunit;

namespace ProjectManagement.Logic.Tests
{
    public class InsertTests
    {
        private int _taskInsertedInStoreCount = 0;

        private int _overlapsInsertedInStoreCount = 0;

        private readonly Mock<IDataStore> _storeMock;

        private readonly Mock<IDocumentCollection<Task>> _tasksMock;

        private readonly Mock<IDocumentCollection<TaskOverlap>> _overlapsMock;

        private readonly Guid _assigneeIdOne = new Guid("10ec1708-a6df-4034-a5ac-adecbc5cc37b");

        public InsertTests()
        {
            _storeMock = new Mock<IDataStore>();

            _tasksMock = MockTasksCollectionInStore(_storeMock);

            _overlapsMock = MockOverlapsCollectionInStore(_storeMock);
        }

        private Mock<IDocumentCollection<Task>> MockTasksCollectionInStore(
            Mock<IDataStore> storeMock)
        {
            var mock = new Mock<IDocumentCollection<Task>>();

            mock.Setup(t => t.InsertOne(It.IsAny<Task>()))
                .Callback(() => _taskInsertedInStoreCount++)
                .Returns(true);

            storeMock.Setup(s => s.GetCollection<Task>(null))
                .Returns(mock.Object);

            return mock;
        }

        private Mock<IDocumentCollection<TaskOverlap>> MockOverlapsCollectionInStore(
            Mock<IDataStore> storeMock)
        {
            var mock = new Mock<IDocumentCollection<TaskOverlap>>();

            mock.Setup(t => t.InsertMany(It.IsAny<IEnumerable<TaskOverlap>>()))
                .Callback<IEnumerable<TaskOverlap>>(
                    overlaps => _overlapsInsertedInStoreCount += overlaps.Count())
                .Returns(true);

            storeMock.Setup(s => s.GetCollection<TaskOverlap>(null))
                .Returns(mock.Object);

            return mock;
        }

        [Fact]
        public void Insert_task_that_ends_before_another_returns_an_empty_list_of_overlaps()
        {
            MockTasksCollectionToReturnOneTaskForFebruary(_assigneeIdOne);

            var january = new Task()
            {
                Id = 1,
                AssigneeId = _assigneeIdOne,
                StartsOn = new DateTime(2020, 1, 1),
                EndsOn = new DateTime(2020, 2, 1)
            };

            var returned = new TasksLogic(_storeMock.Object).Insert(january);

            Assert.Equal(1, _taskInsertedInStoreCount);
            Assert.Equal(0, _overlapsInsertedInStoreCount);
            Assert.Empty(returned);
        }

        [Fact]
        public void Insert_task_that_starts_after_another_returns_an_empty_list_of_overlaps()
        {
            MockTasksCollectionToReturnOneTaskForFebruary(_assigneeIdOne);

            var march = new Task()
            {
                Id = 1,
                AssigneeId = _assigneeIdOne,
                StartsOn = new DateTime(2020, 3, 1),
                EndsOn = new DateTime(2020, 4, 1)
            };

            var returned = new TasksLogic(_storeMock.Object).Insert(march);

            Assert.Equal(1, _taskInsertedInStoreCount);
            Assert.Equal(0, _overlapsInsertedInStoreCount);
            Assert.Empty(returned);
        }

        [Fact]
        public void Insert_task_that_starts_before_and_ends_after_another_returns_one_overlap()
        {
            MockTasksCollectionToReturnOneTaskForFebruary(_assigneeIdOne);

            var janToMarch = new Task()
            {
                Id = 1,
                AssigneeId = _assigneeIdOne,
                StartsOn = new DateTime(2020, 1, 1),
                EndsOn = new DateTime(2020, 4, 1)
            };

            var returnedCount = new TasksLogic(_storeMock.Object).Insert(janToMarch).Count();

            Assert.Equal(1, _taskInsertedInStoreCount);
            Assert.Equal(1, _overlapsInsertedInStoreCount);
            Assert.Equal(1, returnedCount);
        }

        [Fact]
        public void Insert_task_that_starts_before_and_ends_during_another_returns_one_overlap()
        {
            MockTasksCollectionToReturnOneTaskForFebruary(_assigneeIdOne);

            var midJanToMidFeb = new Task()
            {
                Id = 1,
                AssigneeId = _assigneeIdOne,
                StartsOn = new DateTime(2020, 1, 15),
                EndsOn = new DateTime(2020, 2, 15)
            };

            var returnedCount = new TasksLogic(_storeMock.Object).Insert(midJanToMidFeb).Count();

            Assert.Equal(1, _taskInsertedInStoreCount);
            Assert.Equal(1, _overlapsInsertedInStoreCount);
            Assert.Equal(1, returnedCount);
        }

        [Fact]
        public void Insert_task_that_starts_during_and_ends_after_another_returns_one_overlap()
        {
            MockTasksCollectionToReturnOneTaskForFebruary(_assigneeIdOne);

            var midFebToMidMar = new Task()
            {
                Id = 1,
                AssigneeId = _assigneeIdOne,
                StartsOn = new DateTime(2020, 2, 15),
                EndsOn = new DateTime(2020, 3, 15)
            };

            var returnedCount = new TasksLogic(_storeMock.Object).Insert(midFebToMidMar).Count();

            Assert.Equal(1, _taskInsertedInStoreCount);
            Assert.Equal(1, _overlapsInsertedInStoreCount);
            Assert.Equal(1, returnedCount);
        }

        [Fact]
        public void Insert_task_that_starts_and_ends_during_another_returns_one_overlap()
        {
            MockTasksCollectionToReturnOneTaskForFebruary(_assigneeIdOne);

            var withinFeb = new Task()
            {
                Id = 1,
                AssigneeId = _assigneeIdOne,
                StartsOn = new DateTime(2020, 2, 10),
                EndsOn = new DateTime(2020, 2, 20)
            };

            var returnedCount = new TasksLogic(_storeMock.Object).Insert(withinFeb).Count();

            Assert.Equal(1, _taskInsertedInStoreCount);
            Assert.Equal(1, _overlapsInsertedInStoreCount);
            Assert.Equal(1, returnedCount);
        }

        [Fact]
        public void Insert_task_with_different_assignee_returns_an_empty_list_of_overlaps()
        {
            MockTasksCollectionToReturnOneTaskForFebruary(_assigneeIdOne);

            Guid assigneeIdTwo = Guid.NewGuid();
            var withinFeb = new Task()
            {
                Id = 1,
                AssigneeId = assigneeIdTwo,
                StartsOn = new DateTime(2020, 2, 10),
                EndsOn = new DateTime(2020, 2, 20)
            };

            var returned = new TasksLogic(_storeMock.Object).Insert(withinFeb);

            Assert.Equal(1, _taskInsertedInStoreCount);
            Assert.Equal(0, _overlapsInsertedInStoreCount);
            Assert.Empty(returned);
        }

        private void MockTasksCollectionToReturnOneTaskForFebruary(Guid assigneeId)
        {
            var february = new Task()
            {
                Id = 0,
                AssigneeId = assigneeId,
                StartsOn = new DateTime(2020, 2, 1),
                EndsOn = new DateTime(2020, 3, 1)
            };
            var tasks = new List<Task>() { february };

            _tasksMock.Setup(t => t.AsQueryable()).Returns(tasks);
        }
    }
}