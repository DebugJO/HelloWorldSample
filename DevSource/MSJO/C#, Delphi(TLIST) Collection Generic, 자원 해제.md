### C#, Delphi(TLIST) Collection Generic, 자원 해제

##### Delphi 예제
```
unit Unit1;
// 중략 ...
  TCust = class
  public
    ID: string;
    Name: string;
    constructor Create(const _ID: string; const _Name: string);
  end;

  TForm1 = class(TForm)
// 중략 ...
  private
    cList: TList<TCust>;
end;

implementation

constructor TCust.Create(const _ID, _Name: string); // 생성자 구현
begin
  ID := _ID;
  Name := _Name;
end;
//
procedure TForm1.FormCreate(Sender: TObject); // Create
begin
  cList := TList<TCust>.Create;
end;
//
procedure TForm1.FormDestroy(Sender: TObject); // Destory
var
  tc: TCust;
begin
  for tc in cList do
  begin
    tc.Free;
  end;
  FreeAndNil(cList);
end;

procedure TForm1.Button7Click(Sender: TObject); // 테스트 예제
const
  dm: array [1 .. 5] of string = ('서울', '인천', '광주', '부천', '성남');
var
  i: integer;
  cs: TCust;
begin
  Memo1.Clear;

  for cs in cList do // TCust Class 자원 해제
  begin
    cs.Free;
  end;
  cList.Clear; // List 데이터를 초기화

  for i := 1 to 100 do // 대략 입력해보고...
  begin
    cList.Add(TCust.Create('아이디:' + FillZero(IntToSTr(i), 3, 0), '지역:' + dm[RandomRange(1, 6)]));
  end;

  for cs in cList do // Memo에서 확인
  begin
    Memo1.Lines.Add(cs.ID + ' = ' + cs.Name);
  end;
end;

procedure TForm1.Button8Click(Sender: TObject); // Sort, Reverse
begin
  cList.Sort(TComparer<TCust>.Construct(
    function(const T1, T2: TCust): integer
    begin
      Result := CompareText(T1.Name, T2.Name); // 숫자는 CompareValue
    end));
// Reverse는 단순하게 사용함
// cList.Reverse;
end;
```

List에 Set, Get은 다음과 같이 사용 가능
```
TCust(cList.Items[i]).Name
cList.List[i].Name
cList.Items[i].Name
```

##### C#에서 List<T>, Collection Dispose 적용하기
```
using System;
using System.Collections;
using System.Collections.Generic;

namespace ListTest
{
    public struct Addr // 데이터 구조체 선언
    {
        public string ID;
        public string Name;
    }

    public static class Del // Dispose를 위한 클래스와 메쏘드
    {
        public static void ForIn<TItem>(this IEnumerable<TItem> seq, Action<TItem> act)
        {
            foreach (var item in seq) { act(item); }
        }

        public static void DisposeAll(this IEnumerable set)
        {
            foreach (Object obj in set)
            {
                IDisposable disp = obj as IDisposable;
                if (disp != null) { disp.Dispose(); }
            }
        }
    }

    internal class Program // 메인 프로그램
    {
        private static void Main(string[] args)
        {
            List<Addr> aList = new List<Addr>();
            Addr ad = new Addr();

            ad.ID = "1234";
            ad.Name = "가나다라";
            aList.Add(ad);

            ad.ID = "5678";
            ad.Name = "마바사아";
            aList.Add(ad);

            foreach (var a in aList)
            {
                Console.WriteLine("{0} : {1}", a.ID, a.Name);
            }
            
            // aList.OfType<IDisposable>().ForIn(ex => ex.Dispose());
            // 또는 아래처럼... DisposeAll() 호출
            aList.DisposeAll();
            aList.Clear();
            aList = null;
        }
    }
}
```

