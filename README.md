# J3space.Abp.Extensions

[WIP] abp vnext 框架的补充

![IdentityServer 效果图.png](https://i.loli.net/2020/07/31/QbtqvCuNMAkrfjz.png)

## 说明

abp vnext 框架是 C# 平台优秀的开源项目，其模块化的设计，基于 DDD 的清晰的项目结构十分适合二次开发。

但是在 startup 项目中仍然有一些不足的地方。

1. 有些项目只在商业版本中可用，例如 Identity Server 模块对客户端，Api 资源的管理等。
2. 默认基于 Bootstrap 和 jQuery 的用户界面，官方提供的替换页面的方法远不如自行设计来的方便。
3. 封装了一些可能不需要的组件，如作为"一等公民"的多租户功能往往并非每个项目都会用到。

为此，本项目主要是基于自己开发过程中的需求，添加一些模块，或者替换原生的一些模块。

- [ ] Identity Server
  - [ ] 客户端资源管理
    - [x] 后端
    - [ ] 前端
  - [ ] Api 资源管理
    - [x] 后端
    - [ ] 前端
  - [ ] 身份资源管理
    - [x] 后端
    - [ ] 前端
  - [x] Consent 页面
- [ ] Account
  - [x] 纯 css 的登录、注册界面
  - [x] 忘记密码/重置密码功能
  - [x] 外部身份认证

## 测试

以下内容建立在你已经了解 [Abp vnext](https://docs.abp.io/en/abp/latest) 的基础上。

1. 建立数据库，SampleApp 使用的是 MongoDB
2. 克隆本仓库，在 appsettings.json 中配置 GitHub 的 ClientId 以及 Client Secret
3. 运行 J3space.Sample.HttpApi.Web 项目
4. 克隆 [Learn.IdentityServer](https://github.com/taujiong/Learn.IdentityServer)
5. 首先运行 Api1 项目，再依次运行 ConsoleClient，WpfClient，MvcClient 项目
