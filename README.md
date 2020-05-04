# HMA API

HMA API is a part of House Money Accountant solution.
This part contains HTTP REST back-end server written using .NET Core SDK.

# Requirements

Required:
- [.NET Core 3.1 SDK](https://dotnet.microsoft.com)
- [Elasticsearch 7.6.2](https://www.elastic.co/elasticsearch)
- [MongoDB Community server](https://www.mongodb.com)

Optional:
- [Docker Desktop](https://www.docker.com/products/docker-desktop)
- [Kibana 7.6.2](https://www.elastic.co/kibana)
- [Mongo Express](https://github.com/mongo-express/mongo-express)

# Basic setup

1. Create [OAuth 2.0 Client in Google Cloud Platform Console](https://console.cloud.google.com/apis/credentials).

2. Add next allowed JS origins:
    - https://localhost:44361
    - http://localhost:63216
    - http://localhost:4200

3. Add next allowed redirect URIs:
    - https://localhost:44361/oauth2-redirect.html
    - http://localhost:63216/oauth2-redirect.html
    - https://localhost:44361/signin-google
    - http://localhost:63216/signin-google
    - http://localhost:4200

# Setup for development

1. Install .NET Core SDK and Docker Desktop.

2. Run next commands in PowerShell to setup ELK + Mongo stacks:

    ``` PowerShell
    docker network create elastic

    docker run -d --name elasticsearch --net elastic -p 9200:9200 -p 9300:9300 -e "discovery.type=single-node" elasticsearch:7.6.2

    docker run -d --name kibana --net elastic -p 5601:5601 kibana:7.6.2

    docker network create mongo

    docker run -d --name mongo --net mongo -p 27017:27017 mongo:latest

    docker run -d --name mongo-express --net mongo -p 8081:8081 mongo-express:latest
    ```

    > **Caution!** All saved data in your docker images will be lost if docker gets down! System reboot / shutdown, image restart and other events included!

3. Crate a copy of **appsettings.json** file with **appsettings.Development.json** name.

4. Save a tree structure, but remove all values except the `null` ones. Then fill them with corresponding values.

    Example of result file:
    ``` JSON
    {
      "Serilog": {
        "MinimumLevel": {
          "Default": "Debug"
        },
        "WriteTo": [
          {
            "Args": {
              "nodeUris": "http://localhost:9200"
            }
          }
        ]
      },
      "CorsOptions": {
        "AllowedOrigins": [
          "localhost:44361",
          "localhost:63216",
          "localhost:4200"
        ]
      },
      "GoogleOptions": {
        "ClientId": "Google OAuth 2.0 Client ID"
      },
      "MongoDbOptions": {
        "ConnectionString": "mongodb://localhost:27017"
      }
    }
    ```