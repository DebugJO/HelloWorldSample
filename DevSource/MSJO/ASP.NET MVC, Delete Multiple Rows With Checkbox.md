### ASP.NET MVC, Delete Multiple Rows With Checkbox

이번 글에서는 ASP.NET MVC 웹 화면의 그리드에서 체크박스로 나열된 여러 Row를 선택하고 한 번에 삭제하는 방법을 데이터베이스 부분부터 C# 코드의 부분까지 간단하지만 상세한 과정을 살펴보기로 한다. 단 테이블 변수를 사용하는 것이 아닌 테이블 사용자 타입을 생성하고 진행할 것이다.

#### Database
테이블 타입 생성
```
CREATE TYPE dbo.IDList AS TABLE (
	ID varchar(11) COLLATE Korean_Wansung_CI_AS NULL
)
```

스토어드 프로시저 생성 : 테이블명은 SomeTable로 가정
```
CREATE PROCEDURE [dbo].[SP_DeleteList]
	@IDList IDList READONLY
AS
BEGIN
	DELETE FROM SomeTable
	WHERE ID IN (SELECT ID FROM @IDList) 
END
```

#### Model, Dapper Operation
DapperORM class
```
public void ExecuteWithoutReturn(string procedureName, DynamicParameters param)
{
	using (_connection)
  {
		_connection.Execute(procedureName, param, commandType: CommandType.StoredProcedure);
	}
}

public SqlMapper.ICustomQueryParameter ReturnTableParameter(string[] rows)
{
	using (var dt = new DataTable())
  {
		dt.Columns.Add("ID");
		if (rows != null)
		foreach (var s in rows) { dt.Rows.Add(s); }
		return dt.AsTableValuedParameter();
	}
}
```

Repository class
```
private readonly DapperORM _dapperOrm = new DapperORM();
public void DeleteList(string[] rows, string userID)
{
	var param = new DynamicParameters();
	param.AddDynamicParams(new {IDList  = _dapperOrm.ReturnTableParameter(rows), userID});
	 _dapperOrm.ExecuteWithoutReturn("SP_DeleteList");
}
```

#### Controller, View
Controller
```
Repository _repository = new Repository();
[HttpPost]
public ActionResult Delete(string[] rows, userID)
{
	 _repository.DeleteList(rows, userID);
	return RedirectToAction("Index");
}
```
View
```
@foreach (var item in Model)
{
	<tr><td><input type="checkbox" name="rows" value="@item.ID" /></td>
	<td>...</td></tr>
}
<button type="submit" formaction="Delete" formmethod="post">삭제</button>
```
