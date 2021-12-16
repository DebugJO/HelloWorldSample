### 프로젝트 템플릿
* MvxStarter : MVVMCROSS 템플릿
* WPFApp : [BinaryBunny, "WPF C# Professional Modern Chat App UI Tutorial"](https://www.youtube.com/watch?v=V9DkvcT27WI)
* WpfApp1 : WPF 템플릿
* Different_ContentControl_CaliburnMicro : [StackOverflow](https://stackoverflow.com/questions/60197133/caliburn-micro-how-to-bind-a-specific-item-of-conductor-collection-allactive-t)

### WPF High-DPI (app.manifest)
```xml
<application xmlns="urn:schemas-microsoft-com:asm.v3">
  <windowsSettings>
    <dpiAwareness xmlns="http://schemas.microsoft.com/SMI/2016/WindowsSettings">PerMonitorV2, PerMonitor, System</dpiAwareness>
    <dpiAware xmlns="http://schemas.microsoft.com/SMI/2005/WindowsSettings">true/PM</dpiAware>
    <longPathAware xmlns="http://schemas.microsoft.com/SMI/2016/WindowsSettings">true</longPathAware>
  </windowsSettings>
</application>
```
