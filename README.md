<div align="center"><img src=/Jiyu_Manager/Icons/MainIcon-New.png/></div>
<h1><center>Jiyu Manager</center></h1>

### 软件截图
![image](/Github_Pics/MainWindow_Pic.png)
### 语言 & 环境
本程序采用C#编写，开发环境为.NET Framework 4.5 + VS2019
### 功能 & UI
- 通过hook解除控屏 / 黑屏 / 考试等
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

### Hook内容
#### 已实现
- SetWindowPos 已实现
- SetForegroundWindow 已实现
- BringWindowToTop 已实现
#### 未实现
- DeviceIoControl
- SetWindowLongA
- SetWindowLongW
- ShowWindow
- SendMessageW

### 更新内容
| 版本 | 内容 | 对应的Commit | Todo | 发布日期 |
|  ----  | ----  | ---- | ---- | ---- |
| 0.1 Dev | - 软件发布 <br> - 窗口连续置顶 <br> - 基础UI实现 <br> - Easyhook初步 (SetWindowPos) | Commit 1-4 | 完善UI、Hook | 20220618 |
| 0.2 Dev | - 新增Hook: <br> SetForegroundWindow <br> BringWindowToTop <br> 实现极域广播不置顶 <br> - 新增Debug Output，方便调试 <br> - 更换软件图标 <br> (原来那个伪Fluent Design的太丑了) | Commit 5,6,7 | 增加广播窗口调节大小功能 | 20220709 |
