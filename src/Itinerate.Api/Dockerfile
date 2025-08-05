# Stage 1: Build the application
# This stage compiles the C# code
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy the solution file to the root of the app directory
COPY Itinerate.sln .

# Copy the project files for all projects in the solution
# This is crucial for the 'dotnet restore' command to work correctly
COPY src/Itinerate.Api/Itinerate.Api.csproj ./src/Itinerate.Api/
COPY src/Itinerate.Domain/Itinerate.Domain.csproj ./src/Itinerate.Domain/
COPY src/Itinerate.Infrastructure/Itinerate.Infrastructure.csproj ./src/Itinerate.Infrastructure/

# Restore NuGet packages for the entire solution
# This leverages Docker caching. It only re-runs if the .csproj files change.
RUN dotnet restore Itinerate.sln

# Copy the rest of the source code
COPY ./src ./src

# Publish the API project, creating a release-optimized build
WORKDIR /app/src/Itinerate.Api
RUN dotnet publish -c Release -o /app/out

# Stage 2: Create the final, lightweight runtime image
# This stage uses the compiled output from the 'build' stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "Itinerate.Api.dll"]