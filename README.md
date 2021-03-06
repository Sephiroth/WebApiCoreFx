# WebApiDotnetCoreDemo

## 介绍
这是基于现有的流行组件、框架和官方库搭建的一套Web服务的模板,旨在学习 .net core webapi框架和微服务，以及项目使用。方便一些开发中小型Web服务时可以直接拿来用，而不需要自己搭建框架和拼接现有组件。<br>
包括多种IOC、AOP方式, Redis缓存、Memory缓存、EFCore.MySql仓储、微信快捷登录等。

------------------------------------------

### BaseFrameworkDemo
WebApiCoreFx:(dotnet core3.1 + EFCore + Autofac + AspectCore + Log4net + Redis + ...)

基于AspectCore实现AOP;<br>

------------------------------------------

自定义中间件;<br>

------------------------------------------

基于ActionFilterAttribute的A自定义特性AOP;<br>

------------------------------------------

增加WxAppUtil模块,整合微信小程序快捷登录功能<br>
(Demo:接口WebApiCoreFx工程:/api/Login/WxLogin)<br>

------------------------------------------

增加MemoryCache,Redis缓存<br>
(Demo:接口WebApiCoreFx工程:/api/Test/Get):MemoryCache<br>
(Demo:接口WebApiCoreFx工程:/api/Test/GetAll):Redis

------------------------------------------

新增MultipleCache.CoreComponent组件:<br>
    基于ActionFilterAttribute实现API缓存(Redis)

------------------------------------------

优化EFCore.MySql自定义泛型仓储:<br>
    +字段排序; +查询字段选择; (亲测OK)

------------------------------------------
加EncryptionTool.OpenSsl.RSAOpenSslTool,简单RSA加密解密组件;api/Security/GetRSAPublicKey获取公钥

------------------------------------------

添加Ocelot.ConsulExtensions组件，用于把服务注册到consul<br>
参考UserCenterApi项目的用法

------------------------------------------

Ocelot.GatewayProj
Ocelot网关Demo(未完成，近期补充)

------------------------------------------

### ORM_Demo (已迁出为独立demo)
EFCore,Dapper,SqlSugar,LinqToDB

------------------------------------------

### Surging微服务框架参考Demo (已迁出为独立demo)
.Net Core 2.2 + Surging-1.0.0版本的微服务框架

------------------------------------------

## 软件架构
.Net Core WebApi,MVC,微服务

#### 参与贡献
