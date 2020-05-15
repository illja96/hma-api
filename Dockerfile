FROM illja96/dotnet-core-sdk-openjdk:latest AS build
ARG $BUILD_NUMBER
ARG $SONAR_HOST_URL
ARG $SONAR_PROJECTKEY
ARG $SONAR_LOGIN

RUN echo "${BUILD_NUMBER}"
RUN echo "${SONAR_HOST_URL}"
RUN echo "${SONAR_PROJECTKEY}"
RUN echo "${SONAR_LOGIN}"

WORKDIR /app

RUN dotnet tool install --global dotnet-sonarscanner
ENV PATH="${PATH}:/root/.dotnet/tools"

COPY . ./
RUN dotnet restore

RUN dotnet sonarscanner begin /k:"$SONAR_PROJECTKEY" /v:"$BUILD_NUMBER" /d:sonar.host.url="$SONAR_HOST_URL$" /d:sonar.login="$SONAR_LOGIN" /d:sonar.cs.opencover.reportsPaths="**/coverage.opencover.xml"
RUN dotnet build -c Release --no-restore /p:Version="1.0.0.$BUILD_NUMBER" /p:AssemblyVersion="1.0.0.$BUILD_NUMBER" /p:InformationalVersion="1.0.0.$BUILD_NUMBER" /p:FileVersion="1.0.0.$BUILD_NUMBER"
RUN dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover
RUN dotnet sonarscanner end /d:sonar.login="$SONAR_LOGIN"

RUN dotnet publish -c Release -o out --no-restore --no-build /p:Version="1.0.0.$BUILD_NUMBER" /p:AssemblyVersion="1.0.0.$BUILD_NUMBER" /p:InformationalVersion="1.0.0.$BUILD_NUMBER" /p:FileVersion="1.0.0.$BUILD_NUMBER"

FROM mcr.microsoft.com/dotnet/core/aspnet:latest
WORKDIR /app
COPY --from=build /app/out ./
ENTRYPOINT ["dotnet", "HMA.API.dll"]