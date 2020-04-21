# WebApiDotnetCoreDemo

## 介绍
这是基于现有的流行组件和框架搭建的一套Web服务的模板，旨在方便一些开发中小型Web服务时可以直接拿来用，而不需要自己搭建框架和拼接现有组件

------------------------------------------

### BaseFrameworkDemo
WebApiCoreFx:（dotnet core3.1 + EFCore + Autofac + AspectCore + Log4net + Mem）

1.基于AspectCore实现AOP;<br>
2.自定义中间件;<br>
3.基于ActionFilterAttribute的A自定义特性AOP;<br>
4.增加WxAppUtil模块,整合微信小程序快捷登录功能<br>
(Demo:接口WebApiCoreFx工程:/api/Login/WxLogin)<br>
5.增加MemoryCache,Redis缓存<br>
(Demo:接口WebApiCoreFx工程:/api/Test/Get):MemoryCache<br>
(Demo:接口WebApiCoreFx工程:/api/Test/GetAll):Redis<br>

------------------------------------------

Ocelot.GatewayProj:(...)
Ocelot网关Demo

------------------------------------------

### ORM_Demo (已迁出为独立demo)
EFCore,Dapper,SqlSugar,LinqToDB

------------------------------------------

### Serging微服务框架参考Demo (已迁出为独立demo)
.Net Core 2.2 + Surging-1.0.0版本的微服务框架

------------------------------------------

## 软件架构
.Net Core WebApi,MVC,微服务

#### 参与贡献
