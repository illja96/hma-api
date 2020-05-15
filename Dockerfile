FROM illja96/dotnet-core-sdk-openjdk:latest AS build
WORKDIR /app

RUN echo "${BUILD_NUMBER}"
RUN echo "${build_number}"
RUN echo "${build.number}"

RUN dotnet tool install --global dotnet-sonarscanner
ENV PATH="${PATH}:/root/.dotnet/tools"

COPY . ./
RUN dotnet restore

RUN dotnet sonarscanner begin /k:"%sonar.projectKey%" /v:"%build.number%" /d:sonar.host.url="%sonar.host.url%" /d:sonar.login="%sonar.login%" /d:sonar.cs.opencover.reportsPaths="**/coverage.opencover.xml"
RUN dotnet build -c Release --no-restore /p:Version="1.0.0.%build.number%" /p:AssemblyVersion="1.0.0.%build.number%" /p:InformationalVersion="1.0.0.%build.number%" /p:FileVersion="1.0.0.%build.number%"
RUN dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover
RUN dotnet sonarscanner end /d:sonar.login="%sonar.login%"

RUN dotnet publish -c Release -o out --no-restore --no-build /p:Version="1.0.0.%build.number%" /p:AssemblyVersion="1.0.0.%build.number%" /p:InformationalVersion="1.0.0.%build.number%" /p:FileVersion="1.0.0.%build.number%"

FROM mcr.microsoft.com/dotnet/core/aspnet:latest
WORKDIR /app
COPY --from=build /app/out ./
ENTRYPOINT ["dotnet", "HMA.API.dll"]