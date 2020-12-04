# J3space.Abp.Extensions

[WIP] abp vnext 框架的补充

![IdentityServer 效果图.png](./images/login_page.png)

## 说明

abp vnext 框架是 C# 平台优秀的开源项目，其模块化的设计，基于 DDD 的清晰的项目结构十分适合二次开发。

但是在 startup 项目中仍然有一些不足的地方。

1. 有些项目只在商业版本中可用，例如 Identity Server 模块对客户端，Api 资源的管理等。
2. 默认基于 Bootstrap 和 jQuery 的用户界面，官方提供的替换页面的方法远不如自行设计来的方便。

为此，本项目主要是基于自己开发过程中的需求，添加一些模块，或者替换原生的一些模块。

- [x] Identity Server
  - [x] 客户端资源管理接口(添加)
  - [x] Api 资源管理接口(添加)
  - [x] 身份资源管理接口(添加)
  - [x] 权限管理(添加)
  - [x] Consent 页面(重写)
- [x] Account
  - [x] 基于 [Bulma](https://bulma.io/) 轻量 css 框架的全新界面(重写)
  - [x] 忘记密码/重置密码功能(官方已添加)
  - [x] 外部身份认证(官方已添加)
- [x] Setting Management
  - [x] 查看，更改，重置接口(添加)
  - [x] 权限管理(添加)
- [ ] Blogging
  - [x] 文章管理接口(重写)
  - [x] 标签管理接口(重写)
  - [ ] 评论管理接口
  - [ ] 文章阅读访问量

## 测试

以下内容建立在你已经了解 [Abp vnext](https://docs.abp.io/en/abp/latest) 的基础上。

### 本地环境

1. 建立数据库，本项目使用的数据库是 MySQL
2. 克隆本仓库，填写所有 appsettings.json 文件中 “空缺” 的字段
3. 运行 application 目录下的 J3space.Auth 项目，此时可在 https://localhost:5001 查看用户登录相关功能
4. 克隆 [CodeLearner](https://github.com/taujiong/CodeLearner) 仓库
5. 配置 IdentityServer 目录下的 Data.Shared 项目中的 IdsConstants 类，设置相应参数
6. 依次运行 IdentityServer 目录下的 Api1、ConsoleClient、WpfClient、MvcClient 项目
7. 运行 application 目录下的 J3space.Admin 项目
8. 运行 application 目录下的 J3space.Blogging 项目
9. 运行 application 目录下的 J3space.Guard 项目，此时可访问 https://localhost:5004 查看 swagger 页面，此 swagger 页面包含了所有应用的 api 接口
10. 通过 Postman 等工具可以查看 IdentityServer 的授权功能

### docker 环境（推荐）

1. 安装 docker，docker-compose 环境；安装 mkcert，用于本地 https 配置
2. 修改本地 hosts 文件，将 j3space.dev 和 api.j3space.dev 指向 127.0.0.1
3. cd ./dockerize/dev/ssl && mkcert --install && mkcert j3space.dev && mkcert api.j3space.dev
4. docker-compose up -d
5. 运行 application 目录下的 J3space.Auth 项目，此时可在 https://j3space.dev 查看用户登录相关功能
6. 克隆 [CodeLearner](https://github.com/taujiong/CodeLearner) 仓库
7. 配置 IdentityServer 目录下的 Data.Shared 项目中的 IdsConstants 类，设置相应参数
8. 依次运行 IdentityServer 目录下的 Api1、ConsoleClient、WpfClient、MvcClient 项目
9. 运行 application 目录下的 J3space.Admin 项目
10. 运行 application 目录下的 J3space.Blogging 项目
11. 运行 application 目录下的 J3space.Guard 项目，此时可访问 https://api.j3space.dev 查看 swagger 页面，此 swagger 页面包含了所有应用的 api 接口
12. 通过 Postman 等工具可以查看 IdentityServer 的授权功能
