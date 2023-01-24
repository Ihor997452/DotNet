FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
WORKDIR /CommandService
EXPOSE 80

COPY . ./

RUN dotnet restore
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /CommandService
COPY --from=build-env /CommandService/out .
ENTRYPOINT ["dotnet", "CommandService.dll"]
