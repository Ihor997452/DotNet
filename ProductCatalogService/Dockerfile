FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
WORKDIR /ProductCatalogService
EXPOSE 80

COPY . ./

RUN dotnet restore
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /ProductCatalogService
COPY --from=build-env /ProductCatalogService/out .
ENTRYPOINT ["dotnet", "ProductCatalogService.dll"]
