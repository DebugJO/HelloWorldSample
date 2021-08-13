### Desktop UI Showdown
* reference : tim Corey, https://www.youtube.com/watch?v=yq0dSkA1vpM

| <i class="fa fa-windows" aria-hidden="true"></i> | Purpose | Benefits | Drawbacks |
|:-:|:-:|:-:|:-:|
| Console | Automation | Quick/Simple<br>Stable<br>Great for Testing<br>Cross-platform | Not viable for regular users |
| WinForm | Original UI/RAD | Quick/Easy<br>Widely Used<br>Reliable | CPU-Bound<br>Hard to scale |
| WPF | Powerful UI | DirectX Powered<br>Modern Design<br>Bindings/MVVM<br>Supports Designers | Slow Development<br>XAML is confusing<br>Too Many options |
| UWP | Universal/App-style | Sandboxed<br>Store Distribution<br>Clear Standards | Sandboxed<br>Difficult to distribute<br>Only works on Win10/Xbox |

### C# 코딩스타일
1. 클래스 및 구조체에 파스칼 케이스 사용
2. 지역 변수 이름과 함수 매개 변수에 카멜 케이스 사용
3. 모든 메소드 이름은 파스칼 케이스 사용
4. 네임스페이스는 파스칼 케이스 사용
5. boolean 변수 앞에 b붙이고, 프로퍼트에 앞에 is 붙임
6. 인터페이스 앞에는 I를 붙임
7. enum 앞에는 E붙임
8. private 멤버 변수 앞에는 m붙이고, 나머지 파스칼 케이스 사용

http://lonpeach.com/2017/12/24/CSharp-Coding-Standard/ 
https://namu.wiki/w/%EC%BD%94%EB%94%A9%20%EC%8A%A4%ED%83%80%EC%9D%BC

카멜 표기법과 파스칼 표기법을 적절하게 조합하여,
1. 변수명이나 함수명은 카멜 표기법을 따르고 클래스명은 파스칼 표기법을 따르는 작성 스타일, 
2. 혹은 변수명은 카멜로 표기하고 함수와 클래스명은 파스칼로 표기하는 작성 스타일이 대세이다. 
3. 전자는 Java, 후자는 C++(C#)에서 주로 볼 수 있는 스타일이다.

### LINQ Tutorial
* [LINQ Tutorial-kudvenkat](https://www.youtube.com/playlist?list=PL6n9fhu94yhWi8K02Eqxp3Xyh_OmQ0Rp6)


### Async / Await

```cs
private async void Button_Click(object sender, RoutedEventArgs e)
{
    var progress = new Progress<int>(value =>
    {
        _progressBar.Value = value;
        _textBlock.Text = $"{value}%";
    });

    await Task.Run(() => LoopThroughNumbers(100, progress));
	
    _textBlock.Text = "completed";
}

void LoopThroughNumbers(int count, IProgress<int> progress)
{
    for (var x = 0; x < count; x++)
    {
        Thread.Sleep(100);
        var percentComplete = (x * 100) / count;
        progress.Report(percentComplete);
    }
}
```


### Visual Studio Code 사용법
```
// vscode
솔루션 파일 생성
dotnet new sln -n "HelloSln"

콘솔 프로젝트 생성
dotnet new console -n "HelloUI"
 
클래스 라이브러리 생성
dotnet new classlib -n "HelloLib"

프로젝트 등록
bash : dotnet sln HelloSln.sln add **/*.csproj
windows : dotnet sln HelloSln.sln add .\HelloUI\HelloUI.csproj

프로젝트에 라이브러리 참조 추가
dotnet add HelloUI\HelloUI.csproj reference HelloLib\HelloLib.csproj

클래스 라이브러리에 패키지 추가(클래스라이브러리 폴더에서)
dotnet add package dapper
또는 패키지 매니저 사용

SSL reset
dotnet dev-certs https --clean
dotnet dev-certs https --trust

싱글 파일 배포
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeAllContentForSelfExtract=true
* win-x64, win-x86, linux-x64, linux-arm, linux-arm64, osx-x64
```
