FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

WORKDIR /src
COPY ["src/LibrarySystem.Web.Host/LibrarySystem.Web.Host.csproj", "src/LibrarySystem.Web.Host/"]
COPY ["src/LibrarySystem.Web.Core/LibrarySystem.Web.Core.csproj", "src/LibrarySystem.Web.Core/"]
COPY ["src/LibrarySystem.Application/LibrarySystem.Application.csproj", "src/LibrarySystem.Application/"]
COPY ["src/LibrarySystem.Core/LibrarySystem.Core.csproj", "src/LibrarySystem.Core/"]
COPY ["src/LibrarySystem.EntityFrameworkCore/LibrarySystem.EntityFrameworkCore.csproj", "src/LibrarySystem.EntityFrameworkCore/"]
WORKDIR "/src/src/LibrarySystem.Web.Host"
RUN dotnet restore 

WORKDIR /src
COPY ["src/LibrarySystem.Web.Host", "src/LibrarySystem.Web.Host"]
COPY ["src/LibrarySystem.Web.Core", "src/LibrarySystem.Web.Core"]
COPY ["src/LibrarySystem.Application", "src/LibrarySystem.Application"]
COPY ["src/LibrarySystem.Core", "src/LibrarySystem.Core"]
COPY ["src/LibrarySystem.EntityFrameworkCore", "src/LibrarySystem.EntityFrameworkCore"]
WORKDIR "/src/src/LibrarySystem.Web.Host"
RUN dotnet publish -c Release -o /publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:7.0
EXPOSE 80
WORKDIR /app
COPY --from=build /publish .
ENTRYPOINT ["dotnet", "LibrarySystem.Web.Host.dll"]
