FROM illja96/dotnet-core-sdk-openjdk:latest AS build

ARG BUILD_NUMBER
RUN test -n "${BUILD_NUMBER}" || (echo "BUILD_NUMBER argument not provided" && false)

ARG SONAR_HOST_URL
RUN test -n "${SONAR_HOST_URL}" || (echo "SONAR_HOST_URL argument not provided" && false)

ARG SONAR_PROJECTKEY
RUN test -n "${SONAR_PROJECTKEY}" || (echo "SONAR_PROJECTKEY argument not provided" && false)

ARG SONAR_LOGIN
RUN test -n "${SONAR_LOGIN}" || (echo "SONAR_LOGIN argument not provided" && false)

WORKDIR /app
RUN dotnet tool install --global dotnet-sonarscanner
ENV PATH="${PATH}:/root/.dotnet/tools"

COPY . ./
RUN dotnet restore

RUN dotnet sonarscanner begin /k:"$SONAR_PROJECTKEY" /v:"${BUILD_NUMBER}" /d:sonar.host.url="${SONAR_HOST_URL}" /d:sonar.login="${SONAR_LOGIN}" /d:sonar.cs.opencover.reportsPaths="**/coverage.opencover.xml"
RUN dotnet build -c Release --no-restore /p:Version="1.0.0.${BUILD_NUMBER}" /p:AssemblyVersion="1.0.0.${BUILD_NUMBER}" /p:InformationalVersion="1.0.0.${BUILD_NUMBER}" /p:FileVersion="1.0.0.${BUILD_NUMBER}"
RUN dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover
RUN dotnet sonarscanner end /d:sonar.login="${SONAR_LOGIN}"

RUN dotnet publish -c Release -o out --no-restore --no-build /p:Version="1.0.0.${BUILD_NUMBER}" /p:AssemblyVersion="1.0.0.${BUILD_NUMBER}" /p:InformationalVersion="1.0.0.${BUILD_NUMBER}" /p:FileVersion="1.0.0.${BUILD_NUMBER}"

FROM mcr.microsoft.com/dotnet/core/aspnet:latest
WORKDIR /app
COPY --from=build /app/out ./
ENTRYPOINT ["dotnet", "HMA.API.dll"]