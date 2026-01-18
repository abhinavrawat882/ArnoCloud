using Moq;
using ToDoList.Service.DTO;
using ToDoList.Service.Enums;
using ToDoList.Service.Repository;
using ToDoList.Service.Service;
using Xunit;

namespace ToDoList.Service.Test.Services;

public class ToDoServiceTest
{
    // Mocks and System Under Test (SUT)
    private readonly Mock<IToDoListRepo> _mockRepo;
    private readonly ToDoService _sut; // SUT = System Under Test

    public ToDoServiceTest()
    {
        // ARRANGE: Setup the common dependencies for every test
        _mockRepo = new Mock<IToDoListRepo>();
        
        // Inject the mock into the service. 
        // This allows us to isolate the service logic from the database layer.
        _sut = new ToDoService(_mockRepo.Object);
    }

    /// <summary>
    /// Test Case 1: Happy Path
    /// Verifies that when the filter is valid, the service returns the data from the repository.
    /// </summary>
    [Fact]
    public async Task GetListAsync_WhenFilterIsValid_ShouldReturnListOfDtos()
    {
        // ARRANGE
        // 1. Prepare Valid Filter
        var filter = new ToDoListFilter 
        { 
            page = 1, 
            pageSize = 10, 
            State = TodoState.Active 
        };

        // 2. Prepare Expected Data
        var expectedList = new List<ToDoListDTO>
        {
            new ToDoListDTO { Id = 1, Body = "Test Task 1", State = TodoState.Active },
            new ToDoListDTO { Id = 2, Body = "Test Task 2", State = TodoState.Active }
        };

        // 3. Setup Mock Behavior
        // When GetToDOListAsync is called with our specific filter, return expectedList
        _mockRepo.Setup(repo => repo.GetToDOListAsync(filter))
                 .ReturnsAsync(expectedList);

        // ACT
        // Call the method we are testing
        var result = await _sut.GetListAsync(filter);

        // ASSERT
        // Verify the result is exactly what we expected
        Assert.NotNull(result);
        Assert.Equal(expectedList.Count, result.Count);
        Assert.Same(expectedList, result); // Expecting the exact same object reference if pass-through
        
        // Verify that the repository method was called exactly once
        _mockRepo.Verify(repo => repo.GetToDOListAsync(filter), Times.Once);
    }

    /// <summary>
    /// Test Case 2: Empty Result
    /// Verifies that the service handles empty results from the repository gracefully.
    /// </summary>
    [Fact]
    public async Task GetListAsync_WhenRepositoryReturnsEmpty_ShouldReturnEmptyList()
    {
        // ARRANGE
        var filter = new ToDoListFilter { page = 1, pageSize = 10 };
        var emptyList = new List<ToDoListDTO>();

        _mockRepo.Setup(repo => repo.GetToDOListAsync(filter))
                 .ReturnsAsync(emptyList);

        // ACT
        var result = await _sut.GetListAsync(filter);

        // ASSERT
        Assert.NotNull(result);
        Assert.Empty(result);
    }
    
    /// <summary>
    /// Test Case 3: Edge Case - Null Input
    /// Verifies that the service validates the input filter and throws an exception if null.
    /// FAIL INITIALLY: The current implementation throws NotImplementedException, 
    /// but eventually it should throw ArgumentNullException.
    /// </summary>
    [Fact]
    public async Task GetListAsync_WhenFilterIsNull_ShouldThrowArgumentNullException()
    {
        // ARRANGE
        ToDoListFilter nullFilter = null;

        // ACT & ASSERT
        // Assert.ThrowsAsync checks if the code throws the specific exception
        await Assert.ThrowsAsync<ArgumentNullException>(() => _sut.GetListAsync(nullFilter));
    }

    /// <summary>
    /// Test Case 4: Exception Propagation
    /// Verifies that if the repository fails, the service bubbles up the exception 
    /// (or handles it, depending on requirements. Here we assume bubble up).
    /// </summary>
    [Fact]
    public async Task GetListAsync_WhenRepositoryThrows_ShouldThrowException()
    {
        // ARRANGE
        var filter = new ToDoListFilter { page = 1, pageSize = 10 };
        var expectedException = new Exception("Database failure");

        _mockRepo.Setup(repo => repo.GetToDOListAsync(It.IsAny<ToDoListFilter>()))
                 .ThrowsAsync(expectedException);

        // ACT & ASSERT
        var ex = await Assert.ThrowsAsync<Exception>(() => _sut.GetListAsync(filter));
        Assert.Equal("Database failure", ex.Message);
    }

