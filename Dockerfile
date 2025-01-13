# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080

ENV AzureAI_API_KEY="6TCCmMdBSLGN5FEdXTkhv3aMUetKWqSKqF9Ch97jNrh3Mwmj5pl6JQQJ99BAACYeBjFXJ3w3AAAAACOGffM4"
ENV AzureAI_EndPoint="https://rtchat-cognitive-service.cognitiveservices.azure.com/"
ENV AzureDB_Connection="Server=tcp:rtchat.database.windows.net,1433;Initial Catalog=RTChatDB;Persist Security Info=False;User ID=dimasss.net.dev;Password=Dima_2000;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
ENV Front_Url="https://rtchat.azurewebsites.net"
# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["RTChat/RTChat.csproj", "RTChat/"]
COPY ["Infrastructure/Infrastructure.csproj", "Infrastructure/"]
COPY ["Domain/Domain.csproj", "Domain/"]
RUN dotnet restore "./RTChat/RTChat.csproj"
COPY . .
WORKDIR "/src/RTChat"
RUN dotnet build "./RTChat.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./RTChat.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RTChat.dll"]


