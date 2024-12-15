FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["SCT.Users/SCT.Users.csproj", "SCT.Users/"]
COPY ["SCT.Common/SCT.Common.csproj", "SCT.Common/"]
RUN dotnet restore "SCT.Users/SCT.Users.csproj"
COPY . .
WORKDIR "/src/SCT.Users"
RUN dotnet build "SCT.Users.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "SCT.Users.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SCT.Users.dll"]
