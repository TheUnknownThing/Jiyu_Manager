<p><img src=/ICON/MainIcon.png align="middle"/></p>
<h1><center>Jiyu Manager</center></h1>

### 软件截图
![image](/Github_Pics/MainWindow_Pic.png)
### 语言 & 环境
本程序采用C#编写，开发环境为.NET Framework 4.5 + VS2019
### 功能 & UI
- 通过hook解除控屏 / 黑屏 / 考试等 (仍在开发，HookInject遇到问题)
- 始终保持置顶
- Taskkill关闭极域
- NTSD关闭极域 (仍在开发)
- 遵循微软Fluent Design设计语言风格，使用UWP样式控件
- More…
### Pull Request & 协同开发
我会尽量及时将测试通过的Pull request Merge到master分支中，欢迎提交Issues / Requests
### Nuget Packages
- ModernWPF https://github.com/Kinnara/ModernWpf/releases/tag/v0.9.5
- Easyhook https://github.com/EasyHook/EasyHook
### 核心代码
Jiyu_Manager/Jiyu_Manager_Class中的
- TopMostClass
- JiyuManagerHookClass

Jiyu_Hooker中的
- HookLibrary