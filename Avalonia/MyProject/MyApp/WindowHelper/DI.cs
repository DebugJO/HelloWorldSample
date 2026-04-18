using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

namespace MyApp.WindowHelper;

public static class DI
{
    // Default 함수
    // public static T Get<T>() where T : notnull
    //     => App.Services.GetRequiredService<T>();
    //
    // public static object Get(Type type)
    //     => App.Services.GetRequiredService(type);

    /// <summary>
    /// 현재 스레드에서 생성 중인 타입들을 추적 (순환 참조 감지용)
    /// </summary>
    private static readonly ThreadLocal<HashSet<Type>> _resolutionStack = new(() => []);

    /// <summary>
    /// 지정된 타입에 해당하는 의존성 객체(인스턴스) 가져오기.
    /// <![CDATA[
    /// CurrentPage = DI.Get<xxxViewModel>();
    /// ]]>
    /// </summary>
    /// <typeparam name="T">가져올 객체 타입 : View, ViewModel, Service</typeparam>
    /// <returns>컨테이터에 등록된 [T]의 싱글톤, 새 인스턴스 : View, ViewModel, Service</returns>
    /// <exception cref="InvalidOperationException">의존성 주입 과정에서 순환 참조가 발생하거나 등록되지 않은 타입을 요청할 때 발생</exception>
    public static T Get<T>() where T : notnull
    {
        Type type = typeof(T);

        if (_resolutionStack.Value!.Contains(type))
        {
            throw new InvalidOperationException(
                $"[순환 참조] {type.Name}이(가) 생성 완료되기 전에 재요청 감지. 생성자 내부의 DI 호출을 확인하세요."
            );
        }

        try
        {
            _resolutionStack.Value.Add(type);
            return App.Services.GetRequiredService<T>();
        }
        finally
        {
            _resolutionStack.Value.Remove(type);
        }
    }

    private static readonly AsyncLocal<HashSet<Type>> _asyncResolutionStack = new();
    public static async Task<T> GetAsync<T>() where T : notnull
    {
        Type type = typeof(T);
        
        // AsyncLocal 내부에 HashSet이 없다면 초기화
        _asyncResolutionStack.Value ??= [];
        var stack = _asyncResolutionStack.Value;

        if (stack.Contains(type))
        {
            throw new InvalidOperationException(
                $"[비동기 순환 참조] {type.Name}이(가) 완료되기 전 재호출됨. 비동기 생성 로직을 확인하세요."
            );
        }

        try
        {
            stack.Add(type);
            // 비동기 서비스 취득 로직 (예시: GetRequiredService는 보통 동기지만, 
            // 만약 별도의 비동기 초기화 로직이 포함된 확장 메서드를 쓰신다면 유용합니다)
            return await Task.Run(() => App.Services.GetRequiredService<T>());
        }
        finally
        {
            stack.Remove(type);
        }
    }
    
    /// <summary>
    /// 타입(이름)으로  의존성 객체(인스턴스) 가져오기.
    /// 메뉴 버튼이 여러 개인데, 버튼마다 함수를 만들지 않고 CommandParameter에 이름만 넣어서 처리
    /// 파라미터로 뷰모델 이름의 앞부분만 전달 : CommandParameter="ServerConfig", Command는 동일
    /// <![CDATA[
    /// var typeName = $"MyApp.ViewModels.{formName}ViewModel";
    /// var type = Type.GetType(typeName);
    /// CurrentPage = DI.Get<MainViewModel>();
    /// ]]>
    /// </summary>
    /// <param name="type">가져올 객체 타입 : View, ViewModel, Service</param>
    /// <returns>컨테이터에 등록된 [T]의 싱글톤, 새 인스턴스 : View, ViewModel, Service</returns>
    /// <exception cref="InvalidOperationException">순환참고, 동적 요청 중 무한 루프 감지</exception>
    [Obsolete("이 메서드는 DI 내부 전용입니다. 직접 호출하지 마세요.", true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static object Get(Type type)
    {
        if (_resolutionStack.Value!.Contains(type))
        {
            throw new InvalidOperationException(
                $"[순환 참조] {type.Name} 동적 요청 중 무한 루프 감지. 생성자 내부의 DI 호출을 확인하세요."
            );
        }

        try
        {
            _resolutionStack.Value.Add(type);
            return App.Services.GetRequiredService(type);
        }
        finally
        {
            _resolutionStack.Value.Remove(type);
        }
    }
}

/* 블록 안의 코드는 컴파일 시점에 포함 안 함
#if DISABLE_GET_METHOD
public static object Get(Type type) { ... }
#endif
*/