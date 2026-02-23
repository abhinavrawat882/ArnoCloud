# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy solution and project files
COPY ["Arnocloud.sln", "."]
COPY ["Core.API/Core.API.csproj", "Core.API/"]
COPY ["Auth.Service/Auth.Service.csproj", "Auth.Service/"]
COPY ["Notes.Service/Notes.Service.csproj", "Notes.Service/"]
COPY ["Notes.Service.Tests/Notes.Service.Tests.csproj", "Notes.Service.Tests/"]
COPY ["Shared/Shared.csproj", "Shared/"]

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
EXPOSE 80
EXPOSE 443

# Health check
HEALTHCHECK --interval=30s --timeout=3s --start-period=5s --retries=3 \
    CMD curl -f http://localhost/health || exit 1

# Run the application
ENTRYPOINT ["dotnet", "Core.API.dll"]
