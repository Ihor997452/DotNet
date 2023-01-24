FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
WORKDIR /PlatformService
EXPOSE 80

COPY . ./

RUN dotnet restore
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /PlatformService
COPY --from=build-env /PlatformService/out .
ENTRYPOINT ["dotnet", "PlatformService.dll"]