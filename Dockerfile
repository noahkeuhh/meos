# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Kopieer de solution en projectbestanden
COPY *.sln .
COPY Meos/*.csproj ./Meos/
COPY Meos_API/*.csproj ./Meos_API/
COPY Meos_Shared/*.csproj ./Meos_Shared/

# Restore dependencies via de solution
RUN dotnet restore Meos.sln

# Kopieer alle code
COPY . .

# Build en publish API (hosted Blazor WASM)
WORKDIR /app/Meos_API
RUN dotnet publish -c Release -o out

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/Meos_API/out .
ENTRYPOINT ["dotnet", "Meos_API.dll"]
