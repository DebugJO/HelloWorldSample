# HelloWorldSample

Hello World Template, Example by programming language

* C# ∙ Rust ∙ Zig ∙ C++ ∙ Go
* Database : Oracle ∙ SQL Server ∙ MariaDB ∙ PostgreSQL ∙ Firebird
* Desktop : Avalonia, WPF, Qt, Electrobun, Blazor(.NET MAUI Blazor Hybrid)


```bash
# Blazor Hybrid
# /p:HasPackageAndPublishMenu=false: MSIX 설치 패키지 형태가 아니고 일반(폴더/파일) 만들 때
dotnet publish -c Release /p:RuntimeIdentifier=win-x64 /p:PublishSingleFile=true /p:SelfContained=true /p:HasPackageAndPublishMenu=false
```
