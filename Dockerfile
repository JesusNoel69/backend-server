FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copiar el archivo .csproj al contenedor en el directorio adecuado
COPY Backend-Server.csproj ./Backend-Server/

# Cambiar al directorio donde está el .csproj
WORKDIR /app/Backend-Server

# Ejecutar el restore de dependencias
RUN dotnet restore

# Volver al directorio raíz
WORKDIR /app

# Copiar el resto de los archivos del proyecto
COPY . .

# Publicar el proyecto
RUN dotnet publish Backend-Server.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copiar el contenido publicado del contenedor anterior
COPY --from=build /app/publish .

# Definir el comando de entrada para ejecutar la aplicación
ENTRYPOINT ["dotnet", "Backend-Server.dll"]
