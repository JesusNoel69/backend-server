FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY Backend-Server.csproj ./Backend-Server/
WORKDIR /app/Backend
RUN dotnet restore

WORKDIR /app
COPY . . 
RUN dotnet publish Backend-Server.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "BackEnd_Server.dll"]