### C#, 엑셀 컴포넌트 EPPlus (using OOXML) 사용하기

#### 다운로드 및 설치
Visual Studio를 사용하고 있다면 nuget 패키지 관리자를 이용하여 손쉽게 프로젝트에 추가할 수 있다. PM> Install-Package EPPlus -Version 4.1.1 다운로드 사이트인 https://www.nuget.org/packages/EPPlus/에서 메뉴얼 다운로드를 선택하여 수동으로 설치도 가능하다.

#### 사용예제
예제는 데이터를 불러오는 LoadFrom…() 함수 중에서 LoadFromCollection, LoadFromDataTable 이 두 가지 정도의 함수만 살펴볼 것이다. LoadFromCollection은 데이터 리스트를 불러와 엑셀 파일로 저장하는 예제이고 LoadFromDataTable은 화면의 DataGridView를 Table로 변환한 후 이것을 엑셀로 저장하는 것이다.

##### LoadFromCollection 예제
```
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
