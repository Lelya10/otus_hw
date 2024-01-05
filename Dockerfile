FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/nightly/sdk:8.0-preview AS build
ARG BUILD_CONFIGURATION=Release
ARG TARGETARCH
WORKDIR /src
COPY . ./MyService
RUN dotnet restore "MyService/MyService.csproj" -a $TARGETARCH
COPY . .
WORKDIR "/src/MyService"
RUN dotnet build "MyService.csproj" -c $BUILD_CONFIGURATION -o /app/build
RUN dotnet publish "MyService.csproj" -c $BUILD_CONFIGURATION -a $TARGETARCH -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS final
USER $APP_UID
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "MyService.dll"]
