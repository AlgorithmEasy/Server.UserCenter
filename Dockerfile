#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Server.UserCenter.csproj", "."]
RUN --mount=type=secret,id=package_token \
    TOKEN=`cat /run/secrets/package_token` && \
    dotnet nuget add source --username AlgorithmEasy --password $TOKEN --store-password-in-clear-text --name github "https://nuget.pkg.github.com/AlgorithmEasy/index.json"
COPY . .
RUN dotnet build "Server.UserCenter.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Server.UserCenter.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AlgorithmEasy.Server.UserCenter.dll"]