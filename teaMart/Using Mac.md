1. 关于使用sql server 数据库：
解决方案：docker + debeaver

步骤：
1. 安装docker
2. 通过docker的容器安装SQL server for Linux 版本
  ```bash
     docker pull mcr.microsoft.com/mssql/server:2017-latest-ubuntu
  ```
3. 创建数据库实例：
  ```bash
      docker run -d --name teaMart -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=superStrongPwd123' -p 1433:1433 mcr.microsoft.com/mssql/server:2017-latest-ubuntu
  ```
注意：teaMart是数据库实例名，SA数据库连接时的用户名，superStrongPwd123是登录密码（这个密码不能是简单的，否则登录失败）
4. 安装debeaver， 现在sql server， 配置连接端口用户和密码
5. 设置项目配置文件appsettings.Development.json内容
  ```json
    "/连接配置字符串": null,
    "ConnectionStrings": {
    "dblink": "Server=127.0.0.1,1433;Database=TeaShopProject;User Id=sa;Password=superStrongPwd123;TrustServerCertificate=True;"
  }
  ```