# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy solution and project files
COPY ["Arnocloud.sln", "."]
COPY ["Core.API/Core.API.csproj", "Core.API/"]
COPY ["Notes.Service/Notes.Service.csproj", "Notes.Service/"]
COPY ["Notes.Service.Tests/Notes.Service.Tests.csproj", "Notes.Service.Tests/"]
COPY ["ToDoList.Service/ToDoList.Service.csproj", "ToDoList.Service/"]
COPY ["ToDoList.Service.Test/ToDoList.Service.Test.csproj", "ToDoList.Service.Test/"]
COPY ["Workspace.Service/Workspace.Service.csproj", "Workspace.Service/"]
COPY ["Workspace.Service.Test/Workspace.Service.Test.csproj", "Workspace.Service.Test/"]
COPY ["Shared/Shared.csproj", "Shared/"]
COPY ["ActionVault.Service/ActionVault.Service.csproj", "ActionVault.Service/"]

# Restore NuGet packages
RUN dotnet restore "Arnocloud.sln"

# Copy the rest of the source code
COPY . .

# Build the application
RUN dotnet build "Arnocloud.sln" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "Core.API/Core.API.csproj" -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

COPY --from=publish /app/publish .

# Expose port (adjust if your API uses a different port)
EXPOSE 5001
EXPOSE 7232

# Health check
HEALTHCHECK --interval=30s --timeout=3s --start-period=5s --retries=3 \
    CMD curl -f http://localhost/health || exit 1

# Run the application
ENTRYPOINT ["dotnet", "Core.API.dll"]
