# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /source

# Copy csproj and restore dependencies
COPY src/*.csproj ./src/
RUN dotnet restore ./src/InsuranceApp.csproj

# Copy everything else and build
COPY src/ ./src/
WORKDIR /source/src
RUN dotnet publish -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/publish .

# Create Scripts directory and copy SQL scripts
RUN mkdir -p /app/Scripts
COPY --from=build /source/src/Data/Scripts/*.sql /app/Scripts/

# Expose port
EXPOSE 80

ENTRYPOINT ["dotnet", "InsuranceApp.dll"]
