# .NET Core Web API Example

1. DotnetRPG : [Patrick God, ".NET Core 3.1 Web API & Entity Framework Core Jumpstart"](https://www.youtube.com/watch?v=H4qg9HJX_SE)

### kestrel 서비스 설정(CentOS)
/etc/systemd/system : websvcBlazor.service
```
[Unit]
Description=.NET Blazor App running on CentOS

[Service]
WorkingDirectory=/webservice/websvcBlazor
ExecStart=/usr/bin/dotnet /webservice/websvcBlazor/websvcBlazor.dll --urls http://0.0.0.0:50080
Restart=always
# Restart service after 10 seconds if the dotnet service crashes:
RestartSec=10
KillSignal=SIGINT
SyslogIdentifier=dotnet-webservice
User=root
Environment=ASPNETCORE_ENVIRONMENT=Production
Environment=DOTNET_PRINT_TELEMETRY_MESSAGE=false

[Install]
WantedBy=multi-user.target
```
