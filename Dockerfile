FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copiar el archivo .csproj en el contenedor
COPY BackEnd-Server.csproj ./BackEnd-Server/

WORKDIR /app/Backend-Server
RUN dotnet restore

WORKDIR /app
COPY . .
RUN dotnet publish BackEnd-Server.csproj -c Release -o /app/publish


FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copiar el contenido publicado del contenedor anterior
COPY --from=build /app/publish .

# Definir el comando de entrada para ejecutar la aplicación
ENTRYPOINT ["dotnet", "Backend-Server.dll"]