    /// <summary>
    /// Test Case 5: Invalid Filter Values
    /// Verifies that the service rejects invalid pagination values (e.g., non-positive page or pageSize).
    /// </summary>
    [Theory]
    [InlineData(0, 10)]  // Invalid Page 0
    [InlineData(-1, 10)] // Invalid Page -1
    [InlineData(1, 0)]   // Invalid PageSize 0
    [InlineData(1, -5)]  // Invalid PageSize -5
    public async Task GetListAsync_WhenFilterHasInvalidPagination_ShouldThrowArgumentException(int page, int pageSize)
    {
        // ARRANGE
        var filter = new ToDoListFilter 
        { 
            page = page, 
            pageSize = pageSize,
            State = TodoState.Active
        };

        // ACT & ASSERT
        await Assert.ThrowsAsync<ArgumentException>(() => _sut.GetListAsync(filter));
    }

    
    /// Add TO DO Item Async
    
    
    /// <summary>
    /// Test Case 1: Happy Path (Valid input)
    /// Verify : 
    ///     A. If data passsed to Repo Corretly
    ///     B. Returns New Id correctly Back 
    [Fact]
    public async Task AddToDoItemAsync_ValidInputs_ShouldReturnItemID()
    {
        // Assemble

        var input = new ToDoListDTO()
        {
            Id=0,
            Body="Test Item",
            State = TodoState.Active
        };

        _mockRepo.Setup(x=>x.AddToDoListItemAsync(It.IsAny<ToDoListDTO>(//DTO=> 
            // !string.IsNullOrWhiteSpace(DTO.Body) && 
            // DTO.State == TodoState.Active
        ))).ReturnsAsync(10);

        var newId = await _sut.AddToDoItemAsync(input);
        

        // Assersions

        Assert.Equal(10,newId);

        _mockRepo.Verify(r=>
            r.AddToDoListItemAsync(It.Is<ToDoListDTO>(data =>
                data.Body=="Test Item" && 
                data.State == TodoState.Active
            ))
            ,Times.Once()
        );
    }
    /// <summary>
    /// Test Case 2: Sad Path (Invalid Input):
    /// Input:
    ///     Test with invaild values one by one with all the possible ways
    /// Verify : 
    ///     A. Does the Service Validates Input correctly 
    ///     B. Should not call Repo
    [Theory]
    [InlineData(1,"Valid Body",TodoState.Active)] // Id invalid Case. Id should be 0
    [InlineData(0,"",TodoState.Active)] // Invalid Body . Should not be empty or null 
    [InlineData(0,"Valid",TodoState.Archived)] // Invalid Status . Can only create TO DO list in Active State
    [InlineData(0,"Valid",TodoState.Completed)] // Invalid Status . Can only create TO DO list in Active State
    public async Task AddToDoItemAsync_InvalidInputs_ShouldReturnError(int id, string body,TodoState state)
    {
        // Arrange
        var input =new ToDoListDTO()
        {
          Id=id,
          Body=body,
          State=state  
        };
        
       // Act && Assert
       await Assert.ThrowsAsync<ArgumentException>(()=>_sut.AddToDoItemAsync(input));

       //Assert
        _mockRepo.Verify(x=>x.AddToDoListItemAsync(It.IsAny<ToDoListDTO>()),Times.Never);

    }

    [Fact]
    public async Task AddToDoItemAsync_RepoThrows_ShouldPropagateException()
    {
        // Arrange
        var input = new ToDoListDTO { Id = 0, Body = "Valid Task", State = TodoState.Active };
        var repoException = new Exception("Database disk full");

        _mockRepo.Setup(r => r.AddToDoListItemAsync(It.IsAny<ToDoListDTO>()))
                .ThrowsAsync(repoException);

        // Act & Assert
        // We verify that the service throws the EXACT same exception the repo threw
        var thrownException = await Assert.ThrowsAsync<Exception>(() => 
            _sut.AddToDoItemAsync(input));

        Assert.Equal("Database disk full", thrownException.Message);
    }

    /// Update TO DO List 
    
    /// <summary>
    /// Test 1: Happy case : Correct input 
    /// 
    /// Verify Data Sent to the Repo Correctly
    [Fact]
    public async Task UpdateToDoItemAsync_ValidInputs_ShouldReturnUpdatedDTO()
    {
        // Assemble

        var input = new ToDoListDTO()
        {
            Id=10,
            Body="Test Item",
            State = TodoState.Active
        };

       _mockRepo.Setup(r => r.UpdateItemAsync(It.IsAny<ToDoListDTO>()))
         .ReturnsAsync((ToDoListDTO incoming) => {
             return incoming;
         });
        var updatedEntity = await _sut.UpdateItemAsync(input);
        

        // Assersions

        Assert.Equal(input.Id,updatedEntity.Id);
        Assert.Equal(input.Body,updatedEntity.Body);
        Assert.Equal(input.State,updatedEntity.State);

        _mockRepo.Verify(r=>
            r.UpdateItemAsync(It.Is<ToDoListDTO>(data =>
                data.Id==10 &&
                data.Body=="Test Item" && 
                data.State == TodoState.Active
            ))
            ,Times.Once()
        );
    }
    /// <summary>
    /// Test 2: Sad case : Invalid Inputs 
    /// 
    /// Verify:
    ///     1. Service Validates Data 
    ///     2. Service throws correct exception
    [Theory]
    [InlineData(0,"123",TodoState.Active)] // Invalid ID for updating 
    [InlineData(-1,"123",TodoState.Active)] 
    [InlineData(0,"",TodoState.Active)] // No Body 

    public async Task UpdateToDoItemAsync_InvalidInputs_ShouldThrowCorrectError(int id, string body,TodoState state)
    {
        // Given
        var input = new ToDoListDTO()
        {
            Id = id,
            Body = body,
            State = state
        };

        // When
        await Assert.ThrowsAsync<ArgumentException>(() => _sut.UpdateItemAsync(input));
        // Then
        _mockRepo.Verify(r => r.UpdateItemAsync(It.IsAny<ToDoListDTO>()), Times.Never());

    }

    // Delete 

    ///<summary>
    /// 
    /// Test 1. 
}