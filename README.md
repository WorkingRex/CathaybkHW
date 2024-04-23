# CathaybkHW

這邊使用 DDD 架構來實作一個簡單的幣別轉換 API，並且使用 Entity Framework Core 來存取資料庫，並且使用 LocalDB 作為資料庫。

## 建立幣別與其對應名稱的資料表

1. 直接執行 SQL 語法
    ```sql
    CREATE TABLE [CurrencyNames] (
        [Code] nvarchar(450) NOT NULL,
        [Language] nvarchar(450) NOT NULL,
        [Name] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_CurrencyNames] PRIMARY KEY ([Code], [Language])
    );
    ```

2. 使用 Entity Framework Core 的 Code First Migration
    - 使用 Package Manager Console
        ```Visual Studio
        Add-Migration InitialCreate
        Update-Database
        ```
    - 使用 dotnet CLI 來執行以下指令
        ```shell
        dotnet ef migrations add InitialCreate
        dotnet ef database update
        ```



## 實作加分題
1. 印出所有 API 被呼叫以及呼叫外部 API 的 request and response body log
    1. API 被呼叫的 request and response body log 用 Middleware 實作
    2. 呼叫外部 API 的 request and response body log 用 AddHttpMessageHandler 實作

2. Error handling 處理 API response
    - 使用 Middleware 實作，可以在 Middleware 指定特定的 Exception 類型，並且回傳特定的錯誤訊息

3. swagger-ui
    
    1. 以 Debug mode 啟動，並訪問 `https://localhost:44303/swagger/index.html` 查看 API 文件
    2. 參考 [swagger yaml](./swagger-ui.yaml) 生成 API 文件

4. 多語系設計

    /api/Currency/CurrencyRateList 這支 API 的 response body 包含了幣別名稱列表，並且在  DB 維護功能中也有新增修改多語系的功能

5. design pattern 實作

    1. Application 層與 Controller 之間使用中介者模式
    2. HttpContent 使用工廠模式取得實例

6. 能夠運行在 Docker

    因為在 Docker 中沒辦法使用 LocalDB，因此需要在 Docker run 時指定外部的 DB 連線字串，以下是 Dockerfile 的建置指令
    ```shell
    docker build -t CathaybkHW:latest .
    docker run --name CathaybkHW -p 8080:8080 -e ConnectionStrings__CathaybkHWDB={{CathaybkHW}} -d myapp:latest
    ```

7. ~~加解密技術應用 (AES/RSA…etc.)~~
