FROM mcr.microsoft.com/dotnet/sdk:6.0-focal AS build
WORKDIR /source
COPY . .
RUN dotnet restore "./CShop.User.Api/CShop.User.Api.csproj" --disable-parallel
RUN dotnet publish "./CShop.User.Api/CShop.User.Api.csproj" -c release -o /app --no-restore

# serve stage
FROM mcr.microsoft.com/dotnet/aspnet:6.0-focal
WORKDIR /app
COPY --from=build /app ./

EXPOSE 5000

ENTRYPOINT ["dotnet", "CShop.User.Api.dll"]