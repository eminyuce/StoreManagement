# StoreManagement
Store Management Project

The project is a basic content management system for several web application.
Admin users can manage several web applications' contents and design from only one admin panel. 
The web application data comes through web json api or direct database request.

Technologies and Pattern <br>
•C# <br>
•ASP.NET MVC 5.2<br>
•ASP.NET Web API <br>
•SQL Server 2008<br>
•jQuery <br>
•Ajax <br>
•LINQ <br>
•Entity Framework <br>
•Ninject Dependeceny Injection<br>
•Generic Repository Pattern <br>
•Google Drive API <br>
•Bootstrap <br>
•DotLiquid Templating system<br>
•Quartz.NET Scheduling Framework<br>

<br>
Each 'Store' domain is unique entity in the SQL Server 2008 database and the store data such as products, news, blogs etc will be retrieved by using storeId in the service layer. The service layer is a bunch of interface objects which has two different implementation, JSON API implementation or Entity Framework Repository Pattern implementation. The service layer will get data and then DotLiquid Template system will render the dotliquid markup with data and post it to Asp.Net MVC Views so every store will have different design for every page. 
 