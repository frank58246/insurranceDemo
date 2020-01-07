# insurranceDemo

## 框架
.Net Framework 4.8 Web API

## 程式內容

### CustomController
負責客戶相關邏輯，包括基本的CRUD
- 新增客戶
- 取得指定客戶的資料
- 修改客戶基本資料
- 修改客戶保單資料
- 刪除客戶 

### InsurranceController
負責保單的相關邏輯，包括基本的CRUD
- 新增保單
- 取得指定保單的資料
- 修改保單內容
- 刪除保單

## 備註
- 使用本地端SQL Server資料庫
- 佈署在本地端IIS，必須遠端連回本機才能使用
- Swagger測試連結: http://localhost/InsurranceDemo/swagger/ui/index#/Custom
