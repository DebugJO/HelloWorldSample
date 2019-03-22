### C#, 엑셀 컴포넌트 EPPlus (using OOXML) 사용하기

#### 다운로드 및 설치
Visual Studio를 사용하고 있다면 nuget 패키지 관리자를 이용하여 손쉽게 프로젝트에 추가할 수 있다. PM> Install-Package EPPlus -Version 4.1.1 다운로드 사이트인 https://www.nuget.org/packages/EPPlus/ 에서 메뉴얼 다운로드를 선택하여 수동으로 설치도 가능하다.

#### 사용예제
예제는 데이터를 불러오는 LoadFrom…() 함수 중에서 LoadFromCollection, LoadFromDataTable 이 두 가지 정도의 함수만 살펴볼 것이다. LoadFromCollection은 데이터 리스트를 불러와 엑셀 파일로 저장하는 예제이고 LoadFromDataTable은 화면의 DataGridView를 Table로 변환한 후 이것을 엑셀로 저장하는 것이다.

##### LoadFromCollection 예제
```cs
using (var pck = new ExcelPackage())
{
	var ws = pck.Workbook.Worksheets.Add("Sheet1");
	var db = new DataAccess();
	var result = db.GetTestSearch(); // 사용자 함수, 데이터베이스 결과집합 List<T>
	ws.Cells["A1"].LoadFromCollection(result);
	ws.Cells.Style.Numberformat.Format = "@";
	ws.Cells.Style.Font.Name = "굴림";
	ws.Cells.Style.Font.Size = 9;
	ws.Cells.AutoFitColumns();
	using (var fs = new FileStream("output.xlsx", FileMode.OpenOrCreate)) 
	{ 
		pck.SaveAs(fs); 
	}
}
```

##### LoadFromDataTable 예제
```cs
using (var pck = new ExcelPackage())
{
	var ws = pck.Workbook.Worksheets.Add("Sheet1");
	ws.Cells["A1"].LoadFromDataTable(ToDataTable(DGViewSearch), false);
	ws.Cells.Style.Numberformat.Format = "@";
	ws.Cells.Style.Font.Name = "굴림";
	ws.Cells.Style.Font.Size = 9;
	ws.Cells.AutoFitColumns();
	using (var fs = new FileStream("out.xlsx", FileMode.Create))
	{
		pck.SaveAs(fs);
	}
}
Process.Start(Path.GetDirectoryName(Application.ExecutablePath) + @"\out.xlsx");
```

##### DataGridView to DataTable 변환 함수 (LoadFromDataTable 예제에서 사용)
```cs
private static DataTable ToDataTable(DataGridView dataGridView)
{
	var cnt = 0;
	var dt = new DataTable();
	foreach (DataGridViewColumn dataGridViewColumn in dataGridView.Columns)
	{
		if (!dataGridViewColumn.Visible) continue;
		dt.Columns.Add(dataGridViewColumn.HeaderText);
		cnt = cnt + 1;
	}
	var cell = new object[cnt];
	foreach (DataGridViewRow dataGridViewRow in dataGridView.Rows)
	{
		if (dataGridView.Rows[dataGridViewRow.Index].Cells[0].Value == null) continue;
		for (var i = 0; i < cnt; i++)
		{
			cell[i] = dataGridViewRow.Cells[i].Value;
		}
		dt.Rows.Add(cell);
	}
	return dt;
}
```

##### ASP.NET MVC : File Download 예제
```cs
public ActionResult ChargeSummaryData(ChargeSummaryRptParams rptParams)
{
	var fileDownloadName = "sample.xlsx";
	var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
	var package = CreatePivotTable(rptParams);
	var fileStream = new MemoryStream();
	package.SaveAs(fileStream);
	fileStream.Position = 0;
	var fsr = new FileStreamResult(fileStream, contentType);
	fsr.FileDownloadName = fileDownloadName;
	return fsr;
}
```
