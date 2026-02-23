# Use the official .NET 9.0 SDK image for the build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

# Set the working directory
WORKDIR /app

# Copy the project files and restore any dependencies
COPY Core.API/Core.API.csproj Core.API/
RUN dotnet restore Core.API/Core.API.csproj

# Copy the rest of the application code
COPY . .

# Build the application
RUN dotnet build Core.API/Core.API.csproj -c Release -o /app/build

# Use the official runtime image for the final stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime

# Set the working directory for the runtime
WORKDIR /app

# Copy the built application from the build stage
COPY --from=build /app/build .

# Specify the entry point for the application
ENTRYPOINT ["dotnet", "Core.API.dll"]
