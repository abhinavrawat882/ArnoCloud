// ZONE 1: Imports (The Tools)
using Xunit;                 // The Test Runner
using Moq;                   // The Mocking Framework
using Notes.Service.Services;// The Code we are testing
using Notes.Service.Entity;
using AutoMapper;
using Notes.Service.DTO;  // Data structures

namespace Notes.Service.Tests
{
    // ZONE 2: Class Definition & Fields (The Lab Equipment)
    public class NoteServiceTests
    {

        // These are "Private Fields". We declare them here so all tests can use them.
        private readonly Mock<INoteRepository> _mockRepo;
        private readonly Mock<IMapper> _mockMapper;

        // This is the "System Under Test" (SUT) - the actual class we are testing.
        private readonly NoteService _service;

        // ZONE 3: The Constructor (The Setup Phase)
        // xUnit runs this Constructor NEW before EVERY single test method.
        public NoteServiceTests()
        {
            // 1. Create fresh "fake" dependencies
            _mockRepo = new Mock<INoteRepository>();
            _mockMapper = new Mock<IMapper>();

            // 2. Build the real service, injecting the fakes
            // We use .Object to get the actual instance of the fake interface
            _service = new NoteService(_mockRepo.Object, _mockMapper.Object);
        }

        // ZONE 4: The Tests (The Experiments)
        [Fact] // This attribute tells xUnit "Run this method!"
        public async Task GetNotes_ShouldReturnData_WhenCalled()
        {
            // Arrange (Prepare inputs and mock behavior)
            // ...
            var fakeNotes = new List<NoteDTO>
            {
                new NoteDTO { Id = Guid.NewGuid(), Title = "Test Note 1" },
                new NoteDTO { Id = Guid.NewGuid(), Title = "Test Note 2" }
            };
            // Tell the Mock Repository:
            // "When GetNotesAsync is called with ANY integers, return 'fakeNotes'."
            _mockRepo.Setup(repo => repo.GetNotesAsync(It.IsAny<int>(), It.IsAny<int>()))
                     .ReturnsAsync(fakeNotes);
            // Act (Run the real code)
            // ...
            var result = await _service.GetNotes(1, 10);

            // Assert (Verify results)
            // ...
            Assert.NotNull(result);

            // Did we get exactly 2 items?
            Assert.Equal(2, result.Count);

            // Is the first item correct?
            Assert.Equal("Test Note 1", result[0].Title);

            // Verification: Did the Service actually ask the Repository for data?
            _mockRepo.Verify(repo => repo.GetNotesAsync(1, 10), Times.Once);
        }
    }
}