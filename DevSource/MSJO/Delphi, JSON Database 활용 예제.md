### Delphi, JSON Database 활용 예제

#### Database에 질의하여 JSON 데이터 만들기 (GetJSON 함수)
```delphi
function TForm1.jGetJSON: string;
var
  NewArr: TJSONArray;
  NewObj: TJSONObject;
  i: integer;
begin
  try
    try
      TestQry.Close;
      TestQry.SQL.Clear;
      TestQry.SQL.Text := 'select userid, username, usercontent from usertable';
      TestQry.Open;
      TestQry.First;
      if TestQry.RecordCount < 1 then
      begin
        Result := '결과 없음';
        Exit;
      end;

      NewArr := TJSONArray.Create;
      for i := 0 to TestQry.RecordCount - 1 do
      begin
        NewObj := TJSONObject.Create;
        NewObj.AddPair('userid', TestQry.Fields[0].AsString);
        NewObj.AddPair('username', TestQry.Fields[1].AsString);
        NewObj.AddPair('usercontent', TestQry.Fields[2].AsString);
        NewArr.AddElement(NewObj);
        TestQry.Next;
      end;
      Result := '{"result":' + NewArr.ToString + '}';
    except
      on e: Exception do
      begin
        Result := '에러 메시지: ' + e.Message;
        Exit;
      end;
    end;
  finally
    FreeAndNil(NewObj);
    TestQry.Close;
  end;
end;
```

JSON은 해당 Value에 값이 없으면 반환하지 않기 때문에 다음과 같은 조치를 해준다. 일반적으로 2가지 정도의 방법으로 해결한다.
```delphi
// 1. 빈 문자열을 더해서 데이터가 있는 것처럼 사용하고 클라이언트에 Trim을 사용
NewObj.AddPair('usercontent', TestQry.Fields[2].AsString + ' '); 
// 2. 데이터베이스에서 질의할 때 결과값이 null, empty이면 빈 문자열을 더해서 리턴
select iif((col1 is null) or (col1 = ''), ' ', col1) as col1 from usertable 
```

#### JSON 데이터를 파싱하여 원하는 형태로 사용하기
```delphi
procedure TForm1.Button1Click(Sender: TObject);
var
  jResponse: TJSONObject;
  jResult: TJSONArray;
  i: integer;
begin
  if Copy(jGetJSON, 3, 6) <> 'result' then
  begin
    ShowMessage(jGetJSON);
    Exit;
  end;
  try
    jResponse := TJSONObject.ParseJSONValue(jGetJSON) as TJSONObject;
    jResult := jResponse.GetValue('result') as TJSONArray;
    for i := 0 to jResult.Count - 1 do
    begin
// 결과를 확인하기 위한 Memo, StringGrid에 사용하면 됨
      Memo2.Lines.Add(jResult.Items[i].GetValue<string>('userid') + ' | ' + jResult.Items[i].GetValue<string>('username') + ' | ' + jResult.Items[i].GetValue<string>('usercontent'));
    end;
  finally
    FreeAndNil(jResponse);
  end;
end;
```

#### Convert JSON to DataSet
GitHub에 공개된 DataSetConverter4Delphi 라이브러리를 활용한다. 메인페이지에 사용 예제를 보면 쉽게 설명이 되어 있어서 사용하는 데 큰 무리가 없을 것이다. 위의 JSON 데이터를 파싱하여 원하는 형태로 사용하기의 jResult를 활용한다. Convert DataSet to JSON / Convert JSON to DataSet
```delphi
// uses에 DataSetConverter4D, DataSetConverter4D.Impl 추가
ClientDataSet1.Close;
ClientDataSet1.CreateDataSet;
TConverter.New.JSON.Source(jResult).ToDataSet(ClientDataSet1);
// DBGrid에서 스크롤바가 사리지는 것을 방지하기 위한 간단한 팁
DBGrid1.Width := DBGrid1.Width + 1;
DBGrid1.Width := DBGrid1.Width - 1;

// DataSet to JSON
TConverter.New.dataSet.Source(DM.DBQry).AsJSONObject.ToString;
```
