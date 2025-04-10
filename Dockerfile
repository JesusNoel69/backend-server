FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY BackEnd-Server.csproj .
RUN dotnet restore BackEnd-Server.csproj

COPY . .
RUN dotnet publish BackEnd-Server.csproj -c Release -o /app/published

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

COPY --from=build /app/published .

ENTRYPOINT ["dotnet", "BackEnd-Server.dll"]
