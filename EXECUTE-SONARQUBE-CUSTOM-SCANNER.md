1. Install Scanner .NET Core Global Tool

> dotnet tool install --global dotnet-sonarscanner

2. Execute the Scanner

>dotnet sonarscanner begin /k:"universal.opimizer" /d:sonar.host.url="http://localhost:9000"  /d:sonar.token="sqp_d7bbefbdcc1a310cff2986cdb5498d7a00e76f5e"

>dotnet build

>dotnet sonarscanner end /d:sonar.token="sqp_d7bbefbdcc1a310cff2986cdb5498d7a00e76f5e"