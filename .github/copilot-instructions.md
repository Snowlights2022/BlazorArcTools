<!-- .github/copilot-instructions.md - guidance for AI coding agents -->
# Copilot 指南 — BlazorArcTools

目标：让 AI 代理能快速理解并在本仓库中安全、可预测地产生有用变更。以下内容基于项目代码可发现的约定与实现。

- 项目类型：Blazor WebAssembly 应用（目标框架 net9.0），UI 框架使用 MudBlazor。主要入口：`BlazorArcTools/Program.cs`。
- 主要结构：
  - `BlazorArcTools/`：应用源代码（Pages、Layout、Componets、wwwroot 等）。
  - `Pages/`：页面组件（如 `Home.razor`, `Weather.razor`, `NotFound.razor`）。
  - `Layout/`：布局组件（`MainLayout.razor` 使用 MudBlazor providers）。
  - `Componets/`：小组件（例如 `RedirectNotFound.razor` 用于路由重定向）。

工作流与常用命令（可在本地 PowerShell 中运行）：

  - 本地开发（Blazor WASM）：使用 dotnet CLI 在仓库根目录运行 `dotnet build` / `dotnet run`（默认会在 `BlazorArcTools` 项目上工作）。
  - 发布到静态文件夹：项目包含 `PublishToFolder` 输出；发布通常通过 `dotnet publish -c Release` 完成，产物可在 `PublishToFolder/wwwroot` 中找到。

项目约定与注意事项（对改动者和自动更改非常重要）：

- UI 与样式：项目通过 `_Imports.razor` 引入 `MudBlazor`，并在 `wwwroot/index.html` 引用 MudBlazor CSS/JS。凡是涉及样式或 Mud 组件变动，应同时检查 `wwwroot/index.html` 和 `_Imports.razor`。
- 路由与 404：`App.razor` 配置了路由与 NotFound 处理，NotFound 页面通过 `Componets/RedirectNotFound.razor` 导向 `/notfound`。不要直接删除该组件，若修改路由逻辑请同时更新 `NotFound.razor` 的导航回退逻辑（使用 `IJSRuntime` 查询 `history.length`）。
- 依赖管理：核心第三方包为 `MudBlazor`（见 `BlazorArcTools.csproj`）。更新 MudBlazor 时请同时更新 `wwwroot/index.html` 中的版本参数（注释已提示）。

代码风格与常用模式（可据此生成变更）：

- 组合式 Razor 组件：页面一般使用 `@page` 指令并在 @code 块中实现事件/状态（示例：`Pages/Home.razor`）。保持事件同步/异步语义一致（同步事件返回 void，异步事件返回 Task）。
- 依赖注入：在组件中使用 `[Inject]` 或 `@inject` 注入 `NavigationManager`, `IJSRuntime` 和 HttpClient（Program.cs 在 DI 容器中注册 HttpClient）。自动补全注入时，使用现有命名风格（驼峰首字母小写，例如 `NavigationManager NavigationManager` 或 `[Inject] NavigationManager Manager { get; set; }`）。
- 布局模式：`MainLayout.razor` 使用 MudBlazor provider 组件（`MudThemeProvider`, `MudDialogProvider` 等）与 `MudLayout`。新增页面应直接依赖默认布局，若需要自定义布局，创建新的 Layout 组件并在页面上指定 `@layout`。

测试与验证建议（自动更改应执行的最小检查）：

- 在提交含 UI 或依赖变更前运行：
  - `dotnet build`（确保无编译错误）
  - `dotnet publish -c Release`（若更改可能影响发布产物）
- 简单运行验证：在浏览器打开 `index.html` 后端（`dotnet run`）并检查主页面 `/`、`/notfound` 与含 Mud 组件的页面（`/weather`）是否正常渲染。

当你不确定时（自动代理行为准则）：

- 不要擅自升级主要依赖（如 MudBlazor）除非变更与对应的 CSS/JS 引用、版本参数一并更新并通过构建验证。
- 修改路由、导航、或全局布局前，请检查 `App.razor`, `Layout/MainLayout.razor`, `Componets/RedirectNotFound.razor`, 和 `Pages/NotFound.razor` 是否需要配套调整。

文件参考（生成建议时直接引用这些路径）：
- `BlazorArcTools/Program.cs` — 应用入口与 DI 注册
- `BlazorArcTools/BlazorArcTools.csproj` — 依赖清单（MudBlazor）
- `BlazorArcTools/wwwroot/index.html` — 静态入口、MudBlazor CSS/JS 引用
- `BlazorArcTools/Pages/*.razor` — 页面实现样例（Home, Weather, NotFound）
- `BlazorArcTools/Layout/MainLayout.razor` — 布局与 Mud providers
- `BlazorArcTools/Componets/RedirectNotFound.razor` — 路由重定向模式

如果本文件需要补充更多细节（例如 CI、测试、或特定发布步骤），请告诉我想要补充的部分或提供 CI 配置文件以便合并。

---
请审阅并指出希望增加的具体示例或工作流（例如 CI 命令、发布脚本或编码规范）。
